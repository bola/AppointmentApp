# Authentication & Authorization Guide

Complete guide to authentication in AppointmentApp API.

## 🔐 Authentication Flow

AppointmentApp uses **JWT (JSON Web Tokens)** for secure authentication.

### Registration Flow

```
1. Customer submits registration form
   ↓
2. API validates username and email uniqueness
   ↓
3. API hashes password with BCrypt
   ↓
4. Customer record created in database
   ↓
5. JWT token generated and returned
   ↓
6. Frontend stores token (localStorage/AsyncStorage)
```

### Login Flow

```
1. Customer submits username/email + password
   ↓
2. API finds customer by username OR email
   ↓
3. API verifies password with BCrypt
   ↓
4. JWT token generated and returned
   ↓
5. Frontend stores token
```

### Protected API Calls

```
1. Frontend includes token in Authorization header
   ↓
2. API validates token signature and expiration
   ↓
3. API extracts customer ID from token claims
   ↓
4. API processes request with customer context
```

---

## 🔑 API Endpoints

### Register New Customer

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1-555-1234"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2025-11-06T22:00:00Z",
  "customer": {
    "id": "uuid",
    "username": "johndoe",
    "email": "john@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "phone": "+1-555-1234",
    "emailVerified": false
  }
}
```

**Validation Rules:**
- Username: 3-100 characters, required, unique
- Email: Valid email format, required, unique
- Password: Minimum 8 characters, required
- FirstName/LastName: Optional, max 100 characters
- Phone: Optional, valid phone format

**Error Responses:**
```json
// 400 - Username exists
{
  "message": "Username already exists"
}

// 400 - Email exists
{
  "message": "Email already exists"
}

// 400 - Validation failed
{
  "username": ["Username is required"],
  "password": ["Password must be at least 8 characters"]
}
```

---

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "usernameOrEmail": "johndoe",
  "password": "SecurePass123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2025-11-06T22:00:00Z",
  "customer": {
    "id": "uuid",
    "username": "johndoe",
    "email": "john@example.com",
    "firstName": "John",
    "lastName": "Doe"
  }
}
```

**Error Responses:**
```json
// 401 - Invalid credentials
{
  "message": "Invalid credentials"
}

// 401 - Account inactive
{
  "message": "Account is inactive"
}
```

---

### Get Current Customer Profile

```http
GET /api/auth/me
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "id": "uuid",
  "username": "johndoe",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1-555-1234",
  "emailVerified": false
}
```

**Error Responses:**
```json
// 401 - No token or invalid token
{
  "message": "Invalid token"
}

// 404 - Customer not found
{
  "message": "Customer not found"
}
```

---

## 🎫 JWT Token Details

### Token Structure

```
Header:
{
  "alg": "HS256",
  "typ": "JWT"
}

Payload (Claims):
{
  "sub": "customer-uuid",          // Customer ID
  "email": "john@example.com",     // Customer email
  "unique_name": "johndoe",        // Username
  "jti": "token-uuid",             // Unique token ID
  "customerId": "customer-uuid",   // Custom claim for easy access
  "exp": 1730844000,               // Expiration timestamp
  "iss": "AppointmentApp",         // Issuer
  "aud": "AppointmentApp"          // Audience
}

Signature:
HMACSHA256(
  base64UrlEncode(header) + "." + base64UrlEncode(payload),
  secret
)
```

### Token Lifetime

- **Expiration:** 24 hours from generation
- **Clock Skew:** 0 (no tolerance for expired tokens)
- **Refresh:** Not implemented (re-login required after expiration)

---

## 🛡️ Using Authentication in Frontend

### Web (Next.js)

```typescript
// lib/auth.ts
export async function register(data: RegisterRequest) {
  const response = await fetch('/api/auth/register', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data)
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message)
  }

  const authResponse = await response.json()

  // Store token
  localStorage.setItem('token', authResponse.token)
  localStorage.setItem('customer', JSON.stringify(authResponse.customer))

  return authResponse
}

export async function login(usernameOrEmail: string, password: string) {
  const response = await fetch('/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ usernameOrEmail, password })
  })

  if (!response.ok) {
    const error = await response.json()
    throw new Error(error.message)
  }

  const authResponse = await response.json()
  localStorage.setItem('token', authResponse.token)

  return authResponse
}

export function getToken() {
  return localStorage.getItem('token')
}

export function logout() {
  localStorage.removeItem('token')
  localStorage.removeItem('customer')
}

// Make authenticated API calls
export async function fetchProtected(url: string) {
  const token = getToken()

  const response = await fetch(url, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  })

  return response.json()
}
```

### Mobile (React Native)

```typescript
// lib/auth.ts
import AsyncStorage from '@react-native-async-storage/async-storage'

export async function register(data: RegisterRequest) {
  const response = await fetch(`${API_URL}/auth/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data)
  })

  const authResponse = await response.json()

  // Store token
  await AsyncStorage.setItem('token', authResponse.token)
  await AsyncStorage.setItem('customer', JSON.stringify(authResponse.customer))

  return authResponse
}

export async function getToken() {
  return await AsyncStorage.getItem('token')
}

export async function logout() {
  await AsyncStorage.removeItem('token')
  await AsyncStorage.removeItem('customer')
}
```

---

## 📋 Protected Endpoints

### Create Appointment

```http
POST /api/appointments
Authorization: Bearer {token}
Content-Type: application/json

{
  "tenantId": "uuid",
  "scheduledDate": "2025-11-10",
  "scheduledTime": "10:00:00",
  "notes": "Please bring portable table",
  "consentGiven": true,
  "services": [
    {
      "serviceId": "uuid",
      "serviceProviderId": "uuid",
      "preferredGender": null
    }
  ]
}
```

### Get My Appointments

```http
GET /api/appointments/my-appointments
Authorization: Bearer {token}
```

**Optional Query Parameters:**
- `status`: Filter by appointment status (Pending, Confirmed, Completed, Cancelled)

```http
GET /api/appointments/my-appointments?status=Pending
```

### Get Appointment by ID

```http
GET /api/appointments/{id}
Authorization: Bearer {token}
```

**Note:** Customers can only view their own appointments.

### Cancel Appointment

```http
PATCH /api/appointments/{id}/cancel
Authorization: Bearer {token}
```

---

## 🔒 Security Best Practices

### Backend

✅ **Implemented:**
- Password hashing with BCrypt (10 rounds)
- JWT signature verification
- Token expiration validation
- HTTPS recommended in production
- Unique username and email constraints
- Soft delete for customers

⚠️ **To Do (Production):**
- [ ] Rate limiting on auth endpoints
- [ ] Email verification
- [ ] Password reset functionality
- [ ] Refresh tokens
- [ ] Account lockout after failed attempts
- [ ] HTTPS enforcement
- [ ] Change JWT secret in production

### Frontend

✅ **Recommended:**
- Store tokens securely (localStorage for web, AsyncStorage for mobile)
- Clear tokens on logout
- Handle 401 responses (redirect to login)
- Don't expose tokens in URLs
- Use HTTPS in production

---

## 🧪 Testing Authentication

### Register & Login Flow

```bash
# 1. Register new customer
curl -X POST https://your-app.railway.app/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test123456!",
    "firstName": "Test",
    "lastName": "User"
  }'

# Save the token from response
export TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# 2. Get current user profile
curl -X GET https://your-app.railway.app/api/auth/me \
  -H "Authorization: Bearer $TOKEN"

# 3. Create appointment (authenticated)
curl -X POST https://your-app.railway.app/api/appointments \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "tenantId": "your-tenant-id",
    "scheduledDate": "2025-11-15",
    "scheduledTime": "10:00:00",
    "consentGiven": true,
    "services": [
      {
        "serviceId": "your-service-id",
        "serviceProviderId": "your-provider-id"
      }
    ]
  }'

# 4. View my appointments
curl -X GET https://your-app.railway.app/api/appointments/my-appointments \
  -H "Authorization: Bearer $TOKEN"

# 5. Login (if token expires)
curl -X POST https://your-app.railway.app/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "usernameOrEmail": "testuser",
    "password": "Test123456!"
  }'
```

---

## 🚨 Error Handling

### Common Errors

| Status | Error | Cause | Solution |
|--------|-------|-------|----------|
| 400 | Username already exists | Duplicate username | Use different username |
| 400 | Email already exists | Duplicate email | Use different email |
| 401 | Invalid credentials | Wrong password or user not found | Check credentials |
| 401 | Account is inactive | Account disabled | Contact support |
| 401 | Invalid token | Token missing, expired, or invalid | Re-login |
| 404 | Customer not found | Customer deleted or doesn't exist | Re-register |

---

## 🔧 Configuration

### Railway Environment Variables

Add these to your Railway backend service:

```bash
Jwt__Secret=Your-Super-Secret-Key-Min-32-Characters-Change-Me
Jwt__Issuer=AppointmentApp
Jwt__Audience=AppointmentApp
```

**IMPORTANT:** Change the JWT secret in production to a random, secure value!

### Generate Secure JWT Secret

```bash
# Using OpenSSL
openssl rand -base64 64

# Using Node.js
node -e "console.log(require('crypto').randomBytes(64).toString('base64'))"
```

---

## 📊 Token Claims Reference

| Claim | Description | Example |
|-------|-------------|---------|
| `sub` | Subject (Customer ID) | `"3fa85f64-5717-4562-b3fc-2c963f66afa6"` |
| `email` | Customer email | `"john@example.com"` |
| `unique_name` | Username | `"johndoe"` |
| `jti` | JWT ID (unique token identifier) | `"8f7e1234-abcd-..."` |
| `customerId` | Custom claim for easy access | `"3fa85f64-..."` |
| `exp` | Expiration timestamp | `1730844000` |
| `iss` | Issuer | `"AppointmentApp"` |
| `aud` | Audience | `"AppointmentApp"` |

---

## 💡 Tips

1. **Token Expiration**: Tokens expire after 24 hours. Handle 401 responses by redirecting to login.
2. **Password Requirements**: Enforce strong passwords on frontend (min 8 chars, mix of letters/numbers/symbols)
3. **Email Verification**: Currently not implemented. Users can login immediately after registration.
4. **Remember Me**: Not implemented. All sessions last 24 hours.
5. **Multi-Device**: Same token can be used across multiple devices (web + mobile).

---

## 🔜 Future Enhancements

- [ ] Email verification
- [ ] Password reset via email
- [ ] Refresh tokens
- [ ] Social login (Google, Facebook)
- [ ] Two-factor authentication (2FA)
- [ ] Session management
- [ ] Remember me functionality
- [ ] Account lockout protection
