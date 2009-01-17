using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace PaulStovell.TeamBuildMonitorGadget {
    public class DateConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            string result = string.Empty;

            if (value != null) {
                DateTime dt = (DateTime)value;
                if (parameter != null) {
                    if (parameter.ToString() == "today") {
                        if (dt.Date == DateTime.Today) {
                            result = "Today";
                        } else if (dt.Date == DateTime.Today.AddDays(-1)) {
                            result = "Yesterday";
                        } else {
                            result = dt.ToString("dd-MMM");
                        }
                    } else {
                        result = dt.ToString(parameter.ToString());
                    }
                } else {
                    result = dt.ToString("dd-MMM");
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
