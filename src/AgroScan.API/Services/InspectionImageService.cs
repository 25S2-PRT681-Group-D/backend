using AgroScan.Core.DTOs;
using AgroScan.Core.Entities;
using AgroScan.Core.Interfaces;

namespace AgroScan.API.Services;

/// <summary>
/// Service for inspection image operations
/// </summary>
public class InspectionImageService : IInspectionImageService
{
    private readonly IInspectionImageRepository _imageRepository;
    private readonly IInspectionRepository _inspectionRepository;

    /// <summary>
    /// Initializes a new instance of the InspectionImageService
    /// </summary>
    /// <param name="imageRepository">Image repository</param>
    /// <param name="inspectionRepository">Inspection repository</param>
    public InspectionImageService(IInspectionImageRepository imageRepository, IInspectionRepository inspectionRepository)
    {
        _imageRepository = imageRepository;
        _inspectionRepository = inspectionRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<InspectionImageDto>> GetImagesByInspectionIdAsync(int inspectionId)
    {
        var images = await _imageRepository.GetByInspectionIdAsync(inspectionId);
        return images.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task<InspectionImageDto?> GetImageByIdAsync(int id)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        return image != null ? MapToDto(image) : null;
    }

    /// <inheritdoc />
    public async Task<InspectionImageDto> CreateImageAsync(CreateInspectionImageDto createImageDto, int userId, bool isAdmin)
    {
        // Verify the inspection exists and user has access
        var inspection = await _inspectionRepository.GetByIdAsync(createImageDto.InspectionId);
        if (inspection == null)
        {
            throw new InvalidOperationException("Inspection not found");
        }

        // Check authorization - only admin or owner can add images
        if (!isAdmin && inspection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only add images to your own inspections");
        }

        var image = new InspectionImage
        {
            InspectionId = createImageDto.InspectionId,
            Image = createImageDto.Image,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _imageRepository.AddAsync(image);
        await _imageRepository.SaveChangesAsync();

        return MapToDto(image);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteImageAsync(int id, int userId, bool isAdmin)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        if (image == null)
        {
            return false;
        }

        // Get the inspection to check ownership
        var inspection = await _inspectionRepository.GetByIdAsync(image.InspectionId);
        if (inspection == null)
        {
            return false;
        }

        // Check authorization - only admin or owner can delete images
        if (!isAdmin && inspection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete images from your own inspections");
        }

        _imageRepository.Remove(image);
        await _imageRepository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Maps InspectionImage entity to InspectionImageDto
    /// </summary>
    /// <param name="image">Image entity</param>
    /// <returns>InspectionImageDto</returns>
    private static InspectionImageDto MapToDto(InspectionImage image)
    {
        return new InspectionImageDto
        {
            Id = image.Id,
            InspectionId = image.InspectionId,
            Image = image.Image,
            CreatedAt = image.CreatedAt,
            UpdatedAt = image.UpdatedAt
        };
    }
}
