using Inventory.Services.Interface;
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

namespace Inventory_Management.ViewModels
{
    class SupplierViewModel : INotifyPropertyChanged
    {
        readonly ISupplierServices _supplierServices;
        readonly IItemServices _itemServices;

        #region Class Member

        private SupplierDto? _newSupplier;
        public SupplierDto? NewSupplier
        {
            get { return _newSupplier; }
            set
            {
                _newSupplier = value;
                OnPropertyChanged(nameof(NewSupplier));
            }
        }



        ObservableCollection<SupplierDto> _suppliers;
        public ObservableCollection<SupplierDto> Suppliers
        {
            get { return _suppliers; }
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));

                if (LoadSupplierCommand != null)
                    LoadSupplierCommand.NotifyCanExecuteChanged();
            }
        }


        public RelayCommand AddSupplierCommand { get; set; }
        public RelayCommand UpdateSupplierCommand { get; set; }
        public RelayCommand DeleteSupplierCommand { get; set; }
        public RelayCommand LoadSupplierCommand { get; set; }
        public RelayCommand NewCommand { get; set; }
        public RelayCommand LoadSupplierByIdCommand { get; set; }

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
                AddSupplierCommand.NotifyCanExecuteChanged();
            }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
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

        private SupplierDto _selectedSupplier;
        public SupplierDto SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));

                if (LoadSupplierByIdCommand != null)
                    LoadSupplierByIdCommand.Execute(LoadSupplierByIdCommand);
            }
        }

        public SupplierViewModel(ISupplierServices supplierServices)
        {
            _supplierServices = supplierServices;

            Suppliers = new ObservableCollection<SupplierDto>();
            AddSupplierCommand = new RelayCommand(AddSupplier, CanAddSupplier);
            LoadSupplierCommand = new RelayCommand(GetAllSupplier);
            UpdateSupplierCommand = new RelayCommand(UpdateSupplier, CanUpdate);
            DeleteSupplierCommand = new RelayCommand(DeleteSupplier, CanDeleteSupplier);
            LoadSupplierByIdCommand = new RelayCommand(GetById);
            NewCommand = new RelayCommand(ClearSupplier);
            GetAllSupplier();
        }


        #region Methods    
        private async void GetById(object parameter = null)
        {
            if (SelectedSupplier?.Id > 0)
            {
                NewSupplier = await _supplierServices.GetById(SelectedSupplier.Id);
                Name = NewSupplier.Name;
                Contact = NewSupplier.ContactInfo;
                Email = NewSupplier.Email;
                Description = NewSupplier.Description;
            }

        }
        private void GetAllSupplier(object parameter = null)
        {
            Suppliers.Clear();
            Suppliers = new ObservableCollection<SupplierDto>(_supplierServices.GetAll());
        }

        private bool CanAddSupplier(object parameter)
        {
            return !string.IsNullOrEmpty(Name) && SelectedSupplier == null;
        }


        private async void AddSupplier(object parameter)
        {
            NewSupplier = new SupplierDto
            {
                Name = Name,
                Description = Description,
                ContactInfo = Contact,
                Email = Email,
            };
            await _supplierServices.Create(NewSupplier);
            ClearSupplier();
            LoadSupplierCommand.Execute(LoadSupplierCommand);
            AddSupplierCommand.NotifyCanExecuteChanged();
        }
        private void ClearSupplier(object parameter = null)
        {
            NewSupplier = null;
            Name = null;
            Description = null;
            Email = null;
            Contact = null;
            SelectedSupplier = null;
            AddSupplierCommand.NotifyCanExecuteChanged();
        }
    
        private bool CanUpdate(object parameter)
        {
            return SelectedSupplier?.Id > 0;
        }

        private bool IsSupplierUsed(int _CategoryId)
        {
            return _itemServices.GetAll(filter: a => a.CategoryId == _CategoryId).Any();
        }
        private bool CanDeleteSupplier(object parameter)
        {
            return SelectedSupplier != null && SelectedSupplier.Id > 0;
        }
        private async void UpdateSupplier(object parameter)
        {
            var result = await _supplierServices.Update(new SupplierDto
            {
                Name = Name,
                Id = SelectedSupplier.Id,
                Description = this.Description,
                Email = this.Email,
                ContactInfo = Contact ,    
                CreatedDate = SelectedSupplier.CreatedDate
            });

            ClearSupplier();
            LoadSupplierCommand.Execute(this);
            LoadSupplierCommand.NotifyCanExecuteChanged();
            if (result)
                MessageBox.Show("The record has been updated successfully.", "Category", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);

        }
        private async void DeleteSupplier(object parameter)
        {
            if (new Shared().Message_Confirm("Are You Sure delete this item ", "Category", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes))
            {
                if (!IsSupplierUsed(SelectedSupplier.Id))
                {
                    await _supplierServices.Delete(SelectedSupplier.Id);
                    ClearSupplier();
                    LoadSupplierCommand.Execute(this);
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

