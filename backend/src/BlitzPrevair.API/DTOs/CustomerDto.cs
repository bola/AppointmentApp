using BlitzPrevair.Core.Entities;

namespace BlitzPrevair.API.DTOs;

public class CustomerDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public bool EmailVerified { get; set; }
}
