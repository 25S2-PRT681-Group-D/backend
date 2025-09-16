using AgroScan.Core.DTOs;

namespace AgroScan.API.Services;

/// <summary>
/// Interface for inspection service operations
/// </summary>
public interface IInspectionService
{
    /// <summary>
    /// Gets all inspections for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Collection of inspections</returns>
    Task<IEnumerable<InspectionDto>> GetUserInspectionsAsync(int userId);
    
    /// <summary>
    /// Gets all inspections (Admin only)
    /// </summary>
    /// <returns>Collection of all inspections</returns>
    Task<IEnumerable<InspectionDto>> GetAllInspectionsAsync();
    
    /// <summary>
    /// Gets an inspection by ID
    /// </summary>
    /// <param name="id">Inspection ID</param>
    /// <returns>Inspection if found, null otherwise</returns>
    Task<InspectionDto?> GetInspectionByIdAsync(int id);
    
    /// <summary>
    /// Creates a new inspection
    /// </summary>
    /// <param name="createInspectionDto">Inspection creation data</param>
    /// <param name="userId">User ID creating the inspection</param>
    /// <returns>Created inspection</returns>
    Task<InspectionDto> CreateInspectionAsync(CreateInspectionDto createInspectionDto, int userId);
    
    /// <summary>
    /// Updates an existing inspection
    /// </summary>
    /// <param name="id">Inspection ID</param>
    /// <param name="updateInspectionDto">Inspection update data</param>
    /// <param name="userId">User ID updating the inspection</param>
    /// <param name="isAdmin">Whether the user is an admin</param>
    /// <returns>Updated inspection if found and authorized, null otherwise</returns>
    Task<InspectionDto?> UpdateInspectionAsync(int id, UpdateInspectionDto updateInspectionDto, int userId, bool isAdmin);
    
    /// <summary>
    /// Deletes an inspection
    /// </summary>
    /// <param name="id">Inspection ID</param>
    /// <param name="userId">User ID deleting the inspection</param>
    /// <param name="isAdmin">Whether the user is an admin</param>
    /// <returns>True if deleted and authorized, false otherwise</returns>
    Task<bool> DeleteInspectionAsync(int id, int userId, bool isAdmin);
}
