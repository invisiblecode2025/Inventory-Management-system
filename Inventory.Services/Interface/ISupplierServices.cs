using Inventory.ManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Interface
{
    public interface ISupplierServices
    {
        public Task<IEnumerable<SupplierDto>> GetAllAsync();
        public IEnumerable<SupplierDto> GetAll();
        public Task<SupplierDto> GetById(int supplierId);
        public Task<int> Create(SupplierDto Supplier);
        public Task<bool> Update(SupplierDto Supplier);
        public Task<bool> SoftDeleteItems(int Id);
        public Task<bool> Delete(int ItemId);
    }
}
