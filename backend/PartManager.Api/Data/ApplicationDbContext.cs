using Microsoft.EntityFrameworkCore;
using PartManager.Api.Models;

namespace PartManager.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Part> Parts { get; set; } = null!;
    public DbSet<Drawer> Drawers { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.PartNumber).HasMaxLength(100);
            entity.Property(e => e.Manufacturer).HasMaxLength(200);
            entity.Property(e => e.Datasheet).HasMaxLength(500);
            
            entity.HasOne(e => e.Drawer)
                .WithMany(d => d.Parts)
                .HasForeignKey(e => e.DrawerId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Parts)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Drawer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
        });
    }
}
