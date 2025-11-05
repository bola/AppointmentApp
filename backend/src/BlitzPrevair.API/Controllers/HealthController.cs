using Microsoft.AspNetCore.Mvc;

namespace BlitzPrevair.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            service = "AppointmentApp API",
            platform = "Multi-Tenant Appointment Scheduling",
            version = "1.0.0"
        });
    }
}
