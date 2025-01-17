namespace SparkLauncher.Common {
    public enum RuntimeStatus {
        None,
        Pending,
        Succeeded,
        Failed
    }

    public enum CommandType {
        Generate,
        Sign,
        VerifySign
    }

    public enum TaskMsgType {
        Info,
        Success,
        Warn,
        Error,
    }
}
