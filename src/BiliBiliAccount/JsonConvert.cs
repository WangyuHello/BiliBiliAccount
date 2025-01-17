﻿
using BiliBiliAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BiliBiliAPI
{
    public static class JsonConvert
    {
        public  static T Deserialize<T>(string data)
        {
            var setting = new JsonSerializerSettings();
            setting.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            Type type = typeof(T);
            JsonReader reader = new JsonTextReader(new StringReader(data));
            JsonSerializer jsonSerializer = new JsonSerializer();
            
            var d = jsonSerializer.Deserialize<T>(reader);
            return d;
        }


        public static ResultCode<T> ReadObject<T>(string Data)
        {
            JsonReader reader = new JsonTextReader(new StringReader(Data));
            JsonSerializer jsonSerializer = new JsonSerializer();
            JObject jo = JObject.Parse(Data);
            var code = jsonSerializer.Deserialize<T>(reader);
            ResultCode<T> result = new ResultCode<T>();
            result = Deserialize<ResultCode<T>>(Data);
            return result;
        }
    }
}
