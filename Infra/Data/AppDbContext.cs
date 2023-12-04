using Infra.Data.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infra.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        /*
        public DbSet<Order> Orders { get; set; }
        public DbSet<WorkPoint> WorkPoints{ get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }
        public DbSet<Comment> Comments { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            modelBuilder.ApplyConfiguration(new NewsletterConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        
        }*/

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            return result;
        }
    }
}
