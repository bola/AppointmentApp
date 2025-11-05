using BlitzPrevair.API.DTOs;
using BlitzPrevair.Core.Entities;
using BlitzPrevair.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlitzPrevair.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId}/[controller]")]
public class ProvidersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProvidersController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all providers for a tenant
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceProviderDto>>> GetProviders(
        Guid tenantId,
        [FromQuery] Guid? serviceId = null,
        [FromQuery] Gender? gender = null)
    {
        var tenant = await _context.Tenants.FindAsync(tenantId);
        if (tenant == null || tenant.IsDeleted)
            return NotFound(new { message = "Tenant not found" });

        var query = _context.ServiceProviders
            .Include(sp => sp.ServiceProviderServices)
            .Where(sp => sp.TenantId == tenantId && sp.IsActive && !sp.IsDeleted);

        if (serviceId.HasValue)
        {
            query = query.Where(sp => sp.ServiceProviderServices
                .Any(sps => sps.ServiceId == serviceId.Value));
        }

        if (gender.HasValue)
        {
            query = query.Where(sp => sp.Gender == gender.Value);
        }

        var providers = await query
            .Select(sp => new ServiceProviderDto
            {
                Id = sp.Id,
                TenantId = sp.TenantId,
                FirstName = sp.FirstName,
                LastName = sp.LastName,
                Email = sp.Email,
                Phone = sp.Phone,
                Bio = sp.Bio,
                PhotoUrl = sp.PhotoUrl,
                Gender = sp.Gender,
                IsActive = sp.IsActive,
                ServiceIds = sp.ServiceProviderServices.Select(sps => sps.ServiceId).ToList()
            })
            .ToListAsync();

        return Ok(providers);
    }

    /// <summary>
    /// Get provider by ID
    /// </summary>
    [HttpGet("{providerId}")]
    public async Task<ActionResult<ServiceProviderDto>> GetProvider(Guid tenantId, Guid providerId)
    {
        var provider = await _context.ServiceProviders
            .Include(sp => sp.ServiceProviderServices)
            .Where(sp => sp.Id == providerId && sp.TenantId == tenantId && !sp.IsDeleted)
            .FirstOrDefaultAsync();

        if (provider == null)
            return NotFound(new { message = "Provider not found" });

        var providerDto = new ServiceProviderDto
        {
            Id = provider.Id,
            TenantId = provider.TenantId,
            FirstName = provider.FirstName,
            LastName = provider.LastName,
            Email = provider.Email,
            Phone = provider.Phone,
            Bio = provider.Bio,
            PhotoUrl = provider.PhotoUrl,
            Gender = provider.Gender,
            IsActive = provider.IsActive,
            ServiceIds = provider.ServiceProviderServices.Select(sps => sps.ServiceId).ToList()
        };

        return Ok(providerDto);
    }

    /// <summary>
    /// Get provider's availability schedule
    /// </summary>
    [HttpGet("{providerId}/availability")]
    public async Task<ActionResult<IEnumerable<ProviderAvailabilityDto>>> GetProviderAvailability(
        Guid tenantId,
        Guid providerId)
    {
        var provider = await _context.ServiceProviders
            .Where(sp => sp.Id == providerId && sp.TenantId == tenantId && !sp.IsDeleted)
            .FirstOrDefaultAsync();

        if (provider == null)
            return NotFound(new { message = "Provider not found" });

        var availabilities = await _context.ProviderAvailabilities
            .Where(pa => pa.ServiceProviderId == providerId && !pa.IsDeleted)
            .OrderBy(pa => pa.DayOfWeek)
            .ThenBy(pa => pa.StartTime)
            .Select(pa => new ProviderAvailabilityDto
            {
                Id = pa.Id,
                ServiceProviderId = pa.ServiceProviderId,
                DayOfWeek = pa.DayOfWeek,
                StartTime = pa.StartTime,
                EndTime = pa.EndTime,
                IsAvailable = pa.IsAvailable,
                SpecificDate = pa.SpecificDate
            })
            .ToListAsync();

        return Ok(availabilities);
    }

    /// <summary>
    /// Get available time slots for a provider on a specific date
    /// </summary>
    [HttpGet("{providerId}/timeslots")]
    public async Task<ActionResult<IEnumerable<TimeSlotDto>>> GetAvailableTimeSlots(
        Guid tenantId,
        Guid providerId,
        [FromQuery] DateTime date,
        [FromQuery] int durationMinutes = 60)
    {
        var provider = await _context.ServiceProviders
            .Where(sp => sp.Id == providerId && sp.TenantId == tenantId && !sp.IsDeleted)
            .FirstOrDefaultAsync();

        if (provider == null)
            return NotFound(new { message = "Provider not found" });

        var dayOfWeek = date.DayOfWeek;

        // Get provider's availability for this day
        var availability = await _context.ProviderAvailabilities
            .Where(pa => pa.ServiceProviderId == providerId &&
                         pa.DayOfWeek == dayOfWeek &&
                         pa.IsAvailable &&
                         !pa.IsDeleted &&
                         (!pa.SpecificDate.HasValue || pa.SpecificDate.Value.Date == date.Date))
            .FirstOrDefaultAsync();

        if (availability == null)
            return Ok(new List<TimeSlotDto>()); // No availability for this day

        // Get existing appointments for this provider on this date
        var existingAppointments = await _context.AppointmentServices
            .Include(aps => aps.Appointment)
            .Where(aps => aps.ServiceProviderId == providerId &&
                          aps.StartTime.Date == date.Date &&
                          aps.Appointment.Status != AppointmentStatus.Cancelled)
            .Select(aps => new { aps.StartTime, aps.EndTime })
            .ToListAsync();

        // Generate time slots
        var timeSlots = new List<TimeSlotDto>();
        var currentTime = date.Date.Add(availability.StartTime);
        var endTime = date.Date.Add(availability.EndTime);

        while (currentTime.Add(TimeSpan.FromMinutes(durationMinutes)) <= endTime)
        {
            var slotEnd = currentTime.Add(TimeSpan.FromMinutes(durationMinutes));

            // Check if this slot conflicts with existing appointments
            var isAvailable = !existingAppointments.Any(apt =>
                (currentTime >= apt.StartTime && currentTime < apt.EndTime) ||
                (slotEnd > apt.StartTime && slotEnd <= apt.EndTime) ||
                (currentTime <= apt.StartTime && slotEnd >= apt.EndTime));

            timeSlots.Add(new TimeSlotDto
            {
                StartTime = currentTime,
                EndTime = slotEnd,
                IsAvailable = isAvailable,
                ProviderId = providerId
            });

            currentTime = currentTime.Add(TimeSpan.FromMinutes(30)); // 30-minute intervals
        }

        return Ok(timeSlots);
    }
}
