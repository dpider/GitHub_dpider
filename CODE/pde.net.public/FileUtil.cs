using ICSharpCode.SharpZipLib.Zip;
using pde.log;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace pde.pub
{
    [ClassInterface(ClassInterfaceType.None)]
    public static class FileUtil
    {
        public static void DeleteDirectory(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    DirectoryInfo[] childs = dir.GetDirectories();
                    foreach (DirectoryInfo child in childs)
                    {
                        DeleteDirectory(child.FullName); 
                    }
                    dir.Delete(true);
                }
            }
            catch { }
        }

        public static void DeleteFile(string path)
        {
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        #region 遍历查找文件夹下文件
        /// <summary>
        /// 获取文件夹下所有符合条件的文件
        /// </summary>
        /// <param name="root">目录</param>
        /// <param name="bFindInSub">是否查找子目录</param>
        /// <param name="fileExt">查找符合后缀的文件，全部文件使用"*"或默认值</param>
        /// <param name="fileNameLike">查找符合包含设置文件名的文件，全部文件使用空值或默认值</param>
        /// <returns>返回文件列表</returns>
        public static List<string> getFilesInDir(string root, bool bFindInSub = true, string fileExt = "*", string fileNameLike = "")
        {
            DirectoryInfo dir = new DirectoryInfo(root); 
            return getFilesInDir(dir, bFindInSub, fileExt, fileNameLike);
        }

        /// <summary>
        /// 获取文件夹下所有符合条件的文件
        /// </summary>
        /// <param name="root">目录</param>
        /// <param name="bFindInSub">是否查找子目录</param>
        /// <param name="fileExt">查找符合后缀的文件，全部文件使用"*"或默认值</param>
        /// <param name="fileNameLike">查找符合包含设置文件名的文件，全部文件使用空值或默认值</param>
        /// <returns>返回文件列表</returns>
        public static List<string> getFilesInDir(DirectoryInfo root, bool bFindInSub=true, string fileExt="*", string fileNameLike = "")
        {
            List<string> files = new List<string>();
            files.AddRange(getFile(root, fileExt, fileNameLike));
            if (bFindInSub)
            {
                foreach (DirectoryInfo _dir in root.GetDirectories())
                {
                    files.AddRange(getFile(_dir, fileExt, fileNameLike));
                    files.AddRange(getFilesInDir(_dir, bFindInSub, fileNameLike));
                }
            }
            return files;
        }

        private static List<string> getFile(DirectoryInfo root, string fileExt = "*", string fileNameLike = "")
        {
            List<string> files = new List<string>();
            foreach (FileInfo file in root.GetFiles())
            {
                string filename = file.FullName;
                string fileext = Path.GetExtension(filename).ToUpper();
                //后缀符合要求
                if ((fileExt.Equals("*")) || (fileExt.ToUpper().Split(';').Contains(fileext)))  
                {
                    //文件名符合要求
                    if (("".Equals(fileNameLike)) || (filename.Contains(fileNameLike)))
                    {
                        files.Add(filename);
                    }
                }
            }
            return files;
        }
        #endregion

        public static void UnZipFile(string zipFilePath, string outPath)
        {
            if (!File.Exists(zipFilePath))
            {
                throw new Exception("没有找到被解压文件：" + zipFilePath);
            }

            bool ok = false;
            string err = "";
            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        if (theEntry.Name.Equals("")) { continue; }
                        //  Console.WriteLine(theEntry.Name);
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        // create directory
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(outPath + directoryName);
                        }

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(outPath + theEntry.Name))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                ok = true;
            }
            catch (Exception ex)
            {
                err = ex.ToString();
            }

            if (!ok)
            {
                try
                {
                    using (Stream stream = File.OpenRead(zipFilePath))
                    {
                        var reader = ReaderFactory.Open(stream);
                        while (reader.MoveToNextEntry())
                        {
                            if (!reader.Entry.IsDirectory)
                            {
                                reader.WriteEntryToDirectory(outPath);
                            }
                        }
                    }
                    ok = true;
                }
                catch (Exception ex)
                {
                    err = ex.ToString();
                }
            }

            if (!ok)
            {
                throw new Exception("解压文件失败：" + err);
            }
        }
 

        public static string GetMD5HashFromFile(string fileName, bool resultUpper = false)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString(resultUpper ? "X2" : "x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static string GetMD5HashFromFile(Stream fileStream, bool resultUpper = false)
        {
            try
            { 
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fileStream); 
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString(resultUpper ? "X2" : "x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static byte[] getFileHeader(string fileName, int byteCount)
        {
            byte[] data = new byte[byteCount];
            using (var stream = File.OpenRead(fileName))
            {
                stream.Read(data, 0, byteCount);
            }
            return data;
        }

        private static Dictionary<string, string> getFileTypeMap()
        {
            Dictionary<string, string> temp = new Dictionary<string, string> {
                {"ffd8ffe", "JPG,JPEG"},                      //JPEG (jpg)  ffd8ffe000104a464946
                {"89504e470d0a1a0a0000", "PNG"},              //PNG (png) 
                {"424d228c010000000000", "BMP"},              //16色位图(bmp)     
                {"424d8240090000000000", "BMP"},              //24位位图(bmp)     
                {"424d8e1b030000000000", "BMP"},              //256色位图(bmp)     
                {"41433130313500000000", "DWG"},              //CAD (dwg)     
                {"3c21444f435459504520", "HTML"},             //HTML (html)
                {"3c21646f637479706520", "HTM"},              //HTM (htm)
                {"48544d4c207b0d0a0942", "CSS"},              //css
                {"696b2e71623d696b2e71", "JS"},               //js
                {"7b5c727466315c616e73", "RTF"},              //Rich Text Format (rtf)     
                {"38425053000100000000", "PSD"},              //Photoshop (psd)     
                {"46726f6d3a203d3f6762", "EML"},              //Email [Outlook Express 6] (eml)       
                {"d0cf11e0a1b11ae10000", "DOC,XLS,MSI,PPT,VSD,WPS,ET,DPS"},  //MS Excel 注意：word、msi、excel、Visio 和WPS文字wps、表格et、演示dps 的文件头一样 
                {"5374616E64617264204A", "MDB"},              //MS Access (mdb)      
                {"252150532D41646F6265", "PS"},
                {"255044462d312e350d0a", "PDF"},              //Adobe Acrobat (pdf)   
                {"2e524d46000000120001", "RMVB"},             //rmvb/rm相同  
                {"464c5601050000000900", "FLV"},              //flv与f4v相同  
                {"00000020667479706d70", "MP4"},
                {"00000014667479706973", "MP4"},
                {"49443303000000002176", "MP3"},
                {"000001ba210001000180", "MPG"},              //     
                {"3026b2758e66cf11a6d9", "WMV"},              //wmv与asf相同    
                {"52494646e27807005741", "WAV"},              //Wave (wav)  
                {"52494646d07d60074156", "AVI"},
                {"4d546864000000060001", "MID"},              //MIDI (mid)   
                {"504b0304140000000800", "ZIP"},
                {"526172211a0700cf9073", "RAR"},
                {"235468697320636f6e66", "INI"},
                {"504b03040a0000000000", "JAR"},
                {"4d5a9000030000000400", "EXE"},              //可执行文件
                {"3c25402070616765206c", "JSP"},              //jsp文件
                {"49492a00", "TIF,TIFF" },                    //tif文件
                {"4d616e69666573742d56", "MF"},               //MF文件
                {"3c3f786d6c2076657273", "XML"},              //xml文件
                {"494e5345525420494e54", "SQL"},              //xml文件
                {"7061636b616765207765", "JAVA"},             //java文件
                {"406563686f206f66660d", "BAT"},              //bat文件
                {"1f8b0800000000000000", "GZ"},               //gz文件
                {"6c6f67346a2e726f6f74", "PROPERTIES"},       //bat文件
                {"cafebabe0000002e0041", "CLASS"},            //bat文件
                {"49545346030000006000", "CHM"},              //bat文件
                {"04000000010000001300", "MXP"},              //bat文件
                {"504b0304140006000800", "DOCX,XLSX,PPTX"},   //docx文件  
                {"504b0304140002000800", "DOCX,XLSX,PPTX"},   //docx文件   
                {"6431303a637265617465", "TORRENT"},
                {"6d6f6f76", "MOV"},                          //Quicktime (mov)  
                {"ff575043", "WPD"},                          //WordPerfect (wpd)   
                {"cfad12fec5fd746f", "DBX"},                  //Outlook Express (dbx)     
                {"2142444e", "PST"},                          //Outlook (pst)      
                {"ac9ebd8f", "QDF"},                          //Quicken (qdf)     
                {"e3828596", "PWL"},                          //Windows Password (pwl)         
                {"2e7261fd", "RAM"}                           //Real Audio (ram)  
            };
            return temp;
        }

        public static string getFileExtByHeader(string hex)
        {
            string hx = hex.ToLower();
            Dictionary<string, string> file_type_map = getFileTypeMap();
            foreach (KeyValuePair<string, string> kv in file_type_map)
            {
                if (hx.StartsWith(kv.Key) || kv.Key.StartsWith(hx))
                {
                    return kv.Value;
                }
            }
            return ""; 
        }


        public static string getFileExtByHeader(string filePath, int byteCount)
        {
            byte[] hed = getFileHeader(filePath, byteCount);
            string head = getFileExtByHeader(StringUtil.byteToHexStr(hed));
            if (head.Equals(""))
            {
                LogLocal.log().SaveLog(new LogEntity(string.Format("获取文件【{0}】的文件头信息为空！", filePath), LogType.Plat, LogLevel.DEBUG));
            }
            return getFileExtByHeader(StringUtil.byteToHexStr(hed)); 
        }

        /// <summary> 
        /// 判断是否是不带 BOM 的 UTF8 格式 
        /// </summary> 
        /// <param name=“data“></param> 
        /// <returns></returns> 
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        /// <summary> 
        /// 通过给定的文件流，判断文件的编码类型 
        /// </summary> 
        /// <param name=“fs“>文件流</param> 
        /// <returns>文件的编码类型</returns> 
        private static System.Text.Encoding GetType(FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
            Encoding reVal = Encoding.Default;  // System.Text.Encoding.GetEncoding("gb2312");

            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }

        //判断文本文件字符集
        public static Encoding GetFileEncoding(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }
          
        public static void saveNoBomUTF8File(string strContent, string outFile)
        {
            UTF8Encoding utf8 = new UTF8Encoding(false);
            File.WriteAllText(outFile, strContent, utf8);
        }

        /// <summary>
        /// 读取文本文件内容至字符串
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件内容</returns>
        public static string readTxtFile(string path)
        {
            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                sb.AppendLine(line);
            }
            sr.Close();
            return sb.ToString();
        }

        /// <summary>
        /// 写文本内容至文本文件
        /// </summary>
        /// <param name="path">文本文件路径</param>
        /// <param name="content">文本内容</param>
        public static void writeTxtFile(string path, string content)
        {
            FileStream fs;
            if (File.Exists(path))
            {
                fs = new FileStream(path, FileMode.Append);
            }
            else
            {
                fs = new FileStream(path, FileMode.Create);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            //开始写入
            sw.WriteLine(content);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
    }

 
}
