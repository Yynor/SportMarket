using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Market;

public partial class LoginWindow : Window
{
    private int _failedAttempts = 0;
    private const int MaxFailedAttempts = 3;
    private const string CaptchaText = "z8hc2"; // Замените на вашу логику генерации капчи

    public LoginWindow()
    {
        InitializeComponent();
        LoginButton.Click += LoginButton_Click;
        RegistrButton.Click += RegistrButton_Click;
        CaptchaSubmitButton.Click += CaptchaSubmitButton_Click;
    }

    private void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        string log = "";
        string pass = "";
        try
        {
            log= LoginTextBox.Text.Trim();
             pass= PasswordTextBox.Text.Trim();
        }
        catch (Exception exception)
        {
            
        }
        

        if (log.Length > 2 && pass.Length > 2)
        {
            string connectionString = "Server=localhost;Database=Posuda;User Id=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT ID, RoleID, Password FROM Users WHERE Login = @login", connection);
                command.Parameters.AddWithValue("@login", log);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedPassword = reader.GetString(2);
                        List<string> listt = new List<string>();

                        if (pass == storedPassword)
                        {
                            Hide();
                            new MainWindow(reader.GetInt32(0), reader.GetInt32(1), listt).Show();
                            Close();
                        }
                        else
                        {
                            HandleFailedLogin();
                        }
                    }
                    else
                    {
                        HandleFailedLogin();
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Логин и пароль должны содержать более 2 символов.");
        }
    }

    private void HandleFailedLogin()
    {
        _failedAttempts++;

        if (_failedAttempts >= MaxFailedAttempts)
        {
            ShowCaptcha();
        }
        else
        {
            Console.WriteLine("Неверный логин или пароль. Попробуйте снова.");
        }
    }

    private void ShowCaptcha()
    {
        CaptchaPanel.IsVisible = true;
        CaptchaImage.Source = new Bitmap("Assets/capcha.png"); // Замените на вашу логику генерации капчи
        LoginButton.IsEnabled = false; // Блокируем кнопку "Войти"
    }

    private async void CaptchaSubmitButton_Click(object? sender, RoutedEventArgs e)
    {
        if (CaptchaTextBox.Text.Trim() == CaptchaText) // Проверка введенных символов
        {
            CaptchaPanel.IsVisible = false;
            // Скрываем капчу
            LoginButton.IsEnabled = true; // Разблокируем кнопку "Войти"
            _failedAttempts = 0; // Сбрасываем счетчик попыток
        }
        else
        {
            CaptchaTextBox.Text = string.Empty; // Очищаем поле ввода капчи
            Console.WriteLine("Неверная капча. Попробуйте снова.");
            CaptchaPanel.IsVisible = false;
            
            await Task.Delay(5000); 
            LoginButton.IsEnabled = true;// Ждем 5 секунд
             // Скрываем капчу
             // Блокируем кнопку "Войти"
        }
    }

    private async void RegistrButton_Click(object? sender, RoutedEventArgs e)
    {
        var RegWindow = new RegWindow();
        await RegWindow.ShowDialog(this);
    }
}