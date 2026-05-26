import Box from '@mui/joy/Box'
import Card from '@mui/joy/Card'
import CardContent from '@mui/joy/CardContent'
import Typography from '@mui/joy/Typography'
import Button from '@mui/joy/Button'
import Skeleton from '@mui/joy/Skeleton'
import GavelIcon from '@mui/icons-material/Gavel'
import { useNavigate } from 'react-router-dom'
import type { DisputeSummaryStats } from '../../disputes/models/dispute.model'

interface StatTileProps {
  label: string
  count: number
  color: string
  bg: string
}

function StatTile({ label, count, color, bg }: StatTileProps) {
  return (
    <Box
      sx={{
        flex: 1,
        minWidth: 80,
        textAlign: 'center',
        p: 1.5,
        borderRadius: '10px',
        bgcolor: bg,
      }}
    >
      <Typography level="h2" sx={{ color, fontWeight: 800, lineHeight: 1 }}>
        {count}
      </Typography>
      <Typography level="body-xs" sx={{ color, opacity: 0.8, mt: 0.5, fontWeight: 600 }}>
        {label}
      </Typography>
    </Box>
  )
}

interface Props {
  stats: DisputeSummaryStats | null
  loading: boolean
}

export function DisputeStatusSummaryWidget({ stats, loading }: Props) {
  const navigate = useNavigate()

  return (
    <Card sx={{ height: '100%' }}>
      <CardContent>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
          <GavelIcon sx={{ color: 'primary.500', fontSize: 20 }} />
          <Typography level="title-md" fontWeight={700}>My Disputes</Typography>
        </Box>

        {loading ? (
          <Box sx={{ display: 'flex', gap: 1 }}>
            {[0, 1, 2, 3].map(i => <Skeleton key={i} variant="rectangular" height={72} sx={{ flex: 1, borderRadius: '10px' }} />)}
          </Box>
        ) : stats && stats.total > 0 ? (
          <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
            <StatTile label="Submitted"   count={stats.submitted}   color="#0A2463" bg="#e8eef7" />
            <StatTile label="Under Review" count={stats.underReview} color="#b85e00" bg="#fff4e5" />
            <StatTile label="Resolved"    count={stats.resolved}    color="#0d6b60" bg="#e6f6f4" />
            <StatTile label="Rejected"    count={stats.rejected}    color="#9e2020" bg="#fde8e8" />
          </Box>
        ) : (
          <Box sx={{ textAlign: 'center', py: 3 }}>
            <Typography level="body-sm" textColor="neutral.500">No disputes raised yet.</Typography>
          </Box>
        )}

        <Button
          variant="plain"
          size="sm"
          onClick={() => navigate('/disputes')}
          sx={{ mt: 2, alignSelf: 'flex-start', px: 0 }}
        >
          View All Disputes →
        </Button>
      </CardContent>
    </Card>
  )
}
