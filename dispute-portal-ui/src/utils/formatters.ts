import { format } from 'date-fns'

export const formatCurrency = (amount: number) =>
  new Intl.NumberFormat('en-ZA', { style: 'currency', currency: 'ZAR' }).format(amount)

const safeDate = (iso: string | null | undefined): Date | null => {
  if (!iso) return null
  const d = new Date(iso)
  return isNaN(d.getTime()) ? null : d
}

export const formatDate = (iso: string | null | undefined) => {
  const d = safeDate(iso)
  return d ? format(d, 'dd MMM yyyy') : '—'
}

export const formatDateTime = (iso: string | null | undefined) => {
  const d = safeDate(iso)
  return d ? format(d, 'dd MMM yyyy HH:mm') : '—'
}
