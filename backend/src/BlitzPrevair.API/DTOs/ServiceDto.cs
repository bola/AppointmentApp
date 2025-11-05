namespace BlitzPrevair.API.DTOs;

public class ServiceDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public string? ImageUrl { get; set; }
    public bool AllowProviderSelection { get; set; }
    public bool AllowGenderPreference { get; set; }
    public bool IsActive { get; set; }
    public ServiceCategoryDto? Category { get; set; }
}
