namespace AgroScan.Core.Enums;

/// <summary>
/// Represents the status of an inspection analysis
/// </summary>
public enum AnalysisStatus
{
    /// <summary>
    /// Analysis is pending
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// Analysis is in progress
    /// </summary>
    InProgress = 1,
    
    /// <summary>
    /// Analysis is completed
    /// </summary>
    Completed = 2,
    
    /// <summary>
    /// Analysis failed
    /// </summary>
    Failed = 3
}
