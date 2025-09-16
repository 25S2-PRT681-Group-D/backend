using AgroScan.Core.DTOs;
using AgroScan.Core.Entities;
using AgroScan.Core.Interfaces;
using BCrypt.Net;

namespace AgroScan.API.Services;

/// <summary>
/// Service for user operations
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the UserService
    /// </summary>
    /// <param name="userRepository">User repository</param>
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    /// <inheritdoc />
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? MapToDto(user) : null;
    }

    /// <inheritdoc />
    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Check if email already exists
        if (await _userRepository.EmailExistsAsync(createUserDto.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
            Role = createUserDto.Role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return MapToDto(user);
    }

    /// <inheritdoc />
    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return null;
        }

        // Check if email is being changed and if it already exists
        if (user.Email != updateUserDto.Email && await _userRepository.EmailExistsAsync(updateUserDto.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.Email = updateUserDto.Email;
        user.Role = updateUserDto.Role;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return MapToDto(user);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        _userRepository.Remove(user);
        await _userRepository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Maps User entity to UserDto
    /// </summary>
    /// <param name="user">User entity</param>
    /// <returns>UserDto</returns>
    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
