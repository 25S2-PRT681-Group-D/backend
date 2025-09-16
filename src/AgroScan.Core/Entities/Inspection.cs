using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgroScan.Core.Enums;

namespace AgroScan.Core.Entities;

/// <summary>
/// Represents a plant inspection
/// </summary>
public class Inspection : BaseEntity
{
    /// <summary>
    /// Name of the plant being inspected
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string PlantName { get; set; } = string.Empty;
    
    /// <summary>
    /// Date when the inspection was performed
    /// </summary>
    [Required]
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// Country where the inspection took place
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;
    
    /// <summary>
    /// State where the inspection took place
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// City where the inspection took place
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Additional notes about the inspection
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Status of the inspection
    /// </summary>
    [Required]
    public InspectionStatus Status { get; set; }
    
    /// <summary>
    /// Category of the inspection
    /// </summary>
    [Required]
    public InspectionCategory Category { get; set; }
    
    /// <summary>
    /// Foreign key to the user who created this inspection
    /// </summary>
    [Required]
    public int UserId { get; set; }
    
    /// <summary>
    /// Navigation property to the user who created this inspection
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
    
    /// <summary>
    /// Navigation property for images associated with this inspection
    /// </summary>
    public virtual ICollection<InspectionImage> InspectionImages { get; set; } = new List<InspectionImage>();
    
    /// <summary>
    /// Navigation property for analysis of this inspection
    /// </summary>
    public virtual ICollection<InspectionAnalysis> InspectionAnalyses { get; set; } = new List<InspectionAnalysis>();
}
