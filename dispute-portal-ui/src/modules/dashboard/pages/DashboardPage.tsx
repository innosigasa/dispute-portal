import Box from '@mui/joy/Box'
import Typography from '@mui/joy/Typography'
import CircularProgress from '@mui/joy/CircularProgress'
import { useEffect, useState } from 'react'
import toast from 'react-hot-toast'
import { getMyAccounts } from '../../accounts/services/accounts.service'
import { getDisputeSummaryStats } from '../../disputes/services/disputes.service'
import type { BankAccount } from '../../accounts/models/account.model'
import type { DisputeSummaryStats } from '../../disputes/models/dispute.model'
import { AccountCard } from '../components/AccountCard'
import { DisputeStatusSummaryWidget } from '../components/DisputeStatusSummaryWidget'

export function DashboardPage() {
  const [accounts, setAccounts] = useState<BankAccount[]>([])
  const [stats, setStats] = useState<DisputeSummaryStats | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    Promise.all([getMyAccounts(), getDisputeSummaryStats()])
      .then(([accountsRes, statsRes]) => {
        if (accountsRes.isSuccessful && accountsRes.data) setAccounts(accountsRes.data)
        else toast.error('Could not load accounts.')
        if (statsRes.isSuccessful && statsRes.data) setStats(statsRes.data)
      })
      .finally(() => setLoading(false))
  }, [])

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', pt: 10 }}>
        <CircularProgress />
      </Box>
    )
  }

  return (
    <Box>
      <Box sx={{ mb: 3 }}>
        <Typography level="h2" fontWeight={800}>My Dashboard</Typography>
        <Typography level="body-sm" textColor="neutral.500" mt={0.5}>
          Overview of your accounts and disputes
        </Typography>
      </Box>

      {/* Account Cards */}
      <Typography level="title-sm" textColor="neutral.500" fontWeight={700} sx={{ mb: 1.5, textTransform: 'uppercase', letterSpacing: '0.06em', fontSize: '0.7rem' }}>
        My Accounts
      </Typography>
      <Box
        sx={{
          display: 'flex',
          gap: 2,
          overflowX: 'auto',
          pb: 1,
          mb: 3,
          '&::-webkit-scrollbar': { height: 4 },
          '&::-webkit-scrollbar-thumb': { bgcolor: 'neutral.200', borderRadius: 2 },
        }}
      >
        {accounts.map(account => (
          <AccountCard key={account.id} account={account} />
        ))}
      </Box>

      {/* Bottom row */}
      <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', md: '1fr 1fr' }, gap: 2 }}>
        <DisputeStatusSummaryWidget stats={stats} loading={false} />

        {/* Quick links */}
        <Box
          sx={{
            p: 2.5,
            borderRadius: '12px',
            border: '1px solid',
            borderColor: 'neutral.100',
            bgcolor: 'background.surface',
            display: 'flex',
            flexDirection: 'column',
            gap: 1,
          }}
        >
          <Typography level="title-md" fontWeight={700} mb={1}>Quick Links</Typography>
          {[
            { label: 'All Transactions', href: '/transactions' },
            { label: 'My Dispute History', href: '/disputes' },
          ].map(link => (
            <Box
              key={link.href}
              component="a"
              href={link.href}
              sx={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                p: 1.5,
                borderRadius: '8px',
                textDecoration: 'none',
                color: 'text.primary',
                '&:hover': { bgcolor: 'neutral.50' },
              }}
            >
              <Typography level="body-md">{link.label}</Typography>
              <Typography level="body-sm" textColor="neutral.400">→</Typography>
            </Box>
          ))}
        </Box>
      </Box>
    </Box>
  )
}
