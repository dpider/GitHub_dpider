using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;

namespace pde.pub
{ 
    public class WRSetting
    {

        private static WRSetting set = null;
        public static WRSetting Set(string cfgFile = "")
        {
            if (set == null)
            {
                set = new WRSetting(cfgFile);
            }
            return set;
        }

        public static void Close()
        {
            set = null;
        }


        //定义节点变量
        XmlNode NodeRoot = null;               //根节点 
        XmlNode NodeSettings = null;          //系统设置项  

        private string XMLFile = string.Format(@"{0}\{1}.config",
                                               SystemUtil.CurrentDirectory(),
                                               Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));

        private XmlDocument doc;

        #region ====== 初始化，读取配置信息 ======
        public WRSetting(string cfgFile = "")
        {
            try
            {
                doc = new XmlDocument();

                if (!"".Equals(cfgFile))
                {
                    XMLFile = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\" + cfgFile;
                }
                if (!File.Exists(XMLFile))
                {
                    //创建类型声明节点
                    XmlNode node = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                    doc.AppendChild(node);
                    //创建根节点  
                    XmlNode root = doc.CreateElement("PDE");
                    doc.AppendChild(root);
                    doc.Save(XMLFile);
                }
                else
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreComments = true;//忽略文档里面的注释 
                    XmlReader reader = XmlReader.Create(XMLFile, settings);
                    doc.Load(reader);
                    reader.Close();
                }

                //读取根节点
                NodeRoot = doc.SelectSingleNode("PDE");
                if (NodeRoot == null)
                {
                    doc.CreateElement("PDE");
                    NodeRoot = doc.SelectSingleNode("PDE");
                }
                 
                //系统设置项
                NodeSettings = NodeRoot.SelectSingleNode("Settings");
                if (NodeSettings == null)
                {
                    NodeRoot.AppendChild(doc.CreateElement("Settings"));
                    NodeSettings = NodeRoot.SelectSingleNode("Settings");
                }
                 

                //保存节点
               // doc.Save(XMLFile);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            } 
        }

        public XmlDocument XmlDoc()
        {
            return doc;
        }
        #endregion


        #region ====== 系统设置项 ======
        /// <summary>
        /// 获取设置项值
        /// </summary>
        /// <param name="key">设置项参数</param>
        /// <param name="defValue">设置项默认值</param>
        /// <returns>设置项值</returns>
        public string getSettings(string key, string defValue="")
        {
            XmlNode node = NodeSettings.SelectSingleNode(key);
            if (node == null)
            {
                setSettings(key, defValue);
                return defValue;
            }
            return node.InnerText;
        }

        /// <summary>
        /// 判断是否存在设置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool haveSettings(string key)
        {
            XmlNode node = NodeSettings.SelectSingleNode(key); 
            return node != null;
        }

        /// <summary>
        /// 设置设置项值
        /// </summary>
        /// <param name="key">设置项参数</param>
        /// <param name="value">设置项值</param>
        public void setSettings(string key, string value)
        {
            List<string> list = new List<string>(key.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
            XmlNode pNode = NodeSettings;
            foreach (string k in list)
            {
                XmlNode node = pNode.SelectSingleNode(k);
                if (node == null)
                {
                    node = doc.CreateElement(k);
                    pNode = pNode.AppendChild(node);
                }
                else
                {
                    pNode = node;
                }
            }
            pNode.InnerText = value;

            doc.Save(XMLFile);
        }
        #endregion 


    }
}
