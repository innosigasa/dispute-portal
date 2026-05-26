import { httpHelper } from '../../../utils/http.helpers'
import type { RequestResult } from '../../../domain/models/operation-result.model'
import type { PagedResult } from '../../../types'
import type { DisputeDetail, DisputeListItem } from '../../disputes/models/dispute.model'
import type { GetDisputesRequest } from '../../disputes/requests/get-disputes.request'
import type { UpdateDisputeStatusRequest } from '../requests/update-dispute-status.request'

export const getAllDisputes = async (params: GetDisputesRequest): Promise<RequestResult<PagedResult<DisputeListItem>>> =>
  httpHelper.get<PagedResult<DisputeListItem>>('agent/disputes', { params })

export const getAgentDisputeById = async (id: string): Promise<RequestResult<DisputeDetail>> =>
  httpHelper.get<DisputeDetail>(`agent/disputes/${id}`)

export const updateDisputeStatus = async (id: string, payload: UpdateDisputeStatusRequest): Promise<RequestResult<DisputeDetail>> =>
  httpHelper.put<DisputeDetail>(`agent/disputes/${id}/status`, payload)
