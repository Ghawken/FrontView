// ------------------------------------------------------------------------
//    YATSE 2 - A touch screen remote controller for XBMC (.NET 3.5)
//    Copyright (C) 2010  Tolriq (http://yatse.leetzone.org)
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;

namespace Setup
{
    public class DownloadFileInfo : IEquatable<DownloadFileInfo>
    {
        public string FileSource { get; set; }
        public string FileDestination { get; set; }
        public string FileHash { get; set; }

        public bool Equals(DownloadFileInfo other)
        {
            return (other.FileHash == FileHash && other.FileDestination == FileDestination && other.FileSource == FileSource);
        }
    }

    public class ThreadedDownlads : IDisposable
    {
        private readonly EventWaitHandle _wh = new AutoResetEvent(false);
        private readonly Thread[] _worker;
        private readonly object _locker = new object();
        private readonly Queue<DownloadFileInfo> _files = new Queue<DownloadFileInfo>();
        private readonly Collection<string> _currents = new Collection<string>();
        private readonly Queue<DownloadFileInfo> _copies = new Queue<DownloadFileInfo>();
        private readonly string _tempdir;
        private bool _result;
        private readonly bool _backup;

        public ThreadedDownlads(int threads, string tempdir, bool backup)
        {
            _worker = new Thread[threads];
            _tempdir = tempdir;
            _result = true;
            _backup = backup;
            for (var i = 0; i < _worker.Length; i++)
            {
                _worker[i] = new Thread(Work);
                _worker[i].Start();
            }
        }

        public bool GetResult()
        {
            return _result;
        }

        public void EnqueueTask(DownloadFileInfo task)
        {
            lock (_locker) _files.Enqueue(task);
            if (_wh != null)
                _wh.Set();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
            EnqueueTask(null);
            foreach (var t in _worker)
            {
                t.Join();
            }
            Logger.Instance().Log("TheadDl", "Disposed");
            _wh.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool WaitEnd()
        {
            EnqueueTask(null);
            foreach (var t in _worker)
            {
                t.Join();
            }
            return _result ? DoCopies() : _result;
        }

        private bool DownloadFile(string file, string destination, string hash)
        {
            var inCache = false;
            var copyFile = new DownloadFileInfo();
            if (!String.IsNullOrEmpty(hash) && File.Exists(destination))
            {
                if (FileHash.CheckFileHashString(destination, hash))
                {
                    Logger.Instance().Log("ThreadDl" + Thread.CurrentThread.GetHashCode(), "Destination file already correct - " + destination);
                    return true;
                }
                copyFile.FileHash = _backup ? "1" : "0";
            }
            file = file.Replace('\\', '/');
            var index = file.LastIndexOf('/');
            var filename = file;
            if (index > -1)
                filename = file.Substring(index);
            var cont = true;
            for (var i = 0; i < 5; i++ )
            {
                lock (_locker)
                {
                    if (_currents.Count > 0 && _currents.Contains(file))
                        cont = false;
                    else
                        cont = true;
                }
                if (cont)
                    break;
                Thread.Sleep(1000);
            }
            if (!cont)
            {
                Logger.Instance().Log("ThreadDl" + Thread.CurrentThread.GetHashCode(), "Source file downloaded by other thread but not released in 5 sec - " + file,true);
                return false;
            }
            lock (_locker)
            {
                _currents.Add(file);
            }
            if (File.Exists(_tempdir + @"\" + filename))
                if (!String.IsNullOrEmpty(hash))
                {
                    if (FileHash.CheckFileHashString(_tempdir + @"\" + filename, hash))
                        inCache = true;
                    else
                        File.Delete(_tempdir + @"\" + filename);
                }
                else
                    File.Delete(_tempdir + @"\" + filename);
            try
            {
                
                if (!inCache)
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(file, _tempdir + @"\" + filename);
                    }
                }

                if (!String.IsNullOrEmpty(hash))
                {
                    var result = FileHash.CheckFileHashString(_tempdir + @"\" + filename, hash);
                    if (result)
                        Logger.Instance().Log("ThreadDl" + Thread.CurrentThread.GetHashCode(), "File downloaded - " + file);
                    else
                        Logger.Instance().Log("ThreadDl" + Thread.CurrentThread.GetHashCode(), "Error (Hash) - " + file,true);

                    lock (_locker)
                    {
                        if (result)
                        {
                            copyFile.FileSource = _tempdir + @"\" + filename;
                            copyFile.FileDestination = destination;
                            _copies.Enqueue(copyFile);
                        }
                        _currents.Remove(file);
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                Logger.Instance().Log("ThreadDl" + Thread.CurrentThread.GetHashCode(), "Error (" + e.Message + ") - " + file,true);
                lock (_locker)
                {
                    _currents.Remove(file);
                }
                return false;
            }
            Logger.Instance().Log("ThreadDl" + Thread.CurrentThread.GetHashCode(), "File downloaded - " + filename);
            lock (_locker)
            {
                copyFile.FileSource = _tempdir + @"\" + filename;
                copyFile.FileDestination = destination;
                _copies.Enqueue(copyFile);
                _currents.Remove(file);
            }
            return true;
        }

        private bool DoCopies()
        {
            try
            {
                while (_copies.Count > 0)
                {
                    var tmp = _copies.Dequeue();
                    var targetdirectory2 = Path.GetDirectoryName(tmp.FileDestination);
                    if (!Directory.Exists(targetdirectory2))
                        Directory.CreateDirectory(targetdirectory2);
                    if (tmp.FileHash == "1")
                    {
                        if (File.Exists(tmp.FileDestination))
                        {
                            if (File.Exists(tmp.FileDestination+ ".bck")) 
                                File.Delete(tmp.FileDestination + ".bck");
                            File.Move(tmp.FileDestination,tmp.FileDestination + ".bck");
                        }
                    }
                    else
                    {
                        if (File.Exists(tmp.FileDestination))
                            File.Delete(tmp.FileDestination);
                    }
                    File.Copy(tmp.FileSource,tmp.FileDestination);
                    Logger.Instance().Log("Setup", "File copied - " + tmp.FileDestination,true);
                }
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        private void SetError()
        {
            lock (_locker)
            {
                _result = false;
            }
        }

        void Work()
        {
            while (true)
            {
                if (_result == false)
                {
                    return;
                }
                DownloadFileInfo task = null;
                lock (_locker)
                    if (_files.Count > 0)
                    {
                        task = _files.Dequeue();
                        if (task == null)
                        {
                            EnqueueTask(null); // Requeue for other thread to close.
                            return;
                        }
                    }
                if (task != null)
                {
                    if (!DownloadFile(task.FileSource, task.FileDestination, task.FileHash))
                    {
                        SetError();
                    }
                }
                else
                    _wh.WaitOne();
            }
        }
    }
}