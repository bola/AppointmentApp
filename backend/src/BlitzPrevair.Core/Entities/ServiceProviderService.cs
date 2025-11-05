namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Many-to-many relationship between ServiceProviders and Services
/// </summary>
public class ServiceProviderService : BaseEntity
{
    public Guid ServiceProviderId { get; set; }
    public Guid ServiceId { get; set; }

    // Navigation properties
    public ServiceProvider ServiceProvider { get; set; } = null!;
    public Service Service { get; set; } = null!;
}
