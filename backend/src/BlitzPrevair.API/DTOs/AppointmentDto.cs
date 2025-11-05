using BlitzPrevair.Core.Entities;

namespace BlitzPrevair.API.DTOs;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public TimeSpan ScheduledTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public int TotalDurationMinutes { get; set; }
    public string? Notes { get; set; }
    public bool ConsentGiven { get; set; }
    public List<AppointmentServiceDto> Services { get; set; } = new();
}

public class AppointmentServiceDto
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public Guid? ServiceProviderId { get; set; }
    public Gender? PreferredGender { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal Price { get; set; }
    public ServiceDto? Service { get; set; }
    public ServiceProviderDto? ServiceProvider { get; set; }
}
