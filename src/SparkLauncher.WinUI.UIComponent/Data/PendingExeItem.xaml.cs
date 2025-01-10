using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using static SparkLauncher.Models.Enums;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SparkLauncher.WinUI.UIComponent.Data {
    public sealed partial class PendingExeItem : UserControl {
        public PendingExeItem() {
            this.InitializeComponent();
        }

        public string TaskName {
            get { return (string)GetValue(TaskNameProperty); }
            set { SetValue(TaskNameProperty, value); }
        }
        public static readonly DependencyProperty TaskNameProperty =
            DependencyProperty.Register("TaskName", typeof(string), typeof(PendingExeItem), new PropertyMetadata(string.Empty));

        public bool IsRunningOrEnded {
            get { return (bool)GetValue(IsRunningOrEndedProperty); }
            set { SetValue(IsRunningOrEndedProperty, value); }
        }
        public static readonly DependencyProperty IsRunningOrEndedProperty =
            DependencyProperty.Register("IsRunningOrEnded", typeof(bool), typeof(PendingExeItem), new PropertyMetadata(false));

        public RuntimeStatus Status {
            get { return (RuntimeStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(RuntimeStatus), typeof(PendingExeItem), new PropertyMetadata(RuntimeStatus.None));

        public bool IsIndeterminate {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }
        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(PendingExeItem), new PropertyMetadata(true));

        public double Progress {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(PendingExeItem), new PropertyMetadata(0, ProgressChanged));

        private static void ProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var instance = d as PendingExeItem;
            instance._progressPercent = (double)e.NewValue + "%";
        }

        internal string _progressPercent = string.Empty;
    }
}
