namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Represents a service offered by a tenant
/// </summary>
public class Service : BaseEntity
{
    public Guid TenantId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public string? ImageUrl { get; set; }

    /// <summary>
    /// If true, users can select specific provider
    /// If false, users can only specify gender preference
    /// </summary>
    public bool AllowProviderSelection { get; set; } = true;

    /// <summary>
    /// If true, users can specify gender preference (for services like massage, physio)
    /// </summary>
    public bool AllowGenderPreference { get; set; } = false;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ServiceCategory Category { get; set; } = null!;
    public ICollection<ServiceProviderService> ServiceProviderServices { get; set; } = new List<ServiceProviderService>();
    public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
}
