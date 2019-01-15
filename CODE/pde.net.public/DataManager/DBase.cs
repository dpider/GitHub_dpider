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
using System.Data.Odbc;

namespace pde.pub.DataManager
{
    public class DBase : DataBase
    { 
        //通过 vfpoledb 访问
        private string connString = @"Provider=VFPOLEDB.1;Data Source={0};Collating Sequence=MACHINE";
        //通过 Microsoft Visual FoxPro Driver 访问
        // private string connString = @"Driver={Microsoft Visual FoxPro Driver}; SourceType=DBF; sourcedb={0}; BACKGROUNDFETCH=NO; DELETE=NO";
        //通过 Microsoft dBASE Driver 访问
        // private string connString = @"Driver={Microsoft dBASE Driver (*.dbf)}; SourceType=DBF; Data Source={0}; Exclusive=No; NULL=NO; Collate=Machine; BACKGROUNDFETCH=NO; DELETE=NO";

        public DBase()
        { 
           
        }

        public DBase(string path, string psd)
        {  
            if ("".Equals(path))
            {
                throw new Exception("没有数据库文件！");
            }
            // string constr = connString + path; 
            string constr = string.Format(connString, path);
            conn = new OleDbConnection(constr);
        }

        public DBase(string constr)
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

                OdbcDataAdapter da = new OdbcDataAdapter();
                OdbcCommand cmd = new OdbcCommand(sql, (OdbcConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (OdbcTransaction)transaction;
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
                OdbcCommand cmd = new OdbcCommand(sql, (OdbcConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (OdbcTransaction)transaction;
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

        public override string getPageSql(string table, string fields, string where, string order, int page, int rows, string key = "ID")
        {
            string sqls = "";
            if ("".Equals(order.Trim()))
            {
                sqls = string.Format("SELECT {0} FROM {1} WHERE {5} BETWEEN " +
                                     "(SELECT MIN({5}) FROM (SELECT TOP {2} {5} FROM {1} WHERE {4})) " +
                                     "AND " +
                                     "(SELECT MIN({5}) FROM (SELECT TOP {3} {5} FROM {1} WHERE {4}))",
                                    fields, table, (page - 1) * rows, page * rows, where, key);
            }
            else
            {
                sqls = string.Format("SELECT {0} FROM {1} WHERE {6} BETWEEN " +
                                     "(SELECT MIN({6}) FROM (SELECT TOP {2} {6} FROM {1} WHERE {4} ORDER BY {5})) " +
                                     "AND " +
                                     "(SELECT MIN({6}) FROM (SELECT TOP {3} {6} FROM {1} WHERE {4} ORDER BY {5})) " +
                                     "ORDER BY {5}",
                                    fields, table, (page - 1) * rows, page * rows, where, order, key);
            }
            return sqls;
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
    }
}
