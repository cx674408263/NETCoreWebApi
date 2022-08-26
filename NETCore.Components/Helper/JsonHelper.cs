using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Components.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// 转Json回HttpResponseMessage
        /// </summary>
        /// <param name="code"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string toJson(object result)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}
