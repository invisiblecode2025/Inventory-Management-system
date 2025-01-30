
using Moq;
using Inventory_Management.ViewModels;
using Inventory.Services.Interface;
using Inventory.ManagementDto;
using System.Collections.ObjectModel;

namespace InventorymanagementTestUnit
{
    public class InventoryTest
    {

    
        [Fact]
        public void GetAllItems()
        {
            //Arrange
            var listofitem = new List<ItemDto>();
         
            var itemmocservice = new Mock<IItemServices>();
            var catagorymocservice = new Mock<ICategoryServices>();
            var suppliermocservice = new Mock<ISupplierServices>();
            var inventorymocservice = new Mock<IInventoryServices>();

           itemmocservice.Setup(a => a.GetAll(null)).Returns(listofitem);
            catagorymocservice.Setup(a => a.GetAll()).Returns(new List<CategoryDto>());
            suppliermocservice.Setup(a => a.GetAll()).Returns(new List<SupplierDto>());
           // inventorymocservice.Setup(a => a.GetAll(null)).Returns(new List<InventoryDto>());

           var inventoryViewmodel = new InventoryViewModel(itemmocservice.Object,
               catagorymocservice.Object, suppliermocservice.Object, inventorymocservice.Object,false);

           //// ACT
            inventoryViewmodel.LoadItemsCommand.Execute(this);

           // //Assert
            Assert.Equal(listofitem, inventoryViewmodel.Items);


        }

        [Fact]
        public void DataProperty_RaisePropertyChangedEvent()
        {
            // Arrange
            var itemmocservice = new Mock<IItemServices>();
            var catagorymocservice = new Mock<ICategoryServices>();
            var suppliermocservice = new Mock<ISupplierServices>();
            var inventorymocservice = new Mock<IInventoryServices>();

            var inventoryViewmodel = new InventoryViewModel(itemmocservice.Object,
               catagorymocservice.Object, suppliermocservice.Object, inventorymocservice.Object,false);
            bool eventRaised = false;

            inventoryViewmodel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(inventoryViewmodel.Items))
                {
                    eventRaised = true;
                }
            };

            // Act
            inventoryViewmodel.Items = new ObservableCollection<ItemDto?>();

            // Assert
            Assert.True(eventRaised);
        }


    }
}