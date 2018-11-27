using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ICIMS.Controls.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string m_Value = string.Empty;
            if (value != null)
            {
                m_Value = value.ToString();
                DateTime dateValue;
                bool sucessed = DateTime.TryParseExact(m_Value, "yyyyMMddHHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateValue);
                if (sucessed)
                {
                    return dateValue.ToString("MM-dd HH:mm:ss");
                }
                else
                    return "--";

            }
            return m_Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
