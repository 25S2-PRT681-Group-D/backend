namespace AgroScan.Core.DTOs;

/// <summary>
/// Data transfer object for user login
/// </summary>
public class LoginDto
{
    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// User's password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for user registration
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// User's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// User's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// User's password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for authentication response
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// JWT token for authentication
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Token expiration time
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// User information
    /// </summary>
    public UserDto User { get; set; } = null!;
}
