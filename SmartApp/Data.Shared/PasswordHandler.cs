using Microsoft.Extensions.Options;
using Shared.Models.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Data.Shared
{
    public class PasswordHandler
    {
        private readonly string _key;

        public PasswordHandler(IOptions<SecurityData> securityData)
        {
            _key = securityData.Value.EncriptionKey;
        }


        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetEncriptedKey();
                aes.IV = new byte[16]; // Initialization vector (IV), can also be randomized for stronger security

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetEncriptedKey();
                aes.IV = new byte[16]; // Same IV used for encryption

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    var pswd = reader.ReadToEnd();

                    return pswd;
                }
            }
        }

        private byte[] GetEncriptedKey()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(_key));
            }
        }
    }
}
