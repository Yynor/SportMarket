using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Market;
using MySql.Data.MySqlClient;
using System;

namespace Market;

public partial class RegWindow : Window
{
    public RegWindow()
    {
        InitializeComponent();
        RegistrationBtn.Click += RegistrationBtn_Click;
    }
    
    private void RegistrationBtn_Click(object? sender, RoutedEventArgs e)
    {
        if(UserConfirmPasswordBox.Text == UserPasswordBox.Text)
        {
            string UserSurname = Convert.ToString(UserSurnameBox.Text.Trim());
            string UserName = Convert.ToString(UserNameBox.Text.Trim());
            string UserPatronymic = Convert.ToString(UserPatronymicBox.Text.Trim());
            string UserLogin = Convert.ToString(UserLoginBox.Text.Trim());
            string UserPassword = Convert.ToString(UserConfirmPasswordBox.Text.Trim());
            if(UserSurname.Length>3 && UserName.Length>3 && UserPatronymic.Length>3 && UserLogin.Length>3 && UserPassword.Length>3){

            string connectionString = "Server=localhost;Database=shopDB;User Id=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            { 
                try{
                    connection.Open();
                    
                    // Используем параметризованный запрос
                    MySqlCommand command = new MySqlCommand("INSERT INTO user (UserSurname, UserName, UserPatronymic, UserLogin, UserPassword, UserRole) VALUES (@surname, @name, @patronymic, @login, @password, @role)", connection);
                    command.Parameters.AddWithValue("@surname", UserSurname);
                    command.Parameters.AddWithValue("@name", UserName);
                    command.Parameters.AddWithValue("@patronymic", UserPatronymic);
                    command.Parameters.AddWithValue("@login", UserLogin);
                    command.Parameters.AddWithValue("@password", UserPassword);
                    command.Parameters.AddWithValue("@role", 3);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex){
                    Console.WriteLine("Error");
                }
                
            }
            this.Close();
            }
        }
    }
    

}