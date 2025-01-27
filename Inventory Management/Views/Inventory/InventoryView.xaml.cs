using Inventory_Management.Helper;
using Inventory_Management.ViewModels;
using Inventory_Management.Views.Inventory;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class InventoryView : UserControl
    {
        public InventoryView()
        {

            InitializeComponent();

            this.DataContext = App.ServiceProvider.GetRequiredService<InventoryViewModel>();
            Mouse.OverrideCursor = null;

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
    }
}
