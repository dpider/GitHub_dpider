using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace pde.pub.DataManager
{ 
    public class DataBase
    {
        public string DBNOTINIT = "数据库连接未初始化！";
        public string DATASETNULL = "数据集为空！";

        public DbConnection conn = null;
        public DbTransaction transaction = null;

        public DataBase()
        {
            
        }

        public virtual void CloseConnect()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
         
        public virtual bool testCon()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            conn.Close();
            return true;
        }

        public virtual DataTable RunSql(string sql)
        { 
            return null;
        }

        public virtual bool ExecSql(string sql)
        { 
            return true;
        }

        public bool inTranscation = false;
        public virtual void BeginTransaction()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            transaction = conn.BeginTransaction();
            inTranscation = true;
        }

        public virtual void CommitTransaction()
        {
            transaction.Commit();
            inTranscation = false;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public virtual void RollbackTransaction()
        {
            transaction.Rollback();
            inTranscation = false;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="fields">查询字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序条件，为空时表示没有排序条件</param>
        /// <param name="page">查询页数</param>
        /// <param name="rows">查询每页条数</param> 
        /// <returns>分页查询语句</returns>
        public virtual string getPageSql(string table, string fields, string where, string order, int page, int rows, string key = "ID") { return ""; }
         

        public virtual string getDateFormat(DateTime value, string format= "yyyy-MM-dd HH:mm:ss")
        {
            return value.ToString(format);
        }

        public virtual string formatDateField(string field, string format)
        {
            return field;
        }

        /// <summary>
        /// 在数据库中写入文件
        /// </summary>
        /// <param name="table">数据表名</param>
        /// <param name="field">文件字段名</param>
        /// <param name="filePath">文件完整路径</param> 
        /// <param name="bThrowOut"></param>
        /// <returns></returns>
        public virtual bool setFile(string table, string field, string IDValue, string filePath)
        { 
            return true;
        }

        /// <summary>
        /// 从数据库中读取文件
        /// </summary>
        /// <param name="table">数据表名</param>
        /// <param name="field">文件字段名</param>
        /// <param name="IDValue">ID值（条件）</param>
        /// <param name="filePath">文件输出完整路径</param> 
        /// <param name="bThrowOut"></param>
        /// <returns></returns>
        public virtual bool getFile(string table, string field, string IDValue, string filePath)
        { 
            filePath = "";
            return false;
        }

        /// <summary>
        /// 获取数据库的创建时间，并生成机器码，如果是单机数据库则获取机器CPUID，并MD5加密，默认取机本地CPUID码
        /// </summary>
        /// <returns></returns>
        public virtual string getMachineCode()
        {
            string cpuID = HardWare.GetCpuID();
            return cpuID.Equals("") ? "" : CryptUtil.GetMd5Hash(cpuID, true);
        }

        /// <summary>
        /// 获取服务器时间，取数据库时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime getServerTime()
        {
            return DateTime.Now;
        }
    }
}
