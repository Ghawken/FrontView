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
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Setup
{
    public class Logger
    {
        private static Logger _instance;
        private static readonly Object ClassLock = typeof(Logger);
        public string LogFile { get; set; }
        public bool Debug { get; set; }
        public bool DebugTrace { get; set; }
        private const int SourcePadding = 13;

        private Logger()
        {
        }

        public static Logger Instance()
        {
            lock (ClassLock)
            {
                if (_instance == null)
                {
                    _instance = new Logger();

                }
            }
            return _instance;
        }

        public void Log(string source,string message,bool force)
        {
            if (!Debug && !force) return;
            if (String.IsNullOrEmpty(source))
                return;
            lock (ClassLock)
            {
                using (var sw = new StreamWriter(LogFile, true))
                {
                    sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss.ffff", CultureInfo.InvariantCulture) + "] " +
                                 source.PadRight(SourcePadding) + " : " + message);
                }
            }
        }

        public void Trace(string source, string message)
        {
            if (!DebugTrace) return;
            Log("** TRACE **",  source + "\n" + message);
        }


        public void Log(string source, string message)
        {
            Log(source, message, false);
        }

        public void LogDump(string source, object message,bool force)
        {
            if (!Debug) return;
            if (message == null)
                return;
            var s = new XmlSerializer(message.GetType());
            using (var sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                s.Serialize(sw, message);
                Log(source , 
                    sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", "").
                    Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"","")
                ,force);
            }
        }

        public void LogDump(string source, object message)
        {
            LogDump(source, message,false);
        }

        public void TraceDump(string source, object message)
        {
            if (!DebugTrace) return;
            LogDump(source, message, false);
        }

        public void LogException(string source,Exception exception)
        {
            if (exception == null)
                return;
            var trace = new System.Diagnostics.StackTrace(exception, true);
            var errormessage = "Error [ " + exception.Message + " ] " + "( " + exception.GetType() + " )";
            errormessage += "\r\n" + trace.GetFrame(0).GetMethod().Name + " : " + trace.GetFrame(0).GetFileLineNumber() + " / " + trace.GetFrame(0).GetFileColumnNumber();
            errormessage += "\r\n" + exception.StackTrace;
            var inner = exception.InnerException;
            if (inner != null)
                errormessage += "\r\n -- Inner ---\r\n" + inner.StackTrace;
            Log(source,errormessage,true);
        }

        public void RotateLogFile()
        {
            lock (ClassLock)
            {
                try
                {
                    File.Delete(LogFile + ".old");
                    File.Move(LogFile, LogFile + ".old");
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentNullException || ex is ArgumentException || ex is IOException ||
                        ex is NotSupportedException || ex is UnauthorizedAccessException)
                    {
                        return;
                    }
                    throw;
                }
            }
        }

    }
}
