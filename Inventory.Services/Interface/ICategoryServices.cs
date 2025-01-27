using Inventory.ManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Interface
{
    public interface ICategoryServices
    {
        public Task<IList<CategoryDto>> GetAllAsync();

        public List<CategoryDto> GetAll();
        public Task<CategoryDto> GetById(int categoryId);
        public Task<int> Create(CategoryDto category);
        public Task<bool> Update(CategoryDto category);

        public Task<bool> Delete(int categoryId);
    }
}
