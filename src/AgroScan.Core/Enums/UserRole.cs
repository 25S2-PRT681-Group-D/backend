namespace AgroScan.Core.Enums;

/// <summary>
/// Represents the role of a user in the system
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Farmer role - can manage their own inspections
    /// </summary>
    Farmer = 0,
    
    /// <summary>
    /// Admin role - can manage all users and inspections
    /// </summary>
    Admin = 1
}
