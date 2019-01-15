using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb; 

namespace pde.pub.DataManager
{
    public class Access : DataBase
    {  
        private string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=True;Jet OLEDB:Database Password={1}";
      //  private OleDbConnection conn = null;
      //  private OleDbTransaction transaction = null;

        public Access()
        { 
           
        }

        public Access(string path, string psd)
        {  
            if ("".Equals(path))
            {
                throw new Exception("没有数据库文件！");
            }
            string constr = string.Format(connString, path, psd); 
            conn = new OleDbConnection(constr);
        }

        public Access(string constr)
        { 
            conn = new OleDbConnection(constr);
        }

        public override bool testCon()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            conn.Close();
            return true;
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
                conn.Open();

                OleDbDataAdapter da = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand(sql, (OleDbConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (OleDbTransaction)transaction;
                }
                cmd.CommandType = CommandType.Text;
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt); 
                return dt;
            } 
            finally
            {
                conn.Close();
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
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sql, (OleDbConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (OleDbTransaction)transaction;
                }
                cmd.CommandType = CommandType.Text;
                int n = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true; 
            } 
            finally
            {
                conn.Close();
            }
        }

        public override string getPageSql(string table, string fields, string where, string order, int page, int rows, string key= "ID")
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
                foreach (string odr in orderlst)
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

            return sqls;
           /* string sqls = "";
            if ("".Equals(order.Trim()))
            {
                sqls = string.Format("SELECT {0} FROM {1} WHERE {5} BETWEEN " +
                                     "(SELECT MIN({5}) FROM (SELECT TOP {2} {5} FROM {1} WHERE {4})) " +
                                     "AND " +
                                     "(SELECT MIN({5}) FROM (SELECT TOP {3} {5} FROM {1} WHERE {4}))",
                                    fields, table, (page - 1) * rows, page * rows, where, key);
                sqls = string.Format("SELECT {0} FROM {1} WHERE {5} BETWEEN " +
                                     "(SELECT MIN({5}) FROM (SELECT TOP {2} {5} FROM {1} WHERE {4})) " +
                                     "AND " +
                                     "(SELECT MIN({5}) FROM (SELECT TOP {3} {5} FROM {1} WHERE {4}))",
                                    fields, table, (page - 1) * rows, page * rows, where, key);
            }
            else
            {
                sqls = string.Format("SELECT {0} FROM {1} WHERE ID BETWEEN " +
                                     "(SELECT MIN({6}) FROM (SELECT TOP {2} {6} FROM {1} WHERE {4} ORDER BY {5})) " +
                                     "AND " +
                                     "(SELECT MIN({6}) FROM (SELECT TOP {3} {6} FROM {1} WHERE {4} ORDER BY {5})) " +
                                     "ORDER BY {5}",
                                    fields, table, (page - 1) * rows, page * rows, where, order, key);
            }
            return sqls;*/
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
            return string.Format("#{0}#", value.ToString(format));
        }

        public override string formatDateField(string field, string format)
        {
            return field;
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
        }
        */

        public DataTable getTables()
        {
            try
            {
                Application.DoEvents();
                if (conn == null)
                {
                    throw new Exception(DBNOTINIT);
                }
                conn.Open();
                DataTable dt = ((OleDbConnection)conn).GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });  
                return dt;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
