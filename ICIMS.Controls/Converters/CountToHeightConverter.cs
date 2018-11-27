using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ICIMS.Controls.Converters
{
   
    public class CountToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            if (value == null)
            {
                result = 0;
            }
            else
            {
                int num = System.Convert.ToInt32(value.ToString());
                double num2 = (double)num / 4.0;
                int num3 = (int)Math.Floor(num2);
                if (num2 == (double)num3)
                {
                    if (num3 == 1)
                    {
                        result = 80;
                    }
                    else
                    {
                        result = num3 * 68;
                    }
                }
                else if (num3 == 0 && num2 > 0.0)
                {
                    result = 80;
                }
                else
                {
                    result = (num3 + 1) * 68;
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
