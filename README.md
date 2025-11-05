# Blitz Prevair - Multi-Tenant Appointment Scheduling Platform

A comprehensive appointment scheduling solution for wellness and service providers, similar to Soothe.com. Enables multiple companies to manage services, providers, and appointments with a seamless booking experience.

## 🎯 Features

### Core Functionality
- **Multi-Tenant Architecture**: Multiple companies can create and manage their service listings
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

### Multi-Tenancy
Each company (tenant) has isolated data and configurations. Tenants are identified by subdomain or tenant ID.

### Service Configuration
Services can be configured with:
- Category association
- Provider selection rules (specific provider vs gender preference)
- Duration and pricing
- Custom form fields

### Booking Flow
1. Select tenant/company
2. Browse service categories
3. Select service(s)
4. Choose provider or gender preference
5. View and select available time slots
6. Complete registration form
7. Review and accept consent forms
8. Confirm booking

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
