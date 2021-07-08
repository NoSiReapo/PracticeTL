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

        static void Main(string[] args)
        {
            string command = args[0];

            switch (command)
            {
                case "readorders":
                    List<Order> orders = ReadOrder();
                    foreach (Order order in orders)
                    {
                        Console.WriteLine(order.ProductName, ' ', order.Price);
                    }
                    break;

                case "readcustomers":
                    List<Customer> customers = ReadCustomer();
                    foreach (Customer customer in customers)
                    {
                        Console.WriteLine(customer.Name, ' ', customer.City);
                    }
                    break;

                case "insertorder":
                    int createdOrderId = InsertOrder("Мармелад", 140, 1);
                    Console.WriteLine("Created order: " + createdOrderId);
                    break;

                case "insertcustomer":
                    int createdCustomerId = InsertCustomer("Сергей", "Йошкар-Ола");
                    Console.WriteLine("Created cusromer: " + createdCustomerId);
                    break;

                case "updateorderprice":
                    UpdateOrderPrice(1, "100");
                    break;

                case "updatecustomercity":
                    UpdateCustomerCity(1, "Мытищи");
                    break;
            }
        }

        private static List<Order> ReadOrder()
        {
            List<Order> orders = new List<Order>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"SELECT
                            [OrderId],
                            [ProductName],
                            [Price],
                            [CustomerId]
                        FROM Order";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                ProductName = Convert.ToString(reader["ProductName"]),
                                Price = Convert.ToInt32(reader["Price"]),
                                CustomerId = Convert.ToInt32(reader["CustomerId"])
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        private static List<Customer> ReadCustomer()
        {
            List<Customer> customers = new List<Customer>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        @"SELECT
                            [CustomerId],
                            [Name],
                            [City],
                        FROM Customer";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customer = new Customer
                            {
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                Name = Convert.ToString(reader["Name"]),
                                City = Convert.ToString(reader["City"])
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }
            return customers;
        }

        private static int InsertOrder(string productName, int price, int customerId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
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

                    cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = productName;
                    cmd.Parameters.Add("@price", SqlDbType.Int).Value = price;
                    cmd.Parameters.Add("@customerId", SqlDbType.Int).Value = customerId;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private static int InsertCustomer(string name, string city)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO [Customer]
                       ([Name],
                        [City]) 
                    VALUES 
                       (@name,
                        @city)
                    SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = city;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private static void UpdateOrderPrice(int orderId, string newPrice)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE [Order]
                        SET [Price] = @newPrice
                        WHERE OrderId = @orderId";

                    command.Parameters.Add("@newPrice", SqlDbType.Int).Value = newPrice;
                    command.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;
                  
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateCustomerCity(int customerId, string newCity)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE [Customer]
                        SET [City] = @newCity
                        WHERE CustomerId = @customerId";

                    command.Parameters.Add("@newcity", SqlDbType.NVarChar).Value = newCity;
                    command.Parameters.Add("@customerId", SqlDbType.Int).Value = customerId;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}