namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Represents a company/organization using the platform
/// </summary>
public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    public string? ContactPhone { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<ServiceProvider> ServiceProviders { get; set; } = new List<ServiceProvider>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<CustomFormField> CustomFormFields { get; set; } = new List<CustomFormField>();
    public ICollection<ConsentForm> ConsentForms { get; set; } = new List<ConsentForm>();
}
