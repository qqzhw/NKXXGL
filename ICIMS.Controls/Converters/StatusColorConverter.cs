using System;
using System.Windows.Data;

namespace ICIMS.Controls.Converters
{
    public class StatusColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string colorStr = "#FFFF00";
                int result = 0;
                int.TryParse(value.ToString(), out result);
                switch (result)
                {
                    case 0:
                        colorStr = "#FFFF00";
                        break;
                    case 1:
                        colorStr = "#90EE90";
                        break;
                    case 2:
                        colorStr = "Green";
                        break;
                    case 3:
                        colorStr= "#0099FF";
                        break;                    
                    default:
                        colorStr= "#b22222";
                        break;
                }
                return colorStr;
            }
            else
            {
                return "OrangeRed";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
 
}
