# Blitz Prevair - Setup Guide

This guide will help you set up the Blitz Prevair application for local development.

## Prerequisites

Before you begin, ensure you have the following installed:

- **Git**: For version control
- **.NET 8 SDK**: For backend development
  - Download: https://dotnet.microsoft.com/download/dotnet/8.0
- **Node.js 18+**: For frontend development
  - Download: https://nodejs.org/
- **PostgreSQL 15+**: For database (or use Docker)
  - Download: https://www.postgresql.org/download/
- **Docker & Docker Compose**: For containerized development (optional but recommended)
  - Download: https://www.docker.com/get-started

## Quick Start (Docker)

The fastest way to get started is using Docker Compose:

```bash
# Clone the repository
git clone <repository-url>
cd AppointmentApp

# Start PostgreSQL and pgAdmin
docker-compose up -d

# Backend will need to be run manually (see Backend Setup below)
```

This will start:
- PostgreSQL on `localhost:5432`
- pgAdmin on `localhost:5050` (admin@blitzprevair.com / admin)

## Backend Setup (.NET API)

### 1. Install Dependencies

```bash
cd backend
dotnet restore
```

### 2. Configure Database Connection

Update `src/BlitzPrevair.API/appsettings.Development.json` if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=blitzprevair;Username=postgres;Password=postgres"
  }
}
```

### 3. Create Database and Run Migrations

```bash
cd src/BlitzPrevair.API

# Create initial migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### 4. Run the API

```bash
cd src/BlitzPrevair.API
dotnet run
```

The API will be available at `https://localhost:5001` and `http://localhost:5000`.

Test the health endpoint:
```bash
curl http://localhost:5000/api/health
```

### 5. Explore API with Swagger

Open `http://localhost:5000/swagger` in your browser to see the API documentation.

## Frontend Setup (Web)

### 1. Install Dependencies

```bash
cd frontend/web
npm install
```

### 2. Configure Environment

Create `.env.local`:

```bash
cp .env.local.example .env.local
```

Edit `.env.local`:
```
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

### 3. Run Development Server

```bash
npm run dev
```

The web app will be available at `http://localhost:3000`.

## Mobile Setup (React Native + Expo)

### 1. Install Dependencies

```bash
cd frontend/mobile
npm install
```

### 2. Install Expo Go App

Download Expo Go on your mobile device:
- iOS: https://apps.apple.com/app/expo-go/id982107779
- Android: https://play.google.com/store/apps/details?id=host.exp.exponent

### 3. Configure Environment

Create `.env`:

```bash
cp .env.example .env
```

Edit `.env`:
```
API_URL=http://<your-local-ip>:5000/api
```

Note: Replace `<your-local-ip>` with your computer's local IP address (not localhost).

### 4. Start Development Server

```bash
npm start
```

Scan the QR code with:
- iOS: Camera app
- Android: Expo Go app

## Database Management

### Using pgAdmin (Docker)

1. Open `http://localhost:5050`
2. Login with `admin@blitzprevair.com` / `admin`
3. Add server:
   - Host: `postgres` (or `localhost` if not using Docker)
   - Port: `5432`
   - Database: `blitzprevair`
   - Username: `postgres`
   - Password: `postgres`

### Using Command Line

```bash
# Connect to PostgreSQL
psql -h localhost -U postgres -d blitzprevair

# List tables
\dt

# Describe table
\d tenants

# Query data
SELECT * FROM tenants;
```

## Development Workflow

### Backend Development

```bash
# Watch mode (auto-reload on changes)
cd backend/src/BlitzPrevair.API
dotnet watch run

# Run tests
cd backend
dotnet test

# Create new migration
cd src/BlitzPrevair.API
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Frontend Web Development

```bash
cd frontend/web

# Development server
npm run dev

# Type checking
npm run type-check

# Linting
npm run lint

# Build for production
npm run build
npm start
```

### Mobile Development

```bash
cd frontend/mobile

# Start Expo
npm start

# Run on iOS simulator (Mac only)
npm run ios

# Run on Android emulator
npm run android

# Run on web
npm run web
```

## Seeding Sample Data

Create a seed script to populate the database with sample data:

```bash
cd backend/src/BlitzPrevair.API
dotnet run --seed
```

This will create:
- Sample tenant
- Service categories
- Services
- Service providers
- Sample availability schedules

## Troubleshooting

### Backend Issues

**Migration errors:**
```bash
# Drop database and recreate
dotnet ef database drop
dotnet ef database update
```

**Port already in use:**
```bash
# Change port in launchSettings.json or kill process
lsof -ti:5000 | xargs kill -9
```

### Frontend Issues

**Node modules issues:**
```bash
# Clear cache and reinstall
rm -rf node_modules package-lock.json
npm install
```

**Next.js cache issues:**
```bash
rm -rf .next
npm run dev
```

**Expo issues:**
```bash
# Clear Expo cache
expo start -c
```

### Database Issues

**Connection refused:**
- Ensure PostgreSQL is running: `docker-compose ps` or `brew services list`
- Check connection string in appsettings.json
- Verify port 5432 is not blocked

**Permission denied:**
- Check PostgreSQL user permissions
- Ensure database exists: `psql -U postgres -l`

## VS Code Setup (Recommended)

### Extensions

Install these VS Code extensions:
- C# Dev Kit
- ESLint
- Prettier
- Tailwind CSS IntelliSense
- React Native Tools
- Docker

### Launch Configuration

Create `.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (web)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/backend/src/BlitzPrevair.API/bin/Debug/net8.0/BlitzPrevair.API.dll",
      "args": [],
      "cwd": "${workspaceFolder}/backend/src/BlitzPrevair.API",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      },
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  ]
}
```

## Next Steps

1. Read the [API Documentation](./docs/api/README.md)
2. Review the [Database Schema](./docs/database/schema.md)
3. Understand the [Frontend Architecture](./docs/frontend/architecture.md)
4. Check the [Deployment Guide](./docs/deployment/railway.md)

## Getting Help

- Check existing documentation in `/docs`
- Review code comments
- Open an issue on GitHub
- Contact the development team

## Contributing

1. Create a feature branch
2. Make your changes
3. Write/update tests
4. Update documentation
5. Submit a pull request

Happy coding! 🚀
