using System.Security.Cryptography;
using SparkLauncher.Common;
using SparkLauncher.Common.Models;
using SparkLauncher.Security.Interfaces;

namespace SparkLauncher.Security {
    internal class VerifySign : ISecurity {
        public bool CanHandle(CommandType commandType) => commandType == CommandType.VerifySign;

        public SecurityResponse Execute(SecurityTaskArgs args) {
            return ExecuteAsync(args).GetAwaiter().GetResult();
        }

        public async Task<SecurityResponse> ExecuteAsync(SecurityTaskArgs args) {
            return await VerifyFileSignatureAsync(args);
        }

        private static async Task<SecurityResponse> VerifyFileSignatureAsync(SecurityTaskArgs args) {
            var privateArgs = args as VerifySignParams ?? throw new ParamsException("typeof SecurityTaskArgs error");
            VerifySignResponse resp = new(false);

            try {
                using RSA rsa = RSA.Create();
                rsa.ImportFromPem(privateArgs.PublicKeyPem.ToCharArray());
                byte[] fileBytes = await File.ReadAllBytesAsync(privateArgs.FilePath);
                byte[] hash = SHA256.HashData(fileBytes);
                return new VerifySignResponse(rsa.VerifyHash(hash, privateArgs.Signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }
            catch (CryptographicException) {
                // If the signature is invalid or malformed, verification will throw an exception.
                return resp;
            }
        }
    }

    public class VerifySignResponse(bool isVerifid) : SecurityResponse {
        public bool IsVerifid { get; } = isVerifid;
    }

    public class VerifySignParams : SecurityTaskArgs {
        public string FilePath { get; set; } = string.Empty;
        public byte[] Signature { get; set; } = [];
        public string PublicKeyPem { get; set; } = string.Empty;
    }
}
