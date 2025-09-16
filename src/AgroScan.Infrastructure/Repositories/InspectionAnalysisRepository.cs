using Microsoft.EntityFrameworkCore;
using AgroScan.Core.Entities;
using AgroScan.Core.Interfaces;
using AgroScan.Infrastructure.Data;

namespace AgroScan.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for InspectionAnalysis entity
/// </summary>
public class InspectionAnalysisRepository : GenericRepository<InspectionAnalysis>, IInspectionAnalysisRepository
{
    /// <summary>
    /// Initializes a new instance of the InspectionAnalysisRepository
    /// </summary>
    /// <param name="context">Database context</param>
    public InspectionAnalysisRepository(AgroScanDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<InspectionAnalysis>> GetByInspectionIdAsync(int inspectionId)
    {
        return await _dbSet
            .Where(analysis => analysis.InspectionId == inspectionId)
            .OrderByDescending(analysis => analysis.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<InspectionAnalysis?> GetLatestByInspectionIdAsync(int inspectionId)
    {
        return await _dbSet
            .Where(analysis => analysis.InspectionId == inspectionId)
            .OrderByDescending(analysis => analysis.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
