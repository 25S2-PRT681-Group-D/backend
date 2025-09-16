using AgroScan.Core.Entities;

namespace AgroScan.Core.Interfaces;

/// <summary>
/// Repository interface for User entity with specific operations
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// Gets a user by email address
    /// </summary>
    /// <param name="email">User's email address</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<User?> GetByEmailAsync(string email);
    
    /// <summary>
    /// Checks if an email address is already in use
    /// </summary>
    /// <param name="email">Email address to check</param>
    /// <returns>True if email is in use, false otherwise</returns>
    Task<bool> EmailExistsAsync(string email);
}
