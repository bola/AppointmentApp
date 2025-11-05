using System.ComponentModel.DataAnnotations;
using BlitzPrevair.Core.Entities;

namespace BlitzPrevair.API.DTOs;

public class CreateAppointmentRequest
{
    [Required]
    public Guid TenantId { get; set; }

    [Required]
    public DateTime ScheduledDate { get; set; }

    [Required]
    public TimeSpan ScheduledTime { get; set; }

    public string? Notes { get; set; }

    [Required]
    public bool ConsentGiven { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one service is required")]
    public List<AppointmentServiceRequest> Services { get; set; } = new();
}

public class AppointmentServiceRequest
{
    [Required]
    public Guid ServiceId { get; set; }

    public Guid? ServiceProviderId { get; set; }

    public Gender? PreferredGender { get; set; }
}
