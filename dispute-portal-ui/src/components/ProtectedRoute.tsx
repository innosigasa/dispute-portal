import { Navigate } from 'react-router-dom'
import { useAuth } from '../store/authStore'
import type { ReactNode } from 'react'

interface Props {
  role?: 'customer' | 'agent'
  children: ReactNode
}

export function ProtectedRoute({ role, children }: Props) {
  const { isAuthenticated, user } = useAuth()
  if (!isAuthenticated) return <Navigate to="/login" replace />
  if (role && user?.role !== role) {
    return <Navigate to={user?.role === 'agent' ? '/agent/disputes' : '/transactions'} replace />
  }
  return <>{children}</>
}
