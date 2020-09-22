using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SkiTickets.Utils
{
    public interface IDatabase
    { 
        SqlConnection Get();
    }
    
    public class Database : IDatabase
    {
        private readonly SqlConnection _connection;

        public Database(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("MyConnectionString"));
        }

        public SqlConnection Get()
        {
            return _connection;
        }
    }
}