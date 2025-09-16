using AgroScan.Core.DTOs;

namespace AgroScan.API.Services;

/// <summary>
/// Interface for user service operations
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets all users (Admin only)
    /// </summary>
    /// <returns>Collection of users</returns>
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    
    /// <summary>
    /// Gets a user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User if found, null otherwise</returns>
    Task<UserDto?> GetUserByIdAsync(int id);
    
    /// <summary>
    /// Creates a new user (Admin only)
    /// </summary>
    /// <param name="createUserDto">User creation data</param>
    /// <returns>Created user</returns>
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    
    /// <summary>
    /// Updates an existing user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="updateUserDto">User update data</param>
    /// <returns>Updated user if found, null otherwise</returns>
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    
    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteUserAsync(int id);
}
