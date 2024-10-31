using MySql.Data.MySqlClient;
namespace CRUDProduct
{
    public class database
    {
        private MySqlConnection connection;
        private string connectionString = "Server=localhost;Database=crud_product;User=root;Password=;";

        public database()
        {
            connection = new MySqlConnection(connectionString);
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
