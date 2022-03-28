using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dominio.Entidades
{
    public class CryptoEN
    {
        public string DecryptText(string strText, int Tipo = 0)
        {
            switch (Tipo)
            {
                case 0:
                    return Decrypt(strText, "&%#*@1:?");
                case 1:
                    return Decrypt(strText, "1%2*@,:?");
                case 2:
                    return Decrypt(strText, "3%#*4,:?");
                default:
                    return Decrypt(strText, "&5#*@6:?");
            }
        }

        public string EncryptText(string strText, int Tipo = 0)
        {
            switch (Tipo)
            {
                case 0:
                    return Encrypt(strText, "&%#*@1:?");
                case 1:
                    return Encrypt(strText, "1%2*@,:?");
                case 2:
                    return Encrypt(strText, "3%#*4,:?");
                default:
                    return Encrypt(strText, "&5#*@6:?");
            }
        }

        private string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] array = new byte[0];
            byte[] IV = new byte[8] {18,52,86,120,144,171,205,239};

            int length = stringToDecrypt.Length;
            try
            {
                if (stringToDecrypt.Trim().Equals(""))
                {
                    return "";
                }
                byte[] key = Encoding.UTF8.GetBytes(sEncryptionKey.PadLeft(8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                cryptoStream.FlushFinalBlock();
                Encoding uTF = Encoding.UTF8;
                byte[] bytems = ms.ToArray();
                return uTF.GetString(bytems, 0, bytems.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            byte[] array = new byte[0];
            byte[] IV = new byte[8] {18,52,86,120,144,171,205,239};

            try
            {
                if (stringToEncrypt.Trim().Equals(""))
                {
                    return "";
                }
                byte[] key = Encoding.UTF8.GetBytes(SEncryptionKey.PadLeft(8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
