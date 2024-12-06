using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Market;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        LoginButton.Click += LoginButton_Click;
        RegistrButton.Click += RegistrButton_Click;
    }

   private void LoginButton_Click(object? sender, RoutedEventArgs e)
{
    string log =" ";
    string pass=null;

    try{
   log = LoginTextBox.Text.Trim();
    pass  = PasswordTextBox.Text.Trim();
    }
    catch (Exception ex){Console.WriteLine(ex.Message); }
    if(log.Length>2 && pass.Length>2){
    string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        
        // Используем параметризованный запрос
        MySqlCommand command = new MySqlCommand("SELECT UserID,UserRole,UserPassword FROM user WHERE UserLogin = @login", connection);
        command.Parameters.AddWithValue("@login", log);
        
        using (MySqlDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                // Получаем пароль из базы данных
                string storedPassword = reader.GetString(2);
                List<string> listt = new List<string>();
                // Проверяем введенный пароль
                if (pass == storedPassword)
                {
                    Hide();
                    new MainWindow(reader.GetInt32(0),reader.GetInt32(1),listt).Show();
                    Close();
                }
                else
                {
                    Console.WriteLine("Неверный логин либо пароль");
                }
            }
            else
            {
                Console.WriteLine("Пользователь не найден");
            }
        
        }
        
    }
    }
    else Console.WriteLine("sdsd");
}
 
    private async void RegistrButton_Click(object? sender, RoutedEventArgs e){
        var RegWindow = new RegWindow();
          await RegWindow.ShowDialog(this);
          

    }
    
}