using System.IO;
using System.Security.Cryptography;
using System;
using System.Text;

namespace CryptoLibrary
{
    public class FileEncoder
    {
        private byte[] key;
        private byte[] initializationVector;

        public void EncryptFile(string inputFilePath, string outputFilePath)
        {
            using (Aes aes = Aes.Create())
            {
                this.key = aes.Key;
                this.initializationVector = aes.IV;

                using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (FileStream inputFileStream = new FileStream(inputFilePath, FileMode.Open))
                            {
                                int data;
                                while ((data = inputFileStream.ReadByte()) != -1)
                                {
                                    cryptoStream.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"File \"{inputFilePath}\" successfully encrypted.");
            }
        }

        public void DecryptFile(string inputFilePath, string outputFilePath)
        {
            using(Aes aes = Aes.Create())
            {
                aes.Key = this.key;
                aes.IV = this.initializationVector;

                using(FileStream inputFileStream = new FileStream(inputFilePath, FileMode.Open))
                using(FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
                {
                    using(ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        using(CryptoStream cryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
                        {
                            int data;
                            while((data = cryptoStream.ReadByte()) != -1)
                            {
                                outputFileStream.WriteByte((byte)data);
                            }
                        }
                    }
                }

                Console.WriteLine($"File \"{inputFilePath}\" successfully decrypted.");
            }
        }
    }
}
