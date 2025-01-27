using Inventory.Services.Interface;
using Inventory.Services.Services;
using Inventory.ManagementDto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Inventory.DomainModels.Models;
using Inventory_Management.Views.Inventory;
using System.Windows.Controls;
using System.DirectoryServices.ActiveDirectory;
using System.Linq.Expressions;
using Core.Common.ExpCombiner;
using Serilog;

namespace Inventory_Management.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        #region  Members


        readonly IItemServices _itemServices;
        readonly ICategoryServices _categoryServices;
        readonly ISupplierServices _supplierServices;
        readonly IInventoryServices _inventoryServices;
        public RelayCommand LoadItemsCommand { get; set; }
        public RelayCommand LoadInventoryItemCommand { get; set; }
        public RelayCommand AddInventoryItemCommand { get; set; }
        public RelayCommand LoadSuppliersCommand { get; set; }
        public ObservableCollection<CategoryDto> Category { get; set; }
        public RelayCommand UpdateInventoryItemCommand { get; set; }

        public RelayCommand SelectionChangedCommand { get; set; }
        public RelayCommand ShowDialogCommand { get; }
        public RelayCommand LoadCategoryCommand { get; set; }

        ObservableCollection<ItemDto?> _items;

        #endregion
        public ObservableCollection<ItemDto?> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));

                if (LoadItemsCommand != null)
                    LoadItemsCommand.NotifyCanExecuteChanged();
            }
        }

        ObservableCollection<SupplierDto?> _supplier;
        public ObservableCollection<SupplierDto?> Supplier
        {
            get { return _supplier; }
            set
            {
                _supplier = value;
                OnPropertyChanged(nameof(Supplier));

                if (LoadSuppliersCommand != null)
                    LoadSuppliersCommand.NotifyCanExecuteChanged();
            }
        }

        ObservableCollection<InventoryDto?> _inventoryList;
        public ObservableCollection<InventoryDto?> InventoryList
        {
            get { return _inventoryList; }
            set
            {
                _inventoryList = value;
                OnPropertyChanged(nameof(InventoryList));

                if (LoadInventoryItemCommand != null)
                    LoadInventoryItemCommand.NotifyCanExecuteChanged();
            }
        }

        private ItemDto _searchselectedItem;
        public ItemDto SearchSelectedItem
        {
            get { return _searchselectedItem; }
            set
            {
                _searchselectedItem = value;
                if (_searchselectedItem is not null)
                {
                    LoadInventoryItemCommand.Execute(this);
                }
                OnPropertyChanged(nameof(SearchSelectedItem));
            }
        }

        private ItemDto _selectedItem;
        public ItemDto SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private SupplierDto _selectedSupplier;
        public SupplierDto SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
            }
        }


        private SupplierDto _searchselectedSupplier;
        public SupplierDto SearchSelectedSupplier
        {
            get { return _searchselectedSupplier; }
            set
            {
                _searchselectedSupplier = value;
                LoadInventoryItemCommand.Execute(this);
                OnPropertyChanged(nameof(SearchSelectedSupplier));
            }
        }

        private InventoryDto _selectedInventoryItem;
        public InventoryDto SelectedInventoryItem
        {
            get { return _selectedInventoryItem; }
            set
            {
                _selectedInventoryItem = value;
                OnPropertyChanged(nameof(SelectedInventoryItem));
            }
        }


        private InventoryDto _inventory;
        public InventoryDto Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                OnPropertyChanged(nameof(Inventory));

                if (UpdateInventoryItemCommand != null)
                    UpdateInventoryItemCommand.NotifyCanExecuteChanged();
            }
        }


        private decimal _itemPrice;
        public decimal ItemPrice
        {
            get { return _itemPrice; }
            set
            {
                _itemPrice = value;
                OnPropertyChanged(nameof(ItemPrice));
            }
        }

        //InputSearch
        private string _inputSearch;
        public string InputSearch
        {
            get { return _inputSearch; }
            set
            {
                _inputSearch = value;
                OnPropertyChanged(nameof(StockQuantity));
            }
        }

        private int _stockQuantity;
        public int StockQuantity
        {
            get { return _stockQuantity; }
            set
            {
                _stockQuantity = value;
                OnPropertyChanged(nameof(StockQuantity));
            }
        }

        private CategoryDto _selectedCategory;
        public CategoryDto SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {

                _selectedCategory = value;
                if (_selectedCategory != null)
                {
                    LoadInventoryItemCommand.Execute(_selectedCategory);
                }
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private DateTime? _orderDate = null;
        public DateTime? OrderDate
        {
            get { return _orderDate; }
            set
            {
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }

        private void ClearInventoryItems(object parameter = null)
        {
            SelectedItem = null;
            StockQuantity = 0;
            SelectedSupplier = null;
            ItemPrice = 0;
            OrderDate = null;
            AddInventoryItemCommand.NotifyCanExecuteChanged();

        }

        private bool CanAddItem(object parameter)
        {
            // AddInventoryItemCommand.NotifyCanExecuteChanged();
            return SelectedSupplier?.Id > 0 && SelectedItem?.Id > 0 && StockQuantity > 0 && ItemPrice > 0;
        }

        private bool CanUpdateItem(object parameter)
        {
            OnPropertyChanged(nameof(Inventory.StockQuantity));
            return Inventory.Supplier?.Id > 0 && Inventory.Item?.Id > 0
                && Inventory.StockQuantity > 0 && Inventory.ItemPrice > 0;
        }
        private async void AddInventoryItem(object parameter)
        {
            try
            {
                var InventoryNewItem = new InventoryDto
                {
                    ItemId = SelectedItem.Id,
                    StockQuantity = StockQuantity,
                    SupplierId = SelectedSupplier.Id,
                    ItemPrice = ItemPrice,
                    OrderDate = OrderDate ?? default
                };
                await _inventoryServices.Create(InventoryNewItem);
                ClearInventoryItems();
                LoadInventoryItemCommand.Execute(LoadInventoryItemCommand);
                AddInventoryItemCommand.NotifyCanExecuteChanged();

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An Exception occurred while executing Add New Inventory");
            }
        }


        private async void UpdateInventoryItem(object parameter)
        {
            try
            {

                if (Inventory.StockQuantity <= 0 || Inventory.ItemPrice <= 0)
                {
                    MessageBox.Show("Quantity And Price must be greater than zero.", "Inventory", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                    return;
                }
                var InventoryUpdateItem = new InventoryDto
                {
                    Id = Inventory.Id,
                    ItemId = Inventory.Item.Id,
                    StockQuantity = Inventory.StockQuantity,
                    SupplierId = Inventory.Supplier.Id,
                    ItemPrice = Inventory.ItemPrice,
                    OrderDate = Inventory.OrderDate,
                    CreatedDate = Inventory.CreatedDate

                };

                await _inventoryServices.Update(InventoryUpdateItem);
                ClearInventoryItems();
                LoadInventoryItemCommand.Execute(LoadInventoryItemCommand);
                UpdateInventoryItemCommand.NotifyCanExecuteChanged();
                MessageBox.Show("The record has been updated successfully.", "Inventory", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);

            }
            catch (Exception ex)
            {

                Log.Error(ex, "An Exception occurred while executing UpdateInventoryItem");

            }

        }

        private void GetAllCategory(object parameter = null)
        {
            Category.Clear();
            Category = new ObservableCollection<CategoryDto>(_categoryServices.GetAll());
            Category.Insert(0, new CategoryDto() { Id = 0 });
        }

        #region Ctor


        public InventoryViewModel(IItemServices itemServices,
            ICategoryServices categoryServices,
            ISupplierServices supplierServices,
             IInventoryServices inventoryServices)
        {
            _itemServices = itemServices;
            _categoryServices = categoryServices;
            _supplierServices = supplierServices;
            _inventoryServices = inventoryServices;

            SelectedItem = new ItemDto();
            SelectedInventoryItem = new InventoryDto();
            SelectedSupplier = new SupplierDto();
            Inventory = new InventoryDto();
            Category = new ObservableCollection<CategoryDto>();
            Items = new ObservableCollection<ItemDto?>();
            InventoryList = new ObservableCollection<InventoryDto?>();
            Supplier = new ObservableCollection<SupplierDto?>();

            LoadItemsCommand = new RelayCommand(GetAllItems);
            LoadSuppliersCommand = new RelayCommand(GetAllSuppliers);
            AddInventoryItemCommand = new RelayCommand(AddInventoryItem, CanAddItem);
            LoadInventoryItemCommand = new RelayCommand(GetAllInventoryItems);
            ShowDialogCommand = new RelayCommand(ShowDialog);
            UpdateInventoryItemCommand = new RelayCommand(UpdateInventoryItem, CanUpdateItem);
            LoadCategoryCommand = new RelayCommand(GetAllCategory);

            SelectionChangedCommand = new RelayCommand(OnSelectionChanged);
            GetAllItems();
            GetAllSuppliers();
            GetAllInventoryItems();
            GetAllCategory();
        }


        private Expression<Func<Inventory.DomainModels.Models.Inventory, bool>>? BuildQuery()
        {
            Expression<Func<Inventory.DomainModels.Models.Inventory, bool>>? filter = null;

            if (SelectedCategory is not null && SelectedCategory.Id > 0)
            {
                filter = ExpressionCombiner.And(filter, a => a.Item.CategoryId == SelectedCategory.Id);
            }
            if (SearchSelectedItem is not null && SearchSelectedItem.Id > 0)
            {
                filter = ExpressionCombiner.And(filter, a => a.ItemId == SearchSelectedItem.Id);
            }

            if (SearchSelectedSupplier is not null && SearchSelectedSupplier.Id > 0)
            {
                filter = ExpressionCombiner.And(filter, a => a.SupplierId == SearchSelectedSupplier.Id);
            }

            if (!String.IsNullOrEmpty(InputSearch) && InputSearch.Length > 0)
            {

                filter = ExpressionCombiner.And(filter, a => a.Item.Name.Contains(InputSearch)
                || a.Supplier.Name.Contains(InputSearch)
                || a.Item.Category.Name.Contains(InputSearch));
            }

            return filter;
        }
        private void OnSelectionChanged(object parameter)
        {
            Expression<Func<Inventory.DomainModels.Models.Inventory, bool>>? Catfilter = null;
            if (parameter is CategoryDto SelectedCategory)
            {
                // if (SelectedCategory is not null)

                LoadInventoryItemCommand.Execute(this);
            }
        }
        #endregion
        private void ShowDialog(object parameter)
        {
            GetInventoryById();
            InventoryEdit dialog = new InventoryEdit(this);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.ShowDialog();


        }


        private void GetAllItems(object parameter = null)
        {
            Items?.Clear();
            Items = new ObservableCollection<ItemDto?>(_itemServices.GetAll());
            Items.Insert(0, new ItemDto { Id = 0 });
        }


        private void GetAllInventoryItems(object parameter = null)
        {
            InventoryList?.Clear();
            InventoryList = new ObservableCollection<InventoryDto?>(_inventoryServices.GetAll(filter: BuildQuery()));
        }

        private void GetInventoryById(object parameter = null)
        {
            Inventory = _inventoryServices.GetById(SelectedInventoryItem.Id);

        }

        private void GetAllSuppliers(object parameter = null)
        {
            Supplier?.Clear();
            Supplier = new ObservableCollection<SupplierDto?>(_supplierServices.GetAll());
            Supplier.Insert(0, new SupplierDto { Id = 0 });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
