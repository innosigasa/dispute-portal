import type { RequestResult } from '../domain/models/operation-result.model'

const API_BASE_URL = ((import.meta.env.VITE_API_BASE_URL as string) || '/api').replace(/\/$/, '') + '/'
const TOKEN_KEY = 'accessToken'
const REFRESH_TOKEN_KEY = 'refreshToken'
const REQUEST_TIMEOUT_MS = 30_000

function getAuthHeaders(): Record<string, string> {
  const token = localStorage.getItem(TOKEN_KEY)
  return token ? { Authorization: `Bearer ${token}` } : {}
}

// --- Token refresh state ---
let isRefreshing = false
const refreshWaiters: Array<(token: string | null) => void> = []

async function doRefresh(): Promise<string | null> {
  const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY)
  if (!refreshToken) return null
  try {
    const response = await fetch(`${API_BASE_URL}auth/refresh`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ refreshToken }),
      signal: AbortSignal.timeout(REQUEST_TIMEOUT_MS),
    })
    if (!response.ok) return null
    const body = await response.json()
    // Handle both plain and RequestResult-wrapped responses
    const data = (body?.data ?? body) as Record<string, string>
    const newAccess = data?.accessToken
    const newRefresh = data?.refreshToken
    if (!newAccess) return null
    localStorage.setItem(TOKEN_KEY, newAccess)
    if (newRefresh) localStorage.setItem(REFRESH_TOKEN_KEY, newRefresh)
    return newAccess
  } catch {
    return null
  }
}

async function fetchWithAuth(url: string, init: RequestInit): Promise<Response> {
  const response = await fetch(url, init)
  if (response.status !== 401) return response

  // Queue concurrent 401s while one refresh is in flight
  if (isRefreshing) {
    return new Promise<Response>((resolve) => {
      refreshWaiters.push(async (newToken) => {
        if (!newToken) { resolve(response); return }
        resolve(await fetch(url, {
          ...init,
          headers: { ...(init.headers as Record<string, string>), Authorization: `Bearer ${newToken}` },
          signal: AbortSignal.timeout(REQUEST_TIMEOUT_MS),
        }))
      })
    })
  }

  isRefreshing = true
  const newToken = await doRefresh()
  isRefreshing = false

  const waiters = refreshWaiters.splice(0)
  waiters.forEach(cb => cb(newToken))

  if (!newToken) {
    localStorage.clear()
    window.location.href = '/login'
    return response
  }

  return fetch(url, {
    ...init,
    headers: { ...(init.headers as Record<string, string>), Authorization: `Bearer ${newToken}` },
    signal: AbortSignal.timeout(REQUEST_TIMEOUT_MS),
  })
}

async function parseResponse<T>(response: Response): Promise<RequestResult<T>> {
  const contentType = response.headers.get('Content-Type') ?? ''
  const isJson = contentType.includes('application/json')

  // 204 No Content — successful but empty
  if (response.status === 204) {
    return { isSuccessful: true, statusCode: 204 } as RequestResult<T>
  }

  if (!response.ok) {
    if (isJson) {
      try {
        const body = await response.json() as Record<string, unknown>
        if (body && 'isSuccessful' in body) return body as unknown as RequestResult<T>
        return { isSuccessful: false, statusCode: response.status, error: JSON.stringify(body) }
      } catch { /* fall through to text */ }
    }
    const errorText = await response.text().catch(() => 'Unknown error')
    return {
      isSuccessful: false,
      statusCode: response.status,
      error: errorText,
      message: `Request failed with status ${response.status}`,
    }
  }

  if (!isJson) {
    return { isSuccessful: true, statusCode: response.status } as RequestResult<T>
  }

  try {
    const data = await response.json() as Record<string, unknown>
    if (data && 'isSuccessful' in data) return data as unknown as RequestResult<T>
    return { isSuccessful: true, statusCode: response.status, data: data as T }
  } catch {
    return { isSuccessful: false, statusCode: response.status, message: 'Failed to parse response' }
  }
}

interface RequestOptions {
  headers?: Record<string, string>
  params?: Record<string, unknown>
}

export const httpHelper = {
  async get<T>(endpoint: string, options: RequestOptions = {}): Promise<RequestResult<T>> {
    const url = new URL(`${API_BASE_URL}${endpoint}`, window.location.origin)
    if (options.params) {
      for (const [key, value] of Object.entries(options.params)) {
        if (value !== undefined && value !== null) {
          url.searchParams.append(key, String(value))
        }
      }
    }
    const response = await fetchWithAuth(url.toString(), {
      method: 'GET',
      headers: { 'Content-Type': 'application/json', ...getAuthHeaders(), ...options.headers },
      signal: AbortSignal.timeout(REQUEST_TIMEOUT_MS),
    })
    return parseResponse<T>(response)
  },

  async post<T>(endpoint: string, data?: unknown, options: RequestOptions = {}): Promise<RequestResult<T>> {
    const response = await fetchWithAuth(`${API_BASE_URL}${endpoint}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', ...getAuthHeaders(), ...options.headers },
      body: data ? JSON.stringify(data) : undefined,
      signal: AbortSignal.timeout(REQUEST_TIMEOUT_MS),
    })
    return parseResponse<T>(response)
  },

  async put<T>(endpoint: string, data?: unknown, options: RequestOptions = {}): Promise<RequestResult<T>> {
    const response = await fetchWithAuth(`${API_BASE_URL}${endpoint}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json', ...getAuthHeaders(), ...options.headers },
      body: data ? JSON.stringify(data) : undefined,
      signal: AbortSignal.timeout(REQUEST_TIMEOUT_MS),
    })
    return parseResponse<T>(response)
  },

  async delete<T>(endpoint: string, data?: unknown, options: RequestOptions = {}): Promise<RequestResult<T>> {
    const response = await fetchWithAuth(`${API_BASE_URL}${endpoint}`, {
      method: 'DELETE',
      headers: { 'Content-Type': 'application/json', ...getAuthHeaders(), ...options.headers },
      body: data ? JSON.stringify(data) : undefined,
      signal: AbortSignal.timeout(REQUEST_TIMEOUT_MS),
    })
    return parseResponse<T>(response)
  },
}
