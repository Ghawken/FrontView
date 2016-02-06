#region License, Terms and Conditions
//
// Jayrock - JSON and JSON-RPC for Microsoft .NET Framework and Mono
// Written by Atif Aziz (www.raboof.com)
// Copyright (c) 2005 Atif Aziz. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 2.1 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
// details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, Inc.,
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
#endregion

namespace Jayrock.Json
{
    #region Imports

    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using Jayrock.Json.Conversion;

#if !NET_1_0 && !NET_1_1
    using System.Collections.Generic;    
#endif

    #endregion

    /// <summary>
    /// An unordered collection of name/value pairs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Althought the collection should be considered unordered by the user, 
    /// the implementation does internally try to remember the order in which 
    /// the keys were added in order facilitate human-readability as in when
    /// an instance is rendered as text.</para>
    /// <para>
    /// Public Domain 2002 JSON.org, ported to C# by Are Bjolseth (teleplan.no)
    /// and re-adapted by Atif Aziz (www.raboof.com)</para>
    /// </remarks>

    [ Serializable ]
    public class JsonObject : DictionaryBase, IJsonImportable, IJsonExportable, IEnumerable
#if !NET_1_0 && !NET_1_1
        /* ... */ , 
        IEnumerable<JsonMember>
#endif
    {
        private ArrayList _nameIndexList;
        [ NonSerialized ] private IList _readOnlyNameIndexList;

        public JsonObject() {}

        /// <summary>
        /// Construct a JsonObject from a IDictionary
        /// </summary>

        public JsonObject(IDictionary members)
        {
            foreach (DictionaryEntry entry in DictionaryHelper.GetEntries(members))
            {
                if (entry.Key == null)
                    throw new InvalidMemberException();

                InnerHashtable.Add(entry.Key.ToString(), entry.Value);
            }

            _nameIndexList = new ArrayList(members.Keys);
        }

        public JsonObject(string[] keys, object[] values)
        {
            int keyCount = keys == null ? 0 : keys.Length;
            int valueCount = values == null ? 0 : values.Length;
            int count = Math.Max(keyCount, valueCount);

            string key = string.Empty;

            for (int i = 0; i < count; i++)
            {
                if (i < keyCount)
                    key = Mask.NullString(keys[i]);

                Accumulate(key, i < valueCount ? values[i] : null);
            }
        }

        public virtual new JsonMemberEnumerator GetEnumerator()
        {
            return new JsonMemberEnumerator(this, NameIndexList.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

#if !NET_1_0 && !NET_1_1
        IEnumerator<JsonMember> IEnumerable<JsonMember>.GetEnumerator()
        {
            return GetEnumerator();
        }
#endif

        [Serializable]
        public sealed class JsonMemberEnumerator : IEnumerator, IDisposable
#if !NET_1_0 && !NET_1_1
            , IEnumerator<JsonMember>
#endif
        {
            private JsonObject _obj;
            private IEnumerator _enumerator;
            private JsonMember _member;
            private bool _memberInitialized;

            public JsonMemberEnumerator(JsonObject obj, IEnumerator enumerator)
            {
                Debug.Assert(obj != null);
                Debug.Assert(enumerator != null);

                _obj = obj;
                _enumerator = enumerator;
            }

            public bool MoveNext()
            {
                EnsureNotDisposed();
                ResetMember();
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                EnsureNotDisposed();
                ResetMember();
                _enumerator.Reset();
            }

            private void ResetMember()
            {
                _member = new JsonMember();
                _memberInitialized = false;
            }

            public JsonMember Current
            {
                get
                {
                    EnsureNotDisposed();
                    if (!_memberInitialized)
                    {
                        string name = (string) _enumerator.Current;
                        _member = new JsonMember(name, _obj[name]);
                        _memberInitialized = true;
                    }
                    return _member;
                }
            }

            object IEnumerator.Current { get { return Current; } }

            private void EnsureNotDisposed()
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(GetType().FullName);
            }

            private bool IsDisposed { get { return _enumerator == null; } }

            public void Dispose()
            {
                if (IsDisposed)
                    return;

                _obj = null;
                _enumerator = null;
                ResetMember();
            }
        }

        public virtual object this[string key]
        {
            get { return InnerHashtable[key]; }
            set { Put(key, value); }
        }

        public virtual bool HasMembers
        {
            get { return Count > 0; }
        }

        private ArrayList NameIndexList
        {
            get
            {
                if (_nameIndexList == null)
                    _nameIndexList = new ArrayList();

                return _nameIndexList;
            }
        }

        /// <summary>
        /// Accumulate values under a key. It is similar to the Put method except
        /// that if there is already an object stored under the key then a
        /// JsonArray is stored under the key to hold all of the accumulated values.
        /// If there is already a JsonArray, then the new value is appended to it.
        /// In contrast, the Put method replaces the previous value.
        /// </summary>

        public virtual void Accumulate(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            object current = InnerHashtable[name];

            if (current == null)
            {
                Put(name, value);
            }
            else 
            {
                IList values = current as IList;
                
                if (values != null)
                {
                    values.Add(value);
                }
                else
                {
                    values = new JsonArray();
                    values.Add(current);
                    values.Add(value);
                    Put(name, values);
                }
            }
        }

        /// <summary>
        /// Adds a key/value pair to the JsonObject. If the key is already
        /// present then an exception is thrown.
        /// </summary>

        public virtual void Add(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Dictionary.Add(name, value);
        }

        /// <summary>
        /// Put a key/value pair in the JsonObject. If the value is null,
        /// then the key will be removed from the JsonObject if it is present.
        /// </summary>

        public virtual void Put(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Dictionary[name] = value;
        }

        public virtual bool Contains(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return Dictionary.Contains(name);
        }

        public virtual void Remove(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Dictionary.Remove(name);
        }

        public virtual ICollection Names
        {
            get
            {
                if (_readOnlyNameIndexList == null)
                    _readOnlyNameIndexList = ArrayList.ReadOnly(NameIndexList);

                return _readOnlyNameIndexList;
            }
        }

        /// <summary>
        /// Produce a JsonArray containing the names of the elements of this
        /// JsonObject.
        /// </summary>

        public virtual JsonArray GetNamesArray()
        {
            JsonArray names = new JsonArray();
            ListNames(names);
            return names;
        }

        public virtual void ListNames(IList list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            foreach (string name in NameIndexList)
                list.Add(name);
        }

        /// <summary>
        /// Overridden to return a JSON formatted object as a string.
        /// </summary>
        
        public override string ToString()
        {
            StringWriter writer = new StringWriter();
            Export(JsonText.CreateWriter(writer));
            return writer.ToString();
        }
        
        public void Export(JsonWriter writer)
        {
            Export(JsonConvert.CreateExportContext(), writer);
        }

        void IJsonExportable.Export(ExportContext context, JsonWriter writer)
        {
            Export(context, writer);
        }

        protected virtual void Export(ExportContext context, JsonWriter writer)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteStartObject();
            
            foreach (string name in NameIndexList)
            {
                writer.WriteMember(name);    
                context.Export(InnerHashtable[name], writer);
            }

            writer.WriteEndObject();
        }

        /// <remarks>
        /// This method is not exception-safe. If an error occurs while 
        /// reading then the object may be partially imported.
        /// </remarks>
        
        public void Import(JsonReader reader)
        {
            Import(JsonConvert.CreateImportContext(), reader);
        }
        
        void IJsonImportable.Import(ImportContext context, JsonReader reader)
        {
            Import(context, reader);
        }

        protected virtual void Import(ImportContext context, JsonReader reader)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (reader == null)
                throw new ArgumentNullException("reader");

            // FIXME: Consider making this method exception-safe.
            // Right now this is a problem because of reliance on
            // DictionaryBase.
            
            Clear();
            
            reader.ReadToken(JsonTokenClass.Object);
            
            while (reader.TokenClass != JsonTokenClass.EndObject)
                Put(reader.ReadMember(), context.Import(reader));
            
            reader.Read();
        }

        protected override void OnValidate(object key, object value)
        {
            base.OnValidate(key, value);

            if (key == null)
                throw new ArgumentNullException("key");

            if (!(key is string))
                throw new ArgumentException(string.Format("The key cannot be of the supplied type {0}. It must be typed System.String.", key.GetType().FullName), "key");
        }

        protected override void OnInsert(object key, object value)
        {
            //
            // NOTE: OnInsert leads one to believe that keys are ordered in the
            // base dictionary in that they can be inserted somewhere in the
            // middle. However, the base implementation only calls OnInsert
            // during the Add operation, so we known it is safe here to simply
            // add the new key at the end of the name list.
            //

            NameIndexList.Add(key);
        }

        protected override void OnSet(object key, object oldValue, object newValue)
        {
            //
            // NOTE: OnSet is called when the base dictionary is modified via
            // the indexer. We need to trap this and detect when a new key is
            // being added via the indexer. If the old value is null for the
            // key, then there is a big chance it is a new key. But just to be
            // sure, we also check out key index if it does not already exist.
            // Finally, we just delegate to OnInsert. In effect, we're
            // converting OnSet to OnInsert where needed. Ideally, the base
            // implementation would have done this for.
            //

            if (oldValue == null && !NameIndexList.Contains(key))
                OnInsert(key, newValue);
        }

        protected override void OnRemove(object key, object value)
        {
            NameIndexList.Remove(key);
        }

        protected override void OnClear()
        {
            NameIndexList.Clear();
        }
    }
}
