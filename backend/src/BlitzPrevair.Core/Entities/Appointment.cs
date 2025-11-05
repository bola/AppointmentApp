namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Represents a booking/appointment
/// </summary>
public class Appointment : BaseEntity
{
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public TimeSpan ScheduledTime { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public decimal TotalPrice { get; set; }
    public int TotalDurationMinutes { get; set; }
    public string? Notes { get; set; }
    public bool ConsentGiven { get; set; }
    public DateTime? ConsentGivenAt { get; set; }

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
}

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    InProgress,
    Completed,
    Cancelled,
    NoShow
}
