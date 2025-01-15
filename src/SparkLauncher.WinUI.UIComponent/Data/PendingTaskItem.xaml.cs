using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SparkLauncher.Common;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SparkLauncher.WinUI.UIComponent.Data {
    public sealed partial class PendingTaskItem : UserControl {
        public PendingTaskItem() {
            this.InitializeComponent();
        }

        public string TaskName {
            get { return (string)GetValue(TaskNameProperty); }
            set { SetValue(TaskNameProperty, value); }
        }
        public static readonly DependencyProperty TaskNameProperty =
            DependencyProperty.Register("TaskName", typeof(string), typeof(PendingTaskItem), new PropertyMetadata(string.Empty));

        public RuntimeStatus Status {
            get { return (RuntimeStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(RuntimeStatus), typeof(PendingTaskItem), new PropertyMetadata(RuntimeStatus.None, RuntimeStatusChange));

        internal bool IsStarted {
            get { return (bool)GetValue(IsStartedProperty); }
            set { SetValue(IsStartedProperty, value); }
        }
        public static readonly DependencyProperty IsStartedProperty =
            DependencyProperty.Register("IsStarted", typeof(bool), typeof(PendingTaskItem), new PropertyMetadata(false));

        private static void RuntimeStatusChange(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var instance = d as PendingTaskItem;
            if ((RuntimeStatus)e.NewValue == RuntimeStatus.Pending) {
                instance.IsStarted = true;
            }
        }
    }
}
