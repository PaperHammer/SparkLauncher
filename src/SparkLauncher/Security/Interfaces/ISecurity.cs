using SparkLauncher.Common;

namespace SparkLauncher.Security.Interfaces {
    public interface ISecurity {
        bool CanHandle(CommandType commandType);
        SecurityResponse Execute(SecurityParams args);
        Task<SecurityResponse> ExecuteAsync(SecurityParams args);
    }

    public abstract class SecurityResponse {
        //public string PrivateKeyFilePath { get; set; } = string.Empty;
        //public string PublicKeyFilePath { get; set; } = string.Empty;
        //public string[] SignatureFilePaths { get; set; } = [];
    }

    public abstract class SecurityParams { }    
}
