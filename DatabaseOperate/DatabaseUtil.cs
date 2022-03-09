using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseOperate
{
    public class DatabaseUtil
    {
        public static string MakeConnString(string addr, string dbName, string dbUserName, string pwd, string port)
        {
            return String.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};Charset=utf8;", addr, dbName, dbUserName, pwd, port);
        }

        public static bool TryConn(string addr, string dbName, string dbUserName, string pwd, string port)
        {
            string connStr = MakeConnString(addr, dbName, dbUserName, pwd, port);
            return new MysqlHelper(connStr).TryConn();
        }

        public static MysqlHelper GetConn(string addr, string dbName, string dbUserName, string pwd, string port)
        {
            string connStr = MakeConnString(addr, dbName, dbUserName, pwd, port);
            var helper = new MysqlHelper(connStr);
            helper.ConnTo();
            return helper;
        }
    }
}
