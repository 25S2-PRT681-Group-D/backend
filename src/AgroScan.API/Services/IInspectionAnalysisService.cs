using AgroScan.Core.DTOs;

namespace AgroScan.API.Services;

/// <summary>
/// Interface for inspection analysis service operations
/// </summary>
public interface IInspectionAnalysisService
{
    /// <summary>
    /// Gets all analyses for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>Collection of analyses</returns>
    Task<IEnumerable<InspectionAnalysisDto>> GetAnalysesByInspectionIdAsync(int inspectionId);
    
    /// <summary>
    /// Gets an analysis by ID
    /// </summary>
    /// <param name="id">Analysis ID</param>
    /// <returns>Analysis if found, null otherwise</returns>
    Task<InspectionAnalysisDto?> GetAnalysisByIdAsync(int id);
    
    /// <summary>
    /// Gets the latest analysis for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>Latest analysis if found, null otherwise</returns>
    Task<InspectionAnalysisDto?> GetLatestAnalysisByInspectionIdAsync(int inspectionId);
    
    /// <summary>
    /// Creates a new inspection analysis
    /// </summary>
    /// <param name="createAnalysisDto">Analysis creation data</param>
    /// <param name="userId">User ID creating the analysis</param>
    /// <param name="isAdmin">Whether the user is an admin</param>
    /// <returns>Created analysis</returns>
    Task<InspectionAnalysisDto> CreateAnalysisAsync(CreateInspectionAnalysisDto createAnalysisDto, int userId, bool isAdmin);
    
    /// <summary>
    /// Updates an existing inspection analysis
    /// </summary>
    /// <param name="id">Analysis ID</param>
    /// <param name="updateAnalysisDto">Analysis update data</param>
    /// <param name="userId">User ID updating the analysis</param>
    /// <param name="isAdmin">Whether the user is an admin</param>
    /// <returns>Updated analysis if found and authorized, null otherwise</returns>
    Task<InspectionAnalysisDto?> UpdateAnalysisAsync(int id, CreateInspectionAnalysisDto updateAnalysisDto, int userId, bool isAdmin);
    
    /// <summary>
    /// Deletes an inspection analysis
    /// </summary>
    /// <param name="id">Analysis ID</param>
    /// <param name="userId">User ID deleting the analysis</param>
    /// <param name="isAdmin">Whether the user is an admin</param>
    /// <returns>True if deleted and authorized, false otherwise</returns>
    Task<bool> DeleteAnalysisAsync(int id, int userId, bool isAdmin);
}
