using SparkLauncher.Common;
using SparkLauncher.Security;

namespace SparkLauncher.Test {
    internal class Program {
        static async Task Main(string[] args) {
#if RELEASE
            string targetDir = args[0];
            var generateResponse = await SecurityMain.RunAsync(CommandType.Generate, new GenerateKeyParams() {
                KeyOutputDir = targetDir,
            }) as GenerateKeyResponse;
            Console.WriteLine("GenerateKeyPair Done");

            string[] filePaths = Directory.GetFiles(targetDir, PATTERN, SearchOption.TopDirectoryOnly);
            var signResponse = await SecurityMain.RunAsync(CommandType.Sign, new SignParams() {
                FilePaths = filePaths,
                PrivateKeyFilePath = generateResponse.PrivateKeyFilePath,
                SignatureOutputDir = targetDir,
            }) as SignResponse;
            Console.WriteLine("Sign Done");

            foreach (var signData in signResponse.SignDatas) {
                var verifySignRes = await SecurityMain.RunAsync(CommandType.VerifySign, new VerifySignParams() {
                    FilePath = signData.FilePath,
                    Signature = signData.Signature,
                    PublicKeyPem =  generateResponse.PublicKeyPem,
                }) as VerifySignResponse;
                if (!verifySignRes.IsVerifid) {
                    Console.WriteLine($"Verify failed: ${signData.FilePath}");
                }
            }
            Console.WriteLine("Verify Done");
#endif
        }

        private const string PATTERN = "SparkLauncher*.dll";
    }
}
