namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Categories for services (e.g., Massage, Physiotherapy, Facial)
/// </summary>
public class ServiceCategory : BaseEntity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<Service> Services { get; set; } = new List<Service>();
}
