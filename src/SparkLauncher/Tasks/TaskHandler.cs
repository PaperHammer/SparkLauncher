using SparkLauncher.Common;
using SparkLauncher.Common.Utils.Mvvm;
using SparkLauncher.Tasks.TaskEventArgs;

namespace SparkLauncher.Tasks {
    public abstract class TaskHandler : ObservableObject {
        public event EventHandler<TaskPropertyChangedEventArgs>? LatestTaskMsgChanged;
        public event EventHandler<TaskMsg>? LatestTaskMsgAdd;

        private RuntimeStatus _status = RuntimeStatus.None;
        public RuntimeStatus Status {
            get => _status;
            protected set { _status = value; OnPropertyChanged(); }
        }

        public List<TaskMsg> TaskMsgs { get; set; } = [];
        public abstract string TaskName { get; }
        public TaskHandler? Next { get; private set; }
        public TaskRequest? Request { get; protected set; }

        public void LinkIn(TaskHandler task) {
            ArgumentNullException.ThrowIfNull(task);
            task.Next = this.Next;
            this.Next = task;
        }

        protected TaskMsg AddMsg(TaskMsgType type, string content) {
            TaskMsg msg = new() {
                MsgType = type,
                DateTime = DateTime.Now.ToString(),
                Content = content,
            };
            TaskMsgs.Add(msg);
            LatestTaskMsgAdd?.Invoke(this, msg);

            return msg;
        }

        protected void UpdateLatestMsg(string content) {
            TaskMsgs[^1].Content = content;
            LatestTaskMsgChanged?.Invoke(this, new() { Msg = TaskMsgs[^1] });
        }

        public abstract void Handle();
        public abstract Task HandleAsync();
    }

    public class TaskRequest {
        public bool IgnoreError { get; set; }
    }

    public class TaskMsg {
        public TaskMsgType MsgType { get; set; }
        public string DateTime { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
