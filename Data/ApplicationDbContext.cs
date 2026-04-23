using ftpms.Models;
using Microsoft.EntityFrameworkCore;

namespace ftpms.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Measurement> Measurements => Set<Measurement>();
    public DbSet<Design> Designs => Set<Design>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(255);
            entity.Property(x => x.PhoneNumber).HasMaxLength(50);

            entity.HasMany(x => x.Measurements)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Measurement>(entity =>
        {
            entity.Property(x => x.TemplateType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Notes).HasMaxLength(1000);


            entity.HasOne(x => x.ParentMeasurement)
                .WithMany(x => x.ChildMeasurements)
                .HasForeignKey(x => x.ParentMeasurementId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.Chest).HasPrecision(18, 2);
            entity.Property(x => x.Waist).HasPrecision(18, 2);
            entity.Property(x => x.Hip).HasPrecision(18, 2);
            entity.Property(x => x.Shoulder).HasPrecision(18, 2);
            entity.Property(x => x.Neck).HasPrecision(18, 2);
            entity.Property(x => x.SleeveLength).HasPrecision(18, 2);
            entity.Property(x => x.ArmRound).HasPrecision(18, 2);
            entity.Property(x => x.Wrist).HasPrecision(18, 2);
            entity.Property(x => x.Bicep).HasPrecision(18, 2);
            entity.Property(x => x.TopLength).HasPrecision(18, 2);
            entity.Property(x => x.TrouserLength).HasPrecision(18, 2);
            entity.Property(x => x.Thigh).HasPrecision(18, 2);
            entity.Property(x => x.Knee).HasPrecision(18, 2);
            entity.Property(x => x.Ankle).HasPrecision(18, 2);
            entity.Property(x => x.Inseam).HasPrecision(18, 2);
            entity.Property(x => x.BustPoint).HasPrecision(18, 2);
            entity.Property(x => x.UnderBust).HasPrecision(18, 2);
            entity.Property(x => x.RoundSleeve).HasPrecision(18, 2);
            entity.Property(x => x.GownLength).HasPrecision(18, 2);
            entity.Property(x => x.SkirtLength).HasPrecision(18, 2);
        });
    }
}
