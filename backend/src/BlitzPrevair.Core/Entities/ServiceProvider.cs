namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Represents a person who provides services
/// </summary>
public class ServiceProvider : BaseEntity
{
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public Gender Gender { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<ServiceProviderService> ServiceProviderServices { get; set; } = new List<ServiceProviderService>();
    public ICollection<ProviderAvailability> Availabilities { get; set; } = new List<ProviderAvailability>();
    public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
}

public enum Gender
{
    Male,
    Female,
    Other,
    PreferNotToSay
}
