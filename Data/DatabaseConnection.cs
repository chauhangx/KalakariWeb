namespace KalakariWeb.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class DatabaseConnection
    {
        public string ConnectionString { get; }
        public SqlConnection Connection => new SqlConnection(ConnectionString);

        public DatabaseConnection(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
    }

}
