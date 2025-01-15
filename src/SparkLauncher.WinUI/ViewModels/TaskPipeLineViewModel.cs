using System.Collections.ObjectModel;
using Microsoft.UI.Dispatching;
using SparkLauncher.Common.Utils.Mvvm;
using SparkLauncher.Tasks;

namespace SparkLauncher.WinUI.ViewModels {
    internal partial class TaskPipeLineViewModel : ObservableObject {
        public ObservableCollection<TaskHandler> PendingTaskItems { get; set; } = [];

        private string _taskMsg = string.Empty;
        public string TaskMsg {
            get { return _taskMsg; }
            set { _taskMsg = value; OnPropertyChanged(); }
        }

        private int _selectedTaskIdx = -1;
        public int SelectedTaskIdx {
            get { return _selectedTaskIdx; }
            set {
                if (value > -1) {
                    _selectedTaskIdx = value;
                    TaskMsg = PendingTaskItems[value].TaskMsg;
                    OnPropertyChanged();
                }
            }
        }

        public TaskPipeLineViewModel() {
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread() ?? DispatcherQueueController.CreateOnCurrentThread().DispatcherQueue;
            TaskMain.OnForward += TaskMain_OnForward;

            InitUI();
        }

        private void TaskMain_OnForward(object sender, Tasks.TaskEventArgs.ForwardEventArgs e) {
            _dispatcherQueue.TryEnqueue(() => {
                SelectedTaskIdx = e.Index;
            });
        }

        private void InitUI() {
            foreach (var task in TaskMain.TasksInLine) {
                PendingTaskItems.Add(task);
                SubscribeToTaskEvents(task);
            }
        }

        internal async void Run() {
            await TaskMain.RunAsync();
        }

        internal void Cleanup() {
            foreach (var task in PendingTaskItems) {
                UnsubscribeFromTaskEvents(task);
            }
        }

        private void SubscribeToTaskEvents(TaskHandler task) {
            task.TaskMsgChanged += Task_TaskMsgChanged;
        }

        private void UnsubscribeFromTaskEvents(TaskHandler task) {
            task.TaskMsgChanged -= Task_TaskMsgChanged;
        }

        private void Task_TaskMsgChanged(object sender, Tasks.TaskEventArgs.TaskPropertyChangedEventArgs e) {
            _dispatcherQueue.TryEnqueue(() => {
                TaskMsg = e.TaskMsg;
            });
        }

        private readonly DispatcherQueue _dispatcherQueue;
    }
}
