
using Inventory.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;




namespace Inventory.Infrastructure.Interface
{
    public  interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Item> Item { get; }
        IBaseRepository<DomainModels.Models.Inventory> Inventory { get; }
        IBaseRepository<Supplier> Supplier { get; }
        IBaseRepository<Category> Category { get; }
  
        int Complete();
        Task<bool> SaveChangesCommiRollBack();
        Task<int> Commit();
    }
}
