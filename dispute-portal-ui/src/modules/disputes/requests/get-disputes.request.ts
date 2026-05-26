import type { DisputeStatus } from '../models/dispute.model'

export interface GetDisputesRequest {
  page?: number
  pageSize?: number
  status?: DisputeStatus
  dateFrom?: string
  dateTo?: string
  reason?: string
}
