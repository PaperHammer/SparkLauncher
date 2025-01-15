using SparkLauncher.Common;

namespace SparkLauncher.Security.Interfaces {
    public interface ISecurity {
        bool CanHandle(CommandType commandType);
        SecurityResponse Execute(SecurityTaskArgs args);
        Task<SecurityResponse> ExecuteAsync(SecurityTaskArgs args);
    }

    public abstract class SecurityResponse { }

    public abstract class SecurityTaskArgs { }
}
