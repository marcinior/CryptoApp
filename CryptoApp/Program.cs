using CryptoLibrary;
using System;
using System.IO;

namespace CryptoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("-------------------------Text Encryption-------------------------");

                TextEncoder textEncoder = new TextEncoder();
                string textToEncrypt = "Marcin";
                Console.WriteLine($"Text to encrypt:\t{textToEncrypt}");
                textEncoder.DumpBytes("Text to Encrypt in bytes: ", textEncoder.GetBytes(textToEncrypt));
                string fileName = "mb";

                byte[] encryptedBytes = textEncoder.Encrypt(textToEncrypt, fileName);
                string encryptedText = textEncoder.GetString(encryptedBytes);
                Console.WriteLine($"Encrypted text:\t{encryptedText}");
                textEncoder.DumpBytes("Encrypted text in bytes:", encryptedBytes);

                string encrytedTextFromFile = File.ReadAllText(@"Files\" + fileName);
                Console.WriteLine($"Decrypted text: {textEncoder.Decrypt(encrytedTextFromFile)}");

                Console.WriteLine("-------------------------File Encryption-------------------------");

                FileEncoder fileEncoder = new FileEncoder();
                fileEncoder.EncryptFile(@"Files\SampleFile.txt", @"Files\SampleFileEncrypted.txt");
                fileEncoder.DecryptFile(@"Files\SampleFileEncrypted.txt", @"Files\SampleFileDecrypted.txt");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError occured: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
