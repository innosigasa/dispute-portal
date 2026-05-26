import Box from '@mui/joy/Box'
import Typography from '@mui/joy/Typography'
import type { ReactNode } from 'react'

export function DetailField({ label, value }: { label: string; value: ReactNode }) {
  return (
    <Box sx={{ mb: 1.5 }}>
      <Typography level="body-xs" textColor="neutral.500" fontWeight="lg" textTransform="uppercase" letterSpacing="0.05em">
        {label}
      </Typography>
      <Typography level="body-md">{value ?? '—'}</Typography>
    </Box>
  )
}
