using Inventory.ManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Interface
{
   public interface IInventoryServices
    {
        public Task<IEnumerable<InventoryDto>> GetAllAsync();
        public IEnumerable<InventoryDto> GetAll(Expression<Func<DomainModels.Models.Inventory, bool>>? filter = null);
        public Task<InventoryDto> GetByIdAsync(int Id);
        public  Task<int> Create(InventoryDto Inventory);
        public Task<bool> Update(InventoryDto Inventory);
        public Task<bool> Delete(int ItemId);
        public Task<bool> SoftDeleteInventoryItems(int Id);
        public InventoryDto GetById(int Id);
        public IEnumerable<InventoryDto> GetAll(InventoryFilterDto _filterParam);
        Expression<Func<DomainModels.Models.Inventory, bool>>? BuildQuery(InventoryFilterDto _filterParam);

        public Task<IEnumerable<InventoryDto>> GetAllAsync(InventoryFilterDto _filterParam);
    }
}
