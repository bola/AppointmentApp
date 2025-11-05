namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Stores custom form data submitted by customers
/// </summary>
public class CustomerFormData : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid CustomFormFieldId { get; set; }
    public string Value { get; set; } = string.Empty;

    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public CustomFormField CustomFormField { get; set; } = null!;
}
