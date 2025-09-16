using AgroScan.Core.Enums;

namespace AgroScan.Core.DTOs;

/// <summary>
/// Data transfer object for InspectionAnalysis entity
/// </summary>
public class InspectionAnalysisDto
{
    /// <summary>
    /// Analysis ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// ID of the inspection this analysis belongs to
    /// </summary>
    public int InspectionId { get; set; }
    
    /// <summary>
    /// Status of the analysis
    /// </summary>
    public AnalysisStatus Status { get; set; }
    
    /// <summary>
    /// Confidence score of the analysis (0.0 to 1.0)
    /// </summary>
    public decimal ConfidenceScore { get; set; }
    
    /// <summary>
    /// Description of the analysis results
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Treatment recommendations based on the analysis
    /// </summary>
    public string? TreatmentRecommendation { get; set; }
    
    /// <summary>
    /// Date when the analysis was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the analysis was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Data transfer object for creating a new inspection analysis
/// </summary>
public class CreateInspectionAnalysisDto
{
    /// <summary>
    /// ID of the inspection this analysis belongs to
    /// </summary>
    public int InspectionId { get; set; }
    
    /// <summary>
    /// Status of the analysis
    /// </summary>
    public AnalysisStatus Status { get; set; }
    
    /// <summary>
    /// Confidence score of the analysis (0.0 to 1.0)
    /// </summary>
    public decimal ConfidenceScore { get; set; }
    
    /// <summary>
    /// Description of the analysis results
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Treatment recommendations based on the analysis
    /// </summary>
    public string? TreatmentRecommendation { get; set; }
}
