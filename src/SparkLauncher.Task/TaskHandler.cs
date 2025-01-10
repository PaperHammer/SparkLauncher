namespace SparkLauncher.Task {
    public abstract class TaskHandler {
        public void LinkIn(TaskHandler next) => _next = next;
        public TaskHandler? GetNextHandler() => _next;
        public abstract void Handle(Dictionary<string, object> pars);

        private TaskHandler? _next;
    }
}
