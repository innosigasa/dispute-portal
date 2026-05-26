import type { DisputeStatus } from '../../disputes/models/dispute.model'

export interface UpdateDisputeStatusRequest {
  newStatus: DisputeStatus
  notes: string
}
