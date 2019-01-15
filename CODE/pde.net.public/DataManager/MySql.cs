using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pde.pub.DataManager
{
    public class MySql : DataBase
    {
        private string connString = "server={0}; port={1}; user id={2}; password={3}; database={4}; pooling=false; charset=utf8";
      //  private MySqlConnection conn = null;
      //  private MySqlTransaction transaction = null;
        public string dbVer = "";

        public MySql()
        {

        }
         

        public MySql(string ip, string port, string sid, string username, string userpsd, string ver)
        {
            int p = 1521;
            int.TryParse(port, out p);
            dbVer = ver;
            string constr = string.Format(connString, ip, p, username, userpsd, sid);
            conn = new MySqlConnection(constr);
        }

        public MySql(string constr)
        { 
            conn = new MySqlConnection(constr);
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

                MySqlDataAdapter da = new MySqlDataAdapter();  
                MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (MySqlTransaction)transaction;
                }
                da.SelectCommand = cmd;
                //MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
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
                MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (MySqlTransaction)transaction;
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

        public override string getPageSql(string table, string fields, string where, string order, int page, int rows, string key = "ID")
        {
            string sqls = "";
            if ("".Equals(order.Trim()))
            {
                sqls = string.Format("SELECT {0} FROM {1} WHERE {2} LIMIT {3},{4}", fields, table, where, (page - 1) * rows, rows);
            }
            else
            {
                sqls = string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3} LIMIT {4},{5}", fields, table, where, order, (page - 1) * rows, rows);
            }           
            return sqls;
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
                string _format = format.Replace("yyyy", "%Y")
                                     .Replace("yy", "%y")
                                     .Replace("MM", "%m")
                                     .Replace("M", "%c")
                                     .Replace("dd", "%d")
                                     .Replace("d", "%e")
                                     .Replace("HH:", "%H")
                                     .Replace("H:", "%k")
                                     .Replace("hh:", "%h")
                                     .Replace("h:", "%l")
                                     .Replace("mm", "%i")
                                     .Replace("ss", "%S")
                                     .Replace("s", "%s");
                return string.Format("DATE_FORMAT({0}, {1})", field, _format);
            }
        }

        /*
        public override void BeginTransaction()
        {
            conn.Open();
            transaction = conn.BeginTransaction();
            inTranscation = true;
        }

        public override void CommitTransaction()
        {
            transaction.Commit();
            inTranscation = false;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public override void RollbackTransaction()
        {
            transaction.Rollback();
            inTranscation = false;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }*/

        public override string getMachineCode()
        {
            string sql = string.Format("SELECT date_format(CREATE_TIME,'%Y-%m-%d %T') MACCODESOURCE  FROM information_schema.TABLES where  table_schema=DATABASE() and table_name='SYS_REG'");
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
            DataTable dt = RunSql("select date_format(now(),'%Y-%m-%d %T')");
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
