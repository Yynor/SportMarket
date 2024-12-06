using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Market;
using MySql.Data.MySqlClient;
using System;

namespace Market{

    public partial class OrderConfirmWindow : Window
    {
        public OrderConfirmWindow(float cost, string pickup){
            InitializeComponent();
            Cost.Text = Convert.ToString(cost);
            Pick.Text = pickup;
        }
        private void ConfirmOrder_Click(object sender, RoutedEventArgs e){
            this.Close();
        }


    }
}