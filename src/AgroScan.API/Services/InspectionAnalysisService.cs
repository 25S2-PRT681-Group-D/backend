using AgroScan.Core.DTOs;
using AgroScan.Core.Entities;
using AgroScan.Core.Interfaces;

namespace AgroScan.API.Services;

/// <summary>
/// Service for inspection analysis operations
/// </summary>
public class InspectionAnalysisService : IInspectionAnalysisService
{
    private readonly IInspectionAnalysisRepository _analysisRepository;
    private readonly IInspectionRepository _inspectionRepository;

    /// <summary>
    /// Initializes a new instance of the InspectionAnalysisService
    /// </summary>
    /// <param name="analysisRepository">Analysis repository</param>
    /// <param name="inspectionRepository">Inspection repository</param>
    public InspectionAnalysisService(IInspectionAnalysisRepository analysisRepository, IInspectionRepository inspectionRepository)
    {
        _analysisRepository = analysisRepository;
        _inspectionRepository = inspectionRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<InspectionAnalysisDto>> GetAnalysesByInspectionIdAsync(int inspectionId)
    {
        var analyses = await _analysisRepository.GetByInspectionIdAsync(inspectionId);
        return analyses.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task<InspectionAnalysisDto?> GetAnalysisByIdAsync(int id)
    {
        var analysis = await _analysisRepository.GetByIdAsync(id);
        return analysis != null ? MapToDto(analysis) : null;
    }

    /// <inheritdoc />
    public async Task<InspectionAnalysisDto?> GetLatestAnalysisByInspectionIdAsync(int inspectionId)
    {
        var analysis = await _analysisRepository.GetLatestByInspectionIdAsync(inspectionId);
        return analysis != null ? MapToDto(analysis) : null;
    }

    /// <inheritdoc />
    public async Task<InspectionAnalysisDto> CreateAnalysisAsync(CreateInspectionAnalysisDto createAnalysisDto, int userId, bool isAdmin)
    {
        // Verify the inspection exists and user has access
        var inspection = await _inspectionRepository.GetByIdAsync(createAnalysisDto.InspectionId);
        if (inspection == null)
        {
            throw new InvalidOperationException("Inspection not found");
        }

        // Check authorization - only admin or owner can add analyses
        if (!isAdmin && inspection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only add analyses to your own inspections");
        }

        var analysis = new InspectionAnalysis
        {
            InspectionId = createAnalysisDto.InspectionId,
            Status = createAnalysisDto.Status,
            ConfidenceScore = createAnalysisDto.ConfidenceScore,
            Description = createAnalysisDto.Description,
            TreatmentRecommendation = createAnalysisDto.TreatmentRecommendation,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _analysisRepository.AddAsync(analysis);
        await _analysisRepository.SaveChangesAsync();

        return MapToDto(analysis);
    }

    /// <inheritdoc />
    public async Task<InspectionAnalysisDto?> UpdateAnalysisAsync(int id, CreateInspectionAnalysisDto updateAnalysisDto, int userId, bool isAdmin)
    {
        var analysis = await _analysisRepository.GetByIdAsync(id);
        if (analysis == null)
        {
            return null;
        }

        // Get the inspection to check ownership
        var inspection = await _inspectionRepository.GetByIdAsync(analysis.InspectionId);
        if (inspection == null)
        {
            return null;
        }

        // Check authorization - only admin or owner can update analyses
        if (!isAdmin && inspection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only update analyses for your own inspections");
        }

        analysis.Status = updateAnalysisDto.Status;
        analysis.ConfidenceScore = updateAnalysisDto.ConfidenceScore;
        analysis.Description = updateAnalysisDto.Description;
        analysis.TreatmentRecommendation = updateAnalysisDto.TreatmentRecommendation;
        analysis.UpdatedAt = DateTime.UtcNow;

        _analysisRepository.Update(analysis);
        await _analysisRepository.SaveChangesAsync();

        return MapToDto(analysis);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAnalysisAsync(int id, int userId, bool isAdmin)
    {
        var analysis = await _analysisRepository.GetByIdAsync(id);
        if (analysis == null)
        {
            return false;
        }

        // Get the inspection to check ownership
        var inspection = await _inspectionRepository.GetByIdAsync(analysis.InspectionId);
        if (inspection == null)
        {
            return false;
        }

        // Check authorization - only admin or owner can delete analyses
        if (!isAdmin && inspection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can only delete analyses for your own inspections");
        }

        _analysisRepository.Remove(analysis);
        await _analysisRepository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Maps InspectionAnalysis entity to InspectionAnalysisDto
    /// </summary>
    /// <param name="analysis">Analysis entity</param>
    /// <returns>InspectionAnalysisDto</returns>
    private static InspectionAnalysisDto MapToDto(InspectionAnalysis analysis)
    {
        return new InspectionAnalysisDto
        {
            Id = analysis.Id,
            InspectionId = analysis.InspectionId,
            Status = analysis.Status,
            ConfidenceScore = analysis.ConfidenceScore,
            Description = analysis.Description,
            TreatmentRecommendation = analysis.TreatmentRecommendation,
            CreatedAt = analysis.CreatedAt,
            UpdatedAt = analysis.UpdatedAt
        };
    }
}
