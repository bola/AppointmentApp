namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Represents a customer who books appointments
/// </summary>
public class Customer : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public bool EmailVerified { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<CustomerFormData> CustomFormData { get; set; } = new List<CustomerFormData>();
}
