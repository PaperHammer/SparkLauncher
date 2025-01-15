namespace SparkLauncher.Tasks.TaskEventArgs {
    public class ForwardEventArgs(TaskHandler handler, int index) : EventArgs {
        public TaskHandler Handler { get; } = handler;
        public int Index { get; } = index;
    }
}
