using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgroScan.API.Services;
using AgroScan.Core.DTOs;
using AgroScan.Core.Enums;
using System.Security.Claims;

namespace AgroScan.API.Controllers;

/// <summary>
/// Controller for inspection management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class InspectionsController : ControllerBase
{
    private readonly IInspectionService _inspectionService;
    private readonly ILogger<InspectionsController> _logger;

    /// <summary>
    /// Initializes a new instance of the InspectionsController
    /// </summary>
    /// <param name="inspectionService">Inspection service</param>
    /// <param name="logger">Logger</param>
    public InspectionsController(IInspectionService inspectionService, ILogger<InspectionsController> logger)
    {
        _inspectionService = inspectionService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all inspections for the current user or all inspections if admin
    /// </summary>
    /// <returns>Collection of inspections</returns>
    /// <response code="200">Inspections retrieved successfully</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InspectionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InspectionDto>>> GetInspections()
    {
        try
        {
            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            IEnumerable<InspectionDto> inspections;
            if (isAdmin)
            {
                inspections = await _inspectionService.GetAllInspectionsAsync();
            }
            else
            {
                inspections = await _inspectionService.GetUserInspectionsAsync(userId);
            }

            return Ok(inspections);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving inspections");
            return StatusCode(500, new { message = "An error occurred while retrieving inspections" });
        }
    }

    /// <summary>
    /// Gets an inspection by ID
    /// </summary>
    /// <param name="id">Inspection ID</param>
    /// <returns>Inspection if found</returns>
    /// <response code="200">Inspection retrieved successfully</response>
    /// <response code="404">Inspection not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InspectionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionDto>> GetInspection(int id)
    {
        try
        {
            var inspection = await _inspectionService.GetInspectionByIdAsync(id);
            if (inspection == null)
            {
                return NotFound(new { message = "Inspection not found" });
            }

            return Ok(inspection);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving inspection with ID: {InspectionId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the inspection" });
        }
    }

    /// <summary>
    /// Creates a new inspection
    /// </summary>
    /// <param name="createInspectionDto">Inspection creation data</param>
    /// <returns>Created inspection</returns>
    /// <response code="201">Inspection created successfully</response>
    /// <response code="400">Invalid inspection data</response>
    [HttpPost]
    [ProducesResponseType(typeof(InspectionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InspectionDto>> CreateInspection([FromBody] CreateInspectionDto createInspectionDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var inspection = await _inspectionService.CreateInspectionAsync(createInspectionDto, userId);
            
            _logger.LogInformation("Inspection created successfully: {InspectionId} by user {UserId}", inspection.Id, userId);
            return CreatedAtAction(nameof(GetInspection), new { id = inspection.Id }, inspection);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inspection");
            return StatusCode(500, new { message = "An error occurred while creating the inspection" });
        }
    }

    /// <summary>
    /// Updates an existing inspection
    /// </summary>
    /// <param name="id">Inspection ID</param>
    /// <param name="updateInspectionDto">Inspection update data</param>
    /// <returns>Updated inspection</returns>
    /// <response code="200">Inspection updated successfully</response>
    /// <response code="400">Invalid inspection data</response>
    /// <response code="401">Unauthorized - can only update own inspections</response>
    /// <response code="404">Inspection not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(InspectionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionDto>> UpdateInspection(int id, [FromBody] UpdateInspectionDto updateInspectionDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var inspection = await _inspectionService.UpdateInspectionAsync(id, updateInspectionDto, userId, isAdmin);
            if (inspection == null)
            {
                return NotFound(new { message = "Inspection not found" });
            }

            _logger.LogInformation("Inspection updated successfully: {InspectionId} by user {UserId}", id, userId);
            return Ok(inspection);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Inspection update failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inspection with ID: {InspectionId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the inspection" });
        }
    }

    /// <summary>
    /// Deletes an inspection
    /// </summary>
    /// <param name="id">Inspection ID</param>
    /// <returns>No content if deleted successfully</returns>
    /// <response code="204">Inspection deleted successfully</response>
    /// <response code="401">Unauthorized - can only delete own inspections</response>
    /// <response code="404">Inspection not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInspection(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var deleted = await _inspectionService.DeleteInspectionAsync(id, userId, isAdmin);
            if (!deleted)
            {
                return NotFound(new { message = "Inspection not found" });
            }

            _logger.LogInformation("Inspection deleted successfully: {InspectionId} by user {UserId}", id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Inspection deletion failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting inspection with ID: {InspectionId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the inspection" });
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
