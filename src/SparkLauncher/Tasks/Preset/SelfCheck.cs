using SparkLauncher.Common;
using SparkLauncher.Common.Core;
using SparkLauncher.Security;

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
                await Task.Delay(1000);

                Request ??= new SelfCheckRequest();
                if (Request is not SelfCheckRequest privateArgs) throw new ArgumentException();
                AddMsg(TaskMsgType.Info, "SelfCheck running...");

                string pulicKeyFilePath = Path.Combine(privateArgs.TargetDirectory, "keys", "public_key.txt");
                List<SignData> signDatas = await GetSignDatasAsync(privateArgs.TargetDirectory);

                foreach (var signData in signDatas) {
                    if (await SecurityMain.RunAsync(CommandType.VerifySign, new VerifySignParams() {
                        FilePath = signData.FilePath,
                        Signature = signData.Signature,
                        PublicKeyPem = File.ReadAllText(pulicKeyFilePath),
                    }) is not VerifySignResponse verifySignRes || !verifySignRes.IsVerifid) {
                        throw new VerifySignException($"SelfCheck failed for the file: ${signData.FilePath}", signData.FilePath);
                    }
                    AddMsg(TaskMsgType.Success, $"File '{signData.FilePath}' checked.");
                }
                AddMsg(TaskMsgType.Info, "SelfCheck done.");
            }
            catch (Exception ex) {
                Status = RuntimeStatus.Failed;
                AddMsg(TaskMsgType.Error, $"Exception-{ex.Message}");
                if (Request == null || !Request.IgnoreError) {
                    throw;
                }
                AddMsg(TaskMsgType.Warn, $"SelfCheck IgnoreError.");
            }
            finally {
                // 避免覆盖 Failed
                if (Status == RuntimeStatus.Pending) {
                    Status = RuntimeStatus.Succeeded;
                }
                //await Task.Delay(500);
            }
        }

        private async Task<List<SignData>> GetSignDatasAsync(string directory) {
            AddMsg(TaskMsgType.Info, "Reading datas...");

            List<SignData> signDatas = [];

            string[] filePaths = Directory.GetFiles(directory, "SparkLauncher*.dll");
            string[] sigFilePaths = Directory.GetFiles(Path.Combine(directory, "keys"), "SparkLauncher*.sig");
            foreach (string sigFilePath in sigFilePaths) {
                string targetFileName = Path.GetFileNameWithoutExtension(sigFilePath);
                string? filePath = filePaths.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x) == targetFileName)
                    ?? throw new SelfCheckException($"The target file does not exist, and the module may be incomplete. {targetFileName}");

                byte[] sign = await File.ReadAllBytesAsync(sigFilePath);
                signDatas.Add(new(sign, filePath, sigFilePath));
            }

            return signDatas;
        }
    }

    public class SelfCheckRequest : TaskRequest {
        public string TargetDirectory { get; set; } = string.Empty;
    }
}
