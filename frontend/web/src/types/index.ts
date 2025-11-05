export interface Tenant {
  id: string
  name: string
  subdomain: string
  logo?: string
  primaryColor?: string
  secondaryColor?: string
  contactEmail: string
  contactPhone?: string
  isActive: boolean
}

export interface ServiceCategory {
  id: string
  tenantId: string
  name: string
  description?: string
  icon?: string
  displayOrder: number
}

export interface Service {
  id: string
  tenantId: string
  categoryId: string
  name: string
  description?: string
  price: number
  durationMinutes: number
  imageUrl?: string
  allowProviderSelection: boolean
  allowGenderPreference: boolean
  isActive: boolean
}

export enum Gender {
  Male = 'Male',
  Female = 'Female',
  Other = 'Other',
  PreferNotToSay = 'PreferNotToSay'
}

export interface ServiceProvider {
  id: string
  tenantId: string
  firstName: string
  lastName: string
  email: string
  phone?: string
  bio?: string
  photoUrl?: string
  gender: Gender
  isActive: boolean
}

export interface ProviderAvailability {
  id: string
  serviceProviderId: string
  dayOfWeek: number
  startTime: string
  endTime: string
  isAvailable: boolean
  specificDate?: string
}

export interface TimeSlot {
  startTime: string
  endTime: string
  isAvailable: boolean
  providerId?: string
}

export interface Customer {
  id: string
  username: string
  email: string
  firstName?: string
  lastName?: string
  phone?: string
  dateOfBirth?: string
  gender?: Gender
}

export interface Appointment {
  id: string
  tenantId: string
  customerId: string
  scheduledDate: string
  scheduledTime: string
  status: AppointmentStatus
  totalPrice: number
  totalDurationMinutes: number
  notes?: string
  consentGiven: boolean
  services: AppointmentService[]
}

export enum AppointmentStatus {
  Pending = 'Pending',
  Confirmed = 'Confirmed',
  InProgress = 'InProgress',
  Completed = 'Completed',
  Cancelled = 'Cancelled',
  NoShow = 'NoShow'
}

export interface AppointmentService {
  id: string
  appointmentId: string
  serviceId: string
  serviceProviderId?: string
  preferredGender?: Gender
  startTime: string
  endTime: string
  price: number
  service?: Service
  serviceProvider?: ServiceProvider
}

export interface CustomFormField {
  id: string
  tenantId: string
  fieldName: string
  label: string
  fieldType: FormFieldType
  isRequired: boolean
  placeholder?: string
  validationRules?: string
  options?: string
  displayOrder: number
  isActive: boolean
}

export enum FormFieldType {
  Text = 'Text',
  Email = 'Email',
  Phone = 'Phone',
  Number = 'Number',
  Date = 'Date',
  TextArea = 'TextArea',
  Dropdown = 'Dropdown',
  Radio = 'Radio',
  Checkbox = 'Checkbox',
  File = 'File'
}

export interface ConsentForm {
  id: string
  tenantId: string
  title: string
  content: string
  version: string
  isRequired: boolean
  isActive: boolean
  displayOrder: number
}

export interface BookingRequest {
  tenantId: string
  services: {
    serviceId: string
    providerId?: string
    preferredGender?: Gender
  }[]
  scheduledDate: string
  scheduledTime: string
  customer: {
    username: string
    password: string
    email: string
    firstName?: string
    lastName?: string
    phone?: string
  }
  customFormData?: Record<string, any>
  consentFormIds: string[]
}
