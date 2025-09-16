using AgroScan.Core.Enums;

namespace AgroScan.Core.DTOs;

/// <summary>
/// Data transfer object for Inspection entity
/// </summary>
public class InspectionDto
{
    /// <summary>
    /// Inspection ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Name of the plant being inspected
    /// </summary>
    public string PlantName { get; set; } = string.Empty;
    
    /// <summary>
    /// Date when the inspection was performed
    /// </summary>
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// Country where the inspection took place
    /// </summary>
    public string Country { get; set; } = string.Empty;
    
    /// <summary>
    /// State where the inspection took place
    /// </summary>
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// City where the inspection took place
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Additional notes about the inspection
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Status of the inspection
    /// </summary>
    public InspectionStatus Status { get; set; }
    
    /// <summary>
    /// Category of the inspection
    /// </summary>
    public InspectionCategory Category { get; set; }
    
    /// <summary>
    /// ID of the user who created this inspection
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Date when the inspection was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the inspection was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Image URLs linked to this inspection
    /// </summary>
    public List<string> Images { get; set; } = new();
}

/// <summary>
/// Data transfer object for creating a new inspection
/// </summary>
public class CreateInspectionDto
{
    /// <summary>
    /// Name of the plant being inspected
    /// </summary>
    public string PlantName { get; set; } = string.Empty;
    
    /// <summary>
    /// Date when the inspection was performed
    /// </summary>
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// Country where the inspection took place
    /// </summary>
    public string Country { get; set; } = string.Empty;
    
    /// <summary>
    /// State where the inspection took place
    /// </summary>
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// City where the inspection took place
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Additional notes about the inspection
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Status of the inspection
    /// </summary>
    public InspectionStatus Status { get; set; }
    
    /// <summary>
    /// Category of the inspection
    /// </summary>
    public InspectionCategory Category { get; set; }
}

/// <summary>
/// Data transfer object for updating an inspection
/// </summary>
public class UpdateInspectionDto
{
    /// <summary>
    /// Name of the plant being inspected
    /// </summary>
    public string PlantName { get; set; } = string.Empty;
    
    /// <summary>
    /// Date when the inspection was performed
    /// </summary>
    public DateTime InspectionDate { get; set; }
    
    /// <summary>
    /// Country where the inspection took place
    /// </summary>
    public string Country { get; set; } = string.Empty;
    
    /// <summary>
    /// State where the inspection took place
    /// </summary>
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// City where the inspection took place
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Additional notes about the inspection
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Status of the inspection
    /// </summary>
    public InspectionStatus Status { get; set; }
    
    /// <summary>
    /// Category of the inspection
    /// </summary>
    public InspectionCategory Category { get; set; }
}
