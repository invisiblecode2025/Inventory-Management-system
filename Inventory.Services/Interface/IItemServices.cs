using Inventory.DomainModels.Models;
using Inventory.ManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services.Interface
{
    public interface IItemServices
    {

        public Task<List<ItemDto>> GetAllAsync();
        public IEnumerable<ItemDto> GetAll(Expression<Func<Item, bool>>? filter = null);
      public  Task<ItemDto> GetById(int ItemId);
      public Task<int> Create(ItemDto Item);
     public  Task<bool> Update(ItemDto Item);
      public Task<bool> Delete(int ItemId);
    public Task<bool> SoftDeleteItems(int Id);




    }
}
