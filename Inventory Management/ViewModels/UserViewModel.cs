using Inventory.ManagementDto;
using Inventory.Services.Interface;
using Inventory.Services.Services;
using Inventory_Management.Views.login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inventory_Management.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public static bool LoginSuccess { get; private set; } = false;

        readonly IUserServices _userServices;

        public RelayCommand LoginCommand { get; set; }
        public UserViewModel(IUserServices userServices)
        {
            _userServices = userServices;
            LoginCommand = new RelayCommand(Login);
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _passWord;
        public string PassWord
        {
            get { return _passWord; }
            set
            {
                _passWord = value;
                OnPropertyChanged(nameof(PassWord));
            }
        }


        private async void Login(object parameter = null)
        {
            if(!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(PassWord)) 
            {
                if (await _userServices.UserLogin(new UserDto() { UserName = UserName,PassWord = PassWord }) != null)
                {
                    LoginSuccess =true;
                    App.loginWindow?.Hide();
                }
                else
                  MessageBox.Show("Invalid User Name Or Password", "Inventory", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.Yes);

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
