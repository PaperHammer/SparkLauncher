using SparkLauncher.Common;
using SparkLauncher.Security.Interfaces;

namespace SparkLauncher.Security {
    public static class SecurityMain {
        public static SecurityResponse Run(CommandType commandType, SecurityTaskArgs args) {
            return RunAsync(commandType, args).GetAwaiter().GetResult();
        }

        public static async Task<SecurityResponse> RunAsync(CommandType commandType, SecurityTaskArgs args) {
            foreach (var task in _securityTasks) {
                if (task.CanHandle(commandType)) {
                    return await task.ExecuteAsync(args);
                }
            }

            return null;
        }

        private static readonly ISecurity[] _securityTasks = [
            new GenerateKeyPair(),
            new Sign(),
            new VerifySign(),
        ];
    }
}
