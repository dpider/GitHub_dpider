using pde.log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pde.pub.DataManager
{
    public class SqlServer : DataBase
    {
        private string connString = "Data Source={0},{1};Initial Catalog={2};User Id={3};Password={4};MultipleActiveResultSets=true;"; 
        public string dbVer = "2005";
        private string dbaseName = "";

        public SqlServer()
        {

        } 

        public SqlServer(string ip, string port, string sid, string username, string userpsd, string ver)
        {
            int p = 1433;
            int.TryParse(port, out p);
            dbVer = ver;
            dbaseName = sid;
            string constr = string.Format(connString, ip, p, sid, username, userpsd);
            conn = new SqlConnection(constr);
        }

        public SqlServer(string constr)
        { 
            conn = new SqlConnection(constr); 
        }

  
        public override DataTable RunSql(string sql)
        { 
            try
            {  
                Application.DoEvents();
                if (conn == null)
                {
                    throw new Exception(DBNOTINIT);
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter(); //new SqlDataAdapter(sql, conn);
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (SqlTransaction)transaction;
                }
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            } 
            finally
            {
                if (!inTranscation)
                {
                    conn.Close();
                }
            } 
        }

        public override bool ExecSql(string sql)
        { 
            try
            {  
                Application.DoEvents();
                if (conn == null)
                {
                    throw new Exception(DBNOTINIT);
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (SqlTransaction)transaction;
                }
                int n = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            } 
            finally
            {
                if (!inTranscation)
                {
                    conn.Close();
                }
            }
        }

        public override bool setFile(string table, string field, string IDValue, string filePath)
        { 
            try
            {  
                Application.DoEvents();
                if (conn == null)
                {
                    throw new Exception(DBNOTINIT);
                }
                 
                FileInfo fi = new FileInfo(filePath);
                FileStream fs = fi.OpenRead();
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)conn;
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (SqlTransaction)transaction;
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("UPDATE {0} SET {1} = @file WHERE ID='{2}'", table, field, IDValue);
                LogLocal.log().SaveLog(new LogEntity(cmd.CommandText, LogType.Plat, LogLevel.DEBUG));
                SqlParameter spFile = new SqlParameter("@file", SqlDbType.Image);
                spFile.Value = bytes;
                cmd.Parameters.Add(spFile); 
                int n = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            } 
            finally
            {
                if (!inTranscation)
                {
                    conn.Close();
                }
            }
        }

        public override bool getFile(string table, string field, string IDValue, string filePath)
        { 
            FileStream pFileStream = null;
            try
            { 
                Application.DoEvents();
                if (conn == null)
                { 
                    throw new Exception(DBNOTINIT);
                }
              
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string sql = string.Format("SELECT {0} FROM {1} WHERE ID='{2}'", field, table, IDValue);
                LogLocal.log().SaveLog(new LogEntity(sql, LogType.Plat, LogLevel.DEBUG));
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (SqlTransaction)transaction;
                }
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();

                byte[] bytes = (byte[])dr[0];
                pFileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                pFileStream.Write(bytes, 0, bytes.Length); 
                return true;
            } 
            finally
            {
                if (pFileStream != null)
                {
                    pFileStream.Close();
                }
            }
        }

        public override string getPageSql(string table, string fields, string where, string order, int page, int rows, string key = "ID")
        {
            string sqls = "";
            string orderAsc = "";   //默认
            string orderDesc = "";  
            string orderTbl = "";
            if ("".Equals(order.Trim()))
            {
                orderAsc = key + " ASC";
                orderDesc = key + " DESC"; 
                orderTbl = "T1." + key;
            }
            else
            {
                string[] orderlst = order.Split(',');
                foreach(string odr in orderlst)
                {
                    string tmp = odr.Trim().ToUpper();
                    if (tmp.EndsWith("ASC"))
                    {
                        orderAsc += tmp + ",";
                        orderDesc += "T." + tmp.Replace("ASC", "DESC") + ",";  
                    }
                    else if (tmp.EndsWith("DESC"))
                    {
                        orderAsc += tmp + ",";
                        orderDesc += "T." + tmp.Replace("DESC", "ASC") + ","; 
                    }
                    else
                    {
                        orderAsc += tmp + " ASC,";
                        orderDesc += "T." + tmp + " DESC,"; 
                    }
                    orderTbl = "T1." + orderAsc;
                }
                orderAsc = orderAsc.TrimEnd(',');
                orderDesc = orderDesc.TrimEnd(',');
                orderTbl = orderTbl.TrimEnd(',');
            }

            if (page == 1)
            {
                sqls = string.Format("SELECT TOP {4} {0} FROM {1} WHERE {2} ORDER BY {3}",
                                     fields, table, where, orderAsc, rows);
            }
            else
            {
                if (dbVer.Equals("2000"))
                {
                    //支持低版本SQLSERVER 
                    int counts = 0;
                    sqls = string.Format("SELECT COUNT({1}) FROM {0}", table, key);
                    if (!where.Equals(""))
                    {
                        sqls += string.Format(" WHERE {0}", where);
                    }
                    DataTable dt = null;
                    try
                    {
                        dt = RunSql(sqls);
                        counts = int.Parse(dt.Rows[0][0].ToString());
                    } 
                    finally
                    {
                        dt = null;
                    }
                    int r = (counts - (page - 1) * rows);
                    sqls = string.Format("SELECT {0} FROM {1} T1, (" +
                                            "SELECT TOP {6} {9} FROM " +
                                               "(SELECT TOP {7} {9}{8} FROM {1} WHERE {2} ORDER BY {3}) T ORDER BY {4}) T2 " +
                                         "WHERE T1.{9} = T2.{9} ORDER BY {5}",
                                          formatFields(fields, "T1"), table, where, orderAsc, orderDesc, orderTbl, r < rows ? r : rows, page * rows, ("".Equals(order.Trim())) ? "" : " ," + order, key);
                }
                else
                {
                    //支持SQLServer2005以上版本
                    sqls = string.Format("SELECT {0} FROM (SELECT TOP {1} ROW_NUMBER() OVER(ORDER BY {2}) AS ROWID, * FROM {3} WHERE {4}) AS TEMP1 WHERE ROWID>{5}",
                                          fields, page * rows, orderAsc, table, where, (page - 1) * rows);
                }
            }
            return sqls;
        } 

        private string formatFields(string fields, string fix)
        {
            string[] f = fields.Split(',');
            StringBuilder sb = new StringBuilder();
            foreach (string s in f)
            {
                sb.Append("," + fix + "." + s.Trim());
            }
            return sb.ToString().Substring(1);
        }

        public override string getDateFormat(DateTime value, string format = "yyyy-MM-dd HH:mm:ss")
        { 
            return "'" + value.ToString(format) + "'"; 
        }

        public override string formatDateField(string field, string format)
        {
            if (format.Equals(""))
            {
                return field;
            }
            else
            {
                int _format = 20; // 默认 yyyy-MM-dd HH:mm:ss
                if (format.Equals("yyyy-MM-dd"))
                {
                    _format = 23;
                }
                else if (format.Equals("HH:mm:ss"))
                {
                    _format = 108;
                }
                //待扩充，暂不做补充
                return string.Format("CONVERT(varchar(100), {0}, {1})", field, _format);
            }
        }


        public override string getMachineCode()
        {
            string sql = string.Format("select convert(varchar(20),database_id)+convert(varchar(100),create_date,120) as MACCODESOURCE FROM sys.databases where name='{0}'", dbaseName);
            DataTable dt = RunSql(sql);
            if (dt.Rows.Count > 0)
            {
                string code = dt.Rows[0]["MACCODESOURCE"].ToString();
                return code.Equals("") ? "" : CryptUtil.GetMd5Hash(code, true);
            }
            return "";
        }

        public override DateTime getServerTime()
        {
            DataTable dt = RunSql("select CONVERT(varchar,GETDATE(),120)");
            if (dt.Rows.Count > 0)
            {
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
                return DateTime.Parse(dt.Rows[0][0].ToString(), dtFormat);
            }
            return DateTime.Now;
        }
    }
}
