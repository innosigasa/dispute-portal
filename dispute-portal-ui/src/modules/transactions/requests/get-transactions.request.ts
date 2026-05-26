export interface GetTransactionsRequest {
  page?: number
  pageSize?: number
  sortField?: string
  sortDir?: string
  accountId?: string
  dateFrom?: string
  dateTo?: string
  category?: string
  amountMin?: number
  amountMax?: number
  search?: string
}
