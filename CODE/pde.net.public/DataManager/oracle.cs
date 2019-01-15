using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace pde.pub.DataManager
{
    public class Oracle : DataBase
    {

        private string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));Persist Security Info=True;User ID={3};Password={4};";
        public string dbVer = "";

        public Oracle()
        {

        }


        public Oracle(string ip, string port, string sid, string username, string userpsd, string ver)
        {
            int p = 1521;
            int.TryParse(port, out p);
            dbVer = ver;
            // string constr = string.Format(connString, "61.155.202.11", 1521, "ORCL", "sms", "sms"); 
            string constr = string.Format(connString, ip, p, sid, username, userpsd);
            conn = new OracleConnection(constr);
        }

        public Oracle(string constr)
        {
            conn = new OracleConnection(constr);
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

                OracleDataAdapter da = new OracleDataAdapter();
                OracleCommand cmd = new OracleCommand(sql, (OracleConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (OracleTransaction)transaction;
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
                OracleCommand cmd = new OracleCommand(sql, (OracleConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (OracleTransaction)transaction;
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
                sqls = string.Format("SELECT {0} FROM (" +
                                         "SELECT ROWNUM AS rowno, t.* FROM {1} t WHERE {2} AND ROWNUM <= {3}" +
                                      ") table_alias WHERE table_alias.rowno > {4}",
                                   fields, table, where, page * rows, (page - 1) * rows);
            }
            else
            {
                sqls = string.Format("SELECT {0} FROM (" +
                                        "SELECT tt.*, ROWNUM AS rowno FROM (" +
                                              "SELECT t.* FROM {1} t WHERE {2} ORDER BY {3}" +
                                        ") tt WHERE ROWNUM <= {4}" +
                                     ") table_alias WHERE table_alias.rowno > {5}",
                                   fields, table, where, order, page * rows, (page - 1) * rows);
            }
            return sqls;
        }

        public override string getDateFormat(DateTime value, string format = "yyyy-MM-dd HH:mm:ss")
        {
            string _format = format.Replace("yyyy", "YYYY")
                                   .Replace("yy", "YY")
                                   .Replace("dd", "DD")
                                   .Replace("d", "D")
                                   .Replace("HH:", "HH24:")
                                   .Replace("H:", "H24:")
                                   .Replace("hh:", "HH12:")
                                   .Replace("h:", "H12:")
                                   .Replace("mm", "MI")
                                   .Replace("ss", "SS")
                                   .Replace("s", "S");
            return string.Format("to_date('{0}', '{1}')", value.ToString(format), _format);
        }

        public override string formatDateField(string field, string format)
        {
            if (format.Equals(""))
            {
                return field;
            }
            else
            {
                string _format = format.Replace("yyyy", "YYYY")
                                   .Replace("yy", "YY")
                                   .Replace("dd", "DD")
                                   .Replace("d", "D")
                                   .Replace("HH:", "HH24:")
                                   .Replace("H:", "H24:")
                                   .Replace("hh:", "HH12:")
                                   .Replace("h:", "H12:")
                                   .Replace("mm", "MI")
                                   .Replace("ss", "SS")
                                   .Replace("s", "S");
                return string.Format("TO_CHAR({0}, {1})", field, _format);
            }
        }

        /*
        public override void BeginTransaction()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
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
            string sql = string.Format("select OBJECT_ID||to_char(CREATED,''yyyy-MM-dd HH24:mi:ss'') AS MACCODESOURCE from USER_OBJECTS WHERE OBJECT_NAME='SYS_REG'");
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
            DataTable dt = RunSql("select to_char(sysdate,'yyyy-MM-dd HH24:mi:ss') from dual");
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
