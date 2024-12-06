using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Market;
using MySql.Data.MySqlClient;
using System;

namespace Market{

    public partial class MBoxExitAsc : Window
    {
        public event Action<int> RoleChanged;
        public int Id;
        public int Role;
        
        public MBoxExitAsc(int id, int role)
        {
            
            InitializeComponent();
            Id=id;
            Role=role;
            ExitBtn.Click += ExitBtn_Clic;
        }
         private void ExitBtn_Clic(object? sender, RoutedEventArgs e)
        {
            int newRole = 1; // Установите нужное значение Role
            RoleChanged?.Invoke(newRole); // Вызываем событие
            this.Close();
            
        }
    }
}