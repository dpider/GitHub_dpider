using System;
using System.Globalization;
using System.Text;
using System.Drawing;

namespace pde.pub
{
    /// <summary>
    /// 字符串操作工具
    /// </summary>
    static public class StringUtil
    { 
        /// <summary>
        /// 16进制转字符串
        /// </summary>
        /// <param name="hex">16进制字符串</param>
        /// <param name="charset">字符集</param>
        /// <returns>转换后的字符串</returns>
        public static string HexToStr(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }

        /// <summary>
        /// 字符串转16进制
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>16进制</returns>
        public static string StrToHex(string str)
        {
            string strResult;
            byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
            strResult = "";
            foreach (byte b in buffer)
            {
                strResult += b.ToString("X2");//X是16进制大写格式 
            }
            return strResult;
        }

        /// <summary>
        /// 字节数组转16进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 16进制字符串转10进制
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>10进制数组</returns>
        public static byte[] HexToByte(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("utf-8");
            return bytes;
        }

        /// <summary>
        /// 16进制转ASCII码值
        /// </summary>
        /// <param name="hex">16进制字符串</param>
        /// <returns>ASCII码值</returns>
        public static string HexToASCII(string hex)
        {
            byte[] byteTag;
            String tagString;
            String checkValue;

            if (hex.Length < 8)
                checkValue = hex;
            else
                checkValue = hex.Substring(4, 4);

            if (checkValue.ToUpper().Equals("4E54"))
            {
                byteTag = HexStringToByteArray(hex.Substring(8, hex.Length - 8));
                tagString = Encoding.ASCII.GetString(byteTag, 0, byteTag.Length);
            }
            else
            {
                byteTag = HexStringToByteArray(hex.Substring(4, hex.Length - 4));
                tagString = Encoding.ASCII.GetString(byteTag, 0, byteTag.Length);
            }
            return tagString;
        }

        /// <summary>
        /// 16进制字符串转二进制数组
        /// 例：3334(string) --> byte {0x33, 0x34}
        /// </summary>
        /// <param name="hex">16进制字符串</param>
        /// <returns>二进制数组</returns>
        public static byte[] HexStringToByteArray(string hex)
        {
            try
            {
                byte[] HexAsBytes = new byte[hex.Length / 2];
                for (int index = 0; index < HexAsBytes.Length; index++)
                {
                    string byteValue = hex.Substring(index * 2, 2);
                    HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }

                return HexAsBytes;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 二进制数组转16进制字符串
        /// </summary>
        /// <param name="data">二进制数组</param>
        /// <returns>16进制字符串</returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }


        private static bool[] minusColor = { false, false, false };
        public static Color GradientColor(Color color, int gradient = 10)
        {
            int[] rgb = { color.R, color.G, color.B };
            Random dm = new Random();
            for (int i = 0; i < 3; i++)
            {
                int x = dm.Next(1, gradient);
                if (minusColor[i]) { rgb[i] -= x; } else { rgb[i] += x; }
                if (rgb[i] > 255)
                {
                    rgb[i] = 255;
                    minusColor[i] = true;
                }
                else if (rgb[i] < 0)
                {
                    rgb[i] = 0;
                    minusColor[i] = false;
                }
            }
            return Color.FromArgb(color.A, rgb[0], rgb[1], rgb[2]);
        }
         

        /// <summary>
        /// 文件大小字节数，格式化成可读的带单位字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string formatBytes(double value)
        {
            string m_strSize = "";  
            if (value < 1024.00)
                m_strSize = value.ToString("F2") + "Byte";
            else if (value >= 1024.00 && value < 1048576)
                m_strSize = (value / 1024.00).ToString("F2") + "K";
            else if (value >= 1048576 && value < 1073741824)
                m_strSize = (value / 1024.00 / 1024.00).ToString("F2") + "M";
            else if (value >= 1073741824)
                m_strSize = (value / 1024.00 / 1024.00 / 1024.00).ToString("F2") + "G";
            return m_strSize;
        }

        public static int StrToInt(string value, int defInt = 0)
        {
            int val = defInt;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            else
            {
                return defInt;
            }
        }

        #region 字符串的Base64编码/解码
        private static Base64Encoder base64Encoder = new Base64Encoder();
        private static Base64Decoder base64Decoder = new Base64Decoder();

        /// <summary>
        /// Base64字符串编码
        /// </summary>
        /// <param name="value">编码前字符串</param>
        /// <returns>编码后字符串</returns>
        public static string EncodeBase64String(string value)
        {
            if (base64Encoder == null)
            {
                base64Encoder = new Base64Encoder();
            }
            try
            {
                return base64Encoder.GetEncoded(value);
            }
            catch
            {
                return value;
            }
        }

        /// <summary>
        /// Base64字符串解码
        /// </summary>
        /// <param name="value">解码前字符串</param>
        /// <returns>解码后字符串</returns>
        public static string DecodeBase64String(string value)
        {
            if (base64Decoder == null)
            {
                base64Decoder = new Base64Decoder();
            }
            try
            {
                return base64Decoder.GetDecoded(value);
            }
            catch
            {
                return value;
            }
        }
        #endregion

    }
}
