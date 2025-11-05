# AppointmentApp API Endpoints

Complete API documentation for the multi-tenant appointment scheduling platform.

## Base URL

- **Local**: `http://localhost:5000/api`
- **Production**: `https://your-app.railway.app/api`
- **Swagger UI**: `https://your-app.railway.app/swagger`

## Authentication

Currently, endpoints are open. JWT authentication will be added in Phase 2.

---

## 📋 Tenants API

### Get All Tenants
```http
GET /api/tenants
```

**Response:**
```json
[
  {
    "id": "uuid",
    "name": "Blitz Prive",
    "subdomain": "blitzprive",
    "logo": "/logos/blitzprive.png",
    "primaryColor": "#2563eb",
    "secondaryColor": "#60a5fa",
    "contactEmail": "contact@blitzprive.com",
    "contactPhone": "+1-555-0100",
    "isActive": true
  }
]
```

### Get Tenant by ID
```http
GET /api/tenants/{tenantId}
```

### Get Tenant by Subdomain
```http
GET /api/tenants/subdomain/{subdomain}
```

**Example:**
```bash
curl https://your-app.railway.app/api/tenants/subdomain/blitzprive
```

### Get Tenant's Service Categories
```http
GET /api/tenants/{tenantId}/categories
```

**Response:**
```json
[
  {
    "id": "uuid",
    "tenantId": "uuid",
    "name": "Massage",
    "description": "Professional massage therapy services",
    "icon": "massage",
    "displayOrder": 1
  },
  {
    "id": "uuid",
    "tenantId": "uuid",
    "name": "Physiotherapy",
    "description": "Recovery and rehabilitation services",
    "icon": "physiotherapy",
    "displayOrder": 2
  }
]
```

---

## 🛍️ Services API

### Get All Services for Tenant
```http
GET /api/tenants/{tenantId}/services
```

**Query Parameters:**
- `categoryId` (optional): Filter by category
- `isActive` (optional): Filter by active status

**Example:**
```bash
# All services
curl https://your-app.railway.app/api/tenants/{tenantId}/services

# Services in Massage category
curl https://your-app.railway.app/api/tenants/{tenantId}/services?categoryId={categoryId}

# Only active services
curl https://your-app.railway.app/api/tenants/{tenantId}/services?isActive=true
```

**Response:**
```json
[
  {
    "id": "uuid",
    "tenantId": "uuid",
    "categoryId": "uuid",
    "name": "Swedish Massage",
    "description": "Classic relaxing full-body massage",
    "price": 120.00,
    "durationMinutes": 60,
    "imageUrl": null,
    "allowProviderSelection": true,
    "allowGenderPreference": true,
    "isActive": true,
    "category": {
      "id": "uuid",
      "tenantId": "uuid",
      "name": "Massage",
      "description": "Professional massage therapy services",
      "icon": "massage",
      "displayOrder": 1
    }
  }
]
```

### Get Service by ID
```http
GET /api/tenants/{tenantId}/services/{serviceId}
```

**Response:** Same as above, single service object

---

## 👨‍⚕️ Providers API

### Get All Providers for Tenant
```http
GET /api/tenants/{tenantId}/providers
```

**Query Parameters:**
- `serviceId` (optional): Filter by service they provide
- `gender` (optional): Filter by gender (Male, Female, Other, PreferNotToSay)

**Example:**
```bash
# All providers
curl https://your-app.railway.app/api/tenants/{tenantId}/providers

# Providers who offer specific service
curl https://your-app.railway.app/api/tenants/{tenantId}/providers?serviceId={serviceId}

# Female providers only
curl https://your-app.railway.app/api/tenants/{tenantId}/providers?gender=Female
```

**Response:**
```json
[
  {
    "id": "uuid",
    "tenantId": "uuid",
    "firstName": "Sarah",
    "lastName": "Johnson",
    "email": "sarah.johnson@blitzprive.com",
    "phone": "+1-555-0101",
    "bio": "Certified massage therapist with 10+ years of experience",
    "photoUrl": null,
    "gender": "Female",
    "isActive": true,
    "serviceIds": ["uuid1", "uuid2", "uuid3"]
  }
]
```

### Get Provider by ID
```http
GET /api/tenants/{tenantId}/providers/{providerId}
```

### Get Provider's Availability Schedule
```http
GET /api/tenants/{tenantId}/providers/{providerId}/availability
```

**Response:**
```json
[
  {
    "id": "uuid",
    "serviceProviderId": "uuid",
    "dayOfWeek": "Monday",
    "startTime": "09:00:00",
    "endTime": "17:00:00",
    "isAvailable": true,
    "specificDate": null
  },
  {
    "id": "uuid",
    "serviceProviderId": "uuid",
    "dayOfWeek": "Tuesday",
    "startTime": "09:00:00",
    "endTime": "17:00:00",
    "isAvailable": true,
    "specificDate": null
  }
]
```

### Get Available Time Slots
```http
GET /api/tenants/{tenantId}/providers/{providerId}/timeslots
```

**Query Parameters:**
- `date` (required): Date to check (YYYY-MM-DD)
- `durationMinutes` (optional, default: 60): Service duration

**Example:**
```bash
curl "https://your-app.railway.app/api/tenants/{tenantId}/providers/{providerId}/timeslots?date=2025-11-10&durationMinutes=60"
```

**Response:**
```json
[
  {
    "startTime": "2025-11-10T09:00:00",
    "endTime": "2025-11-10T10:00:00",
    "isAvailable": true,
    "providerId": "uuid"
  },
  {
    "startTime": "2025-11-10T09:30:00",
    "endTime": "2025-11-10T10:30:00",
    "isAvailable": true,
    "providerId": "uuid"
  },
  {
    "startTime": "2025-11-10T10:00:00",
    "endTime": "2025-11-10T11:00:00",
    "isAvailable": false,
    "providerId": "uuid"
  }
]
```

---

## 🏥 Health Check

### Get API Health Status
```http
GET /api/health
```

**Response:**
```json
{
  "status": "healthy",
  "timestamp": "2025-11-05T22:00:00Z",
  "service": "AppointmentApp API",
  "platform": "Multi-Tenant Appointment Scheduling",
  "version": "1.0.0"
}
```

---

## 📖 Common Usage Flows

### Flow 1: Browse Services for Blitz Prive

```bash
# 1. Get Blitz Prive tenant
curl https://your-app.railway.app/api/tenants/subdomain/blitzprive

# 2. Get service categories
curl https://your-app.railway.app/api/tenants/{tenantId}/categories

# 3. Get services in Massage category
curl https://your-app.railway.app/api/tenants/{tenantId}/services?categoryId={massageCategoryId}

# 4. Get providers for Swedish Massage
curl https://your-app.railway.app/api/tenants/{tenantId}/providers?serviceId={swedishMassageId}

# 5. Get available time slots for Sarah Johnson on Nov 10
curl "https://your-app.railway.app/api/tenants/{tenantId}/providers/{sarahId}/timeslots?date=2025-11-10&durationMinutes=60"
```

### Flow 2: Find Female Massage Therapists

```bash
# Get Blitz Prive tenant
curl https://your-app.railway.app/api/tenants/subdomain/blitzprive

# Get female providers
curl "https://your-app.railway.app/api/tenants/{tenantId}/providers?gender=Female"

# Check availability for specific provider
curl "https://your-app.railway.app/api/tenants/{tenantId}/providers/{providerId}/timeslots?date=2025-11-10"
```

---

## 🔜 Coming Soon (Phase 2)

- **Authentication API**: `/api/auth/register`, `/api/auth/login`
- **Appointments API**: `/api/appointments` (create, view, cancel)
- **Custom Forms API**: `/api/tenants/{tenantId}/custom-forms`
- **Consent Forms API**: `/api/tenants/{tenantId}/consent-forms`

---

## ❌ Error Responses

All errors return this format:

```json
{
  "message": "Error description"
}
```

**HTTP Status Codes:**
- `200 OK`: Success
- `404 Not Found`: Resource not found
- `400 Bad Request`: Invalid parameters
- `500 Internal Server Error`: Server error

---

## 🧪 Testing with Swagger

Once deployed, visit:
```
https://your-app.railway.app/swagger
```

Swagger UI provides:
- Interactive API testing
- Request/response examples
- Parameter documentation
- Try-it-out functionality

---

## 💡 Tips

1. **Save Tenant ID**: After getting tenant by subdomain, save the ID for subsequent calls
2. **Check Availability First**: Before showing time slots, verify provider availability schedule
3. **Time Zones**: All times are in UTC, convert to user's timezone on frontend
4. **Pagination**: Will be added in future for large datasets
5. **Rate Limiting**: Not currently implemented, will be added in production

---

## 📞 Support

For API issues or questions:
- Check Swagger documentation
- Review Railway logs
- Test with curl or Postman first
