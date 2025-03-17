
using Inventory.DomainModels.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Inventory.Shared.Core.Enum.Common;

namespace Inventory.Repository.DataContext
{
    public partial class ApplicationDbContext : DbContext
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _options = options; 
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SingularizeTableNames(modelBuilder);

            base.OnModelCreating(modelBuilder);


            // Apply global query filter for all entities that implement ISoftDelete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }

        }

        private void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
       where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.DeleteStatus != (byte)DeleteStatus.NotDeleted);
        }

        private void SingularizeTableNames(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=INVISIBLE-CODE\\SQLSERVER2022;Initial Catalog=InventoryManagement; Trusted_Connection=True;" +
                    "TrustServerCertificate=True;MultipleActiveResultSets=true");
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void BeforeSaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added ||
                e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.Now;
                }
       
                entity.LastUpdateDate = DateTime.Now;
            }
        }

        private void BeforeUpdate()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                if (entry.State == EntityState.Modified && entity.DeleteStatus == (int)DeleteStatus.Deleted)
                {
                    entity.DeleteStatus = (int)DeleteStatus.Deleted;
                }
                entity.LastUpdateDate = DateTime.Now;
            }
        }
   
        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            BeforeUpdate();
           return  base.Update(entity);
    
        }



    }
    
    
}
