
using MVCSQLApp.Models;
using System.Data.SqlClient;

namespace MVCSQLApp.Services
{

    // This service will interact with our Product data in the SQL database
    public class ProductService
    {
        

        private SqlConnection GetConnection()
        {
            string connectionString = "Server=tcp:ksday3productsserver.database.windows.net,1433;Initial Catalog=productdb;Persist Security Info=False;User ID=kannan;Password=Kan@3180;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            return new SqlConnection(connectionString);
        }
        public List<Product> GetProducts()
        {
            List<Product> _product_lst = new List<Product>();
            string _statement = "SELECT ProductID,ProductName,Quantity from Products";
            SqlConnection _connection = GetConnection();
            
            _connection.Open();
            
            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);
            
            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product _product = new Product()
                    {
                        ProductID = _reader.GetInt32(0),
                        ProductName = _reader.GetString(1),
                        Quantity = _reader.GetInt32(2)
                    };

                    _product_lst.Add(_product);
                }
            }
            _connection.Close();
            return _product_lst;
        }

    }
}

