using System;
using CryptoLibrary;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    class TextEncodingTests
    {
        private byte[] key;
        private byte[] iv;

        [TestFixtureSetUp]
        public void TestSetup()
        {
            key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        }

        [Test]
        public void TextEncrypt_InputTextNull_ExceptionShouldThrow()
        {
            //Arrange
            TextEncoder textEncoder = new TextEncoder();

            //Act & Assert
            Action encryptAction = () => textEncoder.Encrypt(null, "dsfsd");
            encryptAction
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        public void TextEncrypt_FileNameNull_ExceptionShouldThrow()
        {
            //Arrange
            TextEncoder textEncoder = new TextEncoder();

            //Act & Assert
            Action encryptAction = () => textEncoder.Encrypt("dasasd", null, saveToFile: true);
            encryptAction
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        public void TextEncrypt_SaveToFileNotNeccessary_ExceptionShouldNotThrow()
        {
            //Arrange
            TextEncoder textEncoder = new TextEncoder();

            //Act & Assert
            Action encryptAction = () => textEncoder.Encrypt("dasasd", null, saveToFile: false);
            encryptAction
                .Should()
                .NotThrow<ArgumentNullException>();
        }

        [Test]
        public void TextDectypt_InputTextNull_ExceptionShouldThrow()
        {
            //Arrange
            TextEncoder textEncoder = new TextEncoder();

            //Act & Assert
            Action decryptAction = () => textEncoder.Decrypt(null);
            decryptAction
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        [TestCase("Marcin", "\u0382냿鱸⃧엗젳")]
        [TestCase("BSI", "麖뢻粵輪䚽쏉๐")]
        public void TextEncrypt_Success(string inputText, string expectedEncryptedText)
        {
            //Arrange
            TextEncoder textEncoder = new TextEncoder();

            //Act
            byte[] actual = textEncoder.Encrypt(inputText, "someName", key, iv);
            string actualEncrypted = textEncoder.GetString(actual);

            //Assert
            Assert.AreEqual(expectedEncryptedText, actualEncrypted);
        }

        [Test]
        [TestCase("\u0382냿鱸⃧엗젳", "Marcin")]
        [TestCase("麖뢻粵輪䚽쏉๐", "BSI")]
        public void TextDecrypt_Success(string encryptedText, string expected)
        {
            //Arrange
            TextEncoder textEncoder = new TextEncoder();

            //Act
            string actual = textEncoder.Decrypt(encryptedText, key, iv);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
