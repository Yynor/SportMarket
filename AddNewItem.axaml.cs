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
    public partial class AddNewItem : Window
    {
        private Bitmap selectedImageBitmap;
        public int Id;
        public int Role;
        List<string> List = new List<string>();

        public AddNewItem(int id, int role, List<string> list)
        {
            InitializeComponent();
            Id = id;
            Role = role;
            List = list;
            BackBtn.Click += BackBtn_Click;
            ContentBrowsBtw.Click += ContentBrowsBtw_Click;
            SaveProductSettingsBtn.Click += SaveProductSettingsBtn_Click;
            Console.WriteLine("AddNewItem initialized with Id: {0}, Role: {1}", Id, Role); // Логирование инициализации
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Back button clicked. Navigating to MainWindow.");
            this.Hide();
            new MainWindow(Id, Role, List).Show();
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
                    Console.WriteLine("Database connection opened successfully."); // Логирование успешного соединения

                    string productName = ProductNameBox.Text;
                    string productCategory = ProductCategoryBox.Text;
                    string productMaker = ProductMakerBox.Text;
                    int productQuantity = int.Parse(ProductQuantityBox.Text);
                    int productDiscount = int.Parse(ProductDiscountBox.Text);
                    int productPrice = int.Parse(ProductPriceBox.Text);
                    string descriptionText = DescriptionTextBox.Text;
                    string produсtStatus = "да";
                    string art = ArtBox.Text;

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
                        Console.WriteLine("Product photo converted to byte array."); // Логирование успешного преобразования изображения
                    }

                    // Используем параметризованный запрос
                    string query = "INSERT INTO product (ProductName, ProductDescription, ProductPhoto, ProductCategory, ProductManufacturer, ProductCost, ProductDiscountAmount, ProductQuantityInStock, ProductArticleNumber,ProductStatus) VALUES (@ProductName, @Description, @ProductPhoto, @ProductCategory, @ProductManufacturer, @ProductCost, @ProductDiscount, @ProductQuantity, @ProductArticleNumber,@ProductStatus)";

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
                        command.Parameters.AddWithValue("@ProductStatus", produсtStatus);
                        command.Parameters.AddWithValue("@ProductArticleNumber", art); // Убедитесь, что это уникальное значение
                        command.ExecuteNonQuery();
                        Console.WriteLine("Product saved to database successfully."); // Логирование успешного сохранения
                    }

                    this.Hide();
                    new MainWindow(Id, Role, List).Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error db update: {ex.Message}"); // Выводим сообщение об ошибке
                }
            }
        }

        private async void ContentBrowsBtw_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Content browse button clicked.");
            await SelectFileButton_Click(sender, e);
        }

        private async Task SelectFileButton_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Opening file dialog to select an image...");
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
                Console.WriteLine($"Selected file: {selectedFilePath}"); // Логирование выбранного файла

                // Загружаем изображение в Bitmap
                using (var stream = File.OpenRead(selectedFilePath))
                {
                    selectedImageBitmap = new Bitmap(stream);
                }

                // Отображаем изображение в элемент
                SelectedImage.Source = selectedImageBitmap;
                Console.WriteLine("Image loaded and displayed successfully."); // Логирование успешной загрузки изображения
            }
            else
            {
                Console.WriteLine("No file was selected."); // Логирование, если файл не был выбран
            }
        }
    }
}