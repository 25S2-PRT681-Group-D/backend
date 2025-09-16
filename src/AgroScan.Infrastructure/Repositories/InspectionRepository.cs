using Microsoft.EntityFrameworkCore;
using AgroScan.Core.Entities;
using AgroScan.Core.Interfaces;
using AgroScan.Infrastructure.Data;

namespace AgroScan.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Inspection entity
/// </summary>
public class InspectionRepository : GenericRepository<Inspection>, IInspectionRepository
{
    /// <summary>
    /// Initializes a new instance of the InspectionRepository
    /// </summary>
    /// <param name="context">Database context</param>
    public InspectionRepository(AgroScanDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Inspection>> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .Where(i => i.UserId == userId)
            .Include(i => i.InspectionImages)
            .Include(i => i.InspectionAnalyses)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Inspection?> GetWithRelatedDataAsync(int id)
    {
        return await _dbSet
            .Include(i => i.User)
            .Include(i => i.InspectionImages)
            .Include(i => i.InspectionAnalyses)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}
