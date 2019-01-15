using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pde.pub
{
    class JsonUtil
    {     
        //Hashtable转换为JSON
        public static string hashtableToJsonString(Hashtable ht)
        {
            string json = "";
            IDictionaryEnumerator en = ht.GetEnumerator();  //  遍历哈希表所有的键,读出相应的值
            while (en.MoveNext())
            {
                if (en.Value.GetType() == typeof(List<Hashtable>))
                {
                    if (json.Equals(""))
                        json = string2Json(en.Key.ToString()) + ":" + listToJsonString((List<Hashtable>)en.Value);
                    else
                        json = json + "," + string2Json(en.Key.ToString()) + ":" + listToJsonString((List<Hashtable>)en.Value);
                }
                else
                {
                    if (json.Equals(""))
                        json = string2Json(en.Key.ToString()) + ":" + string2Json(en.Value.ToString());
                    else
                        json = json + "," + string2Json(en.Key.ToString()) + ":" + string2Json(en.Value.ToString());
                }
            }
            return "{" + json + "}";

        }
        public static string listToJsonString(List<Hashtable> list)
        {
            string json = "";
            foreach (Hashtable en in list)   //  遍历
            {
                if (json.Equals(""))
                    json = hashtableToJsonString(en);
                else
                    json = json + "," + hashtableToJsonString(en);
            }
            return "[" + json + "]";
        }

        //处理JSON中特殊字符的转义
        public static String string2Json(String s)
        {
            StringBuilder sb = new StringBuilder(s.Length + 20);
            sb.Append('"');
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
            return sb.ToString();
        }
    }
}
