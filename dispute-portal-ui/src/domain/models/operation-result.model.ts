export interface RequestResult<T = void> {
  isSuccessful: boolean
  statusCode: number
  data?: T
  error?: string
  message?: string
}
