using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Market
{
    public partial class ChoosePickupPoint : Window
    {
        public event Action<int, string> AddressSelected; // Событие для передачи ID и адреса

        public ChoosePickupPoint()
        {
            InitializeComponent();
            LoadAddresses(); // Загружаем адреса из базы данных

            SelectBtn.Click += SelectBtn_Click;
            ExitBtn.Click += ExitBtn_Click;
        }

        private void LoadAddresses()
        {
            string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
            List<AddressItem> addresses = new List<AddressItem>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT placeID, adres FROM placedelivery", connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string address = reader.GetString(1);
                        addresses.Add(new AddressItem { Id = id, Address = address });
                    }
                }
            }

            AddressListBox.ItemsSource = addresses; // Используем ItemsSource для привязки данных
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AddressListBox.SelectedItem is AddressItem selectedAddress)
            {
                AddressSelected?.Invoke(selectedAddress.Id, selectedAddress.Address); // Вызываем событие с ID и адресом
                this.Close(); // Закрываем окно
            }
            else
            {
                // Можно добавить уведомление о том, что адрес не выбран
                Console.WriteLine("Адрес не выбран.");
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрываем окно
        }
    }

    public class AddressItem
    {
        public int Id { get; set; }
        public string Address { get; set; }

        public override string ToString() => Address; // Переопределяем ToString для отображения адреса в ListBox
    }
}