using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgroScan.API.Services;
using AgroScan.Core.DTOs;
using AgroScan.Core.Enums;
using System.Security.Claims;
using AgroScan.Core.Interfaces;

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
    private readonly IInspectionImageRepository _imageRepository;
    private readonly ILogger<InspectionsController> _logger;

    /// <summary>
    /// Initializes a new instance of the InspectionsController
    /// </summary>
    /// <param name="inspectionService">Inspection service</param>
    /// <param name="logger">Logger</param>
    public InspectionsController(IInspectionService inspectionService, IInspectionImageRepository imageRepository, ILogger<InspectionsController> logger)
    {
        _inspectionService = inspectionService;
        _imageRepository = imageRepository;
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
    /// Creates a new inspection from JSON body (no file upload)
    /// </summary>
    /// <param name="createInspectionDto">Inspection creation data (JSON)</param>
    /// <returns>Created inspection</returns>
    /// <response code="201">Inspection created successfully</response>
    /// <response code="400">Invalid inspection data</response>
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
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

            _logger.LogInformation("Inspection created successfully (JSON): {InspectionId} by user {UserId}", inspection.Id, userId);
            return CreatedAtAction(nameof(GetInspection), new { id = inspection.Id }, inspection);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inspection");
            return StatusCode(500, new { message = "An error occurred while creating the inspection" });
        }
    }

    /// <summary>
    /// Creates a new inspection with optional image uploads (multipart/form-data)
    /// </summary>
    /// <param name="createInspectionDto">Inspection creation data (form fields)</param>
    /// <param name="images">Image files to upload</param>
    /// <returns>Created inspection with image URLs</returns>
    /// <response code="201">Inspection created successfully</response>
    /// <response code="400">Invalid inspection data</response>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(InspectionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InspectionDto>> CreateInspectionWithImages([FromForm] CreateInspectionDto createInspectionDto, [FromForm] List<IFormFile>? images)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var inspection = await _inspectionService.CreateInspectionAsync(createInspectionDto, userId);

            if (images != null && images.Count > 0)
            {
                var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsRoot);

                foreach (var file in images.Where(f => f != null && f.Length > 0))
                {
                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                    var filePath = Path.Combine(uploadsRoot, fileName);
                    await using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var imageUrl = $"/uploads/{fileName}";

                    await _imageRepository.AddAsync(new AgroScan.Core.Entities.InspectionImage
                    {
                        InspectionId = inspection.Id,
                        Image = imageUrl,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
                await _imageRepository.SaveChangesAsync();
            }

            var created = await _inspectionService.GetInspectionByIdAsync(inspection.Id) ?? inspection;

            _logger.LogInformation("Inspection created successfully (FORM): {InspectionId} by user {UserId}", created.Id, userId);
            return CreatedAtAction(nameof(GetInspection), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inspection with images");
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
