using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Specialized;

namespace pde.pub.DataManager
{
    abstract public class DataService
    {
        public string LastErrorMessage;

        #region SQL脚本数据结构 SQLData
        public enum SQLSymbol { ssEqual, ssLike, ssLikeLeft, ssLikeRight, ssIn, ssLessThen, ssLessThenOrEqual, ssMoreThen, ssMoreThenOrEqual, ssNone };
        public struct SQLDataStruct
        {
            private string _Connector;
            private string _FieldName;
            private SQLSymbol _Symbol;
            private object _FieldValue;
            private string _before;
            private string _after;
            private string _format;

            public SQLDataStruct(string fieldName, object fieldValue, string connector = "", SQLSymbol symbol = SQLSymbol.ssEqual, string before = "", string after = "", string format = "")
            {
                _Connector = connector;
                _FieldName = fieldName;
                _Symbol = symbol;
                _FieldValue = fieldValue;
                _before = before;
                _after = after;
                _format = format;
            }

            public string Connector
            {
                get { return _Connector; }
                set { _Connector = Connector; }
            }
            public string FieldName
            {
                get { return _FieldName; }
                set { _FieldName = FieldName; }
            }
            public SQLSymbol Symbol
            {
                get { return _Symbol; }
                set { _Symbol = Symbol; }
            }
            public object FieldValue
            {
                get { return _FieldValue; }
                set { _FieldValue = FieldValue; }
            }
            public string Before
            {
                get { return _before; }
                set { _before = Before; }
            }
            public string After
            {
                get { return _after; }
                set { _after = After; }
            }

            public string Format
            {
                get { return _format; }
                set { _format = Format; }
            }
        }
        #endregion


        public abstract bool testCon();
        public abstract void CloseConnect();

        #region === 一般情况数据增删改 ===
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="KV">组表键值对，字段名=值</param>
        /// <returns></returns>
        public abstract bool addData(string table, Dictionary<string, object> KV, bool throwException = true);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="KV">档案盒表键值对，字段名=值</param>
        /// <returns></returns>
        public abstract bool updateData(string table, string ID, Dictionary<string, object> KV, bool throwException = true);

        public abstract bool updateData(string table, Dictionary<string, object> KV, List<SQLDataStruct> WhereData, bool throwException = true);

        /// <summary>
        /// 删除档案盒
        /// </summary>
        /// <param name="KV">删除条件键值对，字段名=值</param>
        /// <returns></returns>
        public abstract bool deleteData(string table, List<SQLDataStruct> WhereData, bool throwException = true);
        #endregion

        #region === 一般情况数据数量查询 ===
        /// <summary>
        /// 获取满足条件的数据总量
        /// </summary>
        /// <param name="KVWhere">查询条件</param>
        /// <returns>数量</returns>
        public abstract int getDataCount(string table, List<SQLDataStruct> WhereData, bool throwException = true);

        /// <summary>
        /// 获取满足条件的数据总量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>数量</returns>
        public abstract int getDataCount(string table, string where, bool throwException = true);
        #endregion

        #region === 一般情况数据集查询 ===
        /// <summary>
        /// 获取数据条目
        /// </summary>
        /// <param name="fields">查询字段</param>
        /// <param name="KVWhere">查询条件键值对</param>
        /// <param name="page">第N页</param>
        /// <param name="rows">每页数据数量</param>
        /// <returns></returns>
        public abstract DataTable getData(string table, string fields, string orders, List<SQLDataStruct> WhereData, int page = 0, int rows = 0, bool throwException = true);

        /// <summary>
        /// 获取数据条目
        /// </summary>
        /// <param name="fields">查询字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="page">第N页</param>
        /// <param name="rows">每页数据数量</param>
        /// <returns></returns>
        public abstract DataTable getData(string table, string fields, string orders, string where, int page = 0, int rows = 0, bool throwException = true);
        #endregion

        #region === 分组数据集查询 ===
        public abstract DataTable getGroupData(string table, string groupField, List<DataService.SQLDataStruct> whereData, bool throwException = true);
        public abstract DataTable getGroupData(string table, string groupField, string whereString, bool throwException = true);
        #endregion

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql">脚本</param>
        /// <returns></returns>
        public abstract bool ExecSql(string sql, bool throwException = true);

        /// <summary>
        /// 查询脚本
        /// </summary>
        /// <param name="sql">脚本</param>
        /// <returns></returns>
        public abstract DataTable RunSql(string sql, bool throwException = true);

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="lastError"></param>
        public abstract void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="lastError"></param>
        public abstract void CommitTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="lastError"></param>
        public abstract void RollbackTransaction();

        public abstract bool setFile(string table, string field, string IDValue, string filePath);

        public abstract bool getFile(string table, string field, string IDValue, string filePath);

        /// <summary>
        /// 获取服务器“机器码”
        /// </summary>
        /// <returns></returns>
        public abstract string getMachineCode();

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public abstract DateTime getServerTime();
    }
}
