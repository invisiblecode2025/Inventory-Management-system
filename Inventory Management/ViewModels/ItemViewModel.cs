using Inventory.DomainModels.Models;
using Inventory.Infrastructure.Interface;
using Inventory.Infrastructure.Repository.UnitOfWork;
using Inventory.Services.Interface;
using Inventory.ManagementDto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq.Expressions;
using Core.Common.ExpCombiner;
using Inventory_Management.Helper;
using System.Windows;
using static Inventory.Shared.Core.Enum.Common;

namespace Inventory_Management.ViewModels
{
   

    public class ItemViewModel : INotifyPropertyChanged
    {
        #region Class Member

    
        readonly IItemServices _itemServices;
        readonly ICategoryServices _categoryServices;
        readonly IInventoryServices _inventoryServices;

        private ItemDto _newItem ; 
        public ItemDto NewItem { 
            get { return _newItem; } 
            set { _newItem = value; 
                OnPropertyChanged(nameof(NewItem));
            }
        }

        private ItemDto _selectedItem;
        public ItemDto SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if(LoadItemCommand != null)
                LoadItemCommand.Execute(LoadItemCommand);
              
            }
        }

  
        private string _searchInput;
        public string SearchInput
        {
            get { return _searchInput; }
            set
            {
                _searchInput = value;
                if(value != null && String.IsNullOrWhiteSpace(value))
                    LoadItemsCommand.Execute(LoadItemsCommand);
                OnPropertyChanged(nameof(SearchInput));
            }
        }

        private CategoryDto _selectedCategory;
        public CategoryDto SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; 
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private CategoryDto _selectedCategoryFilter;
        public CategoryDto SelectedCategoryFilter
        {
            get { return _selectedCategoryFilter; }
            set
            {
                _selectedCategoryFilter = value;
                LoadItemsCommand.Execute(LoadItemsCommand);
                OnPropertyChanged(nameof(SelectedCategoryFilter));
            }
        }

        ObservableCollection<ItemDto> _items;
        public ObservableCollection<ItemDto> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));

                if(LoadItemsCommand != null)
                LoadItemsCommand.NotifyCanExecuteChanged(); 
            }
        }
        public ObservableCollection<CategoryDto> Category { get; set; }
        public RelayCommand AddItemCommand { get; set; }
        public RelayCommand UpdateItemCommand { get; set; }
        public RelayCommand DeleteItemCommand { get; set; }
        public RelayCommand LoadItemsCommand { get; set; }
        public RelayCommand LoadCattegoryCommand { get; set; }
        public RelayCommand NewCommand { get; set; }
        public RelayCommand LoadItemCommand { get; set; }

        private string _search;
        public string Search
        {
            get { return _search; }
            set
            {
                _itemName = value;
                OnPropertyChanged(nameof(Search));
                AddItemCommand.NotifyCanExecuteChanged();
            }
        }

        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                OnPropertyChanged(nameof(ItemName));
                AddItemCommand.NotifyCanExecuteChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));

            }
        }

        #endregion

        #region Ctor
        public ItemViewModel(IItemServices itemServices,
            ICategoryServices categoryServices, IInventoryServices inventoryServices)
        {
            _itemServices = itemServices;
             _categoryServices = categoryServices;
            _inventoryServices = inventoryServices;



             SelectedItem = new ItemDto(); 
            Items = new ObservableCollection<ItemDto>();
            Category = new ObservableCollection<CategoryDto>();
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);
            UpdateItemCommand = new RelayCommand(UpdateItem, CanModifyItem);
            DeleteItemCommand = new RelayCommand(DeleteItem, CanDeleteItem);
            LoadItemsCommand = new RelayCommand(GetAllItems);
            LoadCattegoryCommand = new RelayCommand(GetAllCategory);
            LoadItemCommand = new RelayCommand(GetById);
            NewCommand = new RelayCommand(ClearItems);

            GetAllCategory();
            GetAllItems();
        }
        #endregion


        #region Methods    
        private async void GetById(object parameter = null)
        {
            if (SelectedItem?.Id > 0)
            {
                NewItem = await _itemServices.GetById(SelectedItem.Id);
                ItemName = NewItem.Name;
                SelectedCategory = NewItem.Category;
                Description = NewItem.Description;
                Notes = NewItem.Notes;
            }

        }
        private  void GetAllCategory(object parameter = null)
        {
            Category.Clear();
           Category = new ObservableCollection<CategoryDto>(_categoryServices.GetAll());
            Category.Insert(0, new CategoryDto() { Id = 0 });
        }
        private  void GetAllItems(object parameter = null)
        {
            Items.Clear();
            Items = new ObservableCollection<ItemDto>( _itemServices.GetAll(filter: BuildQuery()));
        }

        private Expression<Func<Item, bool>>? BuildQuery()
        {
            Expression<Func<Item, bool>>? filter = a=> a.DeleteStatus == (int)DeleteStatus.NotDeleted;

            if(SelectedCategoryFilter != null && SelectedCategoryFilter.Id > 0)
            {
                filter = ExpressionCombiner.And(filter, a => a.CategoryId == SelectedCategoryFilter.Id);
            }
            if(!String.IsNullOrEmpty(SearchInput))
            {
                filter = ExpressionCombiner.And(filter, a => a.Name.Contains(SearchInput) ||
                a.Description.Contains(SearchInput)
                || a.Notes.Contains(SearchInput));
            }

            return filter;
        }

        private bool IsItemUsed(int _itemId)
        {
            return _inventoryServices.GetAll(a => a.ItemId == _itemId).Any();
        }

        private bool CanAddItem(object parameter)
        {
            return !string.IsNullOrEmpty(ItemName) && SelectedCategory?.Id > 0  && ( SelectedItem?.Id <=0 || SelectedItem == null );
        }
        private async void AddItem(object parameter)
        {
            NewItem = new ItemDto
            {
                CategoryId = SelectedCategory.Id,
                Name = ItemName,
                Description = this.Description,
                Notes = this.Notes
                
            };
            await _itemServices.Create(NewItem);
            ClearItems();
            LoadItemsCommand.Execute(LoadItemsCommand );
            AddItemCommand.NotifyCanExecuteChanged();
        }
        private void ClearItems(object parameter = null)
        {
            NewItem =null;
            ItemName = null;
            Description = null;
            SelectedCategory = null;
            Notes = null;
            SelectedItem = null;
            AddItemCommand.NotifyCanExecuteChanged();
        }
        private bool CanModifyItem(object parameter) 
        {
            return SelectedItem != null && SelectedItem.Id > 0;
        }
        private bool CanDeleteItem(object parameter)
        {
            
            return SelectedItem != null && SelectedItem.Id > 0;
        }
        private async void UpdateItem(object parameter)
        {

            await _itemServices.Update(new ItemDto
            {
                Name = ItemName,
                CategoryId = SelectedCategory.Id,
                Description = this.Description,
                Notes = this.Notes,
                Id = SelectedItem.Id,
                CreatedDate=SelectedItem.CreatedDate
            });

            ClearItems();
            LoadItemsCommand.Execute(LoadItemsCommand);
            //UpdateItemCommand.NotifyCanExecuteChanged();
        }   
        private async void DeleteItem(object parameter)
        {
 
            if (new Shared().Message_Confirm("Are You Sure delete this item ", "Items", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes))
            {
                if (!IsItemUsed(SelectedItem.Id))
                {
                    MessageBox.Show(" Cant delete Item Is Used", "Inventory", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.Yes);
                    return;
                }

                await _itemServices.SoftDeleteItems(SelectedItem.Id);
                ClearItems();
                LoadItemsCommand.Execute(LoadItemsCommand);
            }


        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
