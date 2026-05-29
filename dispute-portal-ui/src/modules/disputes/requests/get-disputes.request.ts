import type { DisputeStatus } from '../models/dispute.model'

export interface GetDisputesRequest {
  [key: string]: string | number | undefined
  page?: number
  pageSize?: number
  status?: DisputeStatus
  dateFrom?: string
  dateTo?: string
  reason?: string
}
