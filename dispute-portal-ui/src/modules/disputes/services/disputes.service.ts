import { httpHelper } from '../../../utils/http.helpers'
import type { RequestResult } from '../../../domain/models/operation-result.model'
import type { PagedResult } from '../../../types'
import type { DisputeDetail, DisputeListItem, DisputeSummaryStats } from '../models/dispute.model'
import type { RaiseDisputeRequest } from '../requests/raise-dispute.request'
import type { GetDisputesRequest } from '../requests/get-disputes.request'

export const raiseDispute = async (payload: RaiseDisputeRequest): Promise<RequestResult<DisputeDetail>> =>
  httpHelper.post<DisputeDetail>('disputes', payload)

export const getMyDisputes = async (params: GetDisputesRequest): Promise<RequestResult<PagedResult<DisputeListItem>>> =>
  httpHelper.get<PagedResult<DisputeListItem>>('disputes', { params })

export const getDisputeById = async (id: string): Promise<RequestResult<DisputeDetail>> =>
  httpHelper.get<DisputeDetail>(`disputes/${id}`)

export const getDisputeSummaryStats = async (): Promise<RequestResult<DisputeSummaryStats>> =>
  httpHelper.get<DisputeSummaryStats>('disputes/summary')
