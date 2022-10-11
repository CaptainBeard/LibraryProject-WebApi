using System;
using MySqlConnector;

namespace library_project
{
    public class Database : IDisposable
    {
        public MySqlConnection Connection { get; }

        public Database(string connectionString)
        {
            //Connection = new MySqlConnection(connectionString);
            Connection = new MySqlConnection(System.Environment.GetEnvironmentVariable("DATABASE"));
        }

        public void Dispose() => Connection.Dispose();
    }
}