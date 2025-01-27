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

namespace Inventory_Management.ViewModels
{
   

    public class ItemViewModel : INotifyPropertyChanged
    {
        #region Class Member

    
        readonly IItemServices _itemServices;
        readonly ICategoryServices _categoryServices;

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
 
        private CategoryDto _selectedCategory;
        public CategoryDto SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; 
                OnPropertyChanged(nameof(SelectedCategory));
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
            ICategoryServices categoryServices)
        {
            _itemServices = itemServices;
             _categoryServices = categoryServices;  

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
        }
        private  void GetAllItems(object parameter = null)
        {
            Items.Clear();
            Items = new ObservableCollection<ItemDto>( _itemServices.GetAll());
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
            await _itemServices.Delete(SelectedItem.Id);
            ClearItems();
            LoadItemsCommand.Execute(LoadItemsCommand);
         
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
