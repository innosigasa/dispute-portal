import Box from '@mui/joy/Box'
import Button from '@mui/joy/Button'
import Chip from '@mui/joy/Chip'
import Typography from '@mui/joy/Typography'
import Sheet from '@mui/joy/Sheet'
import DashboardIcon from '@mui/icons-material/Dashboard'
import ReceiptLongIcon from '@mui/icons-material/ReceiptLong'
import GavelIcon from '@mui/icons-material/Gavel'
import AccountBalanceIcon from '@mui/icons-material/AccountBalance'
import LogoutIcon from '@mui/icons-material/Logout'
import { Link, Outlet, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../store/authStore'

export function AppLayout() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()

  const handleLogout = async () => {
    await logout()
    navigate('/login')
  }

  const navLinks = user?.role === 'agent'
    ? [{ to: '/agent/disputes', label: 'Dispute Queue', icon: <GavelIcon sx={{ fontSize: 18 }} /> }]
    : [
        { to: '/dashboard',    label: 'Dashboard',    icon: <DashboardIcon sx={{ fontSize: 18 }} /> },
        { to: '/transactions', label: 'Transactions', icon: <ReceiptLongIcon sx={{ fontSize: 18 }} /> },
        { to: '/disputes',     label: 'My Disputes',  icon: <GavelIcon sx={{ fontSize: 18 }} /> },
      ]

  return (
    <Box sx={{ minHeight: '100vh', bgcolor: '#F4F6F9' }}>
      <Sheet
        component="header"
        sx={{
          px: 3,
          display: 'flex',
          alignItems: 'stretch',
          borderBottom: '1px solid',
          borderColor: 'neutral.100',
          position: 'sticky',
          top: 0,
          zIndex: 100,
          bgcolor: '#fff',
          boxShadow: '0 1px 4px rgba(0,0,0,0.06)',
          height: 56,
        }}
      >
        {/* Logo */}
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mr: 4, flexShrink: 0 }}>
          <AccountBalanceIcon sx={{ color: 'primary.600', fontSize: 22 }} />
          <Typography level="title-md" fontWeight={800} sx={{ color: 'primary.900', letterSpacing: '-0.01em' }}>
            DisputePortal
          </Typography>
        </Box>

        {/* Nav links — use Link + useLocation, never NavLink with height */}
        <Box component="nav" sx={{ display: 'flex', flex: 1, alignItems: 'stretch', gap: 0 }}>
          {navLinks.map(link => {
            const isActive = location.pathname === link.to ||
              (link.to !== '/dashboard' && location.pathname.startsWith(link.to))
            return (
              <Link
                key={link.to}
                to={link.to}
                style={{ textDecoration: 'none', display: 'flex', alignItems: 'center' }}
              >
                <Box
                  sx={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: 0.75,
                    px: 2,
                    height: 56,
                    borderBottom: '2px solid',
                    borderColor: isActive ? 'primary.600' : 'transparent',
                    color: isActive ? 'primary.700' : 'neutral.600',
                    fontWeight: isActive ? 700 : 500,
                    fontSize: '0.875rem',
                    whiteSpace: 'nowrap',
                    transition: 'color 0.15s ease, border-color 0.15s ease, background-color 0.15s ease',
                    '&:hover': { color: 'primary.700', bgcolor: 'neutral.50' },
                  }}
                >
                  {link.icon}
                  {link.label}
                </Box>
              </Link>
            )
          })}
        </Box>

        {/* Right: role badge + logout */}
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, flexShrink: 0 }}>
          <Chip
            size="sm"
            color={user?.role === 'agent' ? 'warning' : 'primary'}
            variant="soft"
            sx={{ fontWeight: 700, textTransform: 'capitalize' }}
          >
            {user?.role ?? 'User'}
          </Chip>
          <Button
            size="sm"
            variant="plain"
            color="neutral"
            startDecorator={<LogoutIcon sx={{ fontSize: 16 }} />}
            onClick={handleLogout}
            sx={{ fontWeight: 500 }}
          >
            Logout
          </Button>
        </Box>
      </Sheet>

      <Box component="main" sx={{ maxWidth: 1400, mx: 'auto', px: 3, py: 3 }}>
        <Outlet />
      </Box>
    </Box>
  )
}
