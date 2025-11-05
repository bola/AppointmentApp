using BlitzPrevair.API.DTOs;
using BlitzPrevair.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlitzPrevair.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TenantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all active tenants
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenants()
    {
        var tenants = await _context.Tenants
            .Where(t => t.IsActive && !t.IsDeleted)
            .Select(t => new TenantDto
            {
                Id = t.Id,
                Name = t.Name,
                Subdomain = t.Subdomain,
                Logo = t.Logo,
                PrimaryColor = t.PrimaryColor,
                SecondaryColor = t.SecondaryColor,
                ContactEmail = t.ContactEmail,
                ContactPhone = t.ContactPhone,
                IsActive = t.IsActive
            })
            .ToListAsync();

        return Ok(tenants);
    }

    /// <summary>
    /// Get tenant by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetTenant(Guid id)
    {
        var tenant = await _context.Tenants
            .Where(t => t.Id == id && !t.IsDeleted)
            .Select(t => new TenantDto
            {
                Id = t.Id,
                Name = t.Name,
                Subdomain = t.Subdomain,
                Logo = t.Logo,
                PrimaryColor = t.PrimaryColor,
                SecondaryColor = t.SecondaryColor,
                ContactEmail = t.ContactEmail,
                ContactPhone = t.ContactPhone,
                IsActive = t.IsActive
            })
            .FirstOrDefaultAsync();

        if (tenant == null)
            return NotFound(new { message = "Tenant not found" });

        return Ok(tenant);
    }

    /// <summary>
    /// Get tenant by subdomain
    /// </summary>
    [HttpGet("subdomain/{subdomain}")]
    public async Task<ActionResult<TenantDto>> GetTenantBySubdomain(string subdomain)
    {
        var tenant = await _context.Tenants
            .Where(t => t.Subdomain.ToLower() == subdomain.ToLower() && !t.IsDeleted)
            .Select(t => new TenantDto
            {
                Id = t.Id,
                Name = t.Name,
                Subdomain = t.Subdomain,
                Logo = t.Logo,
                PrimaryColor = t.PrimaryColor,
                SecondaryColor = t.SecondaryColor,
                ContactEmail = t.ContactEmail,
                ContactPhone = t.ContactPhone,
                IsActive = t.IsActive
            })
            .FirstOrDefaultAsync();

        if (tenant == null)
            return NotFound(new { message = $"Tenant with subdomain '{subdomain}' not found" });

        return Ok(tenant);
    }

    /// <summary>
    /// Get service categories for a tenant
    /// </summary>
    [HttpGet("{tenantId}/categories")]
    public async Task<ActionResult<IEnumerable<ServiceCategoryDto>>> GetTenantCategories(Guid tenantId)
    {
        var tenant = await _context.Tenants.FindAsync(tenantId);
        if (tenant == null || tenant.IsDeleted)
            return NotFound(new { message = "Tenant not found" });

        var categories = await _context.ServiceCategories
            .Where(c => c.TenantId == tenantId && !c.IsDeleted)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new ServiceCategoryDto
            {
                Id = c.Id,
                TenantId = c.TenantId,
                Name = c.Name,
                Description = c.Description,
                Icon = c.Icon,
                DisplayOrder = c.DisplayOrder
            })
            .ToListAsync();

        return Ok(categories);
    }
}
