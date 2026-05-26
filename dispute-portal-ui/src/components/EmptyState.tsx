import Box from '@mui/joy/Box'
import Typography from '@mui/joy/Typography'

interface Props {
  icon?: string
  heading: string
  subtext?: string
}

export function EmptyState({ icon = '📭', heading, subtext }: Props) {
  return (
    <Box sx={{ textAlign: 'center', py: 8 }}>
      <Typography level="h3" sx={{ mb: 1 }}>{icon}</Typography>
      <Typography level="h4" sx={{ mb: 0.5 }}>{heading}</Typography>
      {subtext && <Typography level="body-md" textColor="neutral.500">{subtext}</Typography>}
    </Box>
  )
}
