import Box from '@mui/joy/Box'
import Button from '@mui/joy/Button'
import DialogContent from '@mui/joy/DialogContent'
import DialogTitle from '@mui/joy/DialogTitle'
import FormControl from '@mui/joy/FormControl'
import FormHelperText from '@mui/joy/FormHelperText'
import FormLabel from '@mui/joy/FormLabel'
import Modal from '@mui/joy/Modal'
import ModalDialog from '@mui/joy/ModalDialog'
import Option from '@mui/joy/Option'
import Select from '@mui/joy/Select'
import Textarea from '@mui/joy/Textarea'
import Typography from '@mui/joy/Typography'
import { useFormik } from 'formik'
import * as Yup from 'yup'
import { useEffect, useState } from 'react'
import toast from 'react-hot-toast'
import { raiseDispute } from './services/disputes.service'
import { getDisputeReasons } from './services/lookups.service'
import type { DisputeReason } from './models/dispute.model'
import type { TransactionListItem } from '../transactions/models/transaction.model'
import { formatCurrency, formatDate } from '../../utils/formatters'

interface Props {
  open: boolean
  transaction: TransactionListItem | null
  onClose: () => void
  onSuccess: () => void
}

const schema = Yup.object({
  reasonCode: Yup.string().required('Please select a reason'),
  comments: Yup.string().max(1000, 'Max 1000 characters'),
})

export function RaiseDisputeModal({ open, transaction, onClose, onSuccess }: Props) {
  const [reasons, setReasons] = useState<DisputeReason[]>([])
  const [submitted, setSubmitted] = useState(false)
  const [refNumber, setRefNumber] = useState('')

  useEffect(() => {
    getDisputeReasons().then(r => { if (r.isSuccessful && r.data) setReasons(r.data) }).catch(() => {})
  }, [])

  const formik = useFormik({
    initialValues: { reasonCode: '', comments: '' },
    validationSchema: schema,
    onSubmit: async (values, helpers) => {
      if (!transaction) return
      const res = await raiseDispute({ transactionId: transaction.id, reasonCode: values.reasonCode, comments: values.comments })
      if (!res.isSuccessful || !res.data) {
        toast.error(res.error ?? 'Failed to submit dispute. Please try again.')
        helpers.setSubmitting(false)
        return
      }
      setRefNumber(res.data.referenceNumber)
      setSubmitted(true)
      onSuccess()
    },
  })

  const handleClose = () => {
    formik.resetForm()
    setSubmitted(false)
    setRefNumber('')
    onClose()
  }

  return (
    <Modal open={open} onClose={handleClose}>
      <ModalDialog sx={{ maxWidth: 500, width: '100%' }}>
        <DialogTitle>{submitted ? 'Dispute Submitted' : 'Raise a Dispute'}</DialogTitle>
        <DialogContent>
          {submitted ? (
            <Box sx={{ textAlign: 'center', py: 2 }}>
              <Typography level="h4" color="success" mb={1}>Reference: {refNumber}</Typography>
              <Typography level="body-md" mb={2}>We will review your dispute and update you shortly.</Typography>
              <Button onClick={handleClose}>Close</Button>
            </Box>
          ) : (
            <>
              {transaction && (
                <Box sx={{ bgcolor: 'background.level1', p: 1.5, borderRadius: 'sm', mb: 2 }}>
                  <Typography level="body-sm"><strong>{transaction.description}</strong></Typography>
                  <Typography level="body-sm">{formatDate(transaction.transactionDate)} · {formatCurrency(transaction.amount)}</Typography>
                </Box>
              )}
              <form onSubmit={formik.handleSubmit}>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                  <FormControl error={formik.touched.reasonCode && !!formik.errors.reasonCode}>
                    <FormLabel>Dispute Reason</FormLabel>
                    <Select
                      placeholder="Select a reason"
                      value={formik.values.reasonCode || null}
                      onChange={(_, v) => formik.setFieldValue('reasonCode', v)}
                    >
                      {reasons.map((r) => <Option key={r.code} value={r.code}>{r.label}</Option>)}
                    </Select>
                    {formik.touched.reasonCode && formik.errors.reasonCode && (
                      <FormHelperText>{formik.errors.reasonCode}</FormHelperText>
                    )}
                  </FormControl>
                  <FormControl error={formik.touched.comments && !!formik.errors.comments}>
                    <FormLabel>Additional Comments <Typography level="body-xs" textColor="neutral.400">(optional)</Typography></FormLabel>
                    <Textarea minRows={3} {...formik.getFieldProps('comments')} />
                    <Typography level="body-xs" textColor="neutral.400" sx={{ textAlign: 'right' }}>
                      {formik.values.comments.length}/1000
                    </Typography>
                    {formik.touched.comments && formik.errors.comments && (
                      <FormHelperText>{formik.errors.comments}</FormHelperText>
                    )}
                  </FormControl>
                  <Box sx={{ display: 'flex', gap: 1, justifyContent: 'flex-end' }}>
                    <Button variant="plain" color="neutral" onClick={handleClose}>Cancel</Button>
                    <Button type="submit" loading={formik.isSubmitting}>Submit Dispute</Button>
                  </Box>
                </Box>
              </form>
            </>
          )}
        </DialogContent>
      </ModalDialog>
    </Modal>
  )
}
