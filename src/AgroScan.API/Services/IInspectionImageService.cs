using AgroScan.Core.DTOs;

namespace AgroScan.API.Services;

/// <summary>
/// Interface for inspection image service operations
/// </summary>
public interface IInspectionImageService
{
    /// <summary>
    /// Gets all images for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>Collection of images</returns>
    Task<IEnumerable<InspectionImageDto>> GetImagesByInspectionIdAsync(int inspectionId);
    
    /// <summary>
    /// Gets an image by ID
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>Image if found, null otherwise</returns>
    Task<InspectionImageDto?> GetImageByIdAsync(int id);
    
    /// <summary>
    /// Creates a new inspection image
    /// </summary>
    /// <param name="createImageDto">Image creation data</param>
    /// <param name="userId">User ID creating the image</param>
    /// <param name="isAdmin">Whether the user is an admin</param>
    /// <returns>Created image</returns>
    Task<InspectionImageDto> CreateImageAsync(CreateInspectionImageDto createImageDto, int userId, bool isAdmin);
    
    /// <summary>
    /// Deletes an inspection image
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <param name="userId">User ID deleting the image</param>
    /// <param name="isAdmin">Whether the user is an admin</param>
    /// <returns>True if deleted and authorized, false otherwise</returns>
    Task<bool> DeleteImageAsync(int id, int userId, bool isAdmin);
}
