using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace SuperCodeBase.helpers
{
    class DatabaseHelper
    {
        private String dbName = "name";
        private String connectionString;

        public DatabaseHelper()
        {
            connectionString = "Data Source=" + dbName + ".sqlite;Version=3;";
        }

        public void Init()
        {
            SQLiteConnection.CreateFile(dbName+".sqlite");
            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            dbConnection.Open();

            string query = "CREATE TABLE data (name varchar(20), value int)";

            SQLiteCommand command = new SQLiteCommand(query,dbConnection);
            command.ExecuteNonQuery();

            query = "INSERT INTO data (name, value) values ('Nick', 1337), ('Dubbelfris', 10), ('Icetea', 88), ('Water', 0), ('Koffie', '29')";

            command = new SQLiteCommand(query, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();
        }

        public List<Data> SelectAll()
        {
            List<Data> datas = ExecuteObject<Data>("SELECT * FROM data");
            return datas;
        }

        private DataTable ExecuteDataTable(String query)
        {
            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(query, dbConnection);

            using (SQLiteDataReader dr = command.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(dr);
                dbConnection.Close();
                return tb;
            }
        }

        private List<T> ExecuteObject<T>(string sql)
        {
            List<T> items = new List<T>();
            var data = ExecuteDataTable(sql);
            foreach (var row in data.Rows)
            {
                T item = (T)Activator.CreateInstance(typeof(T), row);
                items.Add(item);
            }
            return items;
        }
    }
}
