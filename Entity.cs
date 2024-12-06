using Avalonia.Media.Imaging;
using System;

using System.ComponentModel;



    

namespace Market
{
    public class User
    {
        private int UserID;
        private string UserName;
        private string UserSurname;
        private string UserPatronymic;
        private string UserLogin;
        private string UserPassword;
        private int UserRole;

        public User(int _UserID, string _UserName, string _UserSurname, string _UserPatronymic, string _UserLogin, string _UserPassword, int _UserRole)
        {
            UserID = _UserID;
            UserName = _UserName;
            UserSurname = _UserSurname;
            UserPatronymic = _UserPatronymic;
            UserLogin = _UserLogin;
            UserPassword = _UserPassword;
            UserRole = _UserRole;
        }
    }

    public class Product{
        public string ProductArticleNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public Bitmap? ProductPhoto { get; set; }
        public string ProductManufacturer { get; set; }
        public float ProductCost { get; set; }
        public int ProductDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public string ProductStatus { get; set; } 

        
        public Product(string _ProductArticleNumber, string _ProductName, string _ProductDescription, string _ProductCategory, Bitmap _ProductPhoto, string _ProductManufacturer, float _ProductCost, int _ProductDiscountAmount, int _ProductQuantityInStock, string _ProductStatus)
            {
                ProductArticleNumber = _ProductArticleNumber;
                ProductName = _ProductName;
                ProductDescription = _ProductDescription;
                ProductCategory = _ProductCategory;
                ProductPhoto = _ProductPhoto;
                ProductManufacturer = _ProductManufacturer;
                ProductCost = _ProductCost;
                ProductDiscountAmount = _ProductDiscountAmount;
                ProductQuantityInStock = _ProductQuantityInStock;
                ProductStatus = _ProductStatus;
            }
            
    }

   

    public class UserItemStorage : INotifyPropertyChanged
    {
        private int _itemCount;

        public string ItemArticul { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemPrice { get; set; }
        public int ItemCount
        {
            get => _itemCount;
            set
            {
                if (_itemCount != value)
                {
                    _itemCount = value;
                    OnPropertyChanged(nameof(ItemCount));
                }
            }
        }
        public Bitmap ItemPhoto { get; set; }

        public UserItemStorage(string articul, string name, string description, int price, int count, Bitmap photo)
        {
            ItemArticul = articul;
            ItemName = name;
            ItemDescription = description;
            ItemPrice = price;
            ItemCount = count;
            ItemPhoto = photo;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}