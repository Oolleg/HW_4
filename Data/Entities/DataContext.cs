using Microsoft.EntityFrameworkCore;


namespace HW_4.Data.Entities
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.User> Users { get; set; } = null!;


        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ASP_SPD_111-HW");
        }
    }
}
