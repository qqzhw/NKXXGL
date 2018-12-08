using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICIMS.Service
{
    public static class CommonHelper
    {
        public static void SetValue<T>(T target, T value) where T : class
        {
            var properties = typeof(T).GetProperties();
            foreach (var pro in properties)
            {
                if (pro.CanWrite && pro.CanRead)
                {
                    var val = pro.GetValue(value);
                    pro.SetValue(target, val);
                }
            }
        }

        public static T CopyItem<T>(T val)
        {
            var jsonStr = JsonConvert.SerializeObject(val);
            var newVal = JsonConvert.DeserializeObject<T>(jsonStr);

            return newVal;
        }

        public static string GenerateNextNo(string no)
        {
            var idx = no.LastIndexOf('-');
            var pre = "";
            if(idx > 0)
            {
                pre = no.Substring(0, idx);
            }
            var last = no.Substring(idx + 1);
            if (int.TryParse(last, out int num))
            {
                var len = last.Length;
                last = (num + 1).ToString();
                if(last.Length > len)
                {
                    len = last.Length;
                }
                var rs = last.PadLeft(len, '0');
                if (!string.IsNullOrEmpty(pre))
                {
                    return $"{pre}-{rs}";
                }

                return rs;
            }
            return "";
        }
    }
}
