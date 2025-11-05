namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Tenant-specific custom form fields
/// </summary>
public class CustomFormField : BaseEntity
{
    public Guid TenantId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public FormFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public string? Placeholder { get; set; }
    public string? ValidationRules { get; set; }
    public string? Options { get; set; } // JSON for dropdown/radio options
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
}

public enum FormFieldType
{
    Text,
    Email,
    Phone,
    Number,
    Date,
    TextArea,
    Dropdown,
    Radio,
    Checkbox,
    File
}
