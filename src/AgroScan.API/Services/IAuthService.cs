using AgroScan.Core.DTOs;

namespace AgroScan.API.Services;

/// <summary>
/// Interface for authentication operations
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="registerDto">User registration data</param>
    /// <returns>Authentication response with token</returns>
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    
    /// <summary>
    /// Logs in a user
    /// </summary>
    /// <param name="loginDto">User login data</param>
    /// <returns>Authentication response with token</returns>
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}
