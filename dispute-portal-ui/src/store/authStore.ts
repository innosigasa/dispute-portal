import { createContext, useContext } from 'react'
import type { AuthUser } from '../modules/auth/models/auth-user.model'

export interface AuthState {
  user: AuthUser | null
  accessToken: string | null
  login: (accessToken: string, refreshToken: string, role: string) => void
  logout: () => void
  isAuthenticated: boolean
}

export const AuthContext = createContext<AuthState | null>(null)

export const useAuth = () => {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}

export function parseJwt(token: string): Record<string, unknown> {
  try {
    const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    return JSON.parse(atob(base64))
  } catch {
    return {}
  }
}
