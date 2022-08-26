using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NETCore.Components.Helper
{
    public class MD5Helper
    {

        ///DES加密    
        ///sKey 
        public string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        ///DES解密    
        public string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        public string MD5Encrypt(string pToEncrypt, string sKey, out string msg)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            StringBuilder ret = new StringBuilder();
            try
            {
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                msg = "SUCC:";
                int length = sKey.Length;
                if (length < 8)
                { msg = "ERR:密钥长度不能小于8"; return ""; }
                int _dif = length - 8;
                int _sub_Index = _dif / 2 + _dif % 2;
                _sub_Index = _sub_Index <= 0 ? 0 : _sub_Index;
                sKey = sKey.Substring(_sub_Index, 8);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
            }
            catch (Exception ex)
            {
                msg = "ERR:" + ex.Message;
            }
            return ret.ToString();
        }
        ///DES解密    
        public string MD5Decrypt(string pToDecrypt, string sKey, out string msg)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            string _value = "";

            try
            {
                msg = "SUCC:";
                int length = sKey.Length;
                if (length < 8)
                { msg = "ERR:密钥长度不能小于8"; return ""; }
                int _dif = length - 8;
                int _sub_Index = _dif / 2 + _dif % 2;
                _sub_Index = _sub_Index <= 0 ? 0 : _sub_Index;
                sKey = sKey.Substring(_sub_Index, 8);
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                _value = Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                msg = "ERR:" + ex.Message;
            }

            return _value;
        }

        /// <summary>
        /// 对字符串进行MD5运算
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5String(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            byte[] md5Buffer = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in md5Buffer)
            {
                sb.Append(b.ToString("x2"));
            }
            md5.Clear();
            return sb.ToString();
        }
    }
}
