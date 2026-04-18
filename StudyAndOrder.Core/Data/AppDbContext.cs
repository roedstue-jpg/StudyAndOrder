using Microsoft.EntityFrameworkCore;
using StudyAndOrder.Core.Enums;
using StudyAndOrder.Core.Models;

namespace StudyAndOrder.Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Study> Studies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<IngoingMaterial> IngoingMaterials { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Equipment> Equipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Study → Orders (en studie har mange ordrer)
            modelBuilder.Entity<Study>()
                .HasMany(s => s.Orders)
                .WithOne(o => o.Study)
                .HasForeignKey(o => o.StudyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order → IngoingMaterials (en ordre har mange indgående materialer)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.IngoingMaterials)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Gem ProcessOrderType som tekst i databasen
            modelBuilder.Entity<Study>()
                .Property(s => s.ProcessOrderType)
                .HasConversion<string>();

            // Order -> ProducedMaterial (1..1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ProducedMaterial)
                .WithOne(p => p.Order)
                .HasForeignKey<OrderProducedMaterialLine>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProducedMaterial -> Equipments (many-to-many)
            modelBuilder.Entity<OrderProducedMaterialLine>()
                .HasMany(p => p.Equipments)
                .WithMany(e => e.ProducedMaterialLines);
        }
    }
}