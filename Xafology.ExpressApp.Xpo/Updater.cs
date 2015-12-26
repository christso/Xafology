using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.DbIdentity;

namespace Xafology.ExpressApp.Xpo
{
    public class Updater
    {
        /// <summary>
        /// Call this from your module Updater class
        /// </summary>
        /// <param name="session"></param>
        /// <param name="type">business class type</param>
        public static void SetupIdentityColumn(Session session, Type type)
        {
            string tableName = type.Name;
            var pi = GetIdentityPropertyInfo(type);
            if (pi == null)
                return;
            string columnName = pi.Name;

            if (!TableExists(session, tableName))
            {
                string createTableSql = string.Format("CREATE TABLE [{0}] ([{1}] [int] IDENTITY(1,1) NOT NULL)", tableName, columnName);
                session.ExecuteNonQuery(createTableSql);
                return;
            }

            if (!ColumnExists(session, tableName, columnName))
            {
                string addColumnSql = string.Format("ALTER TABLE {0} Add {1} int IDENTITY(1,1) NOT NULL;", tableName, columnName);
                session.ExecuteNonQuery(addColumnSql);
            }
        }

        public static bool TableExists(Session session, string tableName)
        {
            Tracing.Tracer.LogText("Check is table '{0}' exists", tableName);
            try
            {
                object res = session.ExecuteScalar("SELECT count(1) FROM sysobjects WHERE name = '" + tableName + "' AND type = 'U'");
                return ((int)res) > 0;
            }
            catch
            {
                return false;
            }
        }
        public static bool ColumnExists(Session session, string tableName, string columnName)
        {
            string columnExistsSql = string.Format(@"select Count(*)
from Information_SCHEMA.columns
where Table_name='{0}' and column_name='{1}'", tableName, columnName);
            var tmpResult = Convert.ToInt32(session.ExecuteScalar(columnExistsSql));
            return tmpResult != 0;
        }

        public static PropertyInfo GetIdentityPropertyInfo(Type type)
        {
            PropertyInfo[] pis = type.GetProperties();

            foreach (PropertyInfo pi in pis)
            {
                var attr = pi.GetCustomAttribute<DbIdentityAttribute>(true);
                if (attr != null)
                    return pi;
            }
            return null;
        }
    }
}
