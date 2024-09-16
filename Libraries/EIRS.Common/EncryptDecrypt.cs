using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Common
{
    public class EncryptDecrypt
    {
        private ASCIIEncoding MyASCIIEncoding = new ASCIIEncoding();
        private static TripleDESCryptoServiceProvider cryptDES3 = new TripleDESCryptoServiceProvider();
        private static MD5CryptoServiceProvider cryptMD5Hash = new MD5CryptoServiceProvider();

        public static string Encrypt(string pstrValue)
        {
            try
            {
                string encodedPhrase;

                byte[] Buff;

                cryptDES3.Key = cryptMD5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pstrValue));

                cryptDES3.Mode = CipherMode.ECB;

                ICryptoTransform desdencrypt = cryptDES3.CreateDecryptor();

                Buff = ASCIIEncoding.ASCII.GetBytes(pstrValue);

                encodedPhrase = Convert.ToBase64String(Buff, 0, Buff.Length);

                return encodedPhrase;

            }
            catch (Exception Ex)
            {
                //CommUtil.ExceptionHandler(Ex);
                return null;
            }
        }

        public static string Decrypt(string pstrValue)
        {
            try
            {
                string decodedPhrase;

                byte[] Buff;

                cryptDES3.Key = cryptMD5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pstrValue));

                cryptDES3.Mode = CipherMode.ECB;

                ICryptoTransform desdencrypt = cryptDES3.CreateDecryptor();

                Buff = Convert.FromBase64String(pstrValue);

                decodedPhrase = ASCIIEncoding.ASCII.GetString(Buff, 0, Buff.Length);

                return decodedPhrase;
            }
            catch (Exception Ex)
            {
                //CommUtil.ExceptionHandler(Ex);
                return null;
            }
        }
    }
}
