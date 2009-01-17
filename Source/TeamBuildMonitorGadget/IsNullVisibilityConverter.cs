using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace PaulStovell.TeamBuildMonitorGadget {
    public class IsNullVisibilityConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            Visibility result = Visibility.Visible;

            if (parameter == null) {
                result = (value == null) ? Visibility.Visible : Visibility.Collapsed; 
            } else if (parameter.ToString().ToLower().Contains("not")) {
                result = (value == null) ? Visibility.Collapsed : Visibility.Visible;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
