using Inventory_Management.Views;
using Inventory_Management.Views.Inventory;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            this.MainTextBox.Text = "Home";
            ContentArea.Content = new HomeView();
        }

        private void NavigateInventory(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.MainTextBox.Text = "Inventory";
            ContentArea.Content = new InventoryView();
      
        }

        private void NavigateHome(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.MainTextBox.Text = "Home";
            ContentArea.Content = new HomeView();

        }

        private void NavigateItem(object sender, RoutedEventArgs e)
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            this.MainTextBox.Text = "Items";
            ContentArea.Content = new ItemView();
        }

        private void NavigateSupplier(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.MainTextBox.Text = "Suppiler";
            ContentArea.Content = new SupplierView();
        }

        private void NavigateCatagory(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            this.MainTextBox.Text = "Catagory";
            ContentArea.Content = new CategoryView();
        }

        private void AddImageToContentControl()
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("/background.jpg", UriKind.RelativeOrAbsolute));
            image.Stretch = Stretch.UniformToFill;
            ContentArea.Content = image;

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            Application.Current.Shutdown(); 
        }
        }
    }