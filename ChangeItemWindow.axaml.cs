using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System;


namespace Market
{
    public partial class ChangeItemWindow : Window
    {
        public List<string> list = new List<string>();
        private Bitmap selectedImageBitmap = null;
        public string ProductArticl = null;
        public int Id = 0;
        public int Role1;
        
        public ChangeItemWindow(string productArticle,int ID,int role)
        {
            InitializeComponent();
            Id = ID;
            Role1 = role;
            ProductArticl = productArticle;
            BackBtn.Click += BackBtn_Click;
            ContentBrowsBtw.Click += ContentBrowsBtw_Click;
            SaveProductSettingsBtn.Click += SaveProductSettingsBtn_Click;
            if(role ==2){
                SaveProductSettingsBtn.IsVisible=false;
                ContentBrowsBtw.IsVisible=false;
                ProductNameBox.IsEnabled=false;
                ProductCategoryBox.IsEnabled=false;
                ProductMakerBox.IsEnabled=false;
                ProductQuantityBox.IsEnabled=false;
                ProductDiscountBox.IsEnabled=false;
                ProductPriceBox.IsEnabled=false;
                DescriptionTextBox.IsEnabled=false;

                
            }
            InputDataForRedaction();
        }
        public void InputDataForRedaction()
        {
            string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                
                // Используем параметризованный запрос
                MySqlCommand command = new MySqlCommand("SELECT * FROM product WHERE ProductArticleNumber = @articulNumber", connection);
                command.Parameters.AddWithValue("@articulNumber", ProductArticl);
                
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ProductNameBox.Text =reader.GetString(1);
                        ProductCategoryBox.Text =reader.GetString(3);
                        ProductMakerBox.Text = reader.GetString(5);
                        ProductQuantityBox.Text=Convert.ToString(reader.GetInt32(8));
                        ProductDiscountBox.Text=Convert.ToString(reader.GetInt32(7));
                        ProductPriceBox.Text= Convert.ToString(reader.GetFloat(6));
                        DescriptionTextBox.Text = reader.GetString(2);
                        try{
                        byte[] productPhoto = (byte[])reader["ProductPhoto"];
                        using (var stream = new MemoryStream(productPhoto))
                        {
                            selectedImageBitmap = new Bitmap(stream);
                        }
                        SelectedImage.Source = selectedImageBitmap;
                        }
                         catch (Exception ex)
                         {
                 Console.WriteLine($"Error image show: {ex.Message}"); // Выводим сообщение об ошибке
              

                         }

                           
                        
                            
                    }
                }
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            
            this.Hide();
            new MainWindow(Id,Role1,list).Show();
            this.Close();
        }

        private void SaveProductSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
          
            string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string productName = ProductNameBox.Text;
                    string productCategory = ProductCategoryBox.Text;
                    string productMaker = ProductMakerBox.Text;
                    int productQuantity = int.Parse(ProductQuantityBox.Text);
                    int productDiscount = int.Parse(ProductDiscountBox.Text);
                    int productPrice = int.Parse(ProductPriceBox.Text);
                    string descriptionText = DescriptionTextBox.Text;

                    if (productQuantity < 0)
                        throw new ArgumentOutOfRangeException(nameof(productQuantity), "Количество продукта не может быть отрицательным");

                    // Преобразование изображения в массив байтов (если необходимо)
                    byte[] productPhoto = null;
                    if (selectedImageBitmap != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            // Сохраняем изображение в формате PNG
                            selectedImageBitmap.Save(memoryStream);
                            productPhoto = memoryStream.ToArray();
                        }
                    }

                    // Используем параметризованный запрос
                    string query = "UPDATE product SET ProductName = @ProductName, ProductDescription = @Description, ProductPhoto = @ProductPhoto, ProductCategory = @ProductCategory, ProductManufacturer = @ProductManufacturer, ProductCost = @ProductCost, ProductDiscountAmount = @ProductDiscount, ProductQuantityInStock = @ProductQuantity WHERE ProductArticleNumber = @ProductArticleNumber";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@Description", descriptionText);
                        command.Parameters.AddWithValue("@ProductPhoto", productPhoto);
                        command.Parameters.AddWithValue("@ProductCategory", productCategory);
                        command.Parameters.AddWithValue("@ProductManufacturer", productMaker);
                        command.Parameters.AddWithValue("@ProductCost", productPrice);
                        command.Parameters.AddWithValue("@ProductDiscount", productDiscount);
                        command.Parameters.AddWithValue("@ProductQuantity", productQuantity);
                        command.Parameters.AddWithValue("@ProductArticleNumber", ProductArticl); // Замените на нужный номер

                        command.ExecuteNonQuery();
                        this.Hide();
                        new MainWindow(Id,Role1,list).Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error db update: {ex.Message}"); // Выводим сообщение об ошибке
                }
            }
        }

        private async void ContentBrowsBtw_Click(object? sender, RoutedEventArgs e)
        {
            await SelectFileButton_Click(sender, e);
        }

        private async Task SelectFileButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter
                    {
                        Name = "Изображения",
                        Extensions = { "png", "jpg", "jpeg", "bmp", "gif" }
                    }
                }
            };

            // Открываем диалог выбора файла
            var result = await dialog.ShowAsync(this);

            // Проверяем, был ли выбран файл
            if (result != null && result.Length > 0)
            {
                // Сохраняем путь к выбранному файлу
                string selectedFilePath = result[0];

                // Загружаем изображение в Bitmap
                using (var stream = File.OpenRead(selectedFilePath))
                {
                    selectedImageBitmap = new Bitmap(stream);
                }

                // Отображаем изображение в элемент
                SelectedImage.Source = selectedImageBitmap;
           
          
             } 
        }
    }
}