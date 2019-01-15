using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Specialized;
using pde.log;

namespace pde.pub.DataManager
{
    public class DataODBCService : DataService 
    {
        public DataBase db;


        public DataODBCService()
        {

        }

        //非本地数据库初始化
        public DataODBCService(DataBaseType dbt, string ip, string port, string dbase, string user, string password, string ver)
        {
            switch (dbt)
            {
                case DataBaseType.dbtOracle:
                    db = new Oracle(ip, port, dbase, user, password, ver);
                    break;

                case DataBaseType.dbtMSSQL:
                    db = new SqlServer(ip, port, dbase, user, password, ver);
                    break;

                case DataBaseType.dbtMySQL:
                    db = new MySql(ip, port, dbase, user, password, ver);
                    break; 

                default:
                    throw new Exception("没有设置数据库类型！");
            }
        }

        //本地数据库初始化
        public DataODBCService(DataBaseType dbt, string dbfile, string password)
        {
            switch (dbt)
            { 
                case DataBaseType.dbtAccess:
                    db = new Access(dbfile, password);
                    break;

                case DataBaseType.dbtSQLite:
                    db = new SQLite(dbfile, password);
                    break;

                default:
                    throw new Exception("没有设置数据库类型！");
            }
        }

        /// <summary>
        /// 生成插入脚本
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="KV">字段与值的键值对</param>
        /// <returns></returns>
        private string getInsertSql(string tableName, Dictionary<string, object> KV)
        {
            string fields = "";
            string values = "";
            foreach (KeyValuePair<string, object> kvp in KV)
            {
                fields += string.Format(", {0}", kvp.Key); 
                if (kvp.Value is int)
                {
                    values += string.Format(", {0}", kvp.Value);
                }
                else if (kvp.Value is DateTime)
                {
                    values += string.Format(", {0}", db.getDateFormat((DateTime)kvp.Value));
                }
                else
                {
                    values += string.Format(", '{0}'", kvp.Value);
                }
            }
            if (fields.StartsWith(","))
            {
                fields = fields.Substring(1);
            }
            if (values.StartsWith(","))
            {
                values = values.Substring(1);
            }
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, fields, values);
        }

        #region 拼凑SQL脚本，根据自定义的数据结构，拼成标准的sql条件语句        
        private string getSQLDataString(List<SQLDataStruct> sqlData)
        {
            string temp = "";
            foreach (SQLDataStruct data in sqlData)
            { 
                temp += string.Format(" {0}", getSQLDataString(data)); 
            } 
            return temp;
        }

        private string getSQLDataString(SQLDataStruct sqlData)
        {
            string connect = "";
            if (!sqlData.Connector.Equals(""))
            {
                connect = string.Format(" {0} ", sqlData.Connector);
            }

            switch (sqlData.Symbol)
            {
                case SQLSymbol.ssNone:  //如果是ssNone，一般用于只填充前括号或者后括号之类
                    return string.Format("{0}{1}{2}",
                        connect,
                        sqlData.Before, 
                        sqlData.After);

                case SQLSymbol.ssEqual:
                    return string.Format("{0}{1}({2}={3}){4}", 
                        connect, 
                        sqlData.Before, 
                        getSQLField(sqlData.FieldName, sqlData.FieldValue, sqlData.Format), 
                        getSQLValue(sqlData.FieldValue, sqlData.Format), 
                        sqlData.After); 

                case SQLSymbol.ssLike:
                    return string.Format("{0}{1}({2} LIKE '%{3}%'){4}", 
                        connect, 
                        sqlData.Before, 
                        sqlData.FieldName, 
                        (string)sqlData.FieldValue, 
                        sqlData.After); 

                case SQLSymbol.ssLikeLeft:
                    return string.Format("{0}{1}({2} LIKE '%{3}'){4}", 
                        connect, 
                        sqlData.Before, 
                        sqlData.FieldName, 
                        (string)sqlData.FieldValue, 
                        sqlData.After); 

                case SQLSymbol.ssLikeRight:
                    return string.Format("{0}{1}({2} LIKE '{3}%'){4}", 
                        connect, 
                        sqlData.Before, 
                        sqlData.FieldName, 
                        (string)sqlData.FieldValue, 
                        sqlData.After); 

                case SQLSymbol.ssIn:
                    return string.Format("{0}{1}({2} IN {3}){4}", 
                        connect, 
                        sqlData.Before, 
                        sqlData.FieldName, 
                        getSQLValue(sqlData.FieldValue, sqlData.Format), 
                        sqlData.After);

                case SQLSymbol.ssLessThen:
                    return string.Format("{0}{1}({2}<{3}){4}",
                        connect,
                        sqlData.Before,
                        getSQLField(sqlData.FieldName, sqlData.FieldValue, sqlData.Format),
                        getSQLValue(sqlData.FieldValue, sqlData.Format),
                        sqlData.After);

                case SQLSymbol.ssLessThenOrEqual:
                    return string.Format("{0}{1}({2}<={3}){4}",
                        connect,
                        sqlData.Before,
                        getSQLField(sqlData.FieldName, sqlData.FieldValue, sqlData.Format),
                        getSQLValue(sqlData.FieldValue, sqlData.Format),
                        sqlData.After);

                case SQLSymbol.ssMoreThen:
                    return string.Format("{0}{1}({2}>{3}){4}",
                        connect,
                        sqlData.Before,
                        getSQLField(sqlData.FieldName, sqlData.FieldValue, sqlData.Format),
                        getSQLValue(sqlData.FieldValue, sqlData.Format),
                        sqlData.After);

                case SQLSymbol.ssMoreThenOrEqual:
                    return string.Format("{0}{1}({2}>={3}){4}",
                        connect,
                        sqlData.Before,
                        getSQLField(sqlData.FieldName, sqlData.FieldValue, sqlData.Format),
                        getSQLValue(sqlData.FieldValue, sqlData.Format),
                        sqlData.After);
                default:
                    return "";
            } 
        }
         
        private string getSQLField(string field, object value, string format)
        {
            if (value is DateTime)
            {
                return db.formatDateField(field, format);
            }
            else
            {
                return field;
            }
        }

        private string getSQLValue(object value, string format)
        {
            if (value is int)
            { 
                return Convert.ToInt32(value).ToString();
            }
            else if (value is DateTime)
            {
                return db.getDateFormat((DateTime)value, format);
            }
            else if (value is SQLDataStruct)
            {
                return getSQLDataString((SQLDataStruct)value);
            }
            else if (value is List<SQLDataStruct>)
            {
                return getSQLDataString((List<SQLDataStruct>)value);
            }
            else
            {
                return string.Format("'{0}'", (string)value);
            }
        }
        #endregion


        /// <summary>
        /// 生成更新脚本
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ID">主键值</param>
        /// <param name="KV">字段与值的键值对</param>
        /// <returns></returns>
        private string getUpdateSql(string tableName, string ID, Dictionary<string, object> KV)
        {
            string temp = "";
            foreach (KeyValuePair<string, object> kvp in KV)
            {
                if (kvp.Value is int)
                {
                    temp += string.Format(", {0}={1}", kvp.Key, kvp.Value);
                }
                else if (kvp.Value is DateTime)
                {
                    temp += string.Format(", {0}={1}", kvp.Key, db.getDateFormat((DateTime)kvp.Value));
                }
                else
                {
                    temp += string.Format(", {0}='{1}'", kvp.Key, kvp.Value);
                }
            }
            if (temp.StartsWith(","))
            {
                temp = temp.Substring(1);
            }
            return string.Format("UPDATE {0} SET {1} WHERE ID='{2}'", tableName, temp, ID);
        }

        private string getUpdateSql(string tableName, Dictionary<string, object> KV, List<SQLDataStruct> WhereData)
        {
            string temp = "";
            foreach (KeyValuePair<string, object> kvp in KV)
            {
                if (kvp.Value is int)
                {
                    temp += string.Format(", {0}={1}", kvp.Key, kvp.Value);
                }
                else if (kvp.Value is DateTime)
                {
                    temp += string.Format(", {0}={1}", kvp.Key, db.getDateFormat((DateTime)kvp.Value));
                }
                else
                {
                    temp += string.Format(", {0}='{1}'", kvp.Key, kvp.Value);
                }
            }
            if (temp.StartsWith(","))
            {
                temp = temp.Substring(1);
            }

            string where = getSQLDataString(WhereData);
            if (where.Equals(""))
            {
                return string.Format("UPDATE {0} SET {1}", tableName, temp);
            }
            else
            {
                return string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, temp, where);
            }
        }
         

        /// <summary>
        /// 生成完整删除脚本
        /// </summary>
        /// <param name="tableName">表名</param> 
        /// <param name="KV">删除条件的字段与值的键值对</param>
        /// <returns></returns>
        private string getDeleteSql(string tableName, List<SQLDataStruct> WhereData)
        {
            string temp = getSQLDataString(WhereData);
            if (temp.Equals(""))
            {
                return string.Format("DELETE FROM {0}", tableName);
            }
            else
            {
                return string.Format("DELETE FROM {0} WHERE {1}", tableName, temp);
            } 
        }
         

        public override bool testCon()
        {  
            try
            { 
                return db.testCon();
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
                return false;
            }
        }

        public override void CloseConnect()
        {
            try
            {
                db.CloseConnect();
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message; 
            }
        }

        public override bool addData(string table, Dictionary<string, object> KV, bool throwException = false)
        {
            if (KV.Count == 0) { return false; }
            string sqls = getInsertSql(table, KV);
            return ExecSql(sqls, throwException);
        }

        public override bool updateData(string table, string ID, Dictionary<string, object> KV, bool throwException = false)
        {
            if (KV.Count == 0) { return false; }
            string sqls = getUpdateSql(table, ID, KV);
            return ExecSql(sqls, throwException);
        }

        public override bool updateData(string table, Dictionary<string, object> KV, List<SQLDataStruct> WhereData, bool throwException = false)
        {
            if (KV.Count == 0) { return false; }
            string sqls = getUpdateSql(table, KV, WhereData);
            return ExecSql(sqls, throwException);
        }

        public override bool deleteData(string table, List<SQLDataStruct> WhereData, bool throwException = false)
        {
            string sqls = getDeleteSql(table, WhereData);
            return ExecSql(sqls, throwException);
        }

        public override int getDataCount(string table, List<SQLDataStruct> WhereData, bool throwException = false)
        {
            string whereStr = getSQLDataString(WhereData);
            return getDataCount(table, whereStr, throwException);
        }

        public override int getDataCount(string table, string where, bool throwException = false)
        {
            string sqls = string.Format("SELECT COUNT(ID) FROM {0}", table);
            if (!where.Equals(""))
            {
                sqls += string.Format(" WHERE {0}", where);
            } 
             
            try
            {
                DataTable dt = RunSql(sqls, throwException);
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception e)
            {
                LastErrorMessage = e.Message;
                if (throwException)
                {
                    throw new Exception(e.Message);
                }
                return 0;
            } 
        }

        public override DataTable getData(string table, string fields, string orders, List<SQLDataStruct> WhereData, int page = 0, int rows = 0, bool throwException = false)
        {
            string whereStr = getSQLDataString(WhereData);
            return getData(table, fields, orders, whereStr, page, rows, throwException);
        }

        public override DataTable getData(string table, string fields, string orders, string where, int page = 0, int rows = 0, bool throwException = false)
        {
            string sqls = "";
            string whereString = where.Trim();
            if (whereString.Equals(""))
            {
                whereString = "(1=1)";
            }

            if ((page == 0) || (rows == 0))
            {
                sqls = string.Format("SELECT {0} FROM {1} WHERE {2}", fields, table, whereString);
 
                if (!orders.Trim().Equals(""))
                {
                    sqls += string.Format(" ORDER BY {0}", orders);
                } 
            }
            else
            {
                sqls = db.getPageSql(table, fields, whereString, orders, page, rows);
            }

            return RunSql(sqls, throwException); 
        }

        public override DataTable getGroupData(string table, string groupField, List<SQLDataStruct> whereData, bool throwException = false)
        {
            string whereString = getSQLDataString(whereData);
            return getGroupData(table, groupField, whereString, throwException);
        }

        public override DataTable getGroupData(string table, string groupField, string whereString, bool throwException = false)
        {
            string where = "";
            if (!whereString.Equals(""))
            {
                where += string.Format(" WHERE {0}", whereString);
            }

            string sqls = string.Format("SELECT {0} FROM {1}{2} group by {0} order by {0}", groupField, table, where);
            return RunSql(sqls, throwException); 
        }

        public override bool ExecSql(string sql, bool throwException = false)
        { 
            try
            {
                LogLocal.log().SaveLog(new LogEntity(sql, LogType.Plat, LogLevel.DEBUG));
                return db.ExecSql(sql);
            }
            catch(Exception ex)
            {
                LastErrorMessage = ex.Message;
                LogLocal.log().SaveLog(new LogEntity(ex.Message, LogType.Plat, LogLevel.ERROR));
                if (throwException)
                {
                    throw new Exception(LastErrorMessage);
                }
                return false; 
            } 
        }

        public override DataTable RunSql(string sql, bool throwException = false)
        {
            try
            {
                LogLocal.log().SaveLog(new LogEntity(sql, LogType.Plat, LogLevel.DEBUG));
                DataTable dt = db.RunSql(sql);
                if (dt == null)
                {
                    throw new Exception(db.DATASETNULL);
                }
                return dt;
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
                LogLocal.log().SaveLog(new LogEntity(ex.Message, LogType.Plat, LogLevel.ERROR));
                if (throwException)
                {
                    throw new Exception(LastErrorMessage);
                } 
                return null;
            } 
        }

        public override void BeginTransaction()
        {
            try
            {
                db.BeginTransaction();
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(ex.Message, LogType.Plat, LogLevel.ERROR));
                LastErrorMessage = ex.Message; 
            } 
        }

        public override void CommitTransaction()
        {
            try
            {
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(ex.Message, LogType.Plat, LogLevel.ERROR));
                LastErrorMessage = ex.Message;
            } 
        }

        public override void RollbackTransaction()
        {
            try
            {
                db.RollbackTransaction();
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(ex.Message, LogType.Plat, LogLevel.ERROR));
                LastErrorMessage = ex.Message;
            } 
        }

        public override bool setFile(string table, string field, string IDValue, string filePath)
        {
            try
            {
                LogLocal.log().SaveLog(new LogEntity(string.Format("在表：{0}中ID为：{1}的{2}字段中写入文件：{3}", table, field, IDValue, filePath), LogType.Plat, LogLevel.DEBUG));
                return db.setFile(table, field, IDValue, filePath);
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(ex.Message, LogType.Plat, LogLevel.ERROR));
                LastErrorMessage = ex.Message;
                return false;
            } 
        }

        public override bool getFile(string table, string field, string IDValue, string filePath)
        {
            try
            {
                LogLocal.log().SaveLog(new LogEntity(string.Format("在表：{0}中ID为：{1}的{2}字段中读取文件", table, field, IDValue), LogType.Plat, LogLevel.DEBUG));
                return db.getFile(table, field, IDValue, filePath);
            }
            catch (Exception ex)
            {
                LogLocal.log().SaveLog(new LogEntity(ex.Message, LogType.Plat, LogLevel.ERROR));
                LastErrorMessage = ex.Message;
                return false;
            } 
        }

        public override string getMachineCode()
        {
            return db.getMachineCode();
        }

        public override DateTime getServerTime()
        {
            return db.getServerTime();
        }
    }
}
