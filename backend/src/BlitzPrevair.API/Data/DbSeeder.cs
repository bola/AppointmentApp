using BlitzPrevair.Core.Entities;
using BlitzPrevair.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlitzPrevair.API.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Check if data already exists
        if (await context.Tenants.AnyAsync())
        {
            Console.WriteLine("Database already seeded");
            return;
        }

        Console.WriteLine("Seeding database...");

        // Create Blitz Prive tenant
        var blitzPriveTenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = "Blitz Prive",
            Subdomain = "blitzprive",
            Logo = "/logos/blitzprive.png",
            PrimaryColor = "#2563eb",
            SecondaryColor = "#60a5fa",
            ContactEmail = "contact@blitzprive.com",
            ContactPhone = "+1-555-0100",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Tenants.AddAsync(blitzPriveTenant);

        // Create Service Categories
        var massageCategory = new ServiceCategory
        {
            Id = Guid.NewGuid(),
            TenantId = blitzPriveTenant.Id,
            Name = "Massage",
            Description = "Professional massage therapy services",
            Icon = "massage",
            DisplayOrder = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var physioCategory = new ServiceCategory
        {
            Id = Guid.NewGuid(),
            TenantId = blitzPriveTenant.Id,
            Name = "Physiotherapy",
            Description = "Recovery and rehabilitation services",
            Icon = "physiotherapy",
            DisplayOrder = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var facialCategory = new ServiceCategory
        {
            Id = Guid.NewGuid(),
            TenantId = blitzPriveTenant.Id,
            Name = "Facial & Spa",
            Description = "Premium facial and spa treatments",
            Icon = "facial",
            DisplayOrder = 3,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.ServiceCategories.AddRangeAsync(massageCategory, physioCategory, facialCategory);

        // Create Services
        var services = new List<Service>
        {
            // Massage Services
            new Service
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                CategoryId = massageCategory.Id,
                Name = "Swedish Massage",
                Description = "Classic relaxing full-body massage with gentle pressure",
                Price = 120.00m,
                DurationMinutes = 60,
                AllowProviderSelection = true,
                AllowGenderPreference = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                CategoryId = massageCategory.Id,
                Name = "Deep Tissue Massage",
                Description = "Therapeutic massage targeting deep muscle layers",
                Price = 140.00m,
                DurationMinutes = 60,
                AllowProviderSelection = true,
                AllowGenderPreference = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                CategoryId = massageCategory.Id,
                Name = "Prenatal Massage",
                Description = "Specialized massage for expectant mothers",
                Price = 150.00m,
                DurationMinutes = 60,
                AllowProviderSelection = true,
                AllowGenderPreference = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Physiotherapy Services
            new Service
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                CategoryId = physioCategory.Id,
                Name = "Sports Physiotherapy",
                Description = "Specialized treatment for sports injuries",
                Price = 160.00m,
                DurationMinutes = 60,
                AllowProviderSelection = true,
                AllowGenderPreference = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                CategoryId = physioCategory.Id,
                Name = "Post-Surgery Rehabilitation",
                Description = "Recovery therapy after surgical procedures",
                Price = 180.00m,
                DurationMinutes = 60,
                AllowProviderSelection = true,
                AllowGenderPreference = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            // Facial Services
            new Service
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                CategoryId = facialCategory.Id,
                Name = "European Facial",
                Description = "Classic deep cleansing and hydrating facial",
                Price = 130.00m,
                DurationMinutes = 75,
                AllowProviderSelection = true,
                AllowGenderPreference = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Service
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                CategoryId = facialCategory.Id,
                Name = "Anti-Aging Facial",
                Description = "Advanced facial with anti-aging treatments",
                Price = 180.00m,
                DurationMinutes = 90,
                AllowProviderSelection = true,
                AllowGenderPreference = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.Services.AddRangeAsync(services);

        // Create Service Providers
        var providers = new List<ServiceProvider>
        {
            new ServiceProvider
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                FirstName = "Sarah",
                LastName = "Johnson",
                Email = "sarah.johnson@blitzprive.com",
                Phone = "+1-555-0101",
                Bio = "Certified massage therapist with 10+ years of experience specializing in Swedish and deep tissue massage.",
                Gender = Gender.Female,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ServiceProvider
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                FirstName = "Michael",
                LastName = "Chen",
                Email = "michael.chen@blitzprive.com",
                Phone = "+1-555-0102",
                Bio = "Licensed massage therapist specializing in sports and deep tissue massage.",
                Gender = Gender.Male,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ServiceProvider
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                FirstName = "Emily",
                LastName = "Rodriguez",
                Email = "emily.rodriguez@blitzprive.com",
                Phone = "+1-555-0103",
                Bio = "Physical therapist with specialty in sports injuries and post-surgery rehabilitation.",
                Gender = Gender.Female,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ServiceProvider
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                FirstName = "David",
                LastName = "Williams",
                Email = "david.williams@blitzprive.com",
                Phone = "+1-555-0104",
                Bio = "Certified physiotherapist with 8 years of experience in rehabilitation and recovery.",
                Gender = Gender.Male,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new ServiceProvider
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                FirstName = "Lisa",
                LastName = "Anderson",
                Email = "lisa.anderson@blitzprive.com",
                Phone = "+1-555-0105",
                Bio = "Licensed esthetician specializing in European and anti-aging facial treatments.",
                Gender = Gender.Female,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.ServiceProviders.AddRangeAsync(providers);

        // Link providers to services (many-to-many)
        var providerServices = new List<ServiceProviderService>();

        // Sarah Johnson - Massage services
        foreach (var service in services.Where(s => s.CategoryId == massageCategory.Id).Take(3))
        {
            providerServices.Add(new ServiceProviderService
            {
                Id = Guid.NewGuid(),
                ServiceProviderId = providers[0].Id,
                ServiceId = service.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Michael Chen - Massage services
        foreach (var service in services.Where(s => s.CategoryId == massageCategory.Id && s.Name != "Prenatal Massage"))
        {
            providerServices.Add(new ServiceProviderService
            {
                Id = Guid.NewGuid(),
                ServiceProviderId = providers[1].Id,
                ServiceId = service.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Emily Rodriguez - Physio services
        foreach (var service in services.Where(s => s.CategoryId == physioCategory.Id))
        {
            providerServices.Add(new ServiceProviderService
            {
                Id = Guid.NewGuid(),
                ServiceProviderId = providers[2].Id,
                ServiceId = service.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // David Williams - Physio services
        foreach (var service in services.Where(s => s.CategoryId == physioCategory.Id))
        {
            providerServices.Add(new ServiceProviderService
            {
                Id = Guid.NewGuid(),
                ServiceProviderId = providers[3].Id,
                ServiceId = service.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // Lisa Anderson - Facial services
        foreach (var service in services.Where(s => s.CategoryId == facialCategory.Id))
        {
            providerServices.Add(new ServiceProviderService
            {
                Id = Guid.NewGuid(),
                ServiceProviderId = providers[4].Id,
                ServiceId = service.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await context.ServiceProviderServices.AddRangeAsync(providerServices);

        // Create sample availability for providers (Monday to Friday, 9 AM - 5 PM)
        var availabilities = new List<ProviderAvailability>();
        foreach (var provider in providers)
        {
            for (int day = 1; day <= 5; day++) // Monday to Friday
            {
                availabilities.Add(new ProviderAvailability
                {
                    Id = Guid.NewGuid(),
                    ServiceProviderId = provider.Id,
                    DayOfWeek = (DayOfWeek)day,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        await context.ProviderAvailabilities.AddRangeAsync(availabilities);

        // Create custom form fields
        var customFields = new List<CustomFormField>
        {
            new CustomFormField
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                FieldName = "allergies",
                Label = "Do you have any allergies?",
                FieldType = FormFieldType.TextArea,
                IsRequired = false,
                Placeholder = "Please list any allergies or sensitivities...",
                DisplayOrder = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CustomFormField
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                FieldName = "medical_conditions",
                Label = "Any medical conditions we should know about?",
                FieldType = FormFieldType.TextArea,
                IsRequired = false,
                Placeholder = "E.g., high blood pressure, pregnancy, recent injuries...",
                DisplayOrder = 2,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CustomFormField
            {
                Id = Guid.NewGuid(),
                TenantId = blitzPriveTenant.Id,
                FieldName = "pressure_preference",
                Label = "Massage pressure preference",
                FieldType = FormFieldType.Dropdown,
                IsRequired = false,
                Options = "[\"Light\",\"Medium\",\"Firm\",\"Very Firm\"]",
                DisplayOrder = 3,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.CustomFormFields.AddRangeAsync(customFields);

        // Create consent form
        var consentForm = new ConsentForm
        {
            Id = Guid.NewGuid(),
            TenantId = blitzPriveTenant.Id,
            Title = "Terms and Conditions",
            Content = @"<h2>Blitz Prive Terms of Service</h2>
<p>By booking a service with Blitz Prive, you agree to the following terms:</p>
<ul>
<li>You must be 18 years or older to book services</li>
<li>Cancellations must be made at least 24 hours in advance</li>
<li>Late cancellations or no-shows may incur a fee</li>
<li>Please inform us of any health conditions or allergies</li>
<li>We respect your privacy and handle data per our privacy policy</li>
</ul>
<p>Last updated: November 2025</p>",
            Version = "1.0",
            IsRequired = true,
            IsActive = true,
            DisplayOrder = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.ConsentForms.AddAsync(consentForm);

        // Save all changes
        await context.SaveChangesAsync();

        Console.WriteLine("Database seeded successfully!");
        Console.WriteLine($"Created tenant: {blitzPriveTenant.Name}");
        Console.WriteLine($"Created {services.Count} services");
        Console.WriteLine($"Created {providers.Count} service providers");
        Console.WriteLine($"Created {providerServices.Count} provider-service relationships");
        Console.WriteLine($"Created {availabilities.Count} availability schedules");
    }
}
