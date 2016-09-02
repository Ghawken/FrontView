#region License, Terms and Conditions
//
// Jayrock - A JSON-RPC implementation for the Microsoft .NET Framework
// Written by Atif Aziz (www.raboof.com)
// Copyright (c) Atif Aziz. All rights reserved.
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

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Setup;

namespace Remote.XBMC.Krypton.Api
{
    public class JsonRpcClient : HttpWebClientProtocol
    {
        private int _id;

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var webRequest = (HttpWebRequest)base.GetWebRequest(uri);

            webRequest.KeepAlive = false;
            webRequest.ReadWriteTimeout = 1000000;
            //webRequest.ProtocolVersion = HttpVersion.Version10;
            return webRequest;
        }

        public object Invoke(string method)
        {
            return Invoke(AnyType.Value, method);
        }

        public object Invoke(Type returnType, string method)
        {
            return Invoke(returnType, method, (object) null);
        }

        public object Invoke(string method, object[] args)
        {
            return Invoke(AnyType.Value, method, args);
        }

        public object Invoke(Type returnType, string method, object[] args)
        {
            return Invoke(returnType, method, (object) args);
        }

        public object InvokeVargs(string method, params object[] args)
        {
            return Invoke(method, args);
        }

        public object InvokeVargs(Type returnType, string method, params object[] args)
        {
            return Invoke(returnType, method, args);
        }

        public object Invoke(string method, IDictionary args)
        {
            return Invoke(AnyType.Value, method, args);
        }

        public object Invoke(string method, object args)
        {
            return Invoke(AnyType.Value, method, args);
        }

        public object Invoke(Type returnType, string method, IDictionary args)
        {
            return Invoke(returnType, method, (object) args);
        }

        public virtual object Invoke(Type returnType, string method, object args)
        {
            if (method == null) 
                throw new ArgumentNullException("method");
            if (method.Length == 0)
                throw new ArgumentException(null, "method");
            if (returnType == null) 
                throw new ArgumentNullException("returnType");
            try
            {
                var request = GetWebRequest(new Uri(Url));
                request.Method = "POST";
                request.Timeout = 300000;
                var utf8EmitBom = new UTF8Encoding(false);
                using (var stream = request.GetRequestStream())
                {
                    stream.ReadTimeout = 300000;
                    using (var writer = new StreamWriter(stream, utf8EmitBom))
                    {
                        var call = new JsonObject();
                        call["id"] = ++_id;
                        call["jsonrpc"] = "2.0";
                        call["method"] = method;
                        if (args != null)
                            call["params"] = args;
                        JsonConvert.Export(call, writer);
                    }
                }
                object ret;
                using (var response = GetWebResponse(request))
                    using (var stream = response.GetResponseStream())
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                            ret = OnResponse(JsonText.CreateReader(reader), returnType);
                return ret;
            }
            catch (WebException ex)
            {
                throw new JsonException("Invalid JSON-RPC response. It contains neither a result nor error : " + ex.Message);
            }

        }

        public virtual object InvokeArray(Type returnType, string[] method, object[] args)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            if (method.Length == 0)
                throw new ArgumentException(null, "method");
            if (returnType == null)
                throw new ArgumentNullException("returnType");
            try
            {
                var request = GetWebRequest(new Uri(Url));
                request.Method = "POST";
                var utf8EmitBom = new UTF8Encoding(false);
                using (var stream = request.GetRequestStream())
                using (var writer = new StreamWriter(stream, utf8EmitBom))
                {
                    var calls = new JsonArray();
                    var i = 0;
                    foreach (var meth in method)
                    {
                        var call = new JsonObject();
                        call["id"] = ++_id;
                        call["jsonrpc"] = "2.0";
                        call["method"] = method[i];
                        if (args[i] != null)
                            call["params"] = args[i];
                        calls.Add(call);
                        i++;
                    }
                    Logger.Instance().Log("RPC",calls.ToString());
                    JsonConvert.Export(calls, writer);
                }
                object ret;
                using (var response = GetWebResponse(request))
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                    ret = OnResponseArray(JsonText.CreateReader(reader), returnType);
                return ret;
            }
            catch (WebException ex)
            {
                throw new JsonException("Invalid JSON-RPC response. It contains neither a result nor error : " + ex.Message);
            }

        }

        private object OnResponseArray(JsonReader reader, Type returnType)
        {
            Logger.Instance().Log("RPC", "Response");
            var members = JsonBuffer.From(reader).GetArray();//.GetMembers();// .GetMembersArray();
            
            foreach (var member in members)
            {
                var members2 = member.GetMembersArray();//JsonBuffer.From(member.).GetMembersArray();
                
                foreach (var member2 in members2)
                {
                    
                    if (string.CompareOrdinal(member2.Name, "error") == 0)
                    {
                        var errorObject = JsonConvert.Import(member2.Buffer.CreateReader());
                        if (errorObject != null)
                            OnError(errorObject);
                    }
                    else if (string.CompareOrdinal(member2.Name, "result") == 0)
                    {
                        Logger.Instance().Log("RPC", JsonConvert.Import(returnType, member2.Buffer.CreateReader()).ToString());
                        return returnType != typeof (JsonBuffer)
                                   ? JsonConvert.Import(returnType, member2.Buffer.CreateReader())
                                   : member2.Buffer;
                    }
                }
            }
            throw new JsonException("Invalid JSON-RPC response. It contains neither a result nor error.");
        }


        private object OnResponse(JsonReader reader, Type returnType)
        {
            var members = JsonBuffer.From(reader).GetMembersArray();
            foreach (var member in members)
            {
                if (string.CompareOrdinal(member.Name, "error") == 0)
                {
                    var errorObject = JsonConvert.Import(member.Buffer.CreateReader());
                    if (errorObject != null)
                        OnError(errorObject);
                }
                else if (string.CompareOrdinal(member.Name, "result") == 0)
                {
                    return returnType != typeof(JsonBuffer) 
                               ? JsonConvert.Import(returnType, member.Buffer.CreateReader()) 
                               : member.Buffer;
                }
            }
            throw new JsonException("Invalid JSON-RPC response. It contains neither a result nor error.");
        }

        protected virtual void OnError(object errorValue) 
        {
            var error = errorValue as IDictionary;
            if (error != null)
                throw new JsonException(error["message"] as string);
            throw new JsonException(errorValue as string);
        }
    }
}


