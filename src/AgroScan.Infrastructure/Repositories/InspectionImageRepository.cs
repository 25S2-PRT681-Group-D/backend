using Microsoft.EntityFrameworkCore;
using AgroScan.Core.Entities;
using AgroScan.Core.Interfaces;
using AgroScan.Infrastructure.Data;

namespace AgroScan.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for InspectionImage entity
/// </summary>
public class InspectionImageRepository : GenericRepository<InspectionImage>, IInspectionImageRepository
{
    /// <summary>
    /// Initializes a new instance of the InspectionImageRepository
    /// </summary>
    /// <param name="context">Database context</param>
    public InspectionImageRepository(AgroScanDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<InspectionImage>> GetByInspectionIdAsync(int inspectionId)
    {
        return await _dbSet
            .Where(img => img.InspectionId == inspectionId)
            .OrderBy(img => img.CreatedAt)
            .ToListAsync();
    }
}
