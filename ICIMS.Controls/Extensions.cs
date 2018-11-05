using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICIMS.Controls
{
   public static class Extensions
    {
       public static T ToEnumFromName<T>(this string name)
       {
           if (name==null)
           {
               return default(T);   
           }
           Type enumType = typeof(T);
           if (!enumType.IsEnum)
           {
               throw new ArgumentException("type is not a Enum Type");
           }
           if (!Enum.IsDefined(enumType,name))
           {
               throw new ArgumentException("invalid data to be converted to enum");
           }
           return (T)Enum.Parse(enumType, name);
       }
    }
}
