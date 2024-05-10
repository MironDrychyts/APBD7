using System.Data.SqlClient;

namespace WebApplication1.Repositories;

public class WareHouseRepository : IWareHouseRepository
{
    private readonly IConfiguration _configuration;

    public WareHouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public bool CheckIfProductExists(int IdProduct)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;

                command.Parameters.AddWithValue("@IdProduct", IdProduct);

                command.CommandText = "SELECT COUNT(*) FROM Product WHERE IdProduct = @IdProduct";

                return (int)command.ExecuteScalar() > 0;
            }
        }
    }
    
    public bool CheckIfWareHouseExists(int IdWareHouse)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@IdWareHouse", IdWareHouse);
                
                command.CommandText = "SELECT COUNT(*) FROM Warehouse WHERE IdWareHouse = @IdWareHouse";
                
                return (int)command.ExecuteScalar() > 0;
            }
        }
    }
    
    public bool CheckIfOrderExists(int IdOrder)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@IdOrder", IdOrder);
               

                command.CommandText =
                    "SELECT COUNT(*) FROM [Order] WHERE IdOrder = @IdOrder";
               
                return (int)command.ExecuteScalar() > 0;
            }
        }
    }

    public bool IfIdOrderExistsInProduct_Warehouse(int IdOrder)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@IdOrder", IdOrder);
                
                command.CommandText = "SELECT COUNT(*) FROM Product_Warehouse WHERE IdOrder = @IdOrder";
                
                return (int)command.ExecuteScalar() != 0;
            }
        }
    }
    
    public void UpdateFulfilledAt(int IdOrder)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@IdOrder", IdOrder);
                
                command.CommandText = "UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdOrder = @IdOrder";
                
                command.ExecuteNonQuery();
            }
        }
    }
    
    public double GetProductPrice(int IdProduct)
    {
        double productPrice = 0;

        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@IdProduct", IdProduct);
                
                command.CommandText = "SELECT Price FROM Product WHERE IdProduct = @IdProduct";
               

                var result = command.ExecuteScalar();
                if (result != null)
                {
                    productPrice = Convert.ToDouble(result);
                }
            }
        }

        return productPrice;
    }
    
    public int AddProductWareHouseRecord(int IdOrder, int IdProduct, int IdWareHouse, int Amount, double totalPrice)
    {
        int productWarehouseId = 0;

        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@IdWarehouse", IdWareHouse);
                command.Parameters.AddWithValue("@IdProduct", IdProduct);
                command.Parameters.AddWithValue("@IdOrder", IdOrder);
                command.Parameters.AddWithValue("@Amount", Amount);
                command.Parameters.AddWithValue("@Price", totalPrice);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now); 
                
                command.CommandText = "INSERT INTO Product_WareHouse VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
                
                command.ExecuteNonQuery();

                command.CommandText = "SELECT MAX(IdProductWareHouse) FROM Product_Warehouse";
                
                var result = command.ExecuteScalar();
                
                if (result != DBNull.Value)
                {
                    productWarehouseId = Convert.ToInt32(result);
                }
            }
        }

        return productWarehouseId;
    }

    public int GetOrderId(int IdProduct, int Amount, DateTime CreatedAt)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@IdProduct", IdProduct);
                command.Parameters.AddWithValue("@Amount", Amount);
                command.Parameters.AddWithValue("@CreatedAt", CreatedAt);

                
                command.CommandText =
                    "SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt";
                
                int IdOrder = -1;
                
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    IdOrder = Convert.ToInt32(result);
                }

                return IdOrder;


            }
        }
    }
}