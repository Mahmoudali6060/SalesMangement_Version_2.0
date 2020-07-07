using System;

using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Database.Entities;

namespace Database
{
    public class EntitiesDbContext : DbContext
    {
        public EntitiesDbContext(DbContextOptions<EntitiesDbContext> options) : base(options) { }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<PurechasesHeader> PurechasesHeaders { get; set; }
        public DbSet<PurechasesDetials> PurechasesDetials { get; set; }
        public DbSet<SalesinvoicesHeader> SalesinvoicesHeaders { get; set; }
        public DbSet<SalesinvoicesDetials> SalesinvoicesDetials { get; set; }
        public DbSet<Order_Purechase> Order_Purechases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Safe> Safes { get; set; }
        public DbSet<Company> Companys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(schema: DBGlobals.SchemaName);
            modelBuilder.Entity<Farmer>();
            modelBuilder.Entity<Seller>();
            modelBuilder.Entity<OrderHeader>();
            modelBuilder.Entity<OrderDetails>();
            modelBuilder.Entity<PurechasesHeader>();
            modelBuilder.Entity<PurechasesDetials>();
            modelBuilder.Entity<SalesinvoicesHeader>();
            modelBuilder.Entity<SalesinvoicesDetials>();
            modelBuilder.Entity<Order_Purechase>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Role>();
            modelBuilder.Entity<Company>();


            modelBuilder.Entity<User>()
            .HasOne(a => a.Company)
            .WithOne(a => a.User)
            .HasForeignKey<Company>(c => c.UserId);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            Audit();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            Audit();
            return await base.SaveChangesAsync();
        }

        private void Audit()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                }
            ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }
    }
}

