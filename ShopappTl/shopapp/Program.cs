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
        private const string ConnectionString = @"Data Source=DEVSQL;Initial Catalog=blog-system;Pooling=true;Integrated Security=SSPI;MultiSubnetFailover=true";

        static void Main( string[] args )
        {
            Console.WriteLine( "Enter what you want(ReadOrders | ReadCustomers| InsertOrder | GetInfo)" );
            string command = Console.ReadLine();
            while ( command != "Stop" )
            {
                if ( command == "ReadOrders" )
                {
                    List<Order> orders = ReadOrder();
                    foreach ( Order order in orders )
                    {
                        Console.WriteLine( order.ProductName );
                    }
                }
                else if ( command == "ReadCustomers" )
                {
                    List<Customer> customers = ReadCustomer();
                }
                else if ( command == "InsertOrder" )
                {
                    int createdOrderId = InsertOrder();
                    Console.WriteLine( "Created order: " + createdOrderId );
                }
                else if ( command == "InsertCustomer" )
                {
                    int createdCustomerId = InsertCustomer();
                    Console.WriteLine( "Created customer id: " + createdCustomerId );
                }
                else if ( command == "Getinfo" )
                {
                    GetInfo();
                }
            }
        }

        private static List<Order> ReadOrder()
        {
            List<Order> orders = new List<Order>();
            using ( var connection = new SqlConnection( ConnectionString ) )
            {
                connection.Open();
                using var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText =
                    @"SELECT
                            [OrderId],
                            [ProductName],
                            [Price],
                            [CustomerId]
                        FROM Order";

                using var reader = command.ExecuteReader();
                while ( reader.Read() )
                {
                    var order = new Order
                    {
                        OrderId = Convert.ToInt32( reader[ "OrderId" ] ),
                        ProductName = Convert.ToString( reader[ "ProductName" ] ),
                        Price = Convert.ToInt32( reader[ "Price" ] ),
                        CustomerId = Convert.ToInt32( reader[ "CustomerId" ] )
                    };
                    orders.Add( order );
                }
            }
            return orders;
        }
        private static List<Customer> ReadCustomer()
        {
            List<Customer> customers = new List<Customer>();
            using ( SqlConnection connection = new SqlConnection( ConnectionString ) )
            {
                connection.Open();
                using SqlCommand command = new();
                command.Connection = connection;
                command.CommandText =
                    @"SELECT
                            [customerId],
                            [name],
                            [city],
                        FROM Customer";

                using SqlDataReader reader = command.ExecuteReader();
                while ( reader.Read() )
                {
                    var customer = new Customer
                    {
                        CustomerId = Convert.ToInt32( reader[ "customerId" ] ),
                        Name = Convert.ToString( reader[ "name" ] ),
                        City = Convert.ToString( reader[ "city" ] )
                    };
                    customers.Add( customer );
                }
            }
            return customers;
        }

        private static int InsertOrder()
        {
            Console.WriteLine( "Enter CustomerId: " );
            int CustomerId = int.Parse( Console.ReadLine() );
            Console.WriteLine( "Name of Product: " );
            string ProductName = Console.ReadLine();
            Console.WriteLine( "Price: " );
            int Price = int.Parse( Console.ReadLine() );
            using var connection = new SqlConnection( ConnectionString );
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"
                    INSERT INTO [Order]
                       ([productName],
                        [price],
                        [customerId]) 
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

        private static int InsertCustomer()
        {
            Console.WriteLine( "Enter Name: " );
            string Name = Console.ReadLine();
            Console.WriteLine( "City: " );
            string City = Console.ReadLine();
            using var connection = new SqlConnection( ConnectionString );
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                    INSERT INTO [Customer]
                       ([name],
                        [city]) 
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
