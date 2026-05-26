import { Navigate, Route, Routes } from 'react-router-dom'
import { AppLayout } from './layouts/AppLayout'
import { AuthLayout } from './layouts/AuthLayout'
import { ProtectedRoute } from './components/ProtectedRoute'
import { LoginPage } from './modules/auth/pages/LoginPage'
import { DashboardPage } from './modules/dashboard/pages/DashboardPage'
import { TransactionsPage } from './modules/transactions/pages/TransactionsPage'
import { MyDisputesPage } from './modules/disputes/pages/MyDisputesPage'
import { DisputeDetailPage } from './modules/disputes/pages/DisputeDetailPage'
import { AgentDisputeQueuePage } from './modules/agent/pages/AgentDisputeQueuePage'
import { AgentDisputeDetailPage } from './modules/agent/pages/AgentDisputeDetailPage'
import { useAuth } from './store/authStore'

function RootRedirect() {
  const { user, isAuthenticated } = useAuth()
  if (!isAuthenticated) return <Navigate to="/login" replace />
  return <Navigate to={user?.role === 'agent' ? '/agent/disputes' : '/dashboard'} replace />
}

export default function App() {
  return (
    <Routes>
      <Route element={<AuthLayout />}>
        <Route path="/login" element={<LoginPage />} />
      </Route>
      <Route element={<ProtectedRoute role="customer"><AppLayout /></ProtectedRoute>}>
        <Route path="/dashboard" element={<DashboardPage />} />
        <Route path="/transactions" element={<TransactionsPage />} />
        <Route path="/disputes" element={<MyDisputesPage />} />
        <Route path="/disputes/:id" element={<DisputeDetailPage />} />
      </Route>
      <Route element={<ProtectedRoute role="agent"><AppLayout /></ProtectedRoute>}>
        <Route path="/agent/disputes" element={<AgentDisputeQueuePage />} />
        <Route path="/agent/disputes/:id" element={<AgentDisputeDetailPage />} />
      </Route>
      <Route path="/" element={<RootRedirect />} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
