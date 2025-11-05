# API Documentation

This document describes the REST API endpoints for the Blitz Prevair application.

## Base URL

- Development: `http://localhost:5000/api`
- Production: `https://your-app.railway.app/api`

## Authentication

Most endpoints require JWT authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

## Endpoints

### Health Check

#### GET /api/health

Check API health status.

**Response:**
```json
{
  "status": "healthy",
  "timestamp": "2025-11-05T10:00:00Z",
  "service": "BlitzPrevair API",
  "version": "1.0.0"
}
```

### Tenants

#### GET /api/tenants

Get list of tenants.

**Query Parameters:**
- `page` (int): Page number (default: 1)
- `pageSize` (int): Items per page (default: 10)

**Response:**
```json
{
  "data": [
    {
      "id": "uuid",
      "name": "Wellness Co",
      "subdomain": "wellness",
      "logo": "https://...",
      "primaryColor": "#0066cc",
      "isActive": true
    }
  ],
  "totalCount": 100,
  "page": 1,
  "pageSize": 10
}
```

#### GET /api/tenants/{id}

Get tenant by ID.

#### GET /api/tenants/subdomain/{subdomain}

Get tenant by subdomain.

### Service Categories

#### GET /api/tenants/{tenantId}/categories

Get service categories for a tenant.

**Response:**
```json
[
  {
    "id": "uuid",
    "name": "Massage",
    "description": "Professional massage services",
    "icon": "massage-icon",
    "displayOrder": 1
  }
]
```

### Services

#### GET /api/tenants/{tenantId}/services

Get services for a tenant.

**Query Parameters:**
- `categoryId` (uuid): Filter by category
- `isActive` (bool): Filter by active status

**Response:**
```json
[
  {
    "id": "uuid",
    "name": "Swedish Massage",
    "description": "Relaxing full-body massage",
    "price": 120.00,
    "durationMinutes": 60,
    "imageUrl": "https://...",
    "allowProviderSelection": true,
    "allowGenderPreference": false,
    "categoryId": "uuid",
    "isActive": true
  }
]
```

#### GET /api/services/{id}

Get service by ID with full details including available providers.

### Service Providers

#### GET /api/tenants/{tenantId}/providers

Get service providers for a tenant.

**Query Parameters:**
- `serviceId` (uuid): Filter by service
- `gender` (string): Filter by gender

**Response:**
```json
[
  {
    "id": "uuid",
    "firstName": "Jane",
    "lastName": "Doe",
    "bio": "Certified massage therapist...",
    "photoUrl": "https://...",
    "gender": "Female",
    "services": ["uuid1", "uuid2"],
    "isActive": true
  }
]
```

### Availability

#### GET /api/providers/{providerId}/availability

Get provider availability for a date range.

**Query Parameters:**
- `startDate` (date): Start date (required)
- `endDate` (date): End date (required)
- `serviceId` (uuid): Service to book (required)

**Response:**
```json
{
  "2025-11-05": [
    {
      "startTime": "09:00:00",
      "endTime": "10:00:00",
      "isAvailable": true
    },
    {
      "startTime": "10:00:00",
      "endTime": "11:00:00",
      "isAvailable": false
    }
  ]
}
```

#### GET /api/tenants/{tenantId}/availability

Get all available time slots for multiple providers.

**Query Parameters:**
- `date` (date): Date to check
- `serviceId` (uuid): Service to book
- `gender` (string): Optional gender filter

### Authentication

#### POST /api/auth/register

Register a new customer.

**Request:**
```json
{
  "username": "johndoe",
  "password": "SecurePass123!",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890"
}
```

**Response:**
```json
{
  "id": "uuid",
  "username": "johndoe",
  "email": "john@example.com",
  "token": "jwt-token"
}
```

#### POST /api/auth/login

Login with username/email and password.

**Request:**
```json
{
  "usernameOrEmail": "johndoe",
  "password": "SecurePass123!"
}
```

**Response:**
```json
{
  "token": "jwt-token",
  "expiresAt": "2025-11-06T10:00:00Z",
  "customer": {
    "id": "uuid",
    "username": "johndoe",
    "email": "john@example.com",
    "firstName": "John",
    "lastName": "Doe"
  }
}
```

### Appointments

#### POST /api/appointments

Create a new appointment (requires authentication).

**Request:**
```json
{
  "tenantId": "uuid",
  "scheduledDate": "2025-11-10",
  "scheduledTime": "10:00:00",
  "services": [
    {
      "serviceId": "uuid",
      "providerId": "uuid",
      "preferredGender": null
    }
  ],
  "notes": "Please bring portable table",
  "customFormData": {
    "field1": "value1"
  },
  "consentFormIds": ["uuid1", "uuid2"]
}
```

**Response:**
```json
{
  "id": "uuid",
  "status": "Pending",
  "scheduledDate": "2025-11-10",
  "scheduledTime": "10:00:00",
  "totalPrice": 120.00,
  "totalDurationMinutes": 60,
  "services": [...]
}
```

#### GET /api/appointments/{id}

Get appointment details (requires authentication).

#### GET /api/customers/me/appointments

Get current customer's appointments (requires authentication).

**Query Parameters:**
- `status` (string): Filter by status
- `page` (int): Page number
- `pageSize` (int): Items per page

#### PATCH /api/appointments/{id}/cancel

Cancel an appointment (requires authentication).

### Custom Forms

#### GET /api/tenants/{tenantId}/custom-forms

Get custom form fields for a tenant.

**Response:**
```json
[
  {
    "id": "uuid",
    "fieldName": "allergies",
    "label": "Do you have any allergies?",
    "fieldType": "TextArea",
    "isRequired": false,
    "placeholder": "Please list any allergies...",
    "displayOrder": 1
  }
]
```

### Consent Forms

#### GET /api/tenants/{tenantId}/consent-forms

Get consent forms for a tenant.

**Response:**
```json
[
  {
    "id": "uuid",
    "title": "Terms and Conditions",
    "content": "<p>By using our service...</p>",
    "version": "1.0",
    "isRequired": true,
    "displayOrder": 1
  }
]
```

## Error Responses

All errors follow this format:

```json
{
  "error": {
    "code": "ERROR_CODE",
    "message": "Human-readable error message",
    "details": {
      "field": ["Validation error"]
    }
  }
}
```

### HTTP Status Codes

- `200 OK`: Success
- `201 Created`: Resource created
- `400 Bad Request`: Invalid request
- `401 Unauthorized`: Authentication required
- `403 Forbidden`: Insufficient permissions
- `404 Not Found`: Resource not found
- `409 Conflict`: Resource conflict (e.g., duplicate)
- `500 Internal Server Error`: Server error

## Rate Limiting

API requests are rate-limited to:
- 100 requests per minute for authenticated users
- 20 requests per minute for unauthenticated users

Rate limit headers:
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1636110000
```
