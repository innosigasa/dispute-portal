import { httpHelper } from '../../../utils/http.helpers'
import type { RequestResult } from '../../../domain/models/operation-result.model'
import type { PagedResult } from '../../../types'
import type { TransactionListItem, TransactionDetail } from '../models/transaction.model'
import type { GetTransactionsRequest } from '../requests/get-transactions.request'

export const getTransactions = async (params: GetTransactionsRequest): Promise<RequestResult<PagedResult<TransactionListItem>>> =>
  httpHelper.get<PagedResult<TransactionListItem>>('transactions', { params })

export const getTransactionById = async (id: string): Promise<RequestResult<TransactionDetail>> =>
  httpHelper.get<TransactionDetail>(`transactions/${id}`)
