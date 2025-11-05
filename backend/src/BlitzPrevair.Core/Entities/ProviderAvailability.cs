namespace BlitzPrevair.Core.Entities;

/// <summary>
/// Defines when a service provider is available
/// </summary>
public class ProviderAvailability : BaseEntity
{
    public Guid ServiceProviderId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Optional: for specific date overrides (e.g., holidays, time off)
    /// </summary>
    public DateTime? SpecificDate { get; set; }

    // Navigation properties
    public ServiceProvider ServiceProvider { get; set; } = null!;
}
