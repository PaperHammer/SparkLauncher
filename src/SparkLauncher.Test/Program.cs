using System.Security.Cryptography;
using SparkLauncher.Security;

namespace SparkLauncher.Test {
    internal class Program {
        static void Main(string[] args) {
            var generate = new GenerateKeyPair();
            var generateResponse = (GenerateKeyResponse)generate.Execute(new GenerateKeyParams() {
                KeyOutputDir = "C:\\Users\\PaperHammer\\Desktop",
            });
            Console.WriteLine("GenerateKeyPair Done");

            string[] filePaths = ["C:\\Users\\PaperHammer\\Desktop\\amla_! - 副本.png"];
            var sign = new Sign();
            var signResponse = (SignResponse)sign.Execute(new SignParams() {
                FilePaths = filePaths,
                PrivateKeyFilePath = generateResponse.PrivateKeyFilePath,
                SignatureOutputDir = "C:\\Users\\PaperHammer\\Desktop"
            });
            Console.WriteLine("Sign Done");

            foreach (var signData in signResponse.SignDatas) {
                bool verifyRes = VerifyFileSignature(signData.FilePath, signData.Signature, generateResponse.PublicKeyPem);
                if (!verifyRes) {
                    Console.WriteLine($"Verify failed: ${signData.FilePath}");
                }
            }
            Console.WriteLine("Verify Done");

            Console.ReadLine();
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
