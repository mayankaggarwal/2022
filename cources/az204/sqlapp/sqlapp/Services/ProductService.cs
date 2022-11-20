using Microsoft.Data.SqlClient;
using sqlapp.Models;
using System.Text.Json;

namespace sqlapp.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IConfiguration configuration,ILogger<ProductService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SQLConnection"));
        }

        public List<Product> GetProducts()
        {
            _logger.LogInformation("Starting Get Products");
            var conn = GetConnection();
            var lstProducts = new List<Product>();
            string statement = "SELECT ProductID,ProductName,Quantity FROM Products;";
            conn.Open();
            _logger.LogInformation("Opened Connection");
            var cmd = new SqlCommand(statement, conn);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        ProductID = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                    };
                    lstProducts.Add(product);
                }
            }
            conn.Close();
            _logger.LogInformation("Closing Connection");
            _logger.LogInformation("Fetched products is {products}",JsonSerializer.Serialize(lstProducts));
            return lstProducts;
        }
    }
}
