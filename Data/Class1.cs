using Microsoft.EntityFrameworkCore;
using Model;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<YourTable> YourTables { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<YourTable>(x => x.ToTable("YourTableName_on_MsSQL_DB"));
        }
    }
}
