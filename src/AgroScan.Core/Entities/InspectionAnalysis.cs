using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgroScan.Core.Enums;

namespace AgroScan.Core.Entities;

/// <summary>
/// Represents the analysis result of an inspection
/// </summary>
public class InspectionAnalysis : BaseEntity
{
    /// <summary>
    /// Foreign key to the inspection this analysis belongs to
    /// </summary>
    [Required]
    public int InspectionId { get; set; }
    
    /// <summary>
    /// Status of the analysis
    /// </summary>
    [Required]
    public AnalysisStatus Status { get; set; }
    
    /// <summary>
    /// Confidence score of the analysis (0.0 to 1.0)
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(3,2)")]
    [Range(0.0, 1.0)]
    public decimal ConfidenceScore { get; set; }
    
    /// <summary>
    /// Description of the analysis results
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Treatment recommendations based on the analysis
    /// </summary>
    [MaxLength(2000)]
    public string? TreatmentRecommendation { get; set; }
    
    /// <summary>
    /// Navigation property to the inspection this analysis belongs to
    /// </summary>
    [ForeignKey(nameof(InspectionId))]
    public virtual Inspection Inspection { get; set; } = null!;
}
