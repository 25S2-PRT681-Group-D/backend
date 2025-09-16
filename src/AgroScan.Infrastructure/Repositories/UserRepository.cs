using Microsoft.EntityFrameworkCore;
using AgroScan.Core.Entities;
using AgroScan.Core.Interfaces;
using AgroScan.Infrastructure.Data;

namespace AgroScan.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for User entity
/// </summary>
public class UserRepository : GenericRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the UserRepository
    /// </summary>
    /// <param name="context">Database context</param>
    public UserRepository(AgroScanDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <inheritdoc />
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }
}
