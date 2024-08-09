namespace Orders.Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating((modelBuilder));
    }
}