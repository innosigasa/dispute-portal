import Box from '@mui/joy/Box'
import Button from '@mui/joy/Button'
import Card from '@mui/joy/Card'
import Chip from '@mui/joy/Chip'
import CircularProgress from '@mui/joy/CircularProgress'
import Divider from '@mui/joy/Divider'
import Typography from '@mui/joy/Typography'
import AccountBalanceIcon from '@mui/icons-material/AccountBalance'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { getDisputeById } from '../services/disputes.service'
import type { DisputeDetail } from '../models/dispute.model'
import { StatusBadge } from '../../../components/StatusBadge'
import { DetailField } from '../../../components/DetailField'
import { formatCurrency, formatDate, formatDateTime } from '../../../utils/formatters'

const accountTypeColors: Record<string, 'primary' | 'success' | 'warning' | 'neutral'> = {
  Savings: 'success', Cheque: 'primary', Current: 'warning', Credit: 'neutral',
}

export function DisputeDetailPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const [dispute, setDispute] = useState<DisputeDetail | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    if (!id) return
    getDisputeById(id)
      .then(r => { if (r.isSuccessful) setDispute(r.data ?? null) })
      .finally(() => setLoading(false))
  }, [id])

  if (loading) return <Box sx={{ display: 'flex', justifyContent: 'center', pt: 8 }}><CircularProgress /></Box>
  if (!dispute) return <Typography>Dispute not found.</Typography>

  return (
    <Box>
      {/* Breadcrumb */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2.5 }}>
        <Button variant="plain" color="neutral" startDecorator={<ArrowBackIcon />} onClick={() => navigate('/disputes')} sx={{ px: 0 }}>
          My Disputes
        </Button>
        <Typography level="body-sm" textColor="neutral.400">/</Typography>
        <Typography level="body-sm" fontWeight={600}>{dispute.referenceNumber}</Typography>
      </Box>

      {/* Page header */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 3 }}>
        <Typography level="h3" fontWeight={800}>{dispute.referenceNumber}</Typography>
        <StatusBadge status={dispute.status} />
      </Box>

      <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', md: '1fr 1fr' }, gap: 2, mb: 2 }}>
        {/* Transaction details */}
        <Card>
          <Typography level="title-md" fontWeight={700} mb={1.5}>Transaction Details</Typography>
          <DetailField label="Description" value={dispute.transaction.description} />
          <DetailField label="Amount"      value={formatCurrency(dispute.transaction.amount)} />
          <DetailField label="Date"        value={formatDate(dispute.transaction.transactionDate)} />
          <DetailField label="Category"    value={dispute.transaction.category} />
          <DetailField label="Reference"   value={dispute.transaction.reference} />

          {/* Account section */}
          <Divider sx={{ my: 1.5 }} />
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
            <AccountBalanceIcon sx={{ fontSize: 16, color: 'neutral.500' }} />
            <Typography level="body-xs" fontWeight={700} textColor="neutral.500" sx={{ textTransform: 'uppercase', letterSpacing: '0.05em' }}>
              Account
            </Typography>
          </Box>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <Typography level="body-md" fontWeight={600}>{dispute.account.accountName}</Typography>
            <Chip size="sm" color={accountTypeColors[dispute.account.accountType] ?? 'neutral'} variant="soft">
              {dispute.account.accountType}
            </Chip>
          </Box>
          <Typography level="body-sm" textColor="neutral.400" sx={{ fontFamily: 'monospace', mt: 0.25 }}>
            {dispute.account.accountNumber}
          </Typography>
        </Card>

        {/* Dispute details */}
        <Card>
          <Typography level="title-md" fontWeight={700} mb={1.5}>Dispute Details</Typography>
          <DetailField label="Reason"    value={dispute.reason} />
          <DetailField label="Comments"  value={dispute.comments} />
          <DetailField label="Submitted" value={formatDate(dispute.submittedAt)} />
          {dispute.resolvedAt && <DetailField label="Resolved" value={formatDate(dispute.resolvedAt)} />}
        </Card>
      </Box>

      {/* Resolution notes */}
      {(dispute.status === 'Resolved' || dispute.status === 'Rejected') && dispute.statusHistory.length > 0 && (
        <Card sx={{ mb: 2 }}>
          <Typography level="title-md" fontWeight={700} mb={1}>Resolution Notes</Typography>
          <Typography level="body-md" textColor="neutral.700">{dispute.statusHistory.at(-1)?.notes ?? '—'}</Typography>
        </Card>
      )}

      {/* Status history timeline */}
      <Card>
        <Typography level="title-md" fontWeight={700} mb={2}>Status History</Typography>
        <Box sx={{ position: 'relative', pl: 3 }}>
          {/* Vertical line */}
          <Box sx={{ position: 'absolute', left: 8, top: 0, bottom: 0, width: 2, bgcolor: 'neutral.100' }} />
          {dispute.statusHistory.map((h, i) => (
            <Box key={i} sx={{ position: 'relative', mb: i < dispute.statusHistory.length - 1 ? 2.5 : 0 }}>
              {/* Dot */}
              <Box sx={{ position: 'absolute', left: -19, top: 4, width: 10, height: 10, borderRadius: '50%', bgcolor: i === dispute.statusHistory.length - 1 ? 'primary.500' : 'neutral.300', border: '2px solid #fff' }} />
              <Box sx={{ display: 'flex', justifyContent: 'space-between', gap: 2 }}>
                <Box>
                  <Typography level="body-sm" fontWeight={700}>
                    {h.fromStatus === h.toStatus ? h.toStatus : `${h.fromStatus} → ${h.toStatus}`}
                  </Typography>
                  {h.notes && <Typography level="body-sm" textColor="neutral.600" mt={0.25}>{h.notes}</Typography>}
                </Box>
                <Box sx={{ textAlign: 'right', flexShrink: 0 }}>
                  <Typography level="body-xs" textColor="neutral.400">{formatDateTime(h.changedAt)}</Typography>
                  <Typography level="body-xs" textColor="neutral.400" sx={{ textTransform: 'capitalize' }}>{h.changedByRole}</Typography>
                </Box>
              </Box>
            </Box>
          ))}
        </Box>
      </Card>
    </Box>
  )
}
