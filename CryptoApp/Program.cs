using CryptoLibrary;
using System;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace CryptoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                EncryptText();
                EnctyptTxtFile();
                EncryptOtherFile();             
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError occured: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void EncryptText()
        {
            Console.WriteLine("-------------------------Text Encryption-------------------------\n");

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
        }

        static void EnctyptTxtFile()
        {
            Console.WriteLine("\n-------------------------File Encryption-------------------------\n");

            FileEncoder fileEncoder = new FileEncoder();
            fileEncoder.EncryptFile(@"Files\SampleFile.txt", @"Files\SampleFileEncrypted.txt");
            fileEncoder.DecryptFile(@"Files\SampleFileEncrypted.txt", @"Files\SampleFileDecrypted.txt");
        }

        static void EncryptOtherFile()
        {
            Console.WriteLine("\n-------------------------Other Files----------------------------\n");

            FileEncoder fileEncoder = new FileEncoder();
            string inputFile = @"Files\pollub.png";
            string encryptedFile = @"Files\pollubEncrypted.png";
            string decryptedFile = @"Files\pollubDecrypted.png";
            fileEncoder.EncryptFile(inputFile, encryptedFile);
            fileEncoder.DecryptFile(encryptedFile, decryptedFile);

            string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            string query = "INSERT INTO [dbo].[Crypto] VALUES (@EncryptedData);";
            string encryptedFileContent = File.ReadAllText(encryptedFile);

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("EncryptedData", System.Data.SqlDbType.NVarChar).Value = encryptedFileContent;
                    connection.Open();
                    command.ExecuteNonQuery();

                    Console.WriteLine("Encrypted file has been saved to database successfully.");
                }
            }
        }
    }
}
