using BlitzPrevair.Core.Entities;

namespace BlitzPrevair.API.DTOs;

public class ServiceProviderDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public Gender Gender { get; set; }
    public bool IsActive { get; set; }
    public List<Guid>? ServiceIds { get; set; }
}
