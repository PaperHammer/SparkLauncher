using SparkLauncher.Tasks.Presets;
using SparkLauncher.Tasks.TaskEventArgs;

namespace SparkLauncher.Tasks {
    public static class TaskMain {
        public static event EventHandler<ForwardEventArgs>? OnForward;

        public static List<TaskHandler> TasksInLine { get; } = [
            new SelfCheck(new SelfCheckRequest(){
                IgnoreError = true,
                TargetDirectory = "D:\\Virtuals\\SparkLauncher\\src\\SparkLauncher.WinUI\\bin\\x64\\Release\\net8.0-windows10.0.19041.0\\win-x64"
            }),
            new CheckUpdate(),
            new SelfCheck(new SelfCheckRequest(){
                IgnoreError = true,
            }),
            new SelfCheck(new SelfCheckRequest(){
                IgnoreError = true,
            }),
            new SelfCheck(),
            new SelfCheck(),
            //new Verification(),
            //new CheckUpdate(),
            //new Download(),
            //new Callback(),
        ];

        public static async Task RunAsync() {
            try {
                await _tasksRun.WaitAsync(0);
                InitializeChain();
                var current = _chainHead;
                int idx = 0;
                while (current != null) {
                    try {
                        OnForward?.Invoke(null, new ForwardEventArgs(current, idx));
                        await current.HandleAsync();
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        break;
                    }
                    current = current.Next;
                    idx++;
                }
            }
            finally {
                _tasksRun.Release();
            }
        }

        private static void InitializeChain() {
            TaskHandler? last = null;
            foreach (var task in TasksInLine) {
                _chainHead ??= task;
                last?.LinkIn(task);
                last = task;
            }
        }

        private static TaskHandler? _chainHead;
        private static readonly SemaphoreSlim _tasksRun = new(1, 1);
    }
}
