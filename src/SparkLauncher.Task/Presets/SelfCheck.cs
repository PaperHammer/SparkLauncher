using System.Security.Cryptography;
using SparkLauncher.Task;

namespace SparkLauncher.Task.Presets {
    public class SelfCheck : TaskHandler {
        public override void Handle(Dictionary<string, object> pars) {
            try {
                string dllPath = @"path\to\your\dllfile.dll";
                string embeddedPublicKey = (string)pars["embeddedPublicKey"]; // public key in PEM format
                byte[] embeddedSignature = (byte[])pars["embeddedSignature"]; // signature, pre-generated and embedded

                bool isValid = VerifyFileSignature(dllPath, embeddedSignature, embeddedPublicKey);
                if (!isValid) {
                    Console.WriteLine("DLL 文件签名验证失败，可能已被篡改。");
                    // 可以选择退出应用或采取其他安全措施
                    return;
                }

                Console.WriteLine("DLL 文件签名验证成功，继续执行...");
                // 加载和使用 DLL
            }
            catch (Exception ex) {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }

        public static bool VerifyFileSignature(string filePath, byte[] signature, string publicKeyPem) {
            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem.ToCharArray());
            byte[] fileBytes = File.ReadAllBytes(filePath);
            byte[] hash = SHA256.HashData(fileBytes);

            try {
                return rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch (CryptographicException) {
                // If the signature is invalid or malformed, verification will throw an exception.
                return false;
            }
        }
    }
}
