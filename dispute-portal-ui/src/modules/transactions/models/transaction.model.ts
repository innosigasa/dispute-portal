export interface DisputeSummary {
  id: string
  referenceNumber: string
  status: string
  submittedAt: string
}

export interface AccountInfo {
  id: string
  accountName: string
  accountType: string
  accountNumber: string
}

export interface TransactionListItem {
  id: string
  transactionDate: string
  description: string
  amount: number
  category: string
  reference: string
  isDisputed: boolean
  disputeReference?: string
  accountId: string
  accountName: string
  accountType: string
  accountNumber: string
}

export interface TransactionDetail extends TransactionListItem {
  customerId: string
  createdAt: string
  dispute?: DisputeSummary
  account: AccountInfo
}
