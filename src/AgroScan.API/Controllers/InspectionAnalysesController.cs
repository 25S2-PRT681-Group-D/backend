using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgroScan.API.Services;
using AgroScan.Core.DTOs;
using AgroScan.Core.Enums;
using System.Security.Claims;

namespace AgroScan.API.Controllers;

/// <summary>
/// Controller for inspection analysis management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class InspectionAnalysesController : ControllerBase
{
    private readonly IInspectionAnalysisService _analysisService;
    private readonly ILogger<InspectionAnalysesController> _logger;

    /// <summary>
    /// Initializes a new instance of the InspectionAnalysesController
    /// </summary>
    /// <param name="analysisService">Analysis service</param>
    /// <param name="logger">Logger</param>
    public InspectionAnalysesController(IInspectionAnalysisService analysisService, ILogger<InspectionAnalysesController> logger)
    {
        _analysisService = analysisService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all analyses for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>Collection of analyses</returns>
    /// <response code="200">Analyses retrieved successfully</response>
    [HttpGet("inspection/{inspectionId}")]
    [ProducesResponseType(typeof(IEnumerable<InspectionAnalysisDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InspectionAnalysisDto>>> GetAnalysesByInspectionId(int inspectionId)
    {
        try
        {
            var analyses = await _analysisService.GetAnalysesByInspectionIdAsync(inspectionId);
            return Ok(analyses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving analyses for inspection: {InspectionId}", inspectionId);
            return StatusCode(500, new { message = "An error occurred while retrieving analyses" });
        }
    }

    /// <summary>
    /// Gets the latest analysis for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>Latest analysis if found</returns>
    /// <response code="200">Analysis retrieved successfully</response>
    /// <response code="404">No analysis found</response>
    [HttpGet("inspection/{inspectionId}/latest")]
    [ProducesResponseType(typeof(InspectionAnalysisDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionAnalysisDto>> GetLatestAnalysisByInspectionId(int inspectionId)
    {
        try
        {
            var analysis = await _analysisService.GetLatestAnalysisByInspectionIdAsync(inspectionId);
            if (analysis == null)
            {
                return NotFound(new { message = "No analysis found for this inspection" });
            }

            return Ok(analysis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving latest analysis for inspection: {InspectionId}", inspectionId);
            return StatusCode(500, new { message = "An error occurred while retrieving the analysis" });
        }
    }

    /// <summary>
    /// Gets an analysis by ID
    /// </summary>
    /// <param name="id">Analysis ID</param>
    /// <returns>Analysis if found</returns>
    /// <response code="200">Analysis retrieved successfully</response>
    /// <response code="404">Analysis not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InspectionAnalysisDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionAnalysisDto>> GetAnalysis(int id)
    {
        try
        {
            var analysis = await _analysisService.GetAnalysisByIdAsync(id);
            if (analysis == null)
            {
                return NotFound(new { message = "Analysis not found" });
            }

            return Ok(analysis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving analysis with ID: {AnalysisId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the analysis" });
        }
    }

    /// <summary>
    /// Creates a new inspection analysis
    /// </summary>
    /// <param name="createAnalysisDto">Analysis creation data</param>
    /// <returns>Created analysis</returns>
    /// <response code="201">Analysis created successfully</response>
    /// <response code="400">Invalid analysis data</response>
    /// <response code="401">Unauthorized - can only add analyses to own inspections</response>
    [HttpPost]
    [ProducesResponseType(typeof(InspectionAnalysisDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<InspectionAnalysisDto>> CreateAnalysis([FromBody] CreateInspectionAnalysisDto createAnalysisDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var analysis = await _analysisService.CreateAnalysisAsync(createAnalysisDto, userId, isAdmin);
            
            _logger.LogInformation("Analysis created successfully: {AnalysisId} for inspection {InspectionId} by user {UserId}", 
                analysis.Id, createAnalysisDto.InspectionId, userId);
            return CreatedAtAction(nameof(GetAnalysis), new { id = analysis.Id }, analysis);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Analysis creation failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Analysis creation failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating analysis");
            return StatusCode(500, new { message = "An error occurred while creating the analysis" });
        }
    }

    /// <summary>
    /// Updates an existing inspection analysis
    /// </summary>
    /// <param name="id">Analysis ID</param>
    /// <param name="updateAnalysisDto">Analysis update data</param>
    /// <returns>Updated analysis</returns>
    /// <response code="200">Analysis updated successfully</response>
    /// <response code="400">Invalid analysis data</response>
    /// <response code="401">Unauthorized - can only update analyses for own inspections</response>
    /// <response code="404">Analysis not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(InspectionAnalysisDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionAnalysisDto>> UpdateAnalysis(int id, [FromBody] CreateInspectionAnalysisDto updateAnalysisDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var analysis = await _analysisService.UpdateAnalysisAsync(id, updateAnalysisDto, userId, isAdmin);
            if (analysis == null)
            {
                return NotFound(new { message = "Analysis not found" });
            }

            _logger.LogInformation("Analysis updated successfully: {AnalysisId} by user {UserId}", id, userId);
            return Ok(analysis);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Analysis update failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating analysis with ID: {AnalysisId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the analysis" });
        }
    }

    /// <summary>
    /// Deletes an inspection analysis
    /// </summary>
    /// <param name="id">Analysis ID</param>
    /// <returns>No content if deleted successfully</returns>
    /// <response code="204">Analysis deleted successfully</response>
    /// <response code="401">Unauthorized - can only delete analyses for own inspections</response>
    /// <response code="404">Analysis not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAnalysis(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var deleted = await _analysisService.DeleteAnalysisAsync(id, userId, isAdmin);
            if (!deleted)
            {
                return NotFound(new { message = "Analysis not found" });
            }

            _logger.LogInformation("Analysis deleted successfully: {AnalysisId} by user {UserId}", id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Analysis deletion failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting analysis with ID: {AnalysisId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the analysis" });
        }
    }

    /// <summary>
    /// Gets the current user ID from the JWT token
    /// </summary>
    /// <returns>User ID</returns>
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }

    /// <summary>
    /// Checks if the current user is an admin
    /// </summary>
    /// <returns>True if admin, false otherwise</returns>
    private bool IsCurrentUserAdmin()
    {
        return User.IsInRole(UserRole.Admin.ToString());
    }
}
