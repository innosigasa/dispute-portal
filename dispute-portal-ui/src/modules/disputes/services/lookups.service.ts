import { httpHelper } from '../../../utils/http.helpers'
import type { RequestResult } from '../../../domain/models/operation-result.model'
import type { DisputeReason, TransactionCategory } from '../models/dispute.model'

export const getDisputeReasons = async (): Promise<RequestResult<DisputeReason[]>> =>
  httpHelper.get<DisputeReason[]>('lookups/dispute-reasons')

export const getTransactionCategories = async (): Promise<RequestResult<TransactionCategory[]>> =>
  httpHelper.get<TransactionCategory[]>('lookups/transaction-categories')
