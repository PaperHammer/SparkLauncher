namespace SparkLauncher.Common.Core {
    public abstract class ResponsibilityHandler {
        public void LinkIn(ResponsibilityHandler next) => _next = next;
        public ResponsibilityHandler? GetNextHandler() => _next;
        public abstract void Handle();

        private ResponsibilityHandler? _next;
    }
}
