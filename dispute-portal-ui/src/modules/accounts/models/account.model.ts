export type AccountType = 'Savings' | 'Cheque' | 'Current' | 'Credit'

export interface BankAccount {
  id: string
  accountNumber: string
  accountType: AccountType
  accountName: string
  balance: number
  currency: string
  isDefault: boolean
}
