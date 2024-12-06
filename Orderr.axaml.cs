using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Avalonia.Interactivity; 
using System.IO;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using System.Linq;

namespace Market
{
    
    public partial class Orderr : Window
    {
        public float allPrice = 0;
        public List<UserItemStorage> Products { get; set; }
        public int Role { get; set; }
        public int Idd { get; set; }
        public List<string> lll = new List<string>();

        public Orderr(int id, int role, List<string> listik)
        {
            InitializeComponent();

            lll.AddRange(listik);
            Idd = id;
            Role = role;

            Products = FillFromListic();
            // Устанавливаем источник данных для ListBox
            fff.ItemsSource = Products;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new MainWindow(Idd, Role, lll).Show();
            this.Close();
        }

        private void Pressed(object sender, PointerPressedEventArgs e)
        {
            OpenChoosePickupPoint();
        }

        private void OpenChoosePickupPoint()
        {
            var choosePickupPointWindow = new ChoosePickupPoint();
            choosePickupPointWindow.AddressSelected += OnAddressSelected; // Подписываемся на событие
            choosePickupPointWindow.ShowDialog(this); // Открываем окно как модальное
        }

        private void OnAddressSelected(int id, string address)
        {
            // Обработка выбранного адреса
            Console.WriteLine($"Выбранный адрес: {address}, ID: {id}");
            PickUpp.Text = address;
        }

        private async void BntMakeZakaz_Click(object? sender, RoutedEventArgs e)
        {
            
            var OrgerConfirm = new OrderConfirmWindow(allPrice,PickUpp.Text);
            await OrgerConfirm.ShowDialog(this);
            

        }

        private List<UserItemStorage> FillFromListic()
        {
            List<UserItemStorage> list = new List<UserItemStorage>();
            string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
            int i = 0;
            foreach (string datab in lll)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("SELECT ProductName, ProductDescription, ProductCost, ProductPhoto, ProductQuantityInStock FROM product WHERE ProductArticleNumber = @art", connection);
                        command.Parameters.AddWithValue("@art", datab);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            
                            if (reader.Read())
                            {
                                string name = reader.GetString(0);
                                string discr = reader.GetString(1);
                                int price = reader.GetInt32(2);
                                i+=price;
                                Bitmap image = GetImage(reader); // Получаем изображение
                                int count = reader.GetInt32(4);

                                UserItemStorage userItem = new UserItemStorage(datab, name, discr, price, 1, image);
                                list.Add(userItem);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            GeneralPrice.Text = Convert.ToString(i);
            allPrice = i;
            return list;
        }

        public Bitmap GetImage(MySqlDataReader reader)
        {
            if (!reader.IsDBNull(3)) // Проверяем, есть ли данные в столбце
            {
                byte[] imageData = (byte[])reader[3]; // Получаем байты изображения
                if (imageData != null && imageData.Length > 0) // Проверяем, что данные не пустые
                {
                    try
                    {
                        using (var stream = new MemoryStream(imageData))
                        {
                            return new Bitmap(stream); // Создаем Bitmap из потока
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при загрузке изображения: {ex.Message}");
                    }
                }
            }
            return null; // Возвращаем изображение по умолчанию, если данных нет
        }

        private void RemoveSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранные элементы
            var selectedItems = fff.SelectedItems.Cast<UserItemStorage>().ToList();

            // Выводим количество выбранных элементов
            Console.WriteLine($"Количество выбранных элементов: {selectedItems.Count}");

            // Получаем индексы выбранных элементов
            var indicesToRemove = selectedItems
                .Select(item => Products.IndexOf(item))
                                .Where(index => index >= 0) // Убедимся, что индекс действителен
                .OrderByDescending(index => index) // Сортируем индексы в обратном порядке
                .ToList();

            // Удаляем элементы по индексам
            foreach (var index in indicesToRemove)
            {
                Console.WriteLine($"Удаляем элемент с индексом: {index}");
                Products.RemoveAt(index);
                
            }
            Makelistt(Products);
            // Обновляем источник данных для ListBox
            Products = FillFromListic();

            fff.ItemsSource = null; 
            if(Products.Count>0) {
                Console.WriteLine(Products.Count.ToString());
            fff.ItemsSource =Products ;
            }else {

                Products.Clear(); // Если Products - это ObservableCollection или List
                fff.ItemsSource = Products; // Обновляем ItemsSource
                this.Hide();
                new Orderr(Idd,Role,lll).Show();
                this.Close();
            }

            
             // Устанавливаем обновленный источник

            // Если нужно, можно также обновить другие элементы интерфейса
            Console.WriteLine($"Удалено {indicesToRemove.Count} элементов.");
        }
        public void Makelistt(List<UserItemStorage> products)
        { List<string> madenList = new List<string>();
            foreach(var item in products){
                madenList.Add(item.ItemArticul);


            }
            foreach(var Item in products) Console.WriteLine(Item.ItemArticul.ToString());

            lll = madenList;
        }
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем кнопку, которая была нажата
            var button = sender as Button;

            // Получаем элемент, к которому принадлежит кнопка
            var item = button.DataContext as UserItemStorage; // Замените на ваш тип данных

            if (item != null)
            {
                // Устанавливаем выбранный элемент в ListBox
                fff.SelectedItem = item;

                // Выводим информацию о выбранном элементе
                Console.WriteLine($"Выбранный элемент: {item.ItemName}, Цена: {item.ItemPrice}");
                RemoveSelectedButton_Click(sender,e);
            }
        }
    }
}