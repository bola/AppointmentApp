# Deploying to Railway.io

This guide walks you through deploying the Blitz Prevair application to Railway.io.

## Prerequisites

- A Railway.io account (sign up at https://railway.app)
- Git repository with your code
- Railway CLI (optional but recommended)

## Step 1: Create a New Project

1. Log in to Railway.io
2. Click "New Project"
3. Choose "Deploy from GitHub repo" or "Empty Project"

## Step 2: Add PostgreSQL Database

1. In your Railway project, click "+ New"
2. Select "Database" → "PostgreSQL"
3. Railway will automatically provision a PostgreSQL database
4. Note the connection string from the "Variables" tab

## Step 3: Deploy the Backend API

### Option A: Using GitHub Integration

1. Click "+ New" → "GitHub Repo"
2. Select your repository
3. Railway will detect the Dockerfile automatically
4. Set the root directory to `backend` if needed

### Option B: Using Railway CLI

```bash
# Install Railway CLI
npm i -g @railway/cli

# Login
railway login

# Link to your project
railway link

# Deploy
cd backend
railway up
```

## Step 4: Configure Environment Variables

In your Railway service settings, add these environment variables:

```
ConnectionStrings__DefaultConnection=<your-postgresql-connection-string>
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:8080
```

The PostgreSQL connection string format:
```
Host=<host>;Port=<port>;Database=<database>;Username=<username>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true
```

## Step 5: Deploy the Frontend (Next.js)

### Deploy Web Frontend to Vercel (Recommended)

1. Go to https://vercel.com
2. Import your repository
3. Set root directory to `frontend/web`
4. Add environment variable:
   ```
   NEXT_PUBLIC_API_URL=<your-railway-api-url>/api
   ```
5. Deploy

### Alternative: Deploy to Railway

1. Create a new service in Railway
2. Connect your GitHub repo
3. Set root directory to `frontend/web`
4. Railway will auto-detect Next.js and deploy

## Step 6: Deploy Mobile App

The React Native Expo app can be deployed using Expo Application Services (EAS):

```bash
cd frontend/mobile

# Install EAS CLI
npm install -g eas-cli

# Login to Expo
eas login

# Configure
eas build:configure

# Build for iOS
eas build --platform ios

# Build for Android
eas build --platform android

# Submit to app stores
eas submit --platform ios
eas submit --platform android
```

## Step 7: Run Database Migrations

After deploying the backend, run migrations:

### Using Railway CLI:

```bash
railway run dotnet ef database update --project src/BlitzPrevair.API
```

### Alternative: Add to Startup

Migrations can be run automatically on startup by adding this to `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}
```

## Step 8: Verify Deployment

1. Check your API health endpoint: `https://your-app.railway.app/api/health`
2. Check the web frontend loads correctly
3. Test the mobile app connection to the API

## Environment Variables Reference

### Backend (Railway)
- `ConnectionStrings__DefaultConnection`: PostgreSQL connection string
- `ASPNETCORE_ENVIRONMENT`: `Production`
- `ASPNETCORE_URLS`: `http://0.0.0.0:8080`
- `JWT_SECRET`: Your JWT secret key
- `JWT_ISSUER`: Your domain
- `JWT_AUDIENCE`: Your domain

### Frontend Web (Vercel/Railway)
- `NEXT_PUBLIC_API_URL`: Backend API URL

### Mobile (EAS)
- `API_URL`: Backend API URL (set in app.config.js)

## Troubleshooting

### Database Connection Issues
- Ensure SSL Mode is set correctly in connection string
- Verify database is in the same region as your API service
- Check database credentials in Railway dashboard

### Migration Errors
- Ensure Entity Framework CLI tools are installed
- Check connection string format
- Verify database permissions

### CORS Issues
- Add your frontend domains to CORS policy in `Program.cs`
- Ensure Railway provides consistent domain URLs

## Monitoring

Railway provides built-in monitoring:
- View logs in the Railway dashboard
- Set up health check endpoints
- Monitor resource usage (CPU, Memory, Network)

## Scaling

To scale your application:
1. Go to your service settings in Railway
2. Adjust resources (CPU, Memory)
3. Enable autoscaling if needed
4. Consider adding Redis for caching

## Cost Optimization

- Use Railway's free tier for development
- Upgrade to Pro for production ($5/month + usage)
- PostgreSQL databases included in Pro plan
- Monitor usage to avoid unexpected costs
