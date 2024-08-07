using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace AppNotas.Utils
{
    public class PassThroughConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

