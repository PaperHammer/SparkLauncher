using System;
using Microsoft.UI.Xaml.Data;
using SparkLauncher.WinUI.UIComponent.Utils;

namespace SparkLauncher.WinUI.UIComponent.Converters {
    public partial class KeyToI18nConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is string key) {
                return LanguageUtil.GetI18n(key);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
