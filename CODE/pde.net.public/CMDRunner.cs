using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace pde.pub
{
    public class CMDRunner
    {
        // 1.定义委托  
        public delegate void dgReadStdOutput(string result);
        public delegate void dgReadErrOutput(string result);
        public delegate void dgCMDExit(int exitCode);

        // 2.定义委托事件  
        public event dgReadStdOutput ReadStdOutput = null;
        public event dgReadErrOutput ReadErrOutput = null;
        public event dgCMDExit CMDExit = null;

        private Process CmdProcess = new Process();

        public CMDRunner()
        {
            CmdProcess = new Process();
        }

        public void RunAction(string exeFile, string command)
        {
            CmdProcess.StartInfo.FileName = exeFile;
            CmdProcess.StartInfo.Arguments = command;
            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口  
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入  
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出  
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            //CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;  

            CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            CmdProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            CmdProcess.EnableRaisingEvents = true;                      // 启用Exited事件  
            CmdProcess.Exited += new EventHandler(CmdProcess_Exited);   // 注册进程结束事件  

            CmdProcess.Start();
            //   CmdProcess.StandardInput.WriteLine(command);
            //   CmdProcess.StandardInput.WriteLine("exit");
            CmdProcess.BeginOutputReadLine();
            CmdProcess.BeginErrorReadLine();

            // 如果打开注释，则以同步方式执行命令，此例子中用Exited事件异步执行。  
            //  CmdProcess.WaitForExit();       
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if ((e.Data != null) && (ReadStdOutput != null))
            {
                // 异步调用，外部需要invoke  
                ReadStdOutput(e.Data);
            }
        }

        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if ((e.Data != null) && (ReadErrOutput != null))
            {
                // 异步调用，外部需要invoke  
                ReadErrOutput(e.Data);
            }
        }

        private void CmdProcess_Exited(object sender, EventArgs e)
        {
            // 执行结束后触发  
            try
            {
                int exitCode = CmdProcess.ExitCode;
                // 异步调用，外部需要invoke  
                CMDExit(exitCode);
            }
            catch { }
            finally
            {
                CmdProcess.Close();
            }
        }
    }
}
