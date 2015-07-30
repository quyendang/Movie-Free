using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FreeApp.Utils
{
    public class DurationMillisecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is double))
            {
                return null;
            }
            int num = (int)((double)value / 1000);
            int num1 = num / 60;
            num = num % 60;
            int num2 = num1 / 60;
            if (num2 <= 0)
            {
                return string.Format("{0}:{1}", num1, num.ToString("00"));
            }
            num1 = num1 % 60;
            return string.Format("{0}:{1}:{2}", num2, num1.ToString("00"), num.ToString("00"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
