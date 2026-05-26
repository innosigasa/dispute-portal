import { httpHelper } from '../../../utils/http.helpers'
import type { RequestResult } from '../../../domain/models/operation-result.model'
import type { LoginResult } from '../results/login.result'

export const login = async (email: string, password: string): Promise<RequestResult<LoginResult>> =>
  httpHelper.post<LoginResult>('auth/login', { email, password })

export const logout = async (refreshToken: string): Promise<RequestResult<void>> =>
  httpHelper.post<void>('auth/logout', { refreshToken })
