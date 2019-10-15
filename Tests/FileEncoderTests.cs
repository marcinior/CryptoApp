using CryptoLibrary;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;

namespace Tests
{
    class FileEncoderTests
    {
        private static FileEncoder fileEncoder;

        static FileEncoderTests()
        {
            fileEncoder = new FileEncoder();
        }

        [Test]
        public void EncryptFile_ExpectedFilesDoesNotExists_ExceptionShouldThrow()
        {
            Action encrypt = () => fileEncoder.EncryptFile("sddsfsfd.txt", "aaaaa.txt");
            encrypt
                .Should()
                .Throw<FileNotFoundException>();
        }

        [Test]
        [TestCase(null, null)]
        [TestCase(null, "fdfd")]
        [TestCase(@"Files\sample.txt", null)]
        public void EncryptFile_FilenameIsNull_ExceptionShouldThrow(string inputFile, string outputFile)
        {
            Action encrypt = () => fileEncoder.EncryptFile(inputFile, outputFile);
            encrypt
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        public void DecryptFile_InputFileDoesNotExists_ExceptionShouldTrow()
        {
            fileEncoder.EncryptFile(@"Files\sample.txt", @"Files\sampleEnc.txt");
            Action decrypt = () => fileEncoder.DecryptFile("fdsf.txt", "ssss.txt");
            decrypt
                .Should()
                .Throw<FileNotFoundException>();
        }

        [Test]
        [TestCase(null, null)]
        [TestCase(null, "fdfd")]
        [TestCase(@"Files\sampleEnc.txt", null)]
        public void DecryptFile_FilenameIsNull_ExceptionShouldThrow(string inputFile, string outputFile)
        {
            fileEncoder.EncryptFile(@"Files\sample.txt", @"Files\sampleEnc.txt");
            Action decrypt = () => fileEncoder.DecryptFile(inputFile, outputFile);
            decrypt
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        public void EncodeAndDecodeFile_Success()
        {
            string fileContentBeforeEncrypting = File.ReadAllText(@"Files\sample.txt");

            fileEncoder.EncryptFile(@"Files\sample.txt", @"Files\sampleEnc.txt");
            fileEncoder.DecryptFile(@"Files\sampleEnc.txt", @"Files\sampleDec.txt");

            string fileContentAfterDecoding = File.ReadAllText(@"Files\sampleDec.txt");
            Assert.AreEqual(fileContentBeforeEncrypting, fileContentAfterDecoding);
        }
    }
}
