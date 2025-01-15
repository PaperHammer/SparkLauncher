using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using SparkLauncher.Common;

namespace SparkLauncher.WinUI.UIComponent.Converters {
    public partial class EnumToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is RuntimeStatus status && parameter is string controlTag && Enum.TryParse<RuntimeStatus>(controlTag, out var targetStatus)) {
                return status == targetStatus ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
