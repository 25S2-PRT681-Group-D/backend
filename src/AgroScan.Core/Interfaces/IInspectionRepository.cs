using AgroScan.Core.Entities;

namespace AgroScan.Core.Interfaces;

/// <summary>
/// Repository interface for Inspection entity with specific operations
/// </summary>
public interface IInspectionRepository : IGenericRepository<Inspection>
{
    /// <summary>
    /// Gets inspections for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Collection of inspections for the user</returns>
    Task<IEnumerable<Inspection>> GetByUserIdAsync(int userId);
    
    /// <summary>
    /// Gets an inspection with its related data (images and analyses)
    /// </summary>
    /// <param name="id">Inspection ID</param>
    /// <returns>The inspection with related data if found, null otherwise</returns>
    Task<Inspection?> GetWithRelatedDataAsync(int id);
}
