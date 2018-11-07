using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service
{
    public class ResultData<T>
    {
        public object Result { get; set; }
        // public List<T> Items { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public bool UnAuthorizedRequest { get; set; }
        public string TargetUrl { get; set; }

    }
    public static class Extensions
    {
        public static List<T> ToModel<T>(this ResultData<T> resultData, out long dataCount)
        {
            dataCount = 0;
            if (resultData == null || resultData.Result == null)
            {

                return default(List<T>);
            }
            var josnItems = resultData.Result;
            var item = JObject.Parse(josnItems.ToString());

            var list = JsonConvert.DeserializeObject<List<T>>(item["items"].ToString());
            if (!string.IsNullOrEmpty(item.Property("totalCount")?.Name))
            {
                dataCount = Convert.ToInt64(item["totalCount"]);
            }
            else
                dataCount = list.Count;
            return list;
        }
        public static T ToEntity<T>(this ResultData<T> resultData)
        {
         
            if (resultData == null || resultData.Result == null)
            { 
                return default(T);
            }
            var item = resultData.Result;
             
            var  entity = JsonConvert.DeserializeObject<T>(item.ToString());
            return entity;
        }
    }
}
