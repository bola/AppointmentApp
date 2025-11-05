using BlitzPrevair.API.DTOs;
using BlitzPrevair.Core.Entities;
using BlitzPrevair.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlitzPrevair.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create a new appointment (requires authentication)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment([FromBody] CreateAppointmentRequest request)
    {
        // Validate model
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Get customer ID from JWT
        var customerIdClaim = User.FindFirst("customerId")?.Value;
        if (customerIdClaim == null || !Guid.TryParse(customerIdClaim, out var customerId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        // Verify tenant exists
        var tenant = await _context.Tenants.FindAsync(request.TenantId);
        if (tenant == null || !tenant.IsActive || tenant.IsDeleted)
        {
            return BadRequest(new { message = "Tenant not found or inactive" });
        }

        // Verify consent
        if (!request.ConsentGiven)
        {
            return BadRequest(new { message = "Consent is required to book an appointment" });
        }

        // Validate and calculate appointment details
        var appointmentServices = new List<AppointmentService>();
        decimal totalPrice = 0;
        int totalDuration = 0;
        var currentDateTime = request.ScheduledDate.Date.Add(request.ScheduledTime);

        foreach (var serviceRequest in request.Services)
        {
            // Get service details
            var service = await _context.Services
                .Where(s => s.Id == serviceRequest.ServiceId && s.TenantId == request.TenantId && s.IsActive)
                .FirstOrDefaultAsync();

            if (service == null)
            {
                return BadRequest(new { message = $"Service {serviceRequest.ServiceId} not found or inactive" });
            }

            // Verify provider if specified
            ServiceProvider? provider = null;
            if (serviceRequest.ServiceProviderId.HasValue)
            {
                provider = await _context.ServiceProviders
                    .Include(sp => sp.ServiceProviderServices)
                    .Where(sp => sp.Id == serviceRequest.ServiceProviderId.Value &&
                                sp.TenantId == request.TenantId &&
                                sp.IsActive)
                    .FirstOrDefaultAsync();

                if (provider == null)
                {
                    return BadRequest(new { message = $"Provider {serviceRequest.ServiceProviderId} not found or inactive" });
                }

                // Verify provider offers this service
                if (!provider.ServiceProviderServices.Any(sps => sps.ServiceId == service.Id))
                {
                    return BadRequest(new { message = $"Provider does not offer this service" });
                }

                // Check provider availability
                var isAvailable = await IsProviderAvailable(
                    provider.Id,
                    currentDateTime,
                    service.DurationMinutes);

                if (!isAvailable)
                {
                    return BadRequest(new { message = $"Provider is not available at this time" });
                }
            }

            // Create appointment service
            var endDateTime = currentDateTime.AddMinutes(service.DurationMinutes);
            appointmentServices.Add(new AppointmentService
            {
                Id = Guid.NewGuid(),
                ServiceId = service.Id,
                ServiceProviderId = provider?.Id,
                PreferredGender = serviceRequest.PreferredGender,
                StartTime = currentDateTime,
                EndTime = endDateTime,
                Price = service.Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            totalPrice += service.Price;
            totalDuration += service.DurationMinutes;
            currentDateTime = endDateTime; // Next service starts when previous ends
        }

        // Create appointment
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            CustomerId = customerId,
            ScheduledDate = request.ScheduledDate.Date,
            ScheduledTime = request.ScheduledTime,
            Status = AppointmentStatus.Pending,
            TotalPrice = totalPrice,
            TotalDurationMinutes = totalDuration,
            Notes = request.Notes,
            ConsentGiven = request.ConsentGiven,
            ConsentGivenAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Appointments.Add(appointment);

        // Add appointment services
        foreach (var appointmentService in appointmentServices)
        {
            appointmentService.AppointmentId = appointment.Id;
            _context.AppointmentServices.Add(appointmentService);
        }

        await _context.SaveChangesAsync();

        // Load related data for response
        await _context.Entry(appointment)
            .Collection(a => a.AppointmentServices)
            .Query()
            .Include(aps => aps.Service)
                .ThenInclude(s => s.Category)
            .Include(aps => aps.ServiceProvider)
            .LoadAsync();

        var appointmentDto = MapToAppointmentDto(appointment);

        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointmentDto);
    }

    /// <summary>
    /// Get appointment by ID (customer can only view their own appointments)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(Guid id)
    {
        // Get customer ID from JWT
        var customerIdClaim = User.FindFirst("customerId")?.Value;
        if (customerIdClaim == null || !Guid.TryParse(customerIdClaim, out var customerId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var appointment = await _context.Appointments
            .Include(a => a.AppointmentServices)
                .ThenInclude(aps => aps.Service)
                    .ThenInclude(s => s.Category)
            .Include(a => a.AppointmentServices)
                .ThenInclude(aps => aps.ServiceProvider)
            .Where(a => a.Id == id && a.CustomerId == customerId && !a.IsDeleted)
            .FirstOrDefaultAsync();

        if (appointment == null)
        {
            return NotFound(new { message = "Appointment not found" });
        }

        var appointmentDto = MapToAppointmentDto(appointment);

        return Ok(appointmentDto);
    }

    /// <summary>
    /// Get all appointments for the current customer
    /// </summary>
    [HttpGet("my-appointments")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetMyAppointments(
        [FromQuery] AppointmentStatus? status = null)
    {
        // Get customer ID from JWT
        var customerIdClaim = User.FindFirst("customerId")?.Value;
        if (customerIdClaim == null || !Guid.TryParse(customerIdClaim, out var customerId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var query = _context.Appointments
            .Include(a => a.AppointmentServices)
                .ThenInclude(aps => aps.Service)
                    .ThenInclude(s => s.Category)
            .Include(a => a.AppointmentServices)
                .ThenInclude(aps => aps.ServiceProvider)
            .Where(a => a.CustomerId == customerId && !a.IsDeleted);

        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }

        var appointments = await query
            .OrderByDescending(a => a.ScheduledDate)
            .ThenByDescending(a => a.ScheduledTime)
            .ToListAsync();

        var appointmentDtos = appointments.Select(MapToAppointmentDto).ToList();

        return Ok(appointmentDtos);
    }

    /// <summary>
    /// Cancel an appointment
    /// </summary>
    [HttpPatch("{id}/cancel")]
    public async Task<ActionResult<AppointmentDto>> CancelAppointment(Guid id)
    {
        // Get customer ID from JWT
        var customerIdClaim = User.FindFirst("customerId")?.Value;
        if (customerIdClaim == null || !Guid.TryParse(customerIdClaim, out var customerId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var appointment = await _context.Appointments
            .Where(a => a.Id == id && a.CustomerId == customerId && !a.IsDeleted)
            .FirstOrDefaultAsync();

        if (appointment == null)
        {
            return NotFound(new { message = "Appointment not found" });
        }

        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            return BadRequest(new { message = "Appointment is already cancelled" });
        }

        if (appointment.Status == AppointmentStatus.Completed)
        {
            return BadRequest(new { message = "Cannot cancel a completed appointment" });
        }

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Reload with related data
        await _context.Entry(appointment)
            .Collection(a => a.AppointmentServices)
            .Query()
            .Include(aps => aps.Service)
                .ThenInclude(s => s.Category)
            .Include(aps => aps.ServiceProvider)
            .LoadAsync();

        var appointmentDto = MapToAppointmentDto(appointment);

        return Ok(appointmentDto);
    }

    private async Task<bool> IsProviderAvailable(Guid providerId, DateTime startTime, int durationMinutes)
    {
        var endTime = startTime.AddMinutes(durationMinutes);

        // Check existing appointments
        var hasConflict = await _context.AppointmentServices
            .Include(aps => aps.Appointment)
            .AnyAsync(aps =>
                aps.ServiceProviderId == providerId &&
                aps.Appointment.Status != AppointmentStatus.Cancelled &&
                ((startTime >= aps.StartTime && startTime < aps.EndTime) ||
                 (endTime > aps.StartTime && endTime <= aps.EndTime) ||
                 (startTime <= aps.StartTime && endTime >= aps.EndTime)));

        return !hasConflict;
    }

    private static AppointmentDto MapToAppointmentDto(Appointment appointment)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            TenantId = appointment.TenantId,
            CustomerId = appointment.CustomerId,
            ScheduledDate = appointment.ScheduledDate,
            ScheduledTime = appointment.ScheduledTime,
            Status = appointment.Status,
            TotalPrice = appointment.TotalPrice,
            TotalDurationMinutes = appointment.TotalDurationMinutes,
            Notes = appointment.Notes,
            ConsentGiven = appointment.ConsentGiven,
            Services = appointment.AppointmentServices.Select(aps => new AppointmentServiceDto
            {
                Id = aps.Id,
                ServiceId = aps.ServiceId,
                ServiceProviderId = aps.ServiceProviderId,
                PreferredGender = aps.PreferredGender,
                StartTime = aps.StartTime,
                EndTime = aps.EndTime,
                Price = aps.Price,
                Service = aps.Service == null ? null : new ServiceDto
                {
                    Id = aps.Service.Id,
                    TenantId = aps.Service.TenantId,
                    CategoryId = aps.Service.CategoryId,
                    Name = aps.Service.Name,
                    Description = aps.Service.Description,
                    Price = aps.Service.Price,
                    DurationMinutes = aps.Service.DurationMinutes,
                    ImageUrl = aps.Service.ImageUrl,
                    AllowProviderSelection = aps.Service.AllowProviderSelection,
                    AllowGenderPreference = aps.Service.AllowGenderPreference,
                    IsActive = aps.Service.IsActive,
                    Category = aps.Service.Category == null ? null : new ServiceCategoryDto
                    {
                        Id = aps.Service.Category.Id,
                        TenantId = aps.Service.Category.TenantId,
                        Name = aps.Service.Category.Name,
                        Description = aps.Service.Category.Description,
                        Icon = aps.Service.Category.Icon,
                        DisplayOrder = aps.Service.Category.DisplayOrder
                    }
                },
                ServiceProvider = aps.ServiceProvider == null ? null : new ServiceProviderDto
                {
                    Id = aps.ServiceProvider.Id,
                    TenantId = aps.ServiceProvider.TenantId,
                    FirstName = aps.ServiceProvider.FirstName,
                    LastName = aps.ServiceProvider.LastName,
                    Email = aps.ServiceProvider.Email,
                    Phone = aps.ServiceProvider.Phone,
                    Bio = aps.ServiceProvider.Bio,
                    PhotoUrl = aps.ServiceProvider.PhotoUrl,
                    Gender = aps.ServiceProvider.Gender,
                    IsActive = aps.ServiceProvider.IsActive
                }
            }).ToList()
        };
    }
}
