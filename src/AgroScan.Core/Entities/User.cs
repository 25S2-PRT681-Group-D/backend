using System.ComponentModel.DataAnnotations;
using AgroScan.Core.Enums;

namespace AgroScan.Core.Entities;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// User's first name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// User's last name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// User's role in the system
    /// </summary>
    [Required]
    public UserRole Role { get; set; }
    
    /// <summary>
    /// User's email address
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// User's hashed password
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Navigation property for inspections created by this user
    /// </summary>
    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
}
