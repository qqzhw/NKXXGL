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
                string colorStr = "OrangeRed";
                int result = 0;
                int.TryParse(value.ToString(), out result);
                switch (result)
                {
                    case 0:
                        colorStr = "OrangeRed";
                        break;
                    case 1:
                        colorStr = "Orange";
                        break;
                    case 2:
                        colorStr = "Green";
                        break;
                    case 3:
                        colorStr= "#0099FF";
                        break;                    
                    default:
                        colorStr= "OrangeRed";
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
