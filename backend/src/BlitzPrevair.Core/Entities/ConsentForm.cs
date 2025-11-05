namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Tenant-specific consent forms and terms
/// </summary>
public class ConsentForm : BaseEntity
{
    public Guid TenantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Can contain HTML/Markdown
    public string Version { get; set; } = "1.0";
    public bool IsRequired { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<CustomerConsent> CustomerConsents { get; set; } = new List<CustomerConsent>();
}
