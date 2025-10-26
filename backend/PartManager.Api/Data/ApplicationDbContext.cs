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
    public DbSet<PartAttachment> PartAttachments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.PartNumber).HasMaxLength(100);
            entity.Property(e => e.Manufacturer).HasMaxLength(200);
            entity.Property(e => e.ManufacturerPartNumber).HasMaxLength(100);
            entity.Property(e => e.Package).HasMaxLength(100);
            entity.Property(e => e.Footprint).HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(100);
            entity.Property(e => e.Tolerance).HasMaxLength(50);
            entity.Property(e => e.Voltage).HasMaxLength(50);
            entity.Property(e => e.Current).HasMaxLength(50);
            entity.Property(e => e.Power).HasMaxLength(50);
            entity.Property(e => e.Temperature).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.NfcTagId).HasMaxLength(200);
            entity.Property(e => e.QrCode).HasMaxLength(500);
            
            entity.HasOne(e => e.Drawer)
                .WithMany(d => d.Parts)
                .HasForeignKey(e => e.DrawerId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Parts)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasMany(e => e.Attachments)
                .WithOne(a => a.Part)
                .HasForeignKey(a => a.PartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Drawer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.NfcTagId).HasMaxLength(200);
            entity.Property(e => e.QrCode).HasMaxLength(500);
            
            // Create unique index on NFC tag ID when it's not null
            entity.HasIndex(e => e.NfcTagId).IsUnique().HasFilter("\"NfcTagId\" IS NOT NULL");
            entity.HasIndex(e => e.QrCode).IsUnique().HasFilter("\"QrCode\" IS NOT NULL");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
        });
        
        modelBuilder.Entity<PartAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(1000);
            entity.Property(e => e.FileUrl).HasMaxLength(1000);
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });
    }
}
