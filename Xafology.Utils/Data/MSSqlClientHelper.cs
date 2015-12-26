using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Xafology.Utils.Data
{
    public class MSSqlClientHelper
    {
        public static void DropDatabase(string server, string database)
        {
            string connectionString = @"Data Source=" + server + ";Initial Catalog=master;Integrated security=SSPI";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    // drop database
                    string commandText = @"
IF EXISTS(SELECT Name FROM sys.databases WHERE name = '" + database + @"')
BEGIN
ALTER DATABASE " + database + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [" + database + @"]
END";
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void CreateDatabase(string server, string database, string dbDir)
        {
            string connectionString = @"Data Source=" + server + ";Initial Catalog=master;Integrated security=SSPI";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "CREATE DATABASE " + database + " ON PRIMARY " +
        "(NAME = " + database + "_Data, " +
        "FILENAME = '" + Path.Combine(dbDir, database + ".mdf'") + ", " +
        "SIZE = 5MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
        "LOG ON (NAME = " + database + "_Log, " +
        "FILENAME = '" + Path.Combine(dbDir, database) + ".ldf', " +
        "SIZE = 1MB, " +
        "MAXSIZE = 5MB, " +
        "FILEGROWTH = 10%)";
                    
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void DropAndCreateDatabase(string server, string database, 
            string dbDir)
        {
            DropDatabase(server, database);
            CreateDatabase(server, database, dbDir);
        }
    }
}
