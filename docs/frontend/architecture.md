# Frontend Architecture

This document describes the architecture of the Blitz Prevair frontend applications.

## Overview

The application consists of two frontend platforms:
- **Web**: Next.js 14 with App Router (TypeScript + Tailwind CSS)
- **Mobile**: React Native with Expo (TypeScript + Native styling)

## Shared Architecture Principles

Both platforms follow similar patterns for consistency:

1. **Component-based architecture**: Reusable UI components
2. **API layer abstraction**: Centralized API calls
3. **State management**: React hooks and Zustand for global state
4. **Type safety**: TypeScript throughout
5. **Form handling**: react-hook-form for validation

## Web Application (Next.js)

### Directory Structure

```
frontend/web/
├── src/
│   ├── app/                    # Next.js App Router pages
│   │   ├── layout.tsx         # Root layout
│   │   ├── page.tsx           # Home page
│   │   ├── services/          # Services pages
│   │   ├── booking/           # Booking flow
│   │   └── auth/              # Authentication
│   ├── components/            # Reusable components
│   │   ├── ui/               # Base UI components
│   │   ├── forms/            # Form components
│   │   └── booking/          # Booking-specific
│   ├── lib/                  # Utilities
│   │   ├── api.ts           # API client
│   │   ├── utils.ts         # Helper functions
│   │   └── hooks.ts         # Custom hooks
│   ├── types/               # TypeScript types
│   └── styles/              # Global styles
├── public/                  # Static assets
└── package.json
```

### Key Features

#### 1. Server-Side Rendering (SSR)
Next.js provides SSR for SEO and performance:

```typescript
// app/services/page.tsx
export default async function ServicesPage() {
  const services = await fetch(`${API_URL}/services`).then(r => r.json())
  return <ServicesList services={services} />
}
```

#### 2. API Routes (Optional)
Can add API routes for backend proxy:

```typescript
// app/api/health/route.ts
export async function GET() {
  return Response.json({ status: 'ok' })
}
```

#### 3. Client Components
Interactive components use 'use client':

```typescript
'use client'
import { useState } from 'react'

export function BookingForm() {
  const [date, setDate] = useState('')
  // ...
}
```

### State Management

**Local State**: React hooks (useState, useReducer)
**Global State**: Zustand

```typescript
// lib/store.ts
import { create } from 'zustand'

interface BookingState {
  selectedServices: Service[]
  addService: (service: Service) => void
  removeService: (id: string) => void
}

export const useBookingStore = create<BookingState>((set) => ({
  selectedServices: [],
  addService: (service) =>
    set((state) => ({
      selectedServices: [...state.selectedServices, service]
    })),
  removeService: (id) =>
    set((state) => ({
      selectedServices: state.selectedServices.filter(s => s.id !== id)
    })),
}))
```

### Booking Flow

The booking flow is a multi-step wizard:

1. **Select Services** (`/booking/services`)
2. **Choose Providers** (`/booking/providers`)
3. **Pick Time Slot** (`/booking/time`)
4. **Registration Form** (`/booking/register`)
5. **Consent Forms** (`/booking/consent`)
6. **Confirmation** (`/booking/confirm`)

Each step is a separate route with shared state.

## Mobile Application (React Native + Expo)

### Directory Structure

```
frontend/mobile/
├── app/                      # Expo Router screens
│   ├── _layout.tsx          # Root layout
│   ├── index.tsx            # Home screen
│   ├── services/            # Services screens
│   ├── booking/             # Booking flow
│   └── auth/                # Authentication
├── components/              # Reusable components
│   ├── ui/                 # Base UI components
│   ├── forms/              # Form components
│   └── booking/            # Booking-specific
├── lib/                    # Utilities
│   ├── api.ts             # API client
│   ├── storage.ts         # AsyncStorage wrapper
│   └── hooks.ts           # Custom hooks
├── types/                 # TypeScript types
└── assets/                # Images, fonts, etc.
```

### Key Features

#### 1. Expo Router
File-based routing similar to Next.js:

```typescript
// app/services/index.tsx
export default function ServicesScreen() {
  return (
    <View>
      <Text>Services</Text>
    </View>
  )
}
```

#### 2. Native Components
Uses React Native components:

```typescript
import { View, Text, TouchableOpacity, FlatList } from 'react-native'

export function ServicesList({ services }) {
  return (
    <FlatList
      data={services}
      renderItem={({ item }) => (
        <TouchableOpacity>
          <Text>{item.name}</Text>
        </TouchableOpacity>
      )}
    />
  )
}
```

#### 3. AsyncStorage
For persisting data locally:

```typescript
import AsyncStorage from '@react-native-async-storage/async-storage'

export async function saveToken(token: string) {
  await AsyncStorage.setItem('auth_token', token)
}

export async function getToken() {
  return await AsyncStorage.getItem('auth_token')
}
```

### Navigation

Expo Router provides type-safe navigation:

```typescript
import { useRouter } from 'expo-router'

function ServiceCard({ service }) {
  const router = useRouter()

  return (
    <TouchableOpacity
      onPress={() => router.push(`/services/${service.id}`)}
    >
      <Text>{service.name}</Text>
    </TouchableOpacity>
  )
}
```

## Shared Code

### Types
Both platforms share the same TypeScript types:

```typescript
// types/index.ts (shared)
export interface Service {
  id: string
  name: string
  price: number
  durationMinutes: number
  // ...
}
```

### API Client
Similar API client implementation:

```typescript
// lib/api.ts
import axios from 'axios'

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || process.env.API_URL,
})

// Interceptors for auth tokens
api.interceptors.request.use((config) => {
  const token = getToken() // Platform-specific
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export default api
```

## Forms and Validation

### react-hook-form
Used for form handling:

```typescript
import { useForm } from 'react-hook-form'

interface RegistrationForm {
  username: string
  email: string
  password: string
}

export function RegistrationForm() {
  const { register, handleSubmit, formState: { errors } } = useForm<RegistrationForm>()

  const onSubmit = async (data: RegistrationForm) => {
    await api.post('/auth/register', data)
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input {...register('username', { required: true })} />
      {errors.username && <span>Username is required</span>}
      {/* ... */}
    </form>
  )
}
```

## Styling

### Web (Tailwind CSS)
Utility-first CSS framework:

```typescript
<div className="container mx-auto px-4">
  <h1 className="text-3xl font-bold text-gray-900">
    Welcome
  </h1>
  <button className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700">
    Book Now
  </button>
</div>
```

### Mobile (StyleSheet)
React Native StyleSheet API:

```typescript
import { StyleSheet } from 'react-native'

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#1f2937',
  },
  button: {
    paddingVertical: 12,
    paddingHorizontal: 24,
    backgroundColor: '#2563eb',
    borderRadius: 8,
  },
})
```

## API Integration

### Data Fetching

**Web (Server Components):**
```typescript
// app/services/page.tsx
export default async function ServicesPage() {
  const services = await getServices()
  return <ServicesList services={services} />
}
```

**Web (Client Components) & Mobile:**
```typescript
import { useEffect, useState } from 'react'

export function useServices() {
  const [services, setServices] = useState<Service[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    api.get('/services')
      .then(res => setServices(res.data))
      .finally(() => setLoading(false))
  }, [])

  return { services, loading }
}
```

### Mutations
```typescript
export async function createAppointment(data: BookingRequest) {
  try {
    const response = await api.post('/appointments', data)
    return response.data
  } catch (error) {
    if (error.response?.status === 409) {
      throw new Error('Time slot no longer available')
    }
    throw error
  }
}
```

## Authentication Flow

1. **Login/Register**: User submits credentials
2. **Token Storage**: JWT saved in localStorage (web) / AsyncStorage (mobile)
3. **API Interceptor**: Token added to all requests
4. **Token Refresh**: Refresh token before expiry
5. **Logout**: Clear token and redirect

## Performance Optimization

### Web
- Next.js automatic code splitting
- Image optimization with next/image
- Static generation for public pages
- API response caching

### Mobile
- FlatList virtualization for long lists
- Image caching with expo-image
- Memoization with useMemo/useCallback
- Lazy loading screens

## Testing

### Unit Tests
```bash
# Web
cd frontend/web
npm test

# Mobile
cd frontend/mobile
npm test
```

### E2E Tests
- Web: Playwright or Cypress
- Mobile: Detox or Maestro

## Accessibility

Both platforms follow WCAG 2.1 guidelines:
- Semantic HTML (web) / AccessibilityInfo (mobile)
- Keyboard navigation (web) / Screen reader support (mobile)
- Color contrast ratios
- Focus indicators
- ARIA labels (web) / accessibility props (mobile)

## Internationalization (i18n)

Future support for multiple languages:
- next-intl (web)
- expo-localization (mobile)
