using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgroScan.API.Services;
using AgroScan.Core.DTOs;
using AgroScan.Core.Enums;
using System.Security.Claims;

namespace AgroScan.API.Controllers;

/// <summary>
/// Controller for inspection image management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class InspectionImagesController : ControllerBase
{
    private readonly IInspectionImageService _imageService;
    private readonly ILogger<InspectionImagesController> _logger;

    /// <summary>
    /// Initializes a new instance of the InspectionImagesController
    /// </summary>
    /// <param name="imageService">Image service</param>
    /// <param name="logger">Logger</param>
    public InspectionImagesController(IInspectionImageService imageService, ILogger<InspectionImagesController> logger)
    {
        _imageService = imageService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all images for a specific inspection
    /// </summary>
    /// <param name="inspectionId">Inspection ID</param>
    /// <returns>Collection of images</returns>
    /// <response code="200">Images retrieved successfully</response>
    [HttpGet("inspection/{inspectionId}")]
    [ProducesResponseType(typeof(IEnumerable<InspectionImageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InspectionImageDto>>> GetImagesByInspectionId(int inspectionId)
    {
        try
        {
            var images = await _imageService.GetImagesByInspectionIdAsync(inspectionId);
            return Ok(images);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving images for inspection: {InspectionId}", inspectionId);
            return StatusCode(500, new { message = "An error occurred while retrieving images" });
        }
    }

    /// <summary>
    /// Gets an image by ID
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>Image if found</returns>
    /// <response code="200">Image retrieved successfully</response>
    /// <response code="404">Image not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InspectionImageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InspectionImageDto>> GetImage(int id)
    {
        try
        {
            var image = await _imageService.GetImageByIdAsync(id);
            if (image == null)
            {
                return NotFound(new { message = "Image not found" });
            }

            return Ok(image);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving image with ID: {ImageId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the image" });
        }
    }

    /// <summary>
    /// Creates a new inspection image
    /// </summary>
    /// <param name="createImageDto">Image creation data</param>
    /// <returns>Created image</returns>
    /// <response code="201">Image created successfully</response>
    /// <response code="400">Invalid image data</response>
    /// <response code="401">Unauthorized - can only add images to own inspections</response>
    [HttpPost]
    [ProducesResponseType(typeof(InspectionImageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<InspectionImageDto>> CreateImage([FromBody] CreateInspectionImageDto createImageDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var image = await _imageService.CreateImageAsync(createImageDto, userId, isAdmin);
            
            _logger.LogInformation("Image created successfully: {ImageId} for inspection {InspectionId} by user {UserId}", 
                image.Id, createImageDto.InspectionId, userId);
            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Image creation failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Image creation failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating image");
            return StatusCode(500, new { message = "An error occurred while creating the image" });
        }
    }

    /// <summary>
    /// Deletes an inspection image
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>No content if deleted successfully</returns>
    /// <response code="204">Image deleted successfully</response>
    /// <response code="401">Unauthorized - can only delete images from own inspections</response>
    /// <response code="404">Image not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteImage(int id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var isAdmin = IsCurrentUserAdmin();

            var deleted = await _imageService.DeleteImageAsync(id, userId, isAdmin);
            if (!deleted)
            {
                return NotFound(new { message = "Image not found" });
            }

            _logger.LogInformation("Image deleted successfully: {ImageId} by user {UserId}", id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Image deletion failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting image with ID: {ImageId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the image" });
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
