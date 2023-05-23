using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Crypto
{
    /// <summary>
    /// 字串加密系統
    /// </summary>
    public class Rijndael
    {
        private static string secertString = "EasyArchitect.Framework";
        private static byte[] SALT = System.Text.Encoding.ASCII.GetBytes(secertString); //new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3d };
        private static Rfc2898DeriveBytes _pdb = null;
        private static byte[] _key;
        private static byte[] _iv;

        public Rijndael()
        {
        }

        /// <summary>
        /// 使用 AES 加密方法加密字串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptString(string input)
        {
            _pdb = new Rfc2898DeriveBytes(input, SALT);

            byte[] encrypted;

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = _key = _pdb.GetBytes(32);
                rijAlg.IV = _iv = _pdb.GetBytes(16);

                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }
        /// <summary>
        /// 將字串解密
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static string DecryptStringFromBase64(string base64String)
        {
            if (_pdb == null)
                throw new NullReferenceException("Rfc2898DeriveBytes is null!...");
            if (base64String == null || base64String.Length <= 0)
                throw new ArgumentNullException("base64String is null!.");

            byte[] cipherText = Convert.FromBase64String(base64String);
            string plaintext = null;

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = _key;
                rijAlg.IV = _iv;

                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
