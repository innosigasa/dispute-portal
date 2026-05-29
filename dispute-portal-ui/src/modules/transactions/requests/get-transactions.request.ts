export interface GetTransactionsRequest {
  [key: string]: string | number | undefined
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
