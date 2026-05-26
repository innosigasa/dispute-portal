import Box from '@mui/joy/Box'
import Chip from '@mui/joy/Chip'
import type { DisputeStatus } from '../types'

const config: Record<DisputeStatus, { color: 'primary' | 'warning' | 'success' | 'danger'; dot: string }> = {
  Submitted:   { color: 'primary', dot: '#0A2463' },
  UnderReview: { color: 'warning', dot: '#b85e00' },
  Resolved:    { color: 'success', dot: '#0d6b60' },
  Rejected:    { color: 'danger',  dot: '#9e2020' },
}

const labelMap: Record<DisputeStatus, string> = {
  Submitted:   'Submitted',
  UnderReview: 'Under Review',
  Resolved:    'Resolved',
  Rejected:    'Rejected',
}

export function StatusBadge({ status }: { status: DisputeStatus }) {
  const { color, dot } = config[status] ?? config.Submitted
  return (
    <Chip
      color={color}
      size="sm"
      variant="soft"
      startDecorator={
        <Box sx={{ width: 6, height: 6, borderRadius: '50%', bgcolor: dot, flexShrink: 0 }} />
      }
    >
      {labelMap[status]}
    </Chip>
  )
}
