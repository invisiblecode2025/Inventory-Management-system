using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inventory_Management.Helper
{
    public class Shared
    {
        public bool Message_Confirm(string messageBoxText, string messagecaption, 
            MessageBoxButton button, MessageBoxImage icon, MessageBoxResult messageresualt)
        {

            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, messagecaption, button, icon, messageresualt);

            if(result == MessageBoxResult.Yes) return true;

            return false;
        }
    }
}
