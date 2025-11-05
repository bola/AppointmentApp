using BlitzPrevair.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlitzPrevair.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServiceProvider> ServiceProviders => Set<ServiceProvider>();
    public DbSet<ServiceProviderService> ServiceProviderServices => Set<ServiceProviderService>();
    public DbSet<ProviderAvailability> ProviderAvailabilities => Set<ProviderAvailability>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AppointmentService> AppointmentServices => Set<AppointmentService>();
    public DbSet<CustomFormField> CustomFormFields => Set<CustomFormField>();
    public DbSet<CustomerFormData> CustomerFormData => Set<CustomerFormData>();
    public DbSet<ConsentForm> ConsentForms => Set<ConsentForm>();
    public DbSet<CustomerConsent> CustomerConsents => Set<CustomerConsent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity relationships and constraints
        ConfigureTenant(modelBuilder);
        ConfigureServiceCategory(modelBuilder);
        ConfigureService(modelBuilder);
        ConfigureServiceProvider(modelBuilder);
        ConfigureServiceProviderService(modelBuilder);
        ConfigureProviderAvailability(modelBuilder);
        ConfigureCustomer(modelBuilder);
        ConfigureAppointment(modelBuilder);
        ConfigureAppointmentService(modelBuilder);
        ConfigureCustomFormField(modelBuilder);
        ConfigureCustomerFormData(modelBuilder);
        ConfigureConsentForm(modelBuilder);
        ConfigureCustomerConsent(modelBuilder);
    }

    private void ConfigureTenant(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Subdomain).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ContactEmail).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Subdomain).IsUnique();
        });
    }

    private void ConfigureServiceCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.ServiceCategories)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureService(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Services)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureServiceProvider(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceProvider>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.ServiceProviders)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.TenantId, e.Email }).IsUnique();
        });
    }

    private void ConfigureServiceProviderService(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceProviderService>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.ServiceProvider)
                .WithMany(sp => sp.ServiceProviderServices)
                .HasForeignKey(e => e.ServiceProviderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Service)
                .WithMany(s => s.ServiceProviderServices)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.ServiceProviderId, e.ServiceId }).IsUnique();
        });
    }

    private void ConfigureProviderAvailability(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProviderAvailability>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.ServiceProvider)
                .WithMany(sp => sp.Availabilities)
                .HasForeignKey(e => e.ServiceProviderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }

    private void ConfigureAppointment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalPrice).HasPrecision(18, 2);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Appointments)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureAppointmentService(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppointmentService>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(18, 2);

            entity.HasOne(e => e.Appointment)
                .WithMany(a => a.AppointmentServices)
                .HasForeignKey(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Service)
                .WithMany(s => s.AppointmentServices)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ServiceProvider)
                .WithMany(sp => sp.AppointmentServices)
                .HasForeignKey(e => e.ServiceProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureCustomFormField(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomFormField>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FieldName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Label).IsRequired().HasMaxLength(200);

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.CustomFormFields)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureCustomerFormData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerFormData>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.CustomFormData)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CustomFormField)
                .WithMany()
                .HasForeignKey(e => e.CustomFormFieldId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureConsentForm(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConsentForm>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).IsRequired();

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.ConsentForms)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureCustomerConsent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerConsent>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ConsentForm)
                .WithMany(cf => cf.CustomerConsents)
                .HasForeignKey(e => e.ConsentFormId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
