using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace pde.pub
{
    /// <summary>
    /// 加密解密工具
    /// </summary>
    public static class CryptUtil
    {
        //默認密鈅加密算法
        public static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        #region DES加密算法
        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <param name="encryptKey">密鈅的位數</param>
        /// <returns></returns>
        public static string EncryptionDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        public static string EncryptionDES(string encryptString)
        {
            return EncryptionDES(encryptString, Const.AESPassword);
        }
        #endregion

        #region DES解密算法
        /// <summary>
        /// DES解密方法
        /// </summary>
        /// <param name="decryptString">要解密的字符串</param>
        /// <param name="decryptKey">密鈅的位數</param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        public static string DecryptDES(string decryptString)
        {
            return DecryptDES(decryptString, Const.AESPassword);
        }
        #endregion

        #region AES加密, 自定义密钥
        /// <summary>       
        /// AES加密         
        /// </summary>        
        /// <param name="text">加密字符</param>        
        /// <param name="password">加密的密码</param>        
        /// <param name="iv">密钥(隨機生成)</param>        
        /// <returns></returns>
        public static string AESEncrypt(string text, string password, string iv)
        {
            try
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;
                byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] keyBytes = new byte[16];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                    len = keyBytes.Length;
                System.Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
                rijndaelCipher.IV = ivBytes;
                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(text);
                byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
                return Convert.ToBase64String(cipherBytes);
            }
            catch
            {
                return text;
            }
        }
        #endregion 

        #region 随机生成密钥
        /// <summary>        
        /// 随机生成密钥        
        /// </summary>        
        /// <returns></returns>
        public static string GetIv(int n)
        {
            char[] arrChar = new char[] { 'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v', 'w', 'z', 'y', 'x', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U', 'W', 'X', 'Y', 'Z' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion

        #region AES解密
        /// <summary>        
        /// AES解密        
        /// </summary>        
        /// <param name="text"></param>        
        /// <param name="password"></param>        
        /// <param name="iv"></param>        
        /// <returns></returns>
        public static string AESDecrypt(string text, string password, string iv)
        {
            try
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;
                byte[] encryptedData = Convert.FromBase64String(text);
                byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] keyBytes = new byte[16];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length) len = keyBytes.Length;
                System.Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(iv);
                rijndaelCipher.IV = ivBytes;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.UTF8.GetString(plainText);
            }
            catch
            {
                return text;
            }
        }
        #endregion

        #region 哈希加密
        /// <summary>
        /// 哈希加密(解密哈希需要加密匹配字符串)
        /// </summary>
        /// <param name="text">要加密的字符</param>
        /// <returns></returns>
        public static string HSEncrypt(string text)
        {
            //將數據轉化為字節流
            byte[] keyBytes = System.Text.Encoding.UTF32.GetBytes(text);

            SHA1 SHhash = new SHA1Managed();
            byte[] arHashvalue;
            arHashvalue = SHhash.ComputeHash(keyBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arHashvalue.Length; i++)
            {
                sb.Append(arHashvalue[i].ToString());
            }
            return sb.ToString();
        }
        #endregion

        #region 哈希破解方法（破解數字）
        /// <summary>
        /// 哈希破解方法
        /// </summary>
        /// <param name="text">輸入加密后的哈希值</param>
        /// <returns></returns>
        public static string HSDecrypt(string text)
        {
            //破解數字

            string HSResult = "";
            int num = 1;
            for (int i = 0; i < num; i++)
            {
                HSResult = HSEncrypt(num.ToString());
                if (HSResult == text)
                {
                    return num.ToString();
                }
                else
                {
                    num++;
                }
            }
            return num.ToString();
        }
        #endregion


        #region MD5字符串加密
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="input">要加密的字符</param>
        /// <param name="resultUpper">结果是否转为大写，默认不转</param>
        /// <returns></returns>
        public static string GetMd5Hash(string input, bool resultUpper = false)
        {
            if (input == null)
            {
                return null;
            }

            MD5 md5Hash = MD5.Create();
            //   MD5 md5Hash = new MD5CryptoServiceProvider();

            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString(resultUpper ? "X2" : "x2"));
            }

            // 返回十六进制字符串  
            return sBuilder.ToString();
        }
        #endregion

        #region 文件解密方法 
        /// <param name="filePath">源文件</param>  
        /// <param name="savePath">保存文件</param>  
        /// <param name="keyStr">密钥，要求为8位</param>  
        public static void DecryptFile(string filePath, string savePath, string keyStr)
        {
            //通过des解密  
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //通过流读取文件  
            FileStream fs = File.OpenRead(filePath);
            //获取文件二进制字符  
            byte[] inputByteArray = new byte[fs.Length];
            //读取流文件  
            fs.Read(inputByteArray, 0, (int)fs.Length);
            //关闭流  
            fs.Close();
            //密钥数组  
            //byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            byte[] keyByteArray = Encoding.UTF8.GetBytes(keyStr.Substring(0, 8));
            ////定义哈希变量  
            //SHA1 ha = new SHA1Managed();  
            ////计算指定字节组指定区域哈希值  
            //byte[] hb = ha.ComputeHash(keyByteArray);  
            ////加密密钥数组  
            //byte[] sKey = new byte[8];  
            ////加密变量  
            //byte[] sIV = new byte[8];  
            //for (int i = 0; i < 8; i++)  
            //    sKey[i] = hb[i];  
            //for (int i = 8; i < 16; i++)  
            //    sIV[i - 8] = hb[i];  

            byte[] sKey = keyByteArray;
            byte[] sIV = keyByteArray;

            //获取加密密钥  
            des.Key = sKey;
            //加密变量  
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
        }
        #endregion

        #region 文件加密方法   
        /// <param name="filePath">源文件</param>  
        /// <param name="savePath">保存为文件名称</param>  
        /// <param name="keyStr">密钥，要求为8位</param>  
        public static void EncryptFile(string filePath, string savePath, string keyStr)
        {
            //通过des加密  
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //通过流打开文件  
            FileStream fs = File.OpenRead(filePath);
            //获取文件二进制字符  
            byte[] inputByteArray = new byte[fs.Length];
            //读流文件  
            fs.Read(inputByteArray, 0, (int)fs.Length);
            //关闭流  
            fs.Close();
            //获得加密字符串二进制字符  
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);

            ////计算指定字节组指定区域哈希值  
            //SHA1 ha = new SHA1Managed();  
            //byte[] hb = ha.ComputeHash(keyByteArray);  
            ////加密密钥数组  
            //byte[] sKey = new byte[8];  
            ////加密变量  
            //byte[] sIV = new byte[8];  
            //for (int i = 0; i < 8; i++)  
            //    sKey[i] = hb[i];  
            //for (int i = 8; i < 16; i++)  
            //    sIV[i - 8] = hb[i];  
            byte[] sKey = keyByteArray;
            byte[] sIV = keyByteArray;
            //获取加密密钥      
            des.Key = sKey;
            //设置加密初始化向量  
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);

            }
            fs.Close();
            cs.Close();
            ms.Close();
        }
        #endregion

         
    }
}
