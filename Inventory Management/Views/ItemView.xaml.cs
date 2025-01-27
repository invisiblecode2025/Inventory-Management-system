using Inventory.Services.Interface;
using Inventory_Management.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

namespace Inventory_Management.Views
{
    /// <summary>
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : UserControl
    {
    
        public ItemView()
        {
            InitializeComponent();

            this.DataContext = App.ServiceProvider.GetRequiredService<ItemViewModel>();
            Mouse.OverrideCursor = null;
        }
    }
}
