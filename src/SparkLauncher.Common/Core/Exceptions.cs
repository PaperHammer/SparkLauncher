namespace SparkLauncher.Common.Core {
    public class ParamsException : Exception {
        public ParamsException() {
        }

        public ParamsException(string message)
            : base(message) {
        }

        public ParamsException(string message, Exception inner)
            : base(message, inner) {
        }
    }

    public class SignatureException : Exception {
        public IEnumerable<SignErrorData> ErrorDatas { get; } = [];

        public SignatureException() {
        }

        public SignatureException(string message)
            : base(message) {
        }
        
        public SignatureException(string message, IEnumerable<SignErrorData> errorDatas)
            : base(message) {
            ErrorDatas = errorDatas;
        }

        public SignatureException(string message, Exception inner)
            : base(message, inner) {
        }

        public class SignErrorData {
            public string FilePath { get; set; } = string.Empty;
            public string ErrMsg { get; set; } = string.Empty;
        }
    }
}
