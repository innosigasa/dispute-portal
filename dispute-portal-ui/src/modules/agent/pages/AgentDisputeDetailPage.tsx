import Box from '@mui/joy/Box'
import Button from '@mui/joy/Button'
import Card from '@mui/joy/Card'
import Chip from '@mui/joy/Chip'
import CircularProgress from '@mui/joy/CircularProgress'
import Divider from '@mui/joy/Divider'
import FormControl from '@mui/joy/FormControl'
import FormHelperText from '@mui/joy/FormHelperText'
import FormLabel from '@mui/joy/FormLabel'
import Option from '@mui/joy/Option'
import Select from '@mui/joy/Select'
import Textarea from '@mui/joy/Textarea'
import Typography from '@mui/joy/Typography'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import AccountBalanceIcon from '@mui/icons-material/AccountBalance'
import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { useFormik } from 'formik'
import * as Yup from 'yup'
import toast from 'react-hot-toast'
import { getAgentDisputeById, updateDisputeStatus } from '../services/agent.service'
import type { DisputeDetail, DisputeStatus } from '../../disputes/models/dispute.model'
import type { UpdateDisputeStatusRequest } from '../requests/update-dispute-status.request'
import { StatusBadge } from '../../../components/StatusBadge'
import { DetailField } from '../../../components/DetailField'
import { formatCurrency, formatDate, formatDateTime } from '../../../utils/formatters'

const validTransitions: Record<DisputeStatus, DisputeStatus[]> = {
  Submitted: ['UnderReview'],
  UnderReview: ['Resolved', 'Rejected'],
  Resolved: [],
  Rejected: [],
}

const accountTypeColors: Record<string, 'primary' | 'success' | 'warning' | 'neutral'> = {
  Savings: 'success', Cheque: 'primary', Current: 'warning', Credit: 'neutral',
}

const statusSchema = Yup.object({
  newStatus: Yup.string().required('Please select a status'),
  notes: Yup.string()
    .when('newStatus', { is: (v: string) => v === 'Resolved' || v === 'Rejected', then: (s) => s.required('Notes are required when resolving or rejecting'), otherwise: (s) => s })
    .max(2000),
})

export function AgentDisputeDetailPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const [dispute, setDispute] = useState<DisputeDetail | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    if (!id) return
    getAgentDisputeById(id)
      .then(r => { if (r.isSuccessful) setDispute(r.data ?? null) })
      .finally(() => setLoading(false))
  }, [id])

  const formik = useFormik({
    initialValues: { newStatus: '', notes: '' },
    validationSchema: statusSchema,
    onSubmit: async (values, helpers) => {
      if (!id) return
      const payload: UpdateDisputeStatusRequest = { newStatus: values.newStatus as DisputeStatus, notes: values.notes }
      const result = await updateDisputeStatus(id, payload)
      if (!result.isSuccessful || !result.data) {
        toast.error(result.error ?? 'Failed to update status.')
        helpers.setSubmitting(false)
        return
      }
      setDispute(result.data)
      helpers.resetForm()
      toast.success('Status updated successfully.')
    },
  })

  if (loading) return <Box sx={{ display: 'flex', justifyContent: 'center', pt: 8 }}><CircularProgress /></Box>
  if (!dispute) return <Typography>Dispute not found.</Typography>

  const nextStatuses = validTransitions[dispute.status]

  return (
    <Box>
      {/* Breadcrumb */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2.5 }}>
        <Button variant="plain" color="neutral" startDecorator={<ArrowBackIcon />} onClick={() => navigate('/agent/disputes')} sx={{ px: 0 }}>
          Dispute Queue
        </Button>
        <Typography level="body-sm" textColor="neutral.400">/</Typography>
        <Typography level="body-sm" fontWeight={600}>{dispute.referenceNumber}</Typography>
      </Box>

      {/* Header */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 3 }}>
        <Typography level="h3" fontWeight={800}>{dispute.referenceNumber}</Typography>
        <StatusBadge status={dispute.status} />
      </Box>

      {/* Three-card grid */}
      <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', md: '1fr 1fr 1fr' }, gap: 2, mb: 2 }}>
        <Card>
          <Typography level="title-md" fontWeight={700} mb={1.5}>Transaction</Typography>
          <DetailField label="Description" value={dispute.transaction.description} />
          <DetailField label="Amount"      value={formatCurrency(dispute.transaction.amount)} />
          <DetailField label="Date"        value={formatDate(dispute.transaction.transactionDate)} />
          <DetailField label="Category"    value={dispute.transaction.category} />
          <DetailField label="Reference"   value={dispute.transaction.reference} />
        </Card>

        <Card>
          <Typography level="title-md" fontWeight={700} mb={1.5}>Customer & Account</Typography>
          <DetailField label="Name"  value={dispute.customer.fullName} />
          <DetailField label="Email" value={dispute.customer.email} />
          <Divider sx={{ my: 1.5 }} />
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
            <AccountBalanceIcon sx={{ fontSize: 15, color: 'neutral.500' }} />
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

        <Card>
          <Typography level="title-md" fontWeight={700} mb={1.5}>Dispute</Typography>
          <DetailField label="Reason"    value={dispute.reason} />
          <DetailField label="Comments"  value={dispute.comments} />
          <DetailField label="Submitted" value={formatDate(dispute.submittedAt)} />
          {dispute.resolvedAt && <DetailField label="Resolved" value={formatDate(dispute.resolvedAt)} />}
        </Card>
      </Box>

      {/* Status update form */}
      {nextStatuses.length > 0 && (
        <Card sx={{ mb: 2 }}>
          <Typography level="title-md" fontWeight={700} mb={1.5}>Update Status</Typography>
          <form onSubmit={formik.handleSubmit}>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, maxWidth: 500 }}>
              <FormControl error={formik.touched.newStatus && !!formik.errors.newStatus}>
                <FormLabel>New Status</FormLabel>
                <Select
                  placeholder="Select next status"
                  value={formik.values.newStatus || null}
                  onChange={(_, v) => formik.setFieldValue('newStatus', v)}
                >
                  {nextStatuses.map(s => <Option key={s} value={s}>{s === 'UnderReview' ? 'Under Review' : s}</Option>)}
                </Select>
                {formik.touched.newStatus && formik.errors.newStatus && <FormHelperText>{formik.errors.newStatus}</FormHelperText>}
              </FormControl>
              <FormControl error={formik.touched.notes && !!formik.errors.notes}>
                <FormLabel>Resolution Notes {(formik.values.newStatus === 'Resolved' || formik.values.newStatus === 'Rejected') ? '(required)' : '(optional)'}</FormLabel>
                <Textarea minRows={3} {...formik.getFieldProps('notes')} />
                {formik.touched.notes && formik.errors.notes && <FormHelperText>{formik.errors.notes}</FormHelperText>}
              </FormControl>
              <Button type="submit" loading={formik.isSubmitting} sx={{ alignSelf: 'flex-start' }}>
                Update Status
              </Button>
            </Box>
          </form>
        </Card>
      )}

      {/* Audit trail */}
      <Card>
        <Typography level="title-md" fontWeight={700} mb={2}>Audit Trail</Typography>
        <Box sx={{ position: 'relative', pl: 3 }}>
          <Box sx={{ position: 'absolute', left: 8, top: 0, bottom: 0, width: 2, bgcolor: 'neutral.100' }} />
          {dispute.statusHistory.map((h, i) => (
            <Box key={i} sx={{ position: 'relative', mb: i < dispute.statusHistory.length - 1 ? 2.5 : 0 }}>
              <Box sx={{ position: 'absolute', left: -19, top: 4, width: 10, height: 10, borderRadius: '50%', bgcolor: i === dispute.statusHistory.length - 1 ? 'primary.500' : 'neutral.300', border: '2px solid #fff' }} />
              <Box sx={{ display: 'flex', gap: 2 }}>
                <Box sx={{ flex: 1 }}>
                  <Typography level="body-sm" fontWeight={700}>
                    {h.fromStatus === h.toStatus ? h.toStatus : `${h.fromStatus} → ${h.toStatus}`}
                  </Typography>
                  {h.notes && <Typography level="body-sm" textColor="neutral.600" mt={0.25}>{h.notes}</Typography>}
                </Box>
                <Box sx={{ textAlign: 'right', flexShrink: 0, minWidth: 130 }}>
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
