using BlitzPrevair.API.DTOs;
using BlitzPrevair.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlitzPrevair.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId}/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServicesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all services for a tenant
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices(
        Guid tenantId,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] bool? isActive = null)
    {
        var tenant = await _context.Tenants.FindAsync(tenantId);
        if (tenant == null || tenant.IsDeleted)
            return NotFound(new { message = "Tenant not found" });

        var query = _context.Services
            .Include(s => s.Category)
            .Where(s => s.TenantId == tenantId && !s.IsDeleted);

        if (categoryId.HasValue)
            query = query.Where(s => s.CategoryId == categoryId.Value);

        if (isActive.HasValue)
            query = query.Where(s => s.IsActive == isActive.Value);

        var services = await query
            .Select(s => new ServiceDto
            {
                Id = s.Id,
                TenantId = s.TenantId,
                CategoryId = s.CategoryId,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                DurationMinutes = s.DurationMinutes,
                ImageUrl = s.ImageUrl,
                AllowProviderSelection = s.AllowProviderSelection,
                AllowGenderPreference = s.AllowGenderPreference,
                IsActive = s.IsActive,
                Category = new ServiceCategoryDto
                {
                    Id = s.Category.Id,
                    TenantId = s.Category.TenantId,
                    Name = s.Category.Name,
                    Description = s.Category.Description,
                    Icon = s.Category.Icon,
                    DisplayOrder = s.Category.DisplayOrder
                }
            })
            .ToListAsync();

        return Ok(services);
    }

    /// <summary>
    /// Get service by ID with available providers
    /// </summary>
    [HttpGet("{serviceId}")]
    public async Task<ActionResult<ServiceDto>> GetService(Guid tenantId, Guid serviceId)
    {
        var service = await _context.Services
            .Include(s => s.Category)
            .Include(s => s.ServiceProviderServices)
                .ThenInclude(sps => sps.ServiceProvider)
            .Where(s => s.Id == serviceId && s.TenantId == tenantId && !s.IsDeleted)
            .FirstOrDefaultAsync();

        if (service == null)
            return NotFound(new { message = "Service not found" });

        var serviceDto = new ServiceDto
        {
            Id = service.Id,
            TenantId = service.TenantId,
            CategoryId = service.CategoryId,
            Name = service.Name,
            Description = service.Description,
            Price = service.Price,
            DurationMinutes = service.DurationMinutes,
            ImageUrl = service.ImageUrl,
            AllowProviderSelection = service.AllowProviderSelection,
            AllowGenderPreference = service.AllowGenderPreference,
            IsActive = service.IsActive,
            Category = new ServiceCategoryDto
            {
                Id = service.Category.Id,
                TenantId = service.Category.TenantId,
                Name = service.Category.Name,
                Description = service.Category.Description,
                Icon = service.Category.Icon,
                DisplayOrder = service.Category.DisplayOrder
            }
        };

        return Ok(serviceDto);
    }
}
