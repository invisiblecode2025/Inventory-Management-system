using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Inventory_Management.Helper
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
       DependencyProperty.RegisterAttached("BoundPassword", typeof(string), 
           typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));


        public static readonly DependencyProperty CommandPasswordProperty =
             DependencyProperty.RegisterAttached("PasswordCommand", typeof(ICommand), typeof(PasswordBoxHelper), new PropertyMetadata(OnBoundPasswordkeyDown));


      

        public static void SetPasswordCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandPasswordProperty, value);
        }

        public static ICommand GetPasswordCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandPasswordProperty);
        }

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (!string.Equals(passwordBox.Password, (string)e.NewValue))
                {
                    passwordBox.Password = (string)e.NewValue;
                }
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void OnBoundPasswordkeyDown(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {   
                passwordBox.KeyDown -= OnKeyDown;
                passwordBox.KeyDown += OnKeyDown;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBoundPassword(passwordBox, passwordBox.Password);
            }
        }


        private static void OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                if (sender is PasswordBox passwordBox)
                {
                    var command = GetPasswordCommand(passwordBox);
                    if (command != null && command.CanExecute(null))
                    {
                        command.Execute(null);
                    }
                }
            }
        }

    }


}
