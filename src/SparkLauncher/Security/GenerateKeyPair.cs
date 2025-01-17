using System.Security.Cryptography;
using SparkLauncher.Common;
using SparkLauncher.Common.Models;
using SparkLauncher.Security.Interfaces;

namespace SparkLauncher.Security {
    internal class GenerateKeyPair : ISecurity {
        public bool CanHandle(CommandType commandType) => commandType == CommandType.Generate;

        public SecurityResponse Execute(SecurityTaskArgs args) {
            return ExecuteAsync(args).GetAwaiter().GetResult();
        }

        public async Task<SecurityResponse> ExecuteAsync(SecurityTaskArgs args) {
            return await GenerateAsync(args);
        }

        // 生成密钥对
        private static async Task<SecurityResponse> GenerateAsync(SecurityTaskArgs args) {
            var privateArgs = args as GenerateKeyParams ?? throw new ParamsException("typeof SecurityTaskArgs error");            
            if (string.IsNullOrEmpty(privateArgs.KeyOutputDir)) {
                throw new ParamsException("<KeyOutputDir> is essential");
            }
            
            string output = Path.Combine(privateArgs.KeyOutputDir, "keys");
            if (!Directory.Exists(output)) {
                Directory.CreateDirectory(output);
            }
            string privateKeyPath = Path.Combine(output, Constants.Filds.PrivateKeyFileName);
            string publicKeyPath = Path.Combine(output, Constants.Filds.PublicKeyFileName);

            using RSA rsa = RSA.Create();
            string privateKeyPem = ExportRSAPrivateKeyPem(rsa);
            await File.WriteAllTextAsync(privateKeyPath, privateKeyPem);
            string publicKeyPem = ExportRSAPublicKeyPem(rsa);
            await File.WriteAllTextAsync(publicKeyPath, publicKeyPem);

            return new GenerateKeyResponse(privateKeyPem, publicKeyPem, privateKeyPath, publicKeyPath);
        }

        // 导出私钥为 PEM 格式
        private static string ExportRSAPrivateKeyPem(RSA rsa) {
            byte[] pkcs8 = rsa.ExportPkcs8PrivateKey();
            return "-----BEGIN PRIVATE KEY-----\n" + Convert.ToBase64String(pkcs8).Insert(64, "\n") + "\n-----END PRIVATE KEY-----";
        }

        // 导出公钥为 PEM 格式
        private static string ExportRSAPublicKeyPem(RSA rsa) {
            byte[] spki = rsa.ExportSubjectPublicKeyInfo();
            return "-----BEGIN PUBLIC KEY-----\n" + Convert.ToBase64String(spki).Insert(64, "\n") + "\n-----END PUBLIC KEY-----";
        }
    }

    public class GenerateKeyResponse(string privateKeyPem, string publicKeyPem, string privateKeyPath, string publicKeyPath) : SecurityResponse {
        public string PrivateKeyPem { get; } = privateKeyPem;
        public string PublicKeyPem { get; } = publicKeyPem;
        public string PrivateKeyFilePath { get; } = privateKeyPath;
        public string PublicKeyFilePath { get; } = publicKeyPath;
    }

    public class GenerateKeyParams : SecurityTaskArgs {
        public string KeyOutputDir { get; set; } = string.Empty;
    }
}
