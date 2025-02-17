
using AutoMapper;
using Inventory.DomainModels.Models;
using Inventory.ManagementDto;

namespace InventoryAPI.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<Inventory.DomainModels.Models.Inventory, InventoryDto>().ReverseMap();
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Users, UserDto>().ReverseMap();
        }
    }
}
