import { useState, type ReactNode } from 'react'
import { AuthContext, parseJwt } from './authStore'
import type { AuthUser } from '../modules/auth/models/auth-user.model'
import { logout as apiLogout } from '../modules/auth/services/auth.service'

function readStoredUser(): AuthUser | null {
  const token = localStorage.getItem('accessToken')
  if (!token) return null
  const payload = parseJwt(token)
  return {
    userId: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] as string ?? payload.sub as string ?? '',
    role: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] as 'customer' | 'agent' ?? 'customer',
    customerId: payload['customerId'] as string | undefined,
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(readStoredUser)
  const [accessToken, setAccessToken] = useState<string | null>(() => localStorage.getItem('accessToken'))

  const login = (at: string, rt: string, _role: string) => {
    localStorage.setItem('accessToken', at)
    localStorage.setItem('refreshToken', rt)
    setAccessToken(at)
    setUser(readStoredUser())
  }

  const logout = async () => {
    const rt = localStorage.getItem('refreshToken') ?? ''
    await apiLogout(rt)
    localStorage.clear()
    setUser(null)
    setAccessToken(null)
  }

  return (
    <AuthContext.Provider value={{ user, accessToken, login, logout, isAuthenticated: !!user }}>
      {children}
    </AuthContext.Provider>
  )
}
