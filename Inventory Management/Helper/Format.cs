using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using  Inventory_Management.Helper;

namespace Inventory_Management.Helper
{
    public class DataFormat
    {
        public bool PreviewTextInputs(object sender, TextCompositionEventArgs e, DataFormatType dataFormatType)
        {
            return TextFormat(e.Text, e, dataFormatType);
        }
   

        private bool TextFormat(string text, TextCompositionEventArgs e, DataFormatType dataFormatType)
        {
            bool isAllowed = false ;
            foreach (var echar in e.Text.ToCharArray())
            {
                isAllowed = CharFormat(dataFormatType, echar);
            }
            return isAllowed;
   
        }
        private bool TextFormat(string text, DataObjectPastingEventArgs e, DataFormatType dataFormatType)
        {
            bool isAllowed = false;
            foreach (var echar in text.ToCharArray())
            {
                isAllowed = CharFormat(dataFormatType, echar);
            }
            return isAllowed;

        }

        public bool TextBoxPasting(object sender, DataObjectPastingEventArgs e, DataFormatType dataFormatType)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                if (TextFormat((string)e.DataObject.GetData(typeof(string)), e, dataFormatType))
                    return true ;
            }
            else
            {
                return true;
            }

            return false;
        }

        public enum DataFormatType
        {
            StringOnly = 0,
            NumberOnly = 1,
            CharOnly = 2,
            DateOnly = 3,
            CurrOnly = 4,
            AlphaNumeric = 5,
            NumberPercent = 6,
            AllChar = 7,
            StringNumber = 8
        }



        private  bool CharFormat(DataFormatType Datatype, char StrKey)
        {
            string StrComp;
            StrComp = "÷;~!@#$%&*()^?:،أ][ٍِ~ْآ’,.؟|}{<>ًٌٌَُإ×؛>ِ,";
            if (StrKey == '')
                return true;

            switch (Datatype)
            {

                case DataFormatType.StringOnly:
                    StrComp = StrComp + "1234567890";
                    if (StrComp.IndexOf(StrKey) != -1)
                        return true;
                    break;
                case DataFormatType.NumberOnly:
                    StrComp = "1234567890";
                    if (StrComp.IndexOf(StrKey) == -1)
                        return true;

                    break;
                case DataFormatType.CurrOnly:
                    StrComp = "1234567890.-";
                    if (StrComp.IndexOf(StrKey) == -1)
                        return true;
                    break;

                case DataFormatType.DateOnly:
                    StrComp = "1234567890/-";
                    if (StrComp.IndexOf(StrKey) == -1)
                        return true;
                    break;

                case DataFormatType.CharOnly:
                    if (StrComp.IndexOf(StrKey) != -1)
                        return true;
                    break;
                case DataFormatType.AlphaNumeric:
                    StrComp = ";~!@#$%^&*()_ٌَُّ'=,";

                    if (StrComp.IndexOf(StrKey) != -1)
                        return true;
                    break;
                case DataFormatType.NumberPercent:
                    StrComp = "1234567890.-";
                    if (StrComp.IndexOf(StrKey) == -1)
                        return true;
                    break;

                case DataFormatType.AllChar:
                    return true;

                case DataFormatType.StringNumber:
                    StrComp = StrComp + "";
                    if (StrComp.IndexOf(StrKey) != -1)
                        return true; 
                    break;
            }
           
            return false;

        }

    }
}
