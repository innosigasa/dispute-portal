export type AccountType = 'Savings' | 'Cheque' | 'Current' | 'Credit' | 'Business' | 'FixedDeposit' | 'Investment' | 'Loan' | 'Joint' | 'ForeignCurrency' | 'Student' | 'Retirement' | 'MoneyMarket' | 'Islamic' | 'Trust' | 'DigitalWallet' | 'Corporate' | 'Offshore'

export interface BankAccount {
  id: string
  accountNumber: string
  accountType: AccountType
  accountName: string
  balance: number
  currency: string
  isDefault: boolean
}
