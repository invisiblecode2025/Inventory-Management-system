using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inventory_Management.Helper
{
    public class Shared
    {

        public  List<T> EnumToList<T>()
        {
            return new List<T>(Enum.GetValues(typeof(T)) as T[]);

        }
       List<int> _listof = new List<int>() { 15, 20, 16, 21, 13, 35 };
        public  List<KeyValuePair<int, string>> EnumToDictionary<T>(Func<KeyValuePair<int, string>, bool>? exp =null,bool selectfromexpressiontype =true) where T : Enum
        {
            var enumValues = Enum.GetValues(typeof(T));
            var enumNames = Enum.GetNames(typeof(T));

            var keyValuePairs = new List<KeyValuePair<int, string>>();

            for (int i = 0; i < enumValues.Length; i++)
            {
                keyValuePairs.Add(new KeyValuePair<int, string>((int)enumValues.GetValue(i), enumNames[i]));
            }
            //exp = a => _listof.Contains(a.Key);
            if(selectfromexpressiontype)
            keyValuePairs = keyValuePairs.Where(a => _listof.Contains(a.Key)).ToList();

            if (exp is null)
                exp = a => a.Key > 0;
            return keyValuePairs.Where(exp).ToList(); 
        }


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
