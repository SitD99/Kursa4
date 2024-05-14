using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<CardSet> CardSets { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CardSet>()
                .HasOne(cs => cs.User)
                .WithMany(u => u.CardSets)
                .HasForeignKey(cs => cs.UserId)
                .IsRequired();

            modelBuilder.Entity<Card>()
                .HasOne(c => c.CardSet)
                .WithMany(cs => cs.Cards)
                .HasForeignKey(c => c.CardSetId)
                .IsRequired();
        }
    }
}