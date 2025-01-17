using System;
using Microsoft.UI.Xaml.Data;
using SparkLauncher.Common;

namespace SparkLauncher.WinUI.UIComponent.Converters {
    public partial class MessageTypeToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is TaskMsgType type) {
                return type switch {
                    TaskMsgType.Info => "Info",
                    TaskMsgType.Warn => "Warn",
                    TaskMsgType.Success => "Success",
                    TaskMsgType.Error => "Error",
                    _ => string.Empty,
                };
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
