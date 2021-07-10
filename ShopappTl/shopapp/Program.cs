using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace shopapp
{
    class Program
    {
        private static string _connectionString = @"Data Source=FROGGER\SQLEXPRESS;Initial Catalog=Task1_data;Pooling=true;Integrated Security=SSPI;MultiSubnetFailover=true";

        static void Main( string[] args )
        {
            Console.WriteLine( "Choose one of: ReadOrder | ReadCustomer | InsertOrder | InsertCustomer | UpdateCustomerCity " );
            string command = Console.ReadLine();


            switch ( command )
            {
                case "ReadOrder":
                    List<Order> orders = ReadOrders();
                    foreach ( Order order in orders )
                    {
                        Console.WriteLine( order.ProductName, ' ', order.Price );
                    }
                    break;

                case "ReadCustomer":
                    List<Customer> customers = ReadCustomers();
                    foreach ( Customer customer in customers )
                    {
                        Console.WriteLine( customer.Name, ' ', customer.City );
                    }
                    break;

                case "InsertOrder":
                    int createdOrderId = InsertOrder( "Мармелад", 140, 1 );
                    Console.WriteLine( "Order created: " + createdOrderId );
                    break;

                case "InsertCustomer":
                    int createdCustomerId = InsertCustomer( "Сергей", "Йошкар-Ола" );
                    Console.WriteLine( "Customer created: " + createdCustomerId );
                    break;

                case "UpdateCustomerCity":
                    UpdateCustomerCity( 1, "Нижний Новгород" );
                    break;
            }
        }

        private static List<Order> ReadOrders()
        {
            List<Order> orders = new();
            using ( SqlConnection connection = new( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = new() )
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"SELECT
                            [OrderId],
                            [ProductName],
                            [CustomerId],
                            [Price]
                        FROM Order";

                    using ( SqlDataReader reader = command.ExecuteReader() )
                    {
                        while ( reader.Read() )
                        {
                            var order = new Order
                            {
                                OrderId = Convert.ToInt32( reader[ "OrderId" ] ),
                                ProductName = Convert.ToString( reader[ "ProductName" ] ),
                                CustomerId = Convert.ToInt32( reader[ "CustomerId" ] ), 
                                Price = Convert.ToInt32( reader[ "Price" ] )
                            };
                            orders.Add( order );
                        }
                    }
                }
            }
            return orders;
        }

        private static List<Customer> ReadCustomers()
        {
            List<Customer> customers = new();
            using ( SqlConnection connection = new( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = new() )
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"SELECT
                            [CustomerId],
                            [Name],
                            [City],
                        FROM Customer";

                    using ( SqlDataReader reader = command.ExecuteReader() )
                    {
                        while ( reader.Read() )
                        {
                            var customer = new Customer
                            {
                                CustomerId = Convert.ToInt32( reader[ "CustomerId" ] ),
                                Name = Convert.ToString( reader[ "Name" ] ),
                                City = Convert.ToString( reader[ "City" ] )
                            };
                            customers.Add( customer );
                        }
                    }
                }
            }
            return customers;
        }

        private static int InsertOrder( string productName, int price, int customerId )
        {
            using ( SqlConnection connection = new( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand cmd = connection.CreateCommand() )
                {
                    cmd.CommandText = @"
                    INSERT INTO [Order]
                       ([ProductName],
                        [Price],
                        [CustomerId]) 
                    VALUES 
                       (@productName,
                        @price,
                        @customerId)
                    SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.Add( "@productName", SqlDbType.NVarChar ).Value = productName;
                    cmd.Parameters.Add( "@price", SqlDbType.Int ).Value = price;
                    cmd.Parameters.Add( "@customerId", SqlDbType.Int ).Value = customerId;

                    return Convert.ToInt32( cmd.ExecuteScalar() );
                }
            }
        }

        private static int InsertCustomer( string name, string city )
        {
            using ( SqlConnection connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand cmd = connection.CreateCommand() )
                {
                    cmd.CommandText = @"
                    INSERT INTO [Customer]
                       ([Name],
                        [City]) 
                    VALUES 
                       (@name,
                        @city)
                    SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.Add( "@name", SqlDbType.NVarChar ).Value = name;
                    cmd.Parameters.Add( "@city", SqlDbType.NVarChar ).Value = city;

                    return Convert.ToInt32( cmd.ExecuteScalar() );
                }
            }
        }

        private static void UpdateCustomerCity( int customerId, string newCity )
        {
            using ( SqlConnection connection = new( _connectionString ) )
            {
                using ( SqlCommand command = connection.CreateCommand() )
                {
                    command.CommandText = @"
                        UPDATE [Customer]
                        SET [City] = @newCity
                        WHERE CustomerId = @customerId";

                    command.Parameters.Add( "@customerId", SqlDbType.Int ).Value = customerId;
                    command.Parameters.Add( "@newcity", SqlDbType.NVarChar ).Value = newCity;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
