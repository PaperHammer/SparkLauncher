using SparkLauncher.Common;
using SparkLauncher.Common.Core;
using SparkLauncher.Security;
using SparkLauncher.Tasks.TaskEventArgs;

namespace SparkLauncher.Tasks.Presets {
    public class SelfCheck : TaskHandler {
        public override string TaskName => Constants.I18n.Tasks_SelfCheck;

        public SelfCheck(TaskRequest? request = null) {
            Request = request ?? new();
        }

        public override void Handle() {
            HandleAsync().GetAwaiter().GetResult();
        }

        public override async Task HandleAsync() {
            try {
                Status = RuntimeStatus.Pending;

                Request ??= new SelfCheckRequest();
                if (Request is not SelfCheckRequest privateArgs) throw new ArgumentException();
                
                TaskMsg += $"{DateTime.Now}: SelfCheck running...\n";
                foreach (var signData in privateArgs.SignDatas) {
                    if (await SecurityMain.RunAsync(CommandType.VerifySign, new VerifySignParams() {
                        FilePath = signData.FilePath,
                        Signature = signData.Signature,
                        PublicKeyPem = File.ReadAllText(privateArgs.PublicKeyFilePath),
                    }) is not VerifySignResponse verifySignRes || !verifySignRes.IsVerifid) {
                        throw new VerifySignException($"SelfCheck failed for the file: ${signData.FilePath}", signData.FilePath);
                    }
                    TaskMsg += $"{DateTime.Now}: file '{signData.FilePath}' checked.\n";
                }
                TaskMsg += $"{DateTime.Now}: SelfCheck done.\n";
            }
            catch (Exception ex) {
                Status = RuntimeStatus.Failed;

                TaskMsg += $"{DateTime.Now}: Exception-{ex.Message}\n";
                if (Request == null || !Request.IgnoreFailed) {
                    throw;
                }
                TaskMsg += $"{DateTime.Now}: SelfCheck IgnoreFailed.\n";
            }
            finally {
                // 避免覆盖 Failed
                if (Status == RuntimeStatus.Pending) {
                    Status = RuntimeStatus.Succeeded;
                }
            }
        }
    }

    public class SelfCheckRequest : TaskRequest {
        public string PublicKeyFilePath { get; set; } = string.Empty;
        public List<SignData> SignDatas { get; set; } = [];
    }
}
