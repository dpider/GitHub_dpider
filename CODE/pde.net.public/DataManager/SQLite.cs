using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;
using pde.pub;
using System.Windows.Forms;
using pde.log;

namespace pde.pub.DataManager
{
    public class SQLite : DataBase
    { 
        string conn_str = ""; 

        #region 自有方法
        //创建数据库文件
        public static bool createDB(string dbName, out string errMsg)
        {
            errMsg = "";
            if (File.Exists(dbName))
            {
                errMsg = "数据库已经存在";
                return false;
            }
            else
            {
                try
                {
                    SQLiteConnection.CreateFile(dbName);
                    return true;
                }
                catch(Exception ex)
                {
                    errMsg = ex.Message;
                    return false;
                }
            }
        }

        //----修改数据库密码----
        public bool ChangePassword(string newPassword)
        {
            try
            {
                ((SQLiteConnection)conn).ChangePassword(newPassword);
                return true;
            }
            catch (SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool TableExists(string table)
        {
            string sql = string.Format("select * from sqlite_master where[type] = 'table' and[name] = '{0}'", table);
            DataTable dt = RunSql(sql, null);
            if (dt == null)
            {
                return false;
            }
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 判断视图是否存在
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public bool ViewExists(string view)
        {
            string sql = string.Format("select * from sqlite_master where[type] = 'view' and[name] = '{0}'", view);
            DataTable dt = RunSql(sql, null);
            if (dt == null)
            {
                return false;
            }
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool DropTable(string table)
        {
            try
            {
                using (SQLiteTransaction Transaction = ((SQLiteConnection)conn).BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = (SQLiteConnection)conn;
                        cmd.CommandText = string.Format("DROP TABLE IF EXISTS {0}", table);
                        cmd.ExecuteNonQuery();
                    }
                    Transaction.Commit();
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 收缩数据库 VACUUM 
        /// </summary>
        /// <returns></returns>
        public bool Vacuum()
        {
            try
            {
                using (SQLiteCommand Command = new SQLiteCommand("VACUUM", (SQLiteConnection)conn))
                {
                    Command.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                throw new Exception(ex.Message); 
            } 
        }

        //打开数据连接
        public bool Connect()
        {
            try
            { 
                conn.Open();
                if (conn == null)
                {
                    return false;
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                throw new Exception(ex.Message); 
            }
        }
 

        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public DataTable RunSql(string sql, SQLiteParameter[] parameters)
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
                SQLiteDataAdapter da = new SQLiteDataAdapter();
                SQLiteCommand cmd = new SQLiteCommand(sql, (SQLiteConnection)conn);
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (SQLiteTransaction)transaction;
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
         
        /// <summary> 
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。 
        /// </summary> 
        /// <param name="sql">要执行的增删改的SQL语句</param> 
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public int ExecSql(string sql, SQLiteParameter[] parameters)
        {
            int affectRows = 0;
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
                SQLiteCommand cmd = new SQLiteCommand(sql, (SQLiteConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (SQLiteTransaction)transaction;
                }
                affectRows = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (SQLiteException ex)
            {
                affectRows = -1;
                throw new Exception(ex.Message);
            }
            finally
            { 
                if (!inTranscation)
                {
                    conn.Close();
                }
            } 
            return affectRows;
        } 
        #endregion

        //----创建连接串并连接数据库----
        public SQLite(string path, string password)
        {
            conn_str = "data source=" + path + ";password=" + password;
            conn = new SQLiteConnection(conn_str);
        }
         

        public override DataTable RunSql(string sql)
        {
            return RunSql(sql, null);
        }
         
        public override bool ExecSql(string sql)
        {
            return ExecSql(sql, null) > -1;
        }

        public override string getPageSql(string table, string fields, string where, string order, int page, int rows, string key = "ID")
        {
            //offset代表从第几条记录“之后“开始查询，limit表明查询多少条结果
            if ("".Equals(order.Trim()))
            {
                return string.Format("select {0} from {1} where {2} limit {3} offset {4}",
                                        fields, table, where, rows, (page - 1) * rows);
            }
            else
            {
                return string.Format("select {0} from {1} where {2} order by {3} limit {4} offset {5}",
                                     fields, table, where, order, rows, (page - 1) * rows);
            }
        }

        public override string getDateFormat(DateTime value, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return string.Format("'{0}'", value.ToString(format));  
        }

        public override string formatDateField(string field, string format)
        {
            string _format = format.Replace("yyyy", "%Y")
                                   .Replace("MM", "%m")
                                   .Replace("dd", "%d")
                                   .Replace("HH:", "%H")
                                   .Replace("mm:", "%M")
                                   .Replace("ss", "%S"); 
            return string.Format("strftime('{0}', {1})", _format, field); 
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
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = (SQLiteConnection)conn;
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (SQLiteTransaction)transaction;
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format("UPDATE {0} SET {1} = :file WHERE ID='{2}'", table, field, IDValue);
                LogLocal.log().SaveLog(new LogEntity(cmd.CommandText, LogType.Plat, LogLevel.DEBUG));
                cmd.Parameters.Add("file", DbType.Binary).Value = bytes; 
                int n = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
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
                SQLiteCommand cmd = new SQLiteCommand(sql, (SQLiteConnection)conn);
                if ((inTranscation) && (transaction != null))
                {
                    cmd.Transaction = (SQLiteTransaction)transaction;
                }
                SQLiteDataReader dr = cmd.ExecuteReader();
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
