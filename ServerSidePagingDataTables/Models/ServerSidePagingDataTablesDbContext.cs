using System.Data.Entity;

namespace ServerSidePagingDataTables.Models
{
    public class ServerSidePagingDataTablesDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    }
}