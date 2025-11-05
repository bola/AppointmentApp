# Quick Deploy to Railway (5 Minutes)

Follow these simple steps to deploy AppointmentApp to Railway using the web interface.

## 🚀 Steps

### 1. Create Railway Project (1 minute)

1. Go to https://railway.app
2. Sign in with GitHub
3. Click **"New Project"**
4. Select **"Empty Project"**
5. Name it: `AppointmentApp`

### 2. Add PostgreSQL Database (1 minute)

1. In your project, click **"+ New"**
2. Select **"Database"** → **"PostgreSQL"**
3. Wait 10 seconds for provisioning
4. Click on the PostgreSQL service
5. Go to **"Variables"** tab
6. **Copy the `DATABASE_URL`** value

### 3. Convert Database URL (30 seconds)

You'll see something like:
```
postgresql://postgres:password123@containers-us-west-123.railway.app:5432/railway
```

Convert to .NET format:
```
Host=containers-us-west-123.railway.app;Port=5432;Database=railway;Username=postgres;Password=password123;SSL Mode=Require;Trust Server Certificate=true
```

**Pattern:**
- `Host=` the part after @ and before :port
- `Port=5432` (usually)
- `Database=` the part after the last /
- `Username=` the part between :// and :
- `Password=` the part between first : and @

### 4. Deploy Backend (1 minute)

1. Click **"+ New"** → **"GitHub Repo"**
2. Select your `AppointmentApp` repository
3. Railway auto-detects it as .NET
4. Click on the new service (should say "BlitzPrevair.API" or similar)

### 5. Configure Service (2 minutes)

Click on your backend service, then:

**Settings Tab:**
1. **Root Directory:** `backend`
2. **Start Command:** Leave empty (auto-detected)

**Variables Tab - Click "New Variable" for each:**

```bash
ConnectionStrings__DefaultConnection
<paste your converted connection string from step 3>

ASPNETCORE_ENVIRONMENT
Production

ASPNETCORE_URLS
http://0.0.0.0:$PORT

Jwt__Secret
AppointmentApp-Super-Secret-Key-Min-32-Chars-CHANGE-ME-IN-PRODUCTION

Jwt__Issuer
AppointmentApp

Jwt__Audience
AppointmentApp
```

**Important:** Click **"Add"** after each variable!

### 6. Deploy! (Automatic)

Railway will automatically build and deploy. Watch the **"Deployments"** tab.

Build takes about 2-3 minutes. You'll see:
- ✅ Build succeeded
- ✅ Deployment live

### 7. Get Your URL (30 seconds)

1. Go to **"Settings"** tab
2. Scroll to **"Networking"**
3. Click **"Generate Domain"**
4. Copy your URL: `https://appointmentapp-production-xxxx.up.railway.app`

### 8. Run Migrations (30 seconds)

1. Click on your backend service
2. Go to **"Shell"** tab (if available) or **"Deployments"** → **"View Logs"**
3. If Shell is available, run:

```bash
cd src/BlitzPrevair.API
dotnet ef database update
dotnet run --seed
```

**If Shell is not available:** The migrations will run automatically on first request, but you need to seed data manually using the API or wait for a future update.

### 9. Test Your API! ✅

Open these URLs in your browser:

**Health Check:**
```
https://your-app.up.railway.app/api/health
```

Should return:
```json
{
  "status": "healthy",
  "service": "AppointmentApp API",
  "version": "1.0.0"
}
```

**Swagger UI:**
```
https://your-app.up.railway.app/swagger
```

Interactive API documentation!

**Get Blitz Prive Tenant:**
```
https://your-app.up.railway.app/api/tenants/subdomain/blitzprive
```

## ✅ You're Done!

Your API is now live at: `https://your-app.up.railway.app`

## 🧪 Quick Test

### Register a User

```bash
curl -X POST https://your-app.up.railway.app/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test123456!"
  }'
```

Save the `token` from the response!

### Get Services

```bash
# First get Blitz Prive tenant
curl https://your-app.up.railway.app/api/tenants/subdomain/blitzprive

# Copy the tenant "id", then:
curl https://your-app.up.railway.app/api/tenants/{TENANT_ID}/services
```

### Create Appointment

```bash
curl -X POST https://your-app.up.railway.app/api/appointments \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "tenantId": "TENANT_ID",
    "scheduledDate": "2025-11-20",
    "scheduledTime": "14:00:00",
    "consentGiven": true,
    "services": [{
      "serviceId": "SERVICE_ID"
    }]
  }'
```

## 🚨 Troubleshooting

### "Build Failed"
- Check **"Deployments"** → **"Build Logs"**
- Ensure root directory is set to `backend`
- Verify all `.csproj` files are valid

### "Application Error" / 500
- Check **"Deployments"** → **"View Logs"**
- Verify `ConnectionStrings__DefaultConnection` is correct
- Ensure PostgreSQL service is running

### "Cannot connect to database"
- Double-check connection string format
- Ensure you copied from PostgreSQL Variables tab
- Verify `SSL Mode=Require;Trust Server Certificate=true` is included

### API Returns 404
- Verify deployment succeeded
- Check URL is correct (should have `/api/` in path)
- Wait 30 seconds after deployment completes

## 💰 Cost

**Railway Free Tier:**
- $5 free credits per month
- Enough for development/testing

**Railway Hobby Plan:**
- $5/month + usage
- Recommended for production

## 📚 Documentation

- `AUTHENTICATION.md` - Auth guide with examples
- `API_ENDPOINTS.md` - Complete API reference
- `RAILWAY_DEPLOY.md` - Detailed deployment guide

## 🔐 Security Reminder

**Before going to production:**

1. Change `Jwt__Secret` to a random value:
   ```bash
   openssl rand -base64 64
   ```
2. Update environment variable in Railway
3. Consider enabling:
   - Email verification
   - Rate limiting
   - HTTPS enforcement
   - Database backups

---

## Next Steps

1. ✅ Test all API endpoints in Swagger
2. ✅ Register test users
3. ✅ Create test appointments
4. ✅ Deploy frontend (Vercel for Next.js, EAS for mobile)
5. ✅ Connect frontend to your Railway API URL

**Congratulations! Your AppointmentApp is live! 🎉**
