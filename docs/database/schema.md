# Database Schema

This document describes the database schema for the Blitz Prevair application.

## Overview

The application uses PostgreSQL as the database and Entity Framework Core for ORM. The schema supports multi-tenancy, service booking, and customizable forms.

## Entity Relationship Diagram

```
Tenant
  ├── ServiceCategories
  ├── Services
  ├── ServiceProviders
  ├── Appointments
  ├── CustomFormFields
  └── ConsentForms

Service
  ├── ServiceProviderServices
  └── AppointmentServices

ServiceProvider
  ├── ServiceProviderServices
  ├── ProviderAvailabilities
  └── AppointmentServices

Customer
  ├── Appointments
  ├── CustomerFormData
  └── CustomerConsents

Appointment
  └── AppointmentServices
```

## Core Tables

### Tenants
Stores information about companies using the platform.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| Name | VARCHAR(200) | Company name |
| Subdomain | VARCHAR(100) | Unique subdomain identifier |
| Logo | VARCHAR | Logo URL |
| PrimaryColor | VARCHAR | Brand color |
| SecondaryColor | VARCHAR | Brand color |
| ContactEmail | VARCHAR(255) | Contact email |
| ContactPhone | VARCHAR | Contact phone |
| IsActive | BOOLEAN | Active status |
| CreatedAt | TIMESTAMP | Creation timestamp |
| UpdatedAt | TIMESTAMP | Last update timestamp |
| IsDeleted | BOOLEAN | Soft delete flag |

### ServiceCategories
Organizes services into categories.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| TenantId | GUID | Foreign key to Tenants |
| Name | VARCHAR(200) | Category name |
| Description | TEXT | Category description |
| Icon | VARCHAR | Icon identifier |
| DisplayOrder | INT | Sort order |

### Services
Available services offered by tenants.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| TenantId | GUID | Foreign key to Tenants |
| CategoryId | GUID | Foreign key to ServiceCategories |
| Name | VARCHAR(200) | Service name |
| Description | TEXT | Service description |
| Price | DECIMAL(18,2) | Service price |
| DurationMinutes | INT | Service duration |
| ImageUrl | VARCHAR | Service image |
| AllowProviderSelection | BOOLEAN | Can select specific provider |
| AllowGenderPreference | BOOLEAN | Can specify gender preference |
| IsActive | BOOLEAN | Active status |

### ServiceProviders
People who provide services.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| TenantId | GUID | Foreign key to Tenants |
| FirstName | VARCHAR(100) | First name |
| LastName | VARCHAR(100) | Last name |
| Email | VARCHAR(255) | Email address |
| Phone | VARCHAR | Phone number |
| Bio | TEXT | Biography |
| PhotoUrl | VARCHAR | Photo URL |
| Gender | ENUM | Male, Female, Other, PreferNotToSay |
| IsActive | BOOLEAN | Active status |

### ServiceProviderServices
Many-to-many relationship between providers and services.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| ServiceProviderId | GUID | Foreign key to ServiceProviders |
| ServiceId | GUID | Foreign key to Services |

### ProviderAvailabilities
Defines when providers are available.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| ServiceProviderId | GUID | Foreign key to ServiceProviders |
| DayOfWeek | INT | 0 (Sunday) to 6 (Saturday) |
| StartTime | TIME | Start time |
| EndTime | TIME | End time |
| IsAvailable | BOOLEAN | Availability flag |
| SpecificDate | DATE | Optional: specific date override |

### Customers
Users who book appointments.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| Username | VARCHAR(100) | Unique username |
| PasswordHash | VARCHAR | Hashed password |
| Email | VARCHAR(255) | Email address |
| FirstName | VARCHAR(100) | First name |
| LastName | VARCHAR(100) | Last name |
| Phone | VARCHAR | Phone number |
| DateOfBirth | DATE | Date of birth |
| Gender | ENUM | Gender |
| EmailVerified | BOOLEAN | Email verification status |
| IsActive | BOOLEAN | Active status |

### Appointments
Booking records.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| TenantId | GUID | Foreign key to Tenants |
| CustomerId | GUID | Foreign key to Customers |
| ScheduledDate | DATE | Appointment date |
| ScheduledTime | TIME | Appointment time |
| Status | ENUM | Pending, Confirmed, InProgress, Completed, Cancelled, NoShow |
| TotalPrice | DECIMAL(18,2) | Total cost |
| TotalDurationMinutes | INT | Total duration |
| Notes | TEXT | Additional notes |
| ConsentGiven | BOOLEAN | Consent flag |
| ConsentGivenAt | TIMESTAMP | Consent timestamp |

### AppointmentServices
Services within an appointment (supports multiple services).

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| AppointmentId | GUID | Foreign key to Appointments |
| ServiceId | GUID | Foreign key to Services |
| ServiceProviderId | GUID | Foreign key to ServiceProviders (nullable) |
| PreferredGender | ENUM | Gender preference if no specific provider |
| StartTime | TIMESTAMP | Service start time |
| EndTime | TIMESTAMP | Service end time |
| Price | DECIMAL(18,2) | Service price |

### CustomFormFields
Tenant-specific custom form fields.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| TenantId | GUID | Foreign key to Tenants |
| FieldName | VARCHAR(100) | Field identifier |
| Label | VARCHAR(200) | Display label |
| FieldType | ENUM | Text, Email, Phone, Number, Date, TextArea, Dropdown, Radio, Checkbox, File |
| IsRequired | BOOLEAN | Required flag |
| Placeholder | VARCHAR | Placeholder text |
| ValidationRules | TEXT | JSON validation rules |
| Options | TEXT | JSON options for dropdown/radio |
| DisplayOrder | INT | Sort order |
| IsActive | BOOLEAN | Active status |

### CustomerFormData
Stores custom form responses.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| CustomerId | GUID | Foreign key to Customers |
| CustomFormFieldId | GUID | Foreign key to CustomFormFields |
| Value | TEXT | Field value |

### ConsentForms
Tenant-specific consent forms and terms.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| TenantId | GUID | Foreign key to Tenants |
| Title | VARCHAR(200) | Form title |
| Content | TEXT | Form content (HTML/Markdown) |
| Version | VARCHAR | Version number |
| IsRequired | BOOLEAN | Required flag |
| IsActive | BOOLEAN | Active status |
| DisplayOrder | INT | Sort order |

### CustomerConsents
Tracks customer consent records.

| Column | Type | Description |
|--------|------|-------------|
| Id | GUID | Primary key |
| CustomerId | GUID | Foreign key to Customers |
| ConsentFormId | GUID | Foreign key to ConsentForms |
| Accepted | BOOLEAN | Acceptance flag |
| AcceptedAt | TIMESTAMP | Acceptance timestamp |
| ConsentFormVersion | VARCHAR | Version accepted |
| IpAddress | VARCHAR | User's IP address |

## Indexes

Key indexes for performance:

- `Tenants.Subdomain` (unique)
- `ServiceProviders.(TenantId, Email)` (unique)
- `Customers.Username` (unique)
- `Customers.Email` (unique)
- `Appointments.(TenantId, CustomerId)`
- `ProviderAvailabilities.(ServiceProviderId, DayOfWeek)`

## Migrations

To create a new migration:

```bash
cd backend/src/BlitzPrevair.API
dotnet ef migrations add MigrationName
```

To apply migrations:

```bash
dotnet ef database update
```

## Soft Deletes

All entities inherit from `BaseEntity` which includes `IsDeleted` flag for soft deletion. This allows data recovery and maintains referential integrity.
