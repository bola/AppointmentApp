# AppointmentApp - Multi-Tenant Appointment Scheduling Platform

A comprehensive multi-tenant SaaS platform for appointment scheduling, similar to Soothe.com. Enables multiple wellness and service companies (like **Blitz Prive**) to manage their services, providers, and appointments with a seamless booking experience.

**AppointmentApp** is the platform - **Blitz Prive** is one of many companies (tenants) that use it.

## 🎯 Platform Features

### Multi-Tenancy
AppointmentApp is a **SaaS platform** that hosts multiple independent wellness companies:
- **Tenant Isolation**: Each company (like Blitz Prive) has their own:
  - Branded subdomain (e.g., blitzprive.appointmentapp.com)
  - Services and categories
  - Service providers
  - Custom forms and consent documents
  - Customer base and appointments
- **White-label Ready**: Each tenant can customize branding, colors, and logo

### Core Functionality
- **Service Categories**: Organize services into categories (Massage, Physiotherapy, Facials, etc.)
- **Provider Management**:
  - Select specific service providers
  - Gender preference selection for certain services (massage, physio, facial)
- **Smart Time Slot Booking**: View available time slots for selected services
- **Multiple Service Booking**: Book multiple services in a single appointment
- **Customizable Registration Forms**: Backend-configurable forms with core fields (username, password, name, email)
- **Consent Management**: Terms, conditions, and consent forms
- **Mobile & Web Support**: Full-featured apps for web and native mobile platforms

## 🛠️ Tech Stack

### Backend
- **.NET 8 Web API**: RESTful API with modern C# features
- **PostgreSQL**: Reliable, scalable database
- **Entity Framework Core**: ORM for database operations
- **Multi-tenant architecture**: Tenant isolation and data security

### Frontend
- **Web**: Next.js 14 (React + TypeScript) - SSR, SEO-optimized
- **Mobile**: React Native with Expo - iOS and Android native apps
- **Shared Libraries**: Reusable components and business logic

### Infrastructure
- **Hosting**: Railway.io
- **Database**: PostgreSQL (Railway)
- **Containerization**: Docker for local development

## 📁 Project Structure

```
AppointmentApp/
├── backend/                    # .NET Web API
│   ├── src/
│   │   ├── BlitzPrevair.API/          # API layer
│   │   ├── BlitzPrevair.Core/         # Domain models, interfaces
│   │   ├── BlitzPrevair.Infrastructure/ # Data access, EF Core
│   │   └── BlitzPrevair.Shared/       # Shared utilities
│   └── tests/
├── frontend/
│   ├── web/                   # Next.js web application
│   └── mobile/                # React Native Expo app
├── shared/                    # Shared types, utilities
├── docker-compose.yml         # Local development setup
└── docs/                      # Documentation
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- PostgreSQL 15+
- Docker & Docker Compose (for local development)

### Local Development Setup

#### 1. Clone and Setup
```bash
git clone <repository-url>
cd AppointmentApp
```

#### 2. Start Services with Docker
```bash
docker-compose up -d
```

#### 3. Backend Setup
```bash
cd backend/src/BlitzPrevair.API
dotnet restore
dotnet ef database update
dotnet run
```

#### 4. Web Frontend Setup
```bash
cd frontend/web
npm install
npm run dev
```

#### 5. Mobile App Setup
```bash
cd frontend/mobile
npm install
npx expo start
```

## 🔑 Key Concepts

### Multi-Tenancy Architecture
AppointmentApp is a SaaS platform where each company (tenant) operates independently:
- **Data Isolation**: Complete separation between tenants for security and privacy
- **Custom Branding**: Each tenant has their own logo, colors, and subdomain
- **Independent Configuration**: Services, forms, and consent documents per tenant
- **Tenant Identification**: By subdomain (blitzprive.appointmentapp.com) or tenant ID in API calls

**Example Tenants:**
- **Blitz Prive** - Premium wellness and massage services
- **Wellness Co** - Physiotherapy and rehabilitation
- **Spa Elite** - Facial and beauty treatments

### Service Configuration
Services can be configured with:
- Category association
- Provider selection rules (specific provider vs gender preference)
- Duration and pricing
- Custom form fields

### Customer Booking Flow
When a customer visits a tenant's site (e.g., blitzprive.appointmentapp.com):

1. **Browse Services**: View available services organized by categories
2. **Select Service(s)**: Choose one or multiple services to book
3. **Choose Provider**: Select specific provider or specify gender preference
4. **Pick Time Slot**: View and select available appointment times
5. **Register/Login**: Complete customer registration with tenant's custom form fields
6. **Accept Terms**: Review and accept tenant's consent forms and terms
7. **Confirm Booking**: Finalize appointment and receive confirmation
8. **Receive Notification**: Get email/SMS confirmation with appointment details

## 📚 Documentation

- [API Documentation](./docs/api/README.md)
- [Database Schema](./docs/database/schema.md)
- [Frontend Architecture](./docs/frontend/architecture.md)
- [Deployment Guide](./docs/deployment/railway.md)

## 🧪 Testing

```bash
# Backend tests
cd backend
dotnet test

# Frontend tests
cd frontend/web
npm test
```

## 🚢 Deployment

The application is deployed on Railway.io. See [Deployment Guide](./docs/deployment/railway.md) for details.

## 📝 License

[Your License Here]

## 👥 Contributing

[Contribution guidelines]
