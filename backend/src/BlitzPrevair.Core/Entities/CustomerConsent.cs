namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Tracks customer consent to forms
/// </summary>
public class CustomerConsent : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid ConsentFormId { get; set; }
    public bool Accepted { get; set; }
    public DateTime AcceptedAt { get; set; }
    public string ConsentFormVersion { get; set; } = string.Empty;
    public string? IpAddress { get; set; }

    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public ConsentForm ConsentForm { get; set; } = null!;
}
