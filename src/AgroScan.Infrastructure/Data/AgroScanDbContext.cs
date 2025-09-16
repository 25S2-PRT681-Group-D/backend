using Microsoft.EntityFrameworkCore;
using AgroScan.Core.Entities;

namespace AgroScan.Infrastructure.Data;

/// <summary>
/// Database context for AgroScan application
/// </summary>
public class AgroScanDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the AgroScanDbContext
    /// </summary>
    /// <param name="options">Database context options</param>
    public AgroScanDbContext(DbContextOptions<AgroScanDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Inspections table
    /// </summary>
    public DbSet<Inspection> Inspections { get; set; }

    /// <summary>
    /// Inspection images table
    /// </summary>
    public DbSet<InspectionImage> InspectionImages { get; set; }

    /// <summary>
    /// Inspection analyses table
    /// </summary>
    public DbSet<InspectionAnalysis> InspectionAnalyses { get; set; }

    /// <summary>
    /// Configures the model relationships and constraints
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure Inspection entity
        modelBuilder.Entity<Inspection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PlantName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.InspectionDate).IsRequired();
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.State).IsRequired().HasMaxLength(100);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Category).IsRequired();
            entity.Property(e => e.UserId).IsRequired();

            // Configure relationship with User
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Inspections)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure InspectionImage entity
        modelBuilder.Entity<InspectionImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InspectionId).IsRequired();
            entity.Property(e => e.Image).IsRequired().HasMaxLength(500);

            // Configure relationship with Inspection
            entity.HasOne(e => e.Inspection)
                  .WithMany(i => i.InspectionImages)
                  .HasForeignKey(e => e.InspectionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

            // Configure InspectionAnalysis entity
            modelBuilder.Entity<InspectionAnalysis>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InspectionId).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.ConfidenceScore).IsRequired().HasColumnType("REAL");
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.TreatmentRecommendation).HasMaxLength(2000);

                // Configure relationship with Inspection
                entity.HasOne(e => e.Inspection)
                      .WithMany(i => i.InspectionAnalyses)
                      .HasForeignKey(e => e.InspectionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        // Configure enum conversions
        modelBuilder.Entity<User>()
            .Property(e => e.Role)
            .HasConversion<int>();

        modelBuilder.Entity<Inspection>()
            .Property(e => e.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Inspection>()
            .Property(e => e.Category)
            .HasConversion<int>();

        modelBuilder.Entity<InspectionAnalysis>()
            .Property(e => e.Status)
            .HasConversion<int>();
    }
}
