using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market;
using System.Windows;
using MySql.Data.MySqlClient;
using Avalonia.Media.Imaging;

namespace Market
{
    
    public static class ProductListCreate{
       
        public static List<Product> ListCreate(){
            List<Product> list = new List<Product>();
            string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM products", connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()){
                Product product = new Product
                (reader.GetString(0), 
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                GetImage(reader.GetInt32(0)),
                reader.GetString(5),
                reader.GetFloat(6),
                reader.GetInt32(7),
                reader.GetInt32(8),
                reader.GetString(9));
                
                list.Add(product);
            }
            reader.Close();
            connection.Close();
            // добавляем данные в листбокс
            // listBox1.ItemsSource = list;
            return list;
        }

        public static Bitmap GetImage(int PhotoId){
            return new Bitmap("Assets/logo.png");
        }

    }
}