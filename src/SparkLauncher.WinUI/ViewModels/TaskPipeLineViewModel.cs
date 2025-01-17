using System.Collections.ObjectModel;
using Microsoft.UI.Dispatching;
using SparkLauncher.Common.Utils.Mvvm;
using SparkLauncher.Tasks;

namespace SparkLauncher.WinUI.ViewModels {
    internal partial class TaskPipeLineViewModel : ObservableObject {
        public ObservableCollection<TaskHandler> PendingTaskItems { get; set; } = [];
        public ObservableList<TaskMsg> Blocks { get; set; } = [];

        private int _selectedTaskIdx = -1;
        public int SelectedTaskIdx {
            get { return _selectedTaskIdx; }
            set {
                if (value > -1) {
                    _selectedTaskIdx = value;
                    Blocks.Clear();
                    Blocks.AddRange(PendingTaskItems[value].TaskMsgs);
                    OnPropertyChanged();
                }
            }
        }

        public TaskPipeLineViewModel() {
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread() ?? DispatcherQueueController.CreateOnCurrentThread().DispatcherQueue;
            TaskMain.OnForward += TaskMain_OnForward;

            InitUI();
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

        private void TaskMain_OnForward(object sender, Tasks.TaskEventArgs.ForwardEventArgs e) {
            _dispatcherQueue.TryEnqueue(() => {
                SelectedTaskIdx = e.Index;
            });
        }

        private void Task_LatestTaskMsgChanged(object sender, Tasks.TaskEventArgs.TaskPropertyChangedEventArgs e) {
            _dispatcherQueue.TryEnqueue(() => {
                Blocks[^1] = e.Msg;
            });
        }

        private void Task_LatestTaskMsgAdd(object sender, TaskMsg e) {
            _dispatcherQueue.TryEnqueue(() => {
                Blocks.Add(e);
            });
        }

        private void SubscribeToTaskEvents(TaskHandler task) {
            task.LatestTaskMsgChanged += Task_LatestTaskMsgChanged;
            task.LatestTaskMsgAdd += Task_LatestTaskMsgAdd;
        }

        private void UnsubscribeFromTaskEvents(TaskHandler task) {
            task.LatestTaskMsgChanged -= Task_LatestTaskMsgChanged;
            task.LatestTaskMsgAdd -= Task_LatestTaskMsgAdd;
        }

        private readonly DispatcherQueue _dispatcherQueue;
    }
}
