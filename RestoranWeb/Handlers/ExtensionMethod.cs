using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace RestoranWeb
{
    public static class ExtensionMethod

    {

        private static string encryptionKey = "hjguy6R%&^%&^";


        public static string Encrypt(this string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(this string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return "";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string ToTitleCase(this string text)
        {
         return   CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }
        public static int AlphaNumeric(this string text)
        {
          return  text.Count(char.IsLetterOrDigit);
        } 
        public static int WorCount(this string text)
        {
          return  text.Split(' ').Length;
        }
        public static string ToSlug(this string text)
        {
            return string.Join("", text.Replace(" ", "-").Replace("_", "-").Replace(".", "-").Where(m => char.IsLetterOrDigit(m) || m == '-').ToString());
        }
    }
}
