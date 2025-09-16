using AgroScan.Core.DTOs;
using AgroScan.Core.Entities;
using AgroScan.Core.Interfaces;

namespace AgroScan.API.Services;

/// <summary>
/// Service for inspection operations
/// </summary>
public class InspectionService : IInspectionService
{
    private readonly IInspectionRepository _inspectionRepository;

    /// <summary>
    /// Initializes a new instance of the InspectionService
    /// </summary>
    /// <param name="inspectionRepository">Inspection repository</param>
    public InspectionService(IInspectionRepository inspectionRepository)
    {
        _inspectionRepository = inspectionRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<InspectionDto>> GetUserInspectionsAsync(int userId)
    {
        var inspections = await _inspectionRepository.GetByUserIdAsync(userId);
        return inspections.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<InspectionDto>> GetAllInspectionsAsync()
    {
        var inspections = await _inspectionRepository.GetAllAsync();
        return inspections.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task<InspectionDto?> GetInspectionByIdAsync(int id)
    {
        var inspection = await _inspectionRepository.GetWithRelatedDataAsync(id);
        return inspection != null ? MapToDto(inspection) : null;
    }

    /// <inheritdoc />
    public async Task<InspectionDto> CreateInspectionAsync(CreateInspectionDto createInspectionDto, int userId)
    {
        var inspection = new Inspection
        {
            PlantName = createInspectionDto.PlantName,
            InspectionDate = createInspectionDto.InspectionDate,
            Country = createInspectionDto.Country,
            State = createInspectionDto.State,
            City = createInspectionDto.City,
            Notes = createInspectionDto.Notes,
            Status = createInspectionDto.Status,
            Category = createInspectionDto.Category,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _inspectionRepository.AddAsync(inspection);
        await _inspectionRepository.SaveChangesAsync();

        return MapToDto(inspection);
    }

    /// <inheritdoc />
    public async Task<InspectionDto?> UpdateInspectionAsync(int id, UpdateInspectionDto updateInspectionDto, int userId, bool isAdmin)
    {
        var inspection = await _inspectionRepository.GetByIdAsync(id);
        if (inspection == null)
        {
            return null;
        }

        // Check authorization - only admin or owner can update
        if (!isAdmin && inspection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only update your own inspections");
        }

        inspection.PlantName = updateInspectionDto.PlantName;
        inspection.InspectionDate = updateInspectionDto.InspectionDate;
        inspection.Country = updateInspectionDto.Country;
        inspection.State = updateInspectionDto.State;
        inspection.City = updateInspectionDto.City;
        inspection.Notes = updateInspectionDto.Notes;
        inspection.Status = updateInspectionDto.Status;
        inspection.Category = updateInspectionDto.Category;
        inspection.UpdatedAt = DateTime.UtcNow;

        _inspectionRepository.Update(inspection);
        await _inspectionRepository.SaveChangesAsync();

        return MapToDto(inspection);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteInspectionAsync(int id, int userId, bool isAdmin)
    {
        var inspection = await _inspectionRepository.GetByIdAsync(id);
        if (inspection == null)
        {
            return false;
        }

        // Check authorization - only admin or owner can delete
        if (!isAdmin && inspection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete your own inspections");
        }

        _inspectionRepository.Remove(inspection);
        await _inspectionRepository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Maps Inspection entity to InspectionDto
    /// </summary>
    /// <param name="inspection">Inspection entity</param>
    /// <returns>InspectionDto</returns>
    private static InspectionDto MapToDto(Inspection inspection)
    {
        return new InspectionDto
        {
            Id = inspection.Id,
            PlantName = inspection.PlantName,
            InspectionDate = inspection.InspectionDate,
            Country = inspection.Country,
            State = inspection.State,
            City = inspection.City,
            Notes = inspection.Notes,
            Status = inspection.Status,
            Category = inspection.Category,
            UserId = inspection.UserId,
            CreatedAt = inspection.CreatedAt,
            UpdatedAt = inspection.UpdatedAt
        };
    }
}
