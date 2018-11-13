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
        public static void SetValue<T>(T target, T value) where T:class
        {
            var properties = typeof(T).GetProperties();
            foreach (var pro in properties)
            {
                if(pro.CanWrite && pro.CanRead)
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
    }
}
