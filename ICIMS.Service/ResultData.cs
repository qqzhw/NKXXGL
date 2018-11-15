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
        public T Items { get; set; }
        public int TotalCount { get; set; }
        public bool Success { get; set; }
    }
    public static class Extensions
    {

        public static (int totalcount, T t) ToEntity<T>(this ResultData<T> resultData)
        {
            var items = resultData.Items;
            int total = resultData.TotalCount;

            return (total, items);
        }
    } 
}
