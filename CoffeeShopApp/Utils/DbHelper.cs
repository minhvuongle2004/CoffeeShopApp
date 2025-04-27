using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace CoffeeShopApp.Utils
{
    public class DbHelper
    {
        private static string connectionString = "server=localhost;database=coffee_shop;user=root;password=";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
  