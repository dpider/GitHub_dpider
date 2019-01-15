using Microsoft.Win32; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace pde.pub
{
    class myrar
    { 
        /// 
        /// 解压缩指定的rar文件。
        ///

        /// rar文件（绝对路径）。
        /// 解压缩保存的目录。
        /// 解压缩后删除rar文件。
        public void DecompressRAR(string rarFileToDecompress, string directoryToSave, bool deleteRarFile)
        { 
            try
            {
                if (Directory.Exists(directoryToSave))
                {
                    FileUtil.DeleteDirectory(directoryToSave);
                }

                // RegistryKey the_Reg = Registry.ClassesRoot.OpenSubKey("Applications/WinRar.exe/Shell/Open/Command");
                RegistryKey the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe\");

                String winrarExe = the_Reg.GetValue("").ToString();
                the_Reg.Close();
            //    winrarExe = winrarExe.Substring(1, winrarExe.Length - 7);

                if (new FileInfo(winrarExe).Exists)
                {
                    Process p = new Process();
                    // 需要启动的程序名
                    p.StartInfo.FileName = winrarExe;
                    // 参数  
                    //arguments = " X " + " 1.rar " + " " + "C:/1";
                    string arguments = @"x -inul -y -o+";
                    arguments += " " + rarFileToDecompress + " " + directoryToSave;

                    p.StartInfo.Arguments = arguments;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();//启动
                    while (!p.HasExited)
                    {
                    }
                    p.WaitForExit();
                }
                else
                {
                    throw new Exception("没有检测到安装的WinRAR程序！");
                }
                    
            }
            catch (Exception ex)
            { 
                throw new Exception("解压缩的过程中出现了错误！" + ex.ToString());
            }

            if (deleteRarFile)
            {
                File.Delete(rarFileToDecompress);
            }
            
        }
    }


}
