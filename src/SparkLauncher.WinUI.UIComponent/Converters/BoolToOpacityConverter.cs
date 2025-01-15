using System;
using Microsoft.UI.Xaml.Data;

namespace SparkLauncher.WinUI.UIComponent.Converters {
    public partial class BoolToOpacityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is bool v) {
                return v ^ (parameter as string ?? string.Empty).Equals("Reverse") ?
                    1 : 0.4;
            }
            else {
                return value is not null ^ (parameter as string ?? string.Empty).Equals("Reverse") ?
                    1 : 0.4;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
