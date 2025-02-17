using Inventory.DomainModels.Models;
using Inventory.Infrastructure.Interface;
using Inventory.Repository.DataContext;
using Repository;

namespace Inventory.Infrastructure.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        public IBaseRepository<Item> Item { get; private set; }
        public IBaseRepository<Category> Category { get; private set; }
        public IBaseRepository<Supplier> Supplier { get; private set; }
        public IBaseRepository<DomainModels.Models.Inventory> Inventory { get; private set; }

        public IBaseRepository<Users> Users { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Item = new BaseRepository<Item>(_context);
            Category = new BaseRepository<Category>(_context);
            Supplier = new BaseRepository<Supplier>(_context);
            Inventory = new BaseRepository<DomainModels.Models.Inventory>(_context);
            Users = new BaseRepository<Users>(_context);
            

        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public Task<int> Commit()
        {
            return _context.SaveChangesAsync();
        }

        public Task<bool> SaveChangesCommiRollBack()
        {
            bool returnValue = true;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    returnValue = false;
                    dbContextTransaction.Rollback();
                }
            }

            return Task.FromResult(returnValue);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
