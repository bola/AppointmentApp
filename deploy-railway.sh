#!/bin/bash

# Railway Deployment Script for AppointmentApp
# This script helps you deploy to Railway.io step by step

set -e  # Exit on error

echo "============================================"
echo "  AppointmentApp - Railway Deployment"
echo "============================================"
echo ""

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Check if Railway CLI is installed
if ! command -v railway &> /dev/null; then
    echo -e "${YELLOW}Railway CLI not found. Installing...${NC}"
    npm install -g @railway/cli
fi

echo -e "${GREEN}✓ Railway CLI installed${NC}"
echo ""

# Login to Railway
echo -e "${BLUE}Step 1: Login to Railway${NC}"
echo "Opening Railway login in browser..."
railway login

echo ""
echo -e "${GREEN}✓ Logged in to Railway${NC}"
echo ""

# Initialize or link project
echo -e "${BLUE}Step 2: Create/Link Railway Project${NC}"
echo "Options:"
echo "  1) Create new project"
echo "  2) Link to existing project"
read -p "Choose option (1 or 2): " option

if [ "$option" == "1" ]; then
    railway init
else
    railway link
fi

echo ""
echo -e "${GREEN}✓ Project linked${NC}"
echo ""

# Add PostgreSQL
echo -e "${BLUE}Step 3: Add PostgreSQL Database${NC}"
echo "Adding PostgreSQL plugin..."
railway add --database postgres

echo ""
echo -e "${GREEN}✓ PostgreSQL added${NC}"
echo ""

# Get PostgreSQL connection string
echo -e "${BLUE}Step 4: Get Database Connection String${NC}"
echo "Fetching PostgreSQL variables..."
DB_URL=$(railway variables --json | grep -o '"DATABASE_URL":"[^"]*' | cut -d'"' -f4 || echo "")

if [ -z "$DB_URL" ]; then
    echo -e "${YELLOW}⚠ Could not auto-fetch DATABASE_URL${NC}"
    echo "Please copy it from Railway dashboard and paste here:"
    read -p "DATABASE_URL: " DB_URL
fi

echo -e "${GREEN}✓ Database URL obtained${NC}"
echo ""

# Convert PostgreSQL URL to .NET connection string format
echo -e "${BLUE}Step 5: Converting Connection String${NC}"
echo "Converting PostgreSQL URL to .NET format..."

# Parse URL (simplified - you may need to adjust)
# Format: postgresql://user:pass@host:port/db
# To: Host=host;Port=port;Database=db;Username=user;Password=pass;SSL Mode=Require;Trust Server Certificate=true

echo ""
echo -e "${YELLOW}⚠ Manual step required:${NC}"
echo "Convert this PostgreSQL URL:"
echo "$DB_URL"
echo ""
echo "To .NET format:"
echo "Host=<host>;Port=<port>;Database=<db>;Username=<user>;Password=<pass>;SSL Mode=Require;Trust Server Certificate=true"
echo ""
read -p "Paste converted connection string: " DOTNET_CONN_STRING

# Generate JWT Secret
echo ""
echo -e "${BLUE}Step 6: Generate JWT Secret${NC}"
JWT_SECRET=$(openssl rand -base64 64 | tr -d '\n' || echo "AppointmentApp-$(date +%s)-Secret-Key-Change-This-To-Random-Value")
echo -e "${GREEN}✓ JWT Secret generated${NC}"

# Set environment variables
echo ""
echo -e "${BLUE}Step 7: Setting Environment Variables${NC}"
echo "Setting ConnectionStrings__DefaultConnection..."
railway variables --set "ConnectionStrings__DefaultConnection=$DOTNET_CONN_STRING"

echo "Setting ASPNETCORE_ENVIRONMENT..."
railway variables --set "ASPNETCORE_ENVIRONMENT=Production"

echo "Setting ASPNETCORE_URLS..."
railway variables --set "ASPNETCORE_URLS=http://0.0.0.0:\$PORT"

echo "Setting JWT__Secret..."
railway variables --set "Jwt__Secret=$JWT_SECRET"

echo "Setting JWT__Issuer..."
railway variables --set "Jwt__Issuer=AppointmentApp"

echo "Setting JWT__Audience..."
railway variables --set "Jwt__Audience=AppointmentApp"

echo ""
echo -e "${GREEN}✓ Environment variables set${NC}"
echo ""

# Set root directory
echo -e "${BLUE}Step 8: Configure Build Settings${NC}"
echo "Setting root directory to 'backend'..."
railway service --json | grep -q '"rootDirectory"' || railway service --root backend

echo ""
echo -e "${GREEN}✓ Build settings configured${NC}"
echo ""

# Deploy
echo -e "${BLUE}Step 9: Deploy Application${NC}"
echo "Deploying to Railway..."
railway up

echo ""
echo -e "${GREEN}✓ Deployment initiated${NC}"
echo ""

# Wait for deployment
echo "Waiting for deployment to complete..."
echo "(This may take 2-3 minutes)"
sleep 10

# Get domain
echo ""
echo -e "${BLUE}Step 10: Generate Public Domain${NC}"
echo "Getting deployment URL..."
DOMAIN=$(railway domain 2>&1 || echo "")

if [ -z "$DOMAIN" ]; then
    echo -e "${YELLOW}Generating new domain...${NC}"
    railway domain
    DOMAIN=$(railway domain 2>&1)
fi

echo ""
echo -e "${GREEN}✓ Domain: $DOMAIN${NC}"
echo ""

# Run migrations
echo -e "${BLUE}Step 11: Run Database Migrations${NC}"
echo "Running EF Core migrations..."
railway run dotnet ef database update --project src/BlitzPrevair.API

echo ""
echo -e "${GREEN}✓ Migrations completed${NC}"
echo ""

# Seed database
echo -e "${BLUE}Step 12: Seed Database${NC}"
echo "Seeding Blitz Prive tenant data..."
railway run dotnet run --seed --project src/BlitzPrevair.API

echo ""
echo -e "${GREEN}✓ Database seeded${NC}"
echo ""

# Test deployment
echo -e "${BLUE}Step 13: Test Deployment${NC}"
echo "Testing API health endpoint..."
HEALTH_RESPONSE=$(curl -s "https://$DOMAIN/api/health" || echo "")

if echo "$HEALTH_RESPONSE" | grep -q "healthy"; then
    echo -e "${GREEN}✓ API is healthy!${NC}"
else
    echo -e "${YELLOW}⚠ Could not verify health endpoint${NC}"
    echo "Response: $HEALTH_RESPONSE"
fi

echo ""
echo "============================================"
echo -e "${GREEN}  Deployment Complete! 🎉${NC}"
echo "============================================"
echo ""
echo "Your AppointmentApp API is now live at:"
echo -e "${BLUE}https://$DOMAIN${NC}"
echo ""
echo "Test endpoints:"
echo "  Health: https://$DOMAIN/api/health"
echo "  Swagger: https://$DOMAIN/swagger"
echo "  Tenants: https://$DOMAIN/api/tenants/subdomain/blitzprive"
echo ""
echo "Next steps:"
echo "  1. Test registration: POST https://$DOMAIN/api/auth/register"
echo "  2. Browse services: GET https://$DOMAIN/api/tenants/{id}/services"
echo "  3. View providers: GET https://$DOMAIN/api/tenants/{id}/providers"
echo ""
echo "Documentation:"
echo "  - AUTHENTICATION.md - Auth guide"
echo "  - API_ENDPOINTS.md - API reference"
echo "  - RAILWAY_DEPLOY.md - Deployment guide"
echo ""
echo -e "${GREEN}Happy coding! 🚀${NC}"
