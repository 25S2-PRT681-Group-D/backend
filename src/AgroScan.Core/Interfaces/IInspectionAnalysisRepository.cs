using AgroScan.Core.Entities;

namespace AgroScan.Core.Interfaces;

/// <summary>
/// Repository interface for InspectionAnalysis entity with specific operations
/// </summary>
public interface IInspectionAnalysisRepository : IGenericRepository<InspectionAnalysis>
{
    /// <summary>
    /// Gets all analyses for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>Collection of analyses for the inspection</returns>
    Task<IEnumerable<InspectionAnalysis>> GetByInspectionIdAsync(int inspectionId);
    
    /// <summary>
    /// Gets the latest analysis for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>The latest analysis if found, null otherwise</returns>
    Task<InspectionAnalysis?> GetLatestByInspectionIdAsync(int inspectionId);
}
