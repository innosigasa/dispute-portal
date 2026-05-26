export interface AuthUser {
  userId: string
  role: 'customer' | 'agent'
  customerId?: string
}
