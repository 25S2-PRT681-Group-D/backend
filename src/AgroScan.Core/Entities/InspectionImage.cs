using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroScan.Core.Entities;

/// <summary>
/// Represents an image associated with an inspection
/// </summary>
public class InspectionImage : BaseEntity
{
    /// <summary>
    /// Foreign key to the inspection this image belongs to
    /// </summary>
    [Required]
    public int InspectionId { get; set; }
    
    /// <summary>
    /// Path or URL to the image file
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Image { get; set; } = string.Empty;
    
    /// <summary>
    /// Navigation property to the inspection this image belongs to
    /// </summary>
    [ForeignKey(nameof(InspectionId))]
    public virtual Inspection Inspection { get; set; } = null!;
}
