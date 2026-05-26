import { httpHelper } from '../../../utils/http.helpers'
import type { RequestResult } from '../../../domain/models/operation-result.model'
import type { BankAccount } from '../models/account.model'

export const getMyAccounts = async (): Promise<RequestResult<BankAccount[]>> =>
  httpHelper.get<BankAccount[]>('accounts')

export const getAccountById = async (id: string): Promise<RequestResult<BankAccount>> =>
  httpHelper.get<BankAccount>(`accounts/${id}`)
