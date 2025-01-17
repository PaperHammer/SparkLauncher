using System.Collections.Concurrent;
using System.Security.Cryptography;
using SparkLauncher.Common;
using SparkLauncher.Common.Models;
using SparkLauncher.Security.Interfaces;
using static SparkLauncher.Common.Models.SignatureException;

namespace SparkLauncher.Security
{
    internal class Sign : ISecurity {
        public bool CanHandle(CommandType commandType) => commandType == CommandType.Sign;

        public SecurityResponse Execute(SecurityTaskArgs args) {
            return ExecuteAsync(args).GetAwaiter().GetResult();
        }

        public async Task<SecurityResponse> ExecuteAsync(SecurityTaskArgs args) {
            return await SignFilesAsync(args);
        }

        private static async Task<SecurityResponse> SignFilesAsync(SecurityTaskArgs args) {
            var privateArgs = args as SignParams ?? throw new ParamsException("typeof SecurityTaskArgs error");
            if (privateArgs.FilePaths.Length == 0 || string.IsNullOrEmpty(privateArgs.PrivateKeyFilePath) || string.IsNullOrEmpty(privateArgs.SignatureOutputDir)) {
                throw new ParamsException("<file-path(s)> <private-key-path> <signature-output-path> are essential");
            }

            using var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText(privateArgs.PrivateKeyFilePath).ToCharArray());

            // Prepare a list to collect any errors that occur during parallel processing.
            ConcurrentBag<SignErrorData> errorDatas = [];
            ConcurrentBag<SignData> signedDatas = [];

            string output = Path.Combine(privateArgs.SignatureOutputDir, "keys");
            if (!Directory.Exists(output)) {
                Directory.CreateDirectory(output);
            }

            var signTasks = privateArgs.FilePaths.Select(async filePath => {
                try {
                    if (!File.Exists(filePath)) {
                        errorDatas.Add(new SignErrorData() {
                            FilePath = filePath,
                            ErrMsg = "File not found",
                        });
                        return;
                    }

                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    byte[] signature = await SignFileInternalAsync(filePath, rsa);

                    string signatureFilePath = await WriteSignatureToFileAsync(output, fileNameWithoutExtension, signature);
                    signedDatas.Add(new SignData(signature, filePath, signatureFilePath));
                }
                catch (Exception ex) {
                    errorDatas.Add(new SignErrorData() {
                        FilePath = filePath,
                        ErrMsg = $"Failed to process file: {ex.Message}",
                    });
                }
            }).ToList();

            await Task.WhenAll(signTasks);

            if (!errorDatas.IsEmpty) {
                throw new SignatureException("One or more files failed to sign.", errorDatas);
            }

            return new SignResponse([.. signedDatas]);
        }

        private static async Task<byte[]> SignFileInternalAsync(string filePath, RSA rsa) {
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            byte[] hash = SHA256.HashData(fileBytes);
            return rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        private static async Task<string> WriteSignatureToFileAsync(string signatureOutputPath, string fileNameWithoutExtension, byte[] signature) {
            // Create a unique filename for each signature to avoid overwriting.
            string signatureFilePath = Path.Combine(signatureOutputPath, $"{fileNameWithoutExtension}.sig");
            // Use asynchronous file write operation.
            await File.WriteAllBytesAsync(signatureFilePath, signature);

            return signatureFilePath;
        }
    }

    public class SignResponse(List<SignData> signDatas) : SecurityResponse {
        public List<SignData> SignDatas { get; } = signDatas;
    }

    public class SignData(byte[] signature, string filePath, string signaturePath) {
        public byte[] Signature { get; } = signature;
        public string FilePath { get; } = filePath;
        public string SignatureFilePath { get; } = signaturePath;
    }

    public class SignParams : SecurityTaskArgs {
        public string[] FilePaths { get; set; } = [];
        public string PrivateKeyFilePath { get; set; } = string.Empty;
        public string SignatureOutputDir { get; set; } = string.Empty;
    }
}
