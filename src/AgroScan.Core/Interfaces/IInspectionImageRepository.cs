using AgroScan.Core.Entities;

namespace AgroScan.Core.Interfaces;

/// <summary>
/// Repository interface for InspectionImage entity with specific operations
/// </summary>
public interface IInspectionImageRepository : IGenericRepository<InspectionImage>
{
    /// <summary>
    /// Gets all images for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>Collection of images for the inspection</returns>
    Task<IEnumerable<InspectionImage>> GetByInspectionIdAsync(int inspectionId);
}
