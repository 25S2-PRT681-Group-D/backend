using AgroScan.Core.Entities;

namespace AgroScan.API.Services;

/// <summary>
/// Interface for JWT token operations
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for the specified user
    /// </summary>
    /// <param name="user">User entity</param>
    /// <returns>JWT token string</returns>
    string GenerateToken(User user);
    
    /// <summary>
    /// Validates a JWT token and extracts user information
    /// </summary>
    /// <param name="token">JWT token string</param>
    /// <returns>User ID if token is valid, null otherwise</returns>
    int? ValidateToken(string token);
}
