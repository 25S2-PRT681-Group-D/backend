namespace AgroScan.Core.Enums;

/// <summary>
/// Represents the status of an inspection
/// </summary>
public enum InspectionStatus
{
    /// <summary>
    /// Inspection is pending
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// Inspection is in progress
    /// </summary>
    InProgress = 1,
    
    /// <summary>
    /// Inspection is completed
    /// </summary>
    Completed = 2,
    
    /// <summary>
    /// Inspection is cancelled
    /// </summary>
    Cancelled = 3
}
