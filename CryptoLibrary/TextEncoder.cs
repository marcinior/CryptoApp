using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLibrary
{
    public class TextEncoder
    {
        private byte[] key;
        private byte[] initializationVector;

        public byte[] Encrypt(string textToEncrypt, string fileName)
        {
            if (string.IsNullOrEmpty(textToEncrypt))
                throw new ArgumentNullException(nameof(textToEncrypt));

            using (Aes aes = Aes.Create())
            {
                key = aes.Key;
                initializationVector = aes.IV;
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {

                    using (MemoryStream encryptMemoryStream = new MemoryStream())
                    {
                        using (CryptoStream encryptCryptoStream = new CryptoStream(encryptMemoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(encryptCryptoStream))
                            {
                                swEncrypt.Write(textToEncrypt);
                            }

                            byte[] encryptedBytes = encryptMemoryStream.ToArray();
                            SaveEncryptedTextToFile(GetString(encryptedBytes), fileName);
                            return encryptedBytes;
                        }
                    }
                }
            }
        }

        private void SaveEncryptedTextToFile(string encryptedText, string fileName)
        {
            File.WriteAllText(@"Files\" + fileName, encryptedText);
            Console.WriteLine($"Encrypted text saved to file with name \"{fileName}\"");
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentNullException(nameof(encryptedText));

            byte[] encryptedBytes = GetBytes(encryptedText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = this.key;
                aes.IV = this.initializationVector;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    using (MemoryStream decryptMemoryStream = new MemoryStream(encryptedBytes))
                    {
                        using (CryptoStream decryptCryptoStream = new CryptoStream(decryptMemoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(decryptCryptoStream))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }

        }

        public string GetString(byte[] bytes) => bytes != null ? Encoding.Default.GetString(bytes) : null;

        public byte[] GetBytes(string text) => string.IsNullOrEmpty(text) ? null : Encoding.Default.GetBytes(text);

        public void DumpBytes(string title, byte[] bytes)
        {
            if (bytes == null)
                return;

            Console.Write(title + "\t");
            foreach (byte b in bytes)
            {
                Console.Write("{0:X} ", b);
            }
            Console.WriteLine();
        }
    }
}