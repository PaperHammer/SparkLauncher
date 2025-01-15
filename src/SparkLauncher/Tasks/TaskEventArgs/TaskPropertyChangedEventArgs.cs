using SparkLauncher.Common;

namespace SparkLauncher.Tasks.TaskEventArgs {
    public class TaskPropertyChangedEventArgs : EventArgs {
        public string TaskMsg { get; set; } = string.Empty;
        public RuntimeStatus Status { get; set; }
    }
}
