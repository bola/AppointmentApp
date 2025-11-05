# Railway Deployment Guide - Step by Step

Follow these simple steps to deploy AppointmentApp to Railway.

## 🚀 Quick Deployment (5 minutes)

### Step 1: Create Railway Account & Project

1. Go to https://railway.app
2. Sign in with GitHub
3. Click **"New Project"**
4. Select **"Empty Project"**
5. Name it: `AppointmentApp`

### Step 2: Add PostgreSQL Database

1. In your Railway project, click **"+ New"**
2. Select **"Database"** → **"PostgreSQL"**
3. Railway automatically provisions the database
4. Click on the PostgreSQL service
5. Go to **"Variables"** tab
6. Copy the `DATABASE_URL` (you'll need this)

### Step 3: Deploy Backend API from GitHub

1. In your Railway project, click **"+ New"**
2. Select **"GitHub Repo"**
3. Choose your `AppointmentApp` repository
4. Railway will detect it as a .NET project

### Step 4: Configure Backend Service

1. Click on your backend service
2. Go to **"Settings"**
3. Set **"Root Directory"** to: `backend`
4. Set **"Start Command"** to: `cd src/BlitzPrevair.API && dotnet run`

### Step 5: Set Environment Variables

1. Go to **"Variables"** tab on your backend service
2. Click **"+ New Variable"**
3. Add these variables:

```
ConnectionStrings__DefaultConnection = <paste your DATABASE_URL here, but modify format>
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://0.0.0.0:$PORT
```

**Important:** Railway PostgreSQL URL format needs conversion:

Railway gives: `postgresql://user:pass@host:port/db`

You need: `Host=host;Port=port;Database=db;Username=user;Password=pass;SSL Mode=Require;Trust Server Certificate=true`

**Example:**
```
Railway: postgresql://postgres:abc123@containers-us-west-1.railway.app:5432/railway

Convert to: Host=containers-us-west-1.railway.app;Port=5432;Database=railway;Username=postgres;Password=abc123;SSL Mode=Require;Trust Server Certificate=true
```

### Step 6: Deploy

1. Click **"Deploy"** or push to your GitHub repo
2. Railway automatically builds and deploys
3. Wait 2-3 minutes for build to complete

### Step 7: Run Migrations (One-Time Setup)

After first deployment, you need to create database tables:

**Option A: Using Railway CLI** (if you have it locally)
```bash
railway login
railway link
railway run dotnet ef database update --project backend/src/BlitzPrevair.API
railway run dotnet run --seed --project backend/src/BlitzPrevair.API
```

**Option B: Using Railway Shell** (easiest)
1. Click on your backend service
2. Go to **"Shell"** tab
3. Run these commands:
```bash
cd src/BlitzPrevair.API
dotnet ef database update
dotnet run --seed
```

### Step 8: Get Your API URL

1. Click on your backend service
2. Go to **"Settings"** → **"Networking"**
3. Click **"Generate Domain"**
4. Your API will be at: `https://your-app.up.railway.app`

### Step 9: Test Your API

Open your browser or use curl:
```bash
curl https://your-app.up.railway.app/api/health
```

Should return:
```json
{
  "status": "healthy",
  "service": "AppointmentApp API",
  "platform": "Multi-Tenant Appointment Scheduling",
  "version": "1.0.0"
}
```

Test Blitz Prive tenant:
```bash
curl https://your-app.up.railway.app/api/tenants/subdomain/blitzprive
```

## 🎯 What You Get

After deployment:
- ✅ PostgreSQL database (free tier: 512MB)
- ✅ .NET API running 24/7
- ✅ Blitz Prive tenant with sample data
- ✅ 7 services, 5 providers, availability schedules
- ✅ HTTPS enabled automatically
- ✅ Automatic deployments on git push

## 💰 Cost

Railway Free Tier:
- $5 free credits per month
- Enough for development/testing
- Upgrade to Hobby ($5/month) for production

## 🐛 Troubleshooting

### Build Fails
- Check **"Build Logs"** tab
- Ensure .NET 8 SDK is being used
- Verify all .csproj files are valid

### Database Connection Fails
- Verify `ConnectionStrings__DefaultConnection` format
- Ensure SSL Mode is set: `SSL Mode=Require;Trust Server Certificate=true`
- Check PostgreSQL service is running

### Migrations Fail
- Make sure you're in correct directory: `cd src/BlitzPrevair.API`
- Run: `dotnet tool install --global dotnet-ef` first
- Check database is accessible

### API Returns 404
- Verify the root directory is set to `backend`
- Check start command is correct
- Look at deployment logs

## 📱 Deploy Frontend (Optional)

### Deploy Next.js Web App to Vercel

1. Go to https://vercel.com
2. Import your GitHub repository
3. Set **"Root Directory"** to: `frontend/web`
4. Add environment variable:
   ```
   NEXT_PUBLIC_API_URL = https://your-app.up.railway.app/api
   ```
5. Deploy

### Deploy Mobile App to Expo

```bash
cd frontend/mobile
npm install -g eas-cli
eas login
eas build:configure
eas build --platform ios
eas build --platform android
```

## 🔄 Continuous Deployment

Once set up, every time you push to your GitHub repo:
- Railway automatically rebuilds
- Deploys new version
- Zero downtime deployment

## ✅ Verification Checklist

After deployment, verify:
- [ ] API health check responds: `/api/health`
- [ ] Can fetch Blitz Prive tenant: `/api/tenants/subdomain/blitzprive`
- [ ] Database has seeded data
- [ ] Swagger UI works (in dev): `/swagger`
- [ ] HTTPS is enabled

---

## 🆘 Need Help?

Common Railway commands:
```bash
# View logs
railway logs

# Open shell
railway shell

# Check status
railway status

# Link to project
railway link
```

For more help: https://docs.railway.app
