using AutoMapper;
using Inventory.Infrastructure.Interface;
using Inventory.ManagementDto;
using Inventory.Repository.DataContext;
using Inventory.Services.Interface;
using Inventory.Services.Services;
using InventoryAPI.Mapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorymanagementTestUnit
{

    public class InventoryServiceTests
    {
        private readonly IMapper _mocMapper ;
        private readonly InventoryServices _inventoryService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBaseRepository<Inventory.DomainModels.Models.Inventory>> _inventoryRepository;
        private readonly Mock<IInventoryServices> inventoryServices;
        private readonly ApplicationDbContext _context;

        public InventoryServiceTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mocMapper = mapperConfig.CreateMapper();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _inventoryRepository = new Mock<IBaseRepository<Inventory.DomainModels.Models.Inventory>>();
            _mockUnitOfWork.Setup(uo => uo.Inventory).Returns(_inventoryRepository.Object);
            _inventoryService = new InventoryServices(_mockUnitOfWork.Object, _mocMapper);

            inventoryServices = new Mock<IInventoryServices>();
            


        }

        [Fact]
        public async void Create_Inventory()
        {
            //Arrange
           var _inventory =  new InventoryDto()
            {
                CreatedDate = DateTime.Now,
                ItemId = 1,
                OrderDate = DateTime.Now,
                StockQuantity = 25,
                ItemPrice = 1250.25m,
                SupplierId = 1
            };

            _inventoryRepository.Setup(repo => repo.Add(It.IsAny<Inventory.DomainModels.Models.Inventory>())).Verifiable();
            _mockUnitOfWork.Setup(uow => uow.Commit()).Verifiable();


            //ACT
            var result = await(_inventoryService.Create(_inventory));

            // Assert
            _inventoryRepository.Verify(repo => repo.Add(It.IsAny<Inventory.DomainModels.Models.Inventory>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(1, result.ItemId);
            Assert.Equal(25, result.StockQuantity);
            Assert.Equal(1250.25m, result.ItemPrice);
        }

        [Fact]
        public async void GetById_ShouldReturnInventory()
        {
            //Arrange
            var _inventory = new Inventory.DomainModels.Models.Inventory()
            {
                Id= 12,
                CreatedDate = DateTime.Now,
                Description = "ReturnInventory",
                ItemId = 1,
                OrderDate = DateTime.Now,
                StockQuantity = 25,
                ItemPrice = 1250.25,
                SupplierId = 1
            };

            _inventoryRepository.Setup(repo => repo.GetById(12)).Returns(_inventory);

            // Act
            var result =_inventoryService.GetFindById(12);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(12, result.Id);
            Assert.Equal(25, result.StockQuantity);
            Assert.Equal(1250.25m, result.ItemPrice);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            _inventoryRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Inventory.DomainModels.Models.Inventory)null);

            // Act
            var result = _inventoryService.GetById(999);

            // Assert
            Assert.Null(result);
        }

    }
    }
