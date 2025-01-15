using SparkLauncher.Common;
using SparkLauncher.Common.Utils.Mvvm;
using SparkLauncher.Tasks.TaskEventArgs;

namespace SparkLauncher.Tasks {
    public abstract class TaskHandler : ObservableObject {
        public event EventHandler<TaskPropertyChangedEventArgs>? TaskMsgChanged;

        private string _taskMsg = string.Empty;
        public string TaskMsg {
            get => _taskMsg;
            protected set { _taskMsg = value; TaskMsgChanged?.Invoke(this, new() { TaskMsg = TaskMsg }); }
        }

        private RuntimeStatus _status = RuntimeStatus.None;
        public RuntimeStatus Status {
            get => _status;
            protected set { _status = value; OnPropertyChanged(); }
        }

        public abstract string TaskName { get; }
        public TaskHandler? Next { get; private set; }
        public TaskRequest? Request { get; protected set; }

        public void LinkIn(TaskHandler task) {
            ArgumentNullException.ThrowIfNull(task);
            task.Next = this.Next;
            this.Next = task;
        }

        public abstract void Handle();
        public abstract Task HandleAsync();
    }

    public class TaskRequest {
        public bool IgnoreFailed { get; set; }
    }
}
