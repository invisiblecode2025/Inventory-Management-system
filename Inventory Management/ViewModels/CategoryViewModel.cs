using Inventory.DomainModels.Models;
using Inventory.Services.Interface;
using Inventory.Services.Services;
using Inventory_Management.Helper;
using Inventory.ManagementDto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Azure.Core.HttpHeader;

namespace Inventory_Management.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        readonly ICategoryServices _categoryServices;
        readonly IItemServices _itemServices;

        #region Class Member

        private CategoryDto? _newCategory;
        public CategoryDto? NewCategory
        {
            get { return _newCategory; }
            set
            {
                _newCategory = value;
                OnPropertyChanged(nameof(NewCategory));
            }
        }


       
        //public ObservableCollection<CategoryDto> Categorys { get; set; }

        ObservableCollection<CategoryDto> _categorys;
        public ObservableCollection<CategoryDto> Categorys
        {
            get { return _categorys; }
            set
            {
                _categorys = value;
                OnPropertyChanged(nameof(Categorys));

                if (LoadCategoryCommand != null)
                    LoadCategoryCommand.NotifyCanExecuteChanged();
            }
        }


        public RelayCommand AddCategoryCommand { get; set; }
        public RelayCommand UpdateCategoryCommand { get; set; }
        public RelayCommand DeleteCategoryCommand { get; set; }
        public RelayCommand LoadCategoryCommand { get; set; }
        public RelayCommand NewCommand { get; set; }
        public RelayCommand LoadCategoryByIdCommand { get; set; }

        private string _search;
        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                OnPropertyChanged(nameof(Search));
                
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
                AddCategoryCommand.NotifyCanExecuteChanged();
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

      

        private CategoryDto _selectedCategory;
        public CategoryDto SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; 
               OnPropertyChanged(nameof(SelectedCategory));

                if (LoadCategoryByIdCommand != null)
                    LoadCategoryByIdCommand.Execute(LoadCategoryByIdCommand);
            }
        }

        public CategoryViewModel(ICategoryServices categoryServices, IItemServices itemServices)
        {
            _categoryServices = categoryServices;   
            _itemServices = itemServices;
            Categorys = new ObservableCollection<CategoryDto>();
            AddCategoryCommand = new RelayCommand(AddCategory, CanAddCategory);
            LoadCategoryCommand = new RelayCommand(GetAllCategory);
            UpdateCategoryCommand = new RelayCommand(UpdateCategory, CanUpdate);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory, CanDeleteCategory);
            LoadCategoryByIdCommand = new RelayCommand( GetById );  
            NewCommand = new RelayCommand(ClearCategory);
            GetAllCategory();
        }


        #region Methods    
        private async void GetById(object parameter = null)
        {
            if (SelectedCategory?.Id > 0)
            {
                NewCategory = await _categoryServices.GetById(SelectedCategory.Id);
                Name = NewCategory.Name;
                Description = NewCategory.Description;
            }

        }
        private void GetAllCategory(object parameter = null)
        {
            Categorys.Clear();
            Categorys = new ObservableCollection<CategoryDto>(_categoryServices.GetAll());
        }
   
        private bool CanAddCategory(object parameter)
        {
            return !string.IsNullOrEmpty(Name) && SelectedCategory == null ;
        }

  
            private async void AddCategory(object parameter)
        {
            NewCategory = new CategoryDto
            {
               Name = Name,
               Description = Description,

            };
            await _categoryServices.Create(NewCategory);
            ClearCategory();
            LoadCategoryCommand.Execute(LoadCategoryCommand);
            AddCategoryCommand.NotifyCanExecuteChanged();
        }
        private void ClearCategory(object parameter = null)
        {
            NewCategory = null;
            Name = null;
            Description = null;
            SelectedCategory = null;
            AddCategoryCommand.NotifyCanExecuteChanged();
        }
        private bool CanModifyItem(object parameter)
        {
            return SelectedCategory != null && SelectedCategory.Id > 0;
        }

        private bool CanUpdate(object parameter)
        {
            return  SelectedCategory?.Id > 0;
        }

        private bool IsCategoryUsed(int _CategoryId)
        {
            return _itemServices.GetAll(filter: a => a.CategoryId == _CategoryId).Any();
        }
        private bool CanDeleteCategory(object parameter)
        {

            return SelectedCategory != null && SelectedCategory.Id > 0;
        }
        private async void UpdateCategory(object parameter)
        {
            var resualt = await _categoryServices.Update(new CategoryDto
            {
                Name = Name,
                Id = SelectedCategory.Id,
                Description = this.Description,
                CreatedDate = SelectedCategory.CreatedDate
                
            });

            ClearCategory();
            LoadCategoryCommand.Execute(this);
            UpdateCategoryCommand.NotifyCanExecuteChanged();
            if(resualt)
                MessageBox.Show("The record has been updated successfully.", "Category", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);

        }
        private async void DeleteCategory(object parameter)
        {
            if (new Shared().Message_Confirm("Are You Sure delete this item ", "Category", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes))
            {
                if (!IsCategoryUsed(SelectedCategory.Id))
                {

                    await _categoryServices.Delete(SelectedCategory.Id);
                    ClearCategory();
                    LoadCategoryCommand.Execute(this);
                }
                else
                {
                    MessageBox.Show(" Cant delete Category Is Used", "Inventory", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.Yes);

                }
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
