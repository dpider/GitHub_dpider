using pde.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pde.pub.DataManager
{
    /// <summary>
    /// dbtNone     没有数据库，未知
    /// dbtOracle   Oracle数据库
    /// dbtMSSQL    MSSqlServer数据库
    /// dbtMySQL    MySql数据库
    /// dbtAccess   Access数据库
    /// </summary>
    public enum DataBaseType { dbtNone, dbtOracle, dbtMSSQL, dbtMySQL, dbtAccess, dbtSQLite, dbtDBase };

    /// <summary>
    /// dltNone    未知连接方式
    /// dltODBC    通过odbc直连数据库
    /// dltWeb     通过webservice方式连接数据库
    /// dltOffline 离线使用数据库
    /// </summary>
    public enum DataLinkType { dltNone, dltODBC, dltSocket, dltWeb};

    public class DataCfg
    { 
        //默认使用oracle数据库
        private DataBaseType dbt = DataBaseType.dbtOracle;
        //默认使用直连数据库方式
        private DataLinkType dlt = DataLinkType.dltODBC;

        private static DataService dbService = null;
        private static DataCfg cfg = null;

        //增加后缀配置（suffix），支持同一系统多个数据库连接
        public static DataService dataService(string suffix = "")  
        {
            if (cfg == null)
            {
                try
                {
                    cfg = new DataCfg(suffix);
                }
                catch (Exception ex)
                {
                    LogLocal.log().SaveLog(new LogEntity(ex.Message, LogType.Plat, LogLevel.DEBUG));
                }
            }
            return dbService;
        }

        public static void Close()
        {
            dbService = null;
            cfg = null;
        }

        private DataCfg(string suffix)
        {
            string linkType = WRSetting.Set().getSettings(Const.LinkType + suffix);
            if ("1".Equals(linkType)) { dlt = DataLinkType.dltODBC; }
            else if ("2".Equals(linkType)) { dlt = DataLinkType.dltSocket; }
            else if ("3".Equals(linkType)) { dlt = DataLinkType.dltWeb; } 
            else { dlt = DataLinkType.dltNone; }

            switch (dlt)
            { 
                case DataLinkType.dltODBC: 
                    string dbtype = WRSetting.Set().getSettings(Const.ODBC + suffix + "/" + Const.DBType).ToLower();
                    string ip = WRSetting.Set().getSettings(Const.ODBC + suffix + "/" + Const.DBIP);
                    string port = WRSetting.Set().getSettings(Const.ODBC + suffix + "/" + Const.DBPort);
                    string dbase = WRSetting.Set().getSettings(Const.ODBC + suffix + "/" + Const.DBase);
                    string user = WRSetting.Set().getSettings(Const.ODBC + suffix + "/" + Const.DBUserName);
                    string password = WRSetting.Set().getSettings(Const.ODBC + suffix + "/" + Const.DBUserPass);
                    string ver = WRSetting.Set().getSettings(Const.ODBC + suffix + "/" + Const.DBVer); 
                    string dbfile = WRSetting.Set().getSettings(Const.ODBC + suffix + "/" + Const.DBFile).ToLower();
                    password = CryptUtil.DecryptDES(password);   //密码采用DES加密算法存储在配置文件中，所以此处应该解密
                    if (Const.oracle.Equals(dbtype)) { dbt = DataBaseType.dbtOracle; }
                    else if (Const.mssql.Equals(dbtype)) { dbt = DataBaseType.dbtMSSQL; }
                    else if (Const.mysql.Equals(dbtype)) { dbt = DataBaseType.dbtMySQL; }
                    else if(Const.access.Equals(dbtype)) { dbt = DataBaseType.dbtAccess; }
                    else if (Const.sqlite.Equals(dbtype)) { dbt = DataBaseType.dbtSQLite; }
                    else if (Const.dbase.Equals(dbtype)) { dbt = DataBaseType.dbtDBase; }
                    else { dbt = DataBaseType.dbtNone; } 

                    dbService = new DataODBCService();

                    switch (dbt)
                    {
                        case DataBaseType.dbtOracle:
                            (dbService as DataODBCService).db = new Oracle(ip, port, dbase, user, password, ver);
                            break;

                        case DataBaseType.dbtMSSQL:
                            (dbService as DataODBCService).db = new SqlServer(ip, port, dbase, user, password, ver);
                            break;

                        case DataBaseType.dbtMySQL:
                            (dbService as DataODBCService).db = new MySql(ip, port, dbase, user, password, ver);
                            break;

                        case DataBaseType.dbtAccess:
                            (dbService as DataODBCService).db = new Access(dbfile, password);
                            break;

                        case DataBaseType.dbtSQLite:
                            (dbService as DataODBCService).db = new SQLite(dbfile, password);
                            break;

                        case DataBaseType.dbtDBase:
                            (dbService as DataODBCService).db = new DBase(dbfile, password);
                            break;

                        default:
                            throw new Exception("没有设置数据库类型！");
                    } 
                    break;

                case DataLinkType.dltSocket:
                    string socketIP = WRSetting.Set().getSettings(Const.Socket + suffix + "/" + Const.SocketIP).ToLower();
                    string socketPort = WRSetting.Set().getSettings(Const.Socket + suffix + "/" + Const.SocketPort).ToLower();
                    string socketServer = WRSetting.Set().getSettings(Const.Socket + suffix + "/" + Const.SocketServer).ToLower();
                    break;

                case DataLinkType.dltWeb:
                    string url = WRSetting.Set().getSettings(Const.WebService + suffix + "/" + Const.WSURL).ToLower();
                    break; 

                default:
                    throw new Exception("没有设置连接数据库方式！");
            }
        } 
    }
}
