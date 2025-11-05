namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Represents a service within an appointment (supports multiple services per appointment)
/// </summary>
public class AppointmentService : BaseEntity
{
    public Guid AppointmentId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid? ServiceProviderId { get; set; }

    /// <summary>
    /// If no specific provider selected, store gender preference
    /// </summary>
    public Gender? PreferredGender { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal Price { get; set; }

    // Navigation properties
    public Appointment Appointment { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public ServiceProvider? ServiceProvider { get; set; }
}
