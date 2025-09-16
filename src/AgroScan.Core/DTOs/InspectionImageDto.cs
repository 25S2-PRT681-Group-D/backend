namespace AgroScan.Core.DTOs;

/// <summary>
/// Data transfer object for InspectionImage entity
/// </summary>
public class InspectionImageDto
{
    /// <summary>
    /// Image ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// ID of the inspection this image belongs to
    /// </summary>
    public int InspectionId { get; set; }
    
    /// <summary>
    /// Path or URL to the image file
    /// </summary>
    public string Image { get; set; } = string.Empty;
    
    /// <summary>
    /// Date when the image was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the image was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Data transfer object for creating a new inspection image
/// </summary>
public class CreateInspectionImageDto
{
    /// <summary>
    /// ID of the inspection this image belongs to
    /// </summary>
    public int InspectionId { get; set; }
    
    /// <summary>
    /// Path or URL to the image file
    /// </summary>
    public string Image { get; set; } = string.Empty;
}
