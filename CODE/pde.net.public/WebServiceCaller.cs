using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace pde.pub
{
    public class WebServiceCaller
    { 
        private string xmlNameSpace = string.Empty;
        private Dictionary<string, string> ResultNameSpace = new Dictionary<string, string>();
        private WebServicesUtil.SoapHeader soapHeader = null;

        public string soap_envelope_url = "http://schemas.xmlsoap.org/soap/envelope/";
        public string XMLSchema_url = "http://www.w3.org/2001/XMLSchema";
        public string XMLSchema_instance_url = "http://www.w3.org/2001/XMLSchema-instance/";
        public string ResultLastNode = "//soap:Body/*/ns1:out";
        public int TimeOut = 100000;

        /// <summary>  
        /// 初始化  
        /// </summary>  
        /// <param name="strUserId"> 用户名</param>  
        /// <param name="strPwd"> 密码</param>  
        public WebServiceCaller(WebServicesUtil.SoapHeader header)
        {
            soapHeader = header; 
        }
         
        public WebServiceCaller()
        { 
        }

        public void setResultNS(string key, string value)
        {
            if (ResultNameSpace.ContainsKey(key))
            {
                ResultNameSpace[key] = value;
            }
            else
            {
                ResultNameSpace.Add(key, value);
            }
        }

        /// <summary>  
        /// 通过SOAP协议动态调用webservice   
        /// </summary>  
        /// <param name="url"> webservice地址</param>  
        /// <param name="methodName"> 调用方法名</param>  
        /// <param name="pars"> 参数表</param>  
        /// <returns> 结果集xml</returns>  
        public string QuerySoapWebService(string url, string methodName, params object[] parms)
        {
            Dictionary<string, object> pars = new Dictionary<string, object>();
            for (int i= 0; i<parms.Length; i++)
            {  
                if (parms[i] is KeyValuePair<string, string>)
                {
                    KeyValuePair<string, string> kv = (KeyValuePair<string, string>)parms[i];
                    pars.Add(kv.Key, kv.Value);
                }
                else
                {
                    pars.Add(string.Format("in{0}", i), (string)parms[i]);
                }
            }
            
            if (xmlNameSpace.Equals(string.Empty))
            {
                xmlNameSpace = GetNamespace(url); 
            }

            return QuerySoapWebServiceNS(url, methodName, pars, xmlNameSpace);
        }
         
        /// <summary>  
        /// 通过SOAP协议动态调用webservice    
        /// </summary>  
        /// <param name="url"> webservice地址</param>  
        /// <param name="methodName"> 调用方法名</param>  
        /// <param name="pars"> 参数表</param>  
        /// <param name="xmlNs"> 名字空间</param>  
        /// <returns> 结果集</returns>  
        private string QuerySoapWebServiceNS(string url, string methodName, Dictionary<string, object> pars, string xmlNs)
        {   
            // 获取请求对象  
            if (url.EndsWith("?wsdl", true, null))
            {
                url = url.Substring(0, url.Length - 5);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // 设置请求head  
            request.Method = "POST";
            request.ContentType = "text/xml";
            //request.Headers.Add("SOAPAction", "\"" + xmlNs + (xmlNs.EndsWith("/") ? "" : "/") + methodName + "\"");
            request.Headers.Add("SOAPAction", "");
            // 设置请求身份  
            SetWebRequest(request);
            // 获取soap协议  
            byte[] data = EncodeParsToSoap(pars, xmlNs, methodName);
            // 将soap协议写入请求  
            WriteRequestData(request, data);
            XmlDocument returnDoc = new XmlDocument();

            // 读取服务端响应  
            returnDoc = ReadXmlResponse(request.GetResponse());

            XmlNamespaceManager mgr = new XmlNamespaceManager(returnDoc.NameTable);
            mgr.AddNamespace("soap", soap_envelope_url);
            mgr.AddNamespace("ns1", xmlNs); 

            // 返回结果  
            return returnDoc.SelectSingleNode(ResultLastNode, mgr).InnerText; 
        }

        /// <summary>  
        /// 获取wsdl中的名字空间  
        /// </summary>  
        /// <param name="url"> wsdl地址</param>  
        /// <returns> 名字空间</returns>  
        private string GetNamespace(string url)
        {
            if (!url.EndsWith("?wsdl", true, null))
            {
                url += "?wsdl";
            }

            // 创建wsdl请求对象，并从中读取名字空间  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            SetWebRequest(request);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            sr.Close();
            return doc.SelectSingleNode("//@targetNamespace").Value;
        }

        /// <summary>  
        /// 加入soapheader节点  
        /// </summary>  
        /// <param name="doc"> soap文档</param>  
        private void InitSoapHeader(XmlDocument doc)
        {
            if (soapHeader == null) return;

            XmlElement ele = doc.CreateElement(soapHeader.ClassName, soapHeader.ClassNameSpace);
            foreach(KeyValuePair<string, object> kv in soapHeader.Properties)
            {
                XmlElement element = doc.CreateElement(kv.Key);
                element.InnerText = kv.Value.ToString();
                ele.AppendChild(element);
            }

            // 添加soapheader节点  
            XmlElement soapHeaderEle = doc.CreateElement("SOAP-ENV", "Header", soap_envelope_url);
            soapHeaderEle.AppendChild(ele); 

            doc.ChildNodes[0].AppendChild(soapHeaderEle);
        }

        /// <summary>  
        /// 将以字节数组的形式返回soap协议  
        /// </summary>  
        /// <param name="pars"> 参数表</param>  
        /// <param name="xmlNs"> 名字空间</param>  
        /// <param name="methodName"> 方法名</param>  
        /// <returns> 字节数组</returns>  
        private byte[] EncodeParsToSoap(Dictionary<string, object> pars, String xmlNs, String methodName)
        {
            XmlDocument doc = new XmlDocument();
            // 构建soap文档  
            doc.LoadXml("<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"" + soap_envelope_url + "\" xmlns:xsd=\"" + XMLSchema_url + "\" xmlns:xsi=\"" + XMLSchema_instance_url + "\"></SOAP-ENV:Envelope>");
  
            //// 加入soapHeader节点  
            InitSoapHeader(doc);

            // 创建soapbody节点  
            XmlElement soapBody = doc.CreateElement("SOAP-ENV", "Body", soap_envelope_url);

            // 根据要调用的方法创建一个方法节点  
            XmlElement soapMethod = doc.CreateElement(methodName);
            soapMethod.SetAttribute("xmlns", xmlNs);

            // 遍历参数表中的参数键  
            foreach (string key in pars.Keys)
            {
                // 根据参数表中的键值对，生成一个参数节点，并加入方法节点内  
                XmlElement soapPar = doc.CreateElement(key);
                soapPar.InnerXml = ObjectToSoapXml(pars[key]);
                soapMethod.AppendChild(soapPar);
            }

            // soapbody节点中加入方法节点  
            soapBody.AppendChild(soapMethod);

            // soap文档中加入soapbody节点  
            doc.DocumentElement.AppendChild(soapBody);

            // 添加声明  
            AddDelaration(doc);

            // 以字节数组的形式返回soap文档  
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        /// <summary>  
        /// 将参数对象中的内容取出  
        /// </summary>  
        /// <param name="o">参数值对象</param>  
        /// <returns>字符型值对象</returns>  
        private string ObjectToSoapXml(object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            else
            {
                return o.ToString();
            }
        }

        /// <summary>  
        /// 设置请求身份  
        /// </summary>  
        /// <param name="request"> 请求</param>  
        private void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = TimeOut;  
        }

        /// <summary>  
        /// 将soap协议写入请求  
        /// </summary>  
        /// <param name="request"> 请求</param>  
        /// <param name="data"> soap协议</param>  
        private void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        /// <summary>  
        /// 将响应对象读取为xml对象  
        /// </summary>  
        /// <param name="response"> 响应对象</param>  
        /// <returns> xml对象</returns>  
        private XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }

        /// <summary>  
        /// 给xml文档添加声明  
        /// </summary>  
        /// <param name="doc"> xml文档</param>  
        private void AddDelaration(XmlDocument doc)
        {
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", null, null);
            doc.InsertBefore(decl, doc.DocumentElement);
        }
    }
     
}
