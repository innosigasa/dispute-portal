import Box from '@mui/joy/Box'
import Card from '@mui/joy/Card'
import CardContent from '@mui/joy/CardContent'
import Chip from '@mui/joy/Chip'
import Typography from '@mui/joy/Typography'
import Button from '@mui/joy/Button'
import SavingsIcon from '@mui/icons-material/Savings'
import AccountBalanceIcon from '@mui/icons-material/AccountBalance'
import CreditCardIcon from '@mui/icons-material/CreditCard'
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet'
import { useNavigate } from 'react-router-dom'
import type { BankAccount, AccountType } from '../../../modules/accounts/models/account.model'
import { formatCurrency } from '../../../utils/formatters'

const accountTypeConfig: Record<AccountType, {
  icon: React.ReactNode
  color: 'primary' | 'success' | 'warning' | 'neutral'
  bg: string
}> = {
  Savings:         { icon: <SavingsIcon />,              color: 'success', bg: 'linear-gradient(135deg, #1B998B 0%, #0d6b60 100%)' },
  Cheque:          { icon: <AccountBalanceIcon />,       color: 'primary', bg: 'linear-gradient(135deg, #0A2463 0%, #1e4491 100%)' },
  Current:         { icon: <AccountBalanceWalletIcon />, color: 'warning', bg: 'linear-gradient(135deg, #e07b00 0%, #b85e00 100%)' },
  Credit:          { icon: <CreditCardIcon />,           color: 'neutral', bg: 'linear-gradient(135deg, #4e5f74 0%, #364455 100%)' },
  Business:        { icon: <AccountBalanceIcon />,       color: 'primary', bg: 'linear-gradient(135deg, #16425B 0%, #1f6f8b 100%)' },
  FixedDeposit:    { icon: <SavingsIcon />,              color: 'success', bg: 'linear-gradient(135deg, #2A9D8F 0%, #1f776d 100%)' },
  Investment:      { icon: <SavingsIcon />,              color: 'success', bg: 'linear-gradient(135deg, #3A7D44 0%, #27562f 100%)' },
  Loan:            { icon: <CreditCardIcon />,           color: 'warning', bg: 'linear-gradient(135deg, #C97A00 0%, #8f5800 100%)' },
  Joint:           { icon: <AccountBalanceWalletIcon />, color: 'primary', bg: 'linear-gradient(135deg, #5C677D 0%, #3f4858 100%)' },
  ForeignCurrency: { icon: <AccountBalanceWalletIcon />, color: 'warning', bg: 'linear-gradient(135deg, #0081A7 0%, #005f7a 100%)' },
  Student:         { icon: <AccountBalanceWalletIcon />, color: 'success', bg: 'linear-gradient(135deg, #52B788 0%, #2d6a4f 100%)' },
  Retirement:      { icon: <SavingsIcon />,              color: 'neutral', bg: 'linear-gradient(135deg, #6C757D 0%, #495057 100%)' },
  MoneyMarket:     { icon: <SavingsIcon />,              color: 'success', bg: 'linear-gradient(135deg, #40916C 0%, #2d6a4f 100%)' },
  Islamic:         { icon: <AccountBalanceIcon />,       color: 'primary', bg: 'linear-gradient(135deg, #006D77 0%, #004f56 100%)' },
  Trust:           { icon: <AccountBalanceIcon />,       color: 'neutral', bg: 'linear-gradient(135deg, #7D8597 0%, #495057 100%)' },
  DigitalWallet:   { icon: <AccountBalanceWalletIcon />, color: 'warning', bg: 'linear-gradient(135deg, #9C6644 0%, #6b442d 100%)' },
  Corporate:       { icon: <AccountBalanceIcon />,       color: 'primary', bg: 'linear-gradient(135deg, #1D3557 0%, #16324a 100%)' },
  Offshore:        { icon: <AccountBalanceWalletIcon />, color: 'neutral', bg: 'linear-gradient(135deg, #3D5A80 0%, #293f59 100%)' },
}

function maskAccountNumber(num: string) {
  const parts = num.split('-')
  if (parts.length < 2) return `•••• ${num.slice(-4)}`
  return parts.slice(0, -1).map(() => '••••').join('-') + '-' + parts[parts.length - 1]
}

interface AccountCardProps {
  account: BankAccount
}

export function AccountCard({ account }: AccountCardProps) {
  const navigate = useNavigate()
  const config = accountTypeConfig[account.accountType] ?? accountTypeConfig.Savings
  const isNegative = account.balance < 0

  return (
    <Card
      sx={{
        minWidth: 260,
        maxWidth: 300,
        flex: '0 0 auto',
        background: config.bg,
        color: '#fff',
        transition: 'transform 0.18s ease, box-shadow 0.18s ease',
        '&:hover': { transform: 'translateY(-3px)', boxShadow: '0 8px 24px rgba(0,0,0,0.18)' },
        cursor: 'default',
      }}
    >
      <CardContent sx={{ gap: 0 }}>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
          <Box sx={{ opacity: 0.9, '& svg': { fontSize: 28 } }}>{config.icon}</Box>
          {account.isDefault && (
            <Chip size="sm" sx={{ bgcolor: 'rgba(255,255,255,0.2)', color: '#fff', fontWeight: 600, fontSize: '0.7rem' }}>
              Default
            </Chip>
          )}
        </Box>

        <Typography level="title-sm" sx={{ color: 'rgba(255,255,255,0.75)', mb: 0.25, letterSpacing: '0.05em', textTransform: 'uppercase', fontSize: '0.7rem' }}>
          {account.accountType} Account
        </Typography>
        <Typography level="title-md" sx={{ color: '#fff', fontWeight: 700, mb: 1.5 }}>
          {account.accountName}
        </Typography>

        <Typography level="body-xs" sx={{ color: 'rgba(255,255,255,0.6)', fontFamily: 'monospace', letterSpacing: '0.1em', mb: 0.5 }}>
          {maskAccountNumber(account.accountNumber)}
        </Typography>

        <Typography
          level="h3"
          sx={{ color: '#fff', fontWeight: 800, mt: 1, mb: 2, fontSize: '1.6rem' }}
        >
          <Typography component="span" sx={{ fontSize: '0.9rem', fontWeight: 500, opacity: 0.8, mr: 0.5 }}>
            {account.currency}
          </Typography>
          <Typography component="span" sx={{ color: isNegative ? '#ffb3b3' : '#fff' }}>
            {formatCurrency(Math.abs(account.balance)).replace('ZAR ', '').replace('ZAR ', '')}
          </Typography>
        </Typography>

        <Button
          size="sm"
          variant="outlined"
          sx={{
            borderColor: 'rgba(255,255,255,0.5)',
            color: '#fff',
            '&:hover': { bgcolor: 'rgba(255,255,255,0.15)', borderColor: '#fff' },
          }}
          onClick={() => navigate(`/transactions?accountId=${account.id}`)}
        >
          View Transactions
        </Button>
      </CardContent>
    </Card>
  )
}
