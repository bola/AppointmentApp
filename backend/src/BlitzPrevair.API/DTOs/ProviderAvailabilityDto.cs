namespace BlitzPrevair.API.DTOs;

public class ProviderAvailabilityDto
{
    public Guid Id { get; set; }
    public Guid ServiceProviderId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime? SpecificDate { get; set; }
}

public class TimeSlotDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
    public Guid? ProviderId { get; set; }
}
