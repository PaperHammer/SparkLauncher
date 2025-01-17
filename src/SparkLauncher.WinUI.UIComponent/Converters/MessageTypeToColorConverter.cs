using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using SparkLauncher.Common;

namespace SparkLauncher.WinUI.UIComponent.Converters {
    public partial class MessageTypeToColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is TaskMsgType type) {
                return type switch {
                    TaskMsgType.Info => new SolidColorBrush(Colors.CornflowerBlue),
                    TaskMsgType.Warn => new SolidColorBrush(Colors.DarkOrange),
                    TaskMsgType.Success => new SolidColorBrush(Colors.DarkGreen),
                    TaskMsgType.Error => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Black),
                };
            }
            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
