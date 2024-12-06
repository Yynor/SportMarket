using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Avalonia.Interactivity; 
using System.Linq;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using System.IO;
using Avalonia.Media;



namespace Market
{
    public partial class MainWindow : Window
    {
         public List<Product> Products { get; set; } = new List<Product>();
        public List<Product> FilteredProducts { get; set; } = new List<Product>();
        public List<Product> totototo { get; set; } = new List<Product>();
        public List<string> TakedItems = new List<string>();
        public int? scenary { get; set; }
        public int Role;
        public int Idd;
        public int? Scenary;
        public List<string> listt = new List<string>();
       public MainWindow(int id,int role,List<string> list)
       {
        
           InitializeComponent();

           Idd = id;
           listt.AddRange(list);

           NameMaker();
           Scenary= scenary;
            if(role == 1){
                StoreBtn.IsVisible=false;
                AddButton.IsVisible=true;
            }
            if(role == 3){
                StoreBtn.IsVisible=true;
                AddButton.IsVisible=false;
            }
            if(role == 2){
                StoreBtn.IsVisible=false;
                AddButton.IsVisible=false;
            }
            

           
        if(scenary!=null)
        {
            this.Hide();
            new LoginWindow().Show();
            this.Close();
        }

           Role = role;
           Products = ListCreate();
           totototo = Products;
           FilteredProducts = new List<Product>(Products); // Изначально все продукты видимы
           DataContext = this; // Устанавливаем контекст данных
           Fff.ItemsSource = FilteredProducts; // Устанавливаем источник данных для ListBox
            

            CBoxMaker.Items.Add("По умолчанию");           
            CBoxMaker.Items.Add("Webber");
            CBoxMaker.Items.Add("Luminarc");
            CBoxMaker.Items.Add("Нева");
            CBoxMaker.Items.Add("Эмаль");
            CBoxMaker.Items.Add("Solaris");
            CBoxMaker.Items.Add("Galaxy");
            CBoxMaker.Items.Add("Tefal");


            CBoxCategory.Items.Add("По умолчанию");
            CBoxCategory.Items.Add("Посуда");
            CBoxCategory.Items.Add("Сковорода");
            CBoxCategory.Items.Add("Сервиз");
            CBoxCategory.Items.Add("Кострюли");

            CBoxPrice.Items.Add("По умолчанию");
            CBoxPrice.Items.Add("По возрастанию");
            CBoxPrice.Items.Add("По убыванию");
            



            
           
        }
        public void NameMaker()
{
    string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
    string fullName = "ФИО"; // Инициализируем с "ФИО"

    try
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Используем параметризованный запрос
            using (MySqlCommand command = new MySqlCommand("SELECT UserSurname, UserName, UserPatronymic FROM user WHERE UserID = @id", connection))
            {
                command.Parameters.AddWithValue("@id", Idd);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Форматируем ФИО
                        Console.WriteLine("читает");
                        fullName = $"{reader.GetString(0)} {reader.GetString(1)} {reader.GetString(2)}";
                    }
                     Console.WriteLine("читает");
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Обработка исключений (например, вывод сообщения об ошибке)
        Console.WriteLine($"Ошибка: {ex.Message}");
    }

    FullUserName.Text = fullName; // Обновляем текст
}
       
      private void ButtonStorage_Click(object sender, RoutedEventArgs e)
      {
        this.Hide();
        listt.AddRange(TakedItems);
        new Orderr(Idd,Role,listt).Show();
        this.Close();
       }
       private void AddButtin_Click(object sender, RoutedEventArgs e)
      {
        this.Hide();
        new AddNewItem(Idd,Role,listt).Show();
        this.Close();
      }
       
        private async void fullUserNameTextBox_Pressed(object sender, PointerPressedEventArgs e)
        {
          var mb = new MBoxExitAsc(Idd,Role);
          mb.RoleChanged += (newRole) => 
            {
                Scenary = newRole; // Обновляем значение Role в MainWindow
                // Дополнительная логика, если необходимо
            };
          await mb.ShowDialog(this);

          if(Scenary!=null){

            this.Hide();
            new LoginWindow().Show();
            this.Close();
          }
        }
       public List<Product> ListCreate()
        {
            string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
            List<Product> list = new List<Product>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM product", connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productArticleNumber = reader.GetString(0);
                            string productName = reader.GetString(1);
                            string productDescription = reader.GetString(2);
                            string productCategory = reader.GetString(3);
                            Bitmap productPhoto = GetImage(reader); // Замените на реальный путь к изображению
                            string productManufacturer = reader.GetString(5);
                            float productCost = reader.GetFloat(6); // Используйте GetFloat для получения значения типа float
                            int productDiscountAmount = reader.GetInt32(7);
                            int productQuantityInStock = reader.GetInt32(8);
                            string productStatus = reader.GetString(9);

                            Product product = new Product(productArticleNumber,
                                productName, productDescription, productCategory, productPhoto,
                                productManufacturer, productCost, productDiscountAmount, productQuantityInStock, productStatus);
                            list.Add(product);
                            Console.WriteLine("новый продукт должнен был быть отображен");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            return list;
        }
        public Bitmap GetImage(MySqlDataReader reader)
        {
            if (!reader.IsDBNull(3)) // Проверяем, есть ли данные в столбце
            {
                byte[] imageData = (byte[])reader[4]; // Получаем байты изображения
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
 private void ShowArticleButton_Click(object sender, RoutedEventArgs e)
{
    if(Role == 3){
        if (sender is Button button && button.CommandParameter is Product product)
        {
            
            TakedItems.Add(product.ProductArticleNumber);
            

        }
    }
    if(Role==2){
        if (sender is Button button && button.CommandParameter is Product product)
        {
           
        this.Hide();
        new ChangeItemWindow(product.ProductArticleNumber, Idd,Role).Show();
        this.Close();
        }
    }
    if(Role==1){
        if (sender is Button button && button.CommandParameter is Product product)
        {
           
        this.Hide();
        new ChangeItemWindow(product.ProductArticleNumber, Idd,Role).Show();
        this.Close();
        }
    }
}

        public Bitmap GetImage(int photoId)
        {
            return new Bitmap("Assets/logo.png");
        }
    

        // Обработчик для изменения текста в TextBox
        private void SearchTextBox_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
        {
            FilterAndSortProducts();
        }

        // Обработчик для изменения выбора в ComboBox для производителя
        private void CBoxMaker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortProducts();
        }

        // Обработчик для изменения выбора в ComboBox для категории
        private void CBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortProducts();
        }

        // Обработчик для изменения выбора в ComboBox для сортировки
        private void CBoxPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortProducts();
        }

            // Метод для фильтрации и сортировки продуктов
        private void FilterAndSortProducts()
        {
            var selectedManufacturer = CBoxMaker.SelectedItem as string;
            var selectedCategory = CBoxCategory.SelectedItem as string;
            var sortOption = CBoxPrice.SelectedItem as string;
            var searchText = SearchTextBox.Text;

            // Фильтрация
            var filtered = Products.AsQueryable();

            // Фильтрация по производителю
            // Фильтрация по производителю
            if (selectedManufacturer != "По умолчанию" && !string.IsNullOrEmpty(selectedManufacturer))
            {
                filtered = filtered.Where(p => p.ProductManufacturer == selectedManufacturer);
            }
            // Фильтрация по категории
            if (selectedCategory != "По умолчанию" && !string.IsNullOrEmpty(selectedCategory))
            {
                filtered = filtered.Where(p => p.ProductCategory == selectedCategory);
            }
            // Фильтрация по названию
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(p => p.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            // Сортировка
            if (sortOption == "По возрастанию")
            {
                filtered = filtered.OrderBy(p => p.ProductCost);
            }
            else if (sortOption == "По убыванию")
            {
                filtered = filtered.OrderByDescending(p => p.ProductCost);
            }

            // Обновление списка
            FilteredProducts = filtered.ToList();
            Fff.ItemsSource = FilteredProducts; // Обновляем источник данных для ListBox
        }
    }
}