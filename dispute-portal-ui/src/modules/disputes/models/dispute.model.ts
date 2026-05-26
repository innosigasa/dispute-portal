import type { AccountInfo, TransactionDetail } from '../../transactions/models/transaction.model'

export type DisputeStatus = 'Submitted' | 'UnderReview' | 'Resolved' | 'Rejected'

export interface DisputeStatusHistoryItem {
  fromStatus: string
  toStatus: string
  changedByRole: string
  notes: string
  changedAt: string
}

export interface CustomerInfo {
  id: string
  fullName: string
  email: string
}

export interface DisputeListItem {
  id: string
  referenceNumber: string
  transactionDate: string
  amount: number
  reason: string
  status: DisputeStatus
  submittedAt: string
  updatedAt: string
  customerName?: string
  accountType?: string
  accountNumber?: string
}

export interface DisputeDetail {
  id: string
  referenceNumber: string
  reason: string
  comments: string
  status: DisputeStatus
  submittedAt: string
  resolvedAt?: string
  updatedAt: string
  transaction: TransactionDetail
  customer: CustomerInfo
  account: AccountInfo
  statusHistory: DisputeStatusHistoryItem[]
}

export interface DisputeReason {
  id: number
  code: string
  label: string
}

export interface TransactionCategory {
  id: number
  code: string
  label: string
}

export interface DisputeSummaryStats {
  submitted: number
  underReview: number
  resolved: number
  rejected: number
  total: number
}
