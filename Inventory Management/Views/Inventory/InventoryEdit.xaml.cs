using Inventory.DomainModels.Models;
using Inventory_Management.ViewModels;
using Inventory.ManagementDto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventory_Management.Views.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryEdit.xaml
    /// </summary>
    public partial class InventoryEdit : Window, INotifyPropertyChanged
    {
        public InventoryEdit(InventoryViewModel _inventoryViewModel)
        {
            InitializeComponent();

            this.DataContext = _inventoryViewModel;

            Mouse.OverrideCursor = null;

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void PreviewTextBoxInput(object sender, TextCompositionEventArgs e)
        {
            var dataFormatType = new Helper.DataFormat.DataFormatType();

            var objectName = ((FrameworkElement)e.Source).Name;
            switch (objectName)
            {
                case "stockQty":
                    dataFormatType = Helper.DataFormat.DataFormatType.NumberOnly;
                    break;
                case "itemprice":
                    dataFormatType = Helper.DataFormat.DataFormatType.CurrOnly;
                    break;
                default:
                    break;
            }
            e.Handled = new Helper.DataFormat().PreviewTextInputs(sender, e, dataFormatType);
        }

        private void validatetexterror()
        {
            var ffgf = "ggg";
        }
        private void TextBoxParsing(object sender, DataObjectPastingEventArgs e)
        {
            var dataFormatType = new Helper.DataFormat.DataFormatType();

            var objectName = ((FrameworkElement)e.Source).Name;
            switch (objectName)
            {
                case "stockQty":
                    dataFormatType = Helper.DataFormat.DataFormatType.NumberOnly;
                    break;
                case "itemprice":
                    dataFormatType = Helper.DataFormat.DataFormatType.CurrOnly;
                    break;
                default:
                    break;
            }

            if (new Helper.DataFormat().TextBoxPasting(sender, e, dataFormatType))
                e.CancelCommand();

        }

 

        private void stockQty_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttSave.IsEnabled = true;
            if (((TextBox)sender).Text.Length <= 0)
            {
                buttSave.IsEnabled = false;
            }
        }

        private void orderDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            buttSave.IsEnabled = true;
            if (((DatePicker)sender).SelectedDate == null)
            {
                buttSave.IsEnabled = false;
            }

        }
    }
}
