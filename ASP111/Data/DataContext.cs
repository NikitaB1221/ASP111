using ASP111.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP111.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Theme> Themes{ get; set; }
        public DbSet<Comment> Comments{ get; set; }
        public DbSet<Visit> Visits{ get; set; }
        public DbSet<Rate> Rates{ get; set; }



        public DataContext(DbContextOptions options) : base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("asp111");

            modelBuilder.Entity<Rate>().HasKey(nameof(Rate.ItemId), nameof(Rate.UserId));

            modelBuilder.Entity<Section>().HasOne(s => s.Author).WithMany().HasForeignKey(s => s.AuthorId);

            modelBuilder.Entity<Section>().HasMany(s => s.Rates).WithOne().HasForeignKey(r => r.ItemId);

            modelBuilder.Entity<Topic>().HasOne(t => t.Author).WithMany().HasForeignKey(t => t.AuthorId);

            modelBuilder.Entity<Theme>().HasOne(t => t.Author).WithMany().HasForeignKey(t => t.AuthorId);

            modelBuilder.Entity<Theme>().HasMany(t => t.Comments).WithOne(c => c.Theme).HasForeignKey(c => c.ThemeId);

            modelBuilder.Entity<Comment>().HasOne(c => c.Author).WithMany().HasForeignKey(c => c.AuthorId);
        }
    }
}
