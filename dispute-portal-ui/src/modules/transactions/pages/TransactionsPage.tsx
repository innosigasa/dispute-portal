import Box from '@mui/joy/Box'
import Button from '@mui/joy/Button'
import Chip from '@mui/joy/Chip'
import Option from '@mui/joy/Option'
import Select from '@mui/joy/Select'
import Typography from '@mui/joy/Typography'
import { useCallback, useEffect, useRef, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import type { ColDef, IDatasource, IGetRowsParams } from 'ag-grid-community'
import { GridTable, type GridTableHandle } from '../../../components/GridTable'
import { getTransactions } from '../services/transactions.service'
import type { TransactionListItem } from '../models/transaction.model'
import { formatCurrency, formatDate } from '../../../utils/formatters'
import { RaiseDisputeModal } from '../../disputes/RaiseDisputeModal'
import { getMyAccounts } from '../../accounts/services/accounts.service'
import type { BankAccount } from '../../accounts/models/account.model'

export function TransactionsPage() {
  const gridRef = useRef<GridTableHandle>(null)
  const searchRef = useRef('')
  const accountIdRef = useRef<string | undefined>(undefined)
  const [disputeTarget, setDisputeTarget] = useState<TransactionListItem | null>(null)
  const [modalOpen, setModalOpen] = useState(false)
  const [accounts, setAccounts] = useState<BankAccount[]>([])
  const [selectedAccountId, setSelectedAccountId] = useState<string | undefined>(undefined)
  const [searchParams] = useSearchParams()

  // Load accounts for the filter dropdown
  useEffect(() => {
    getMyAccounts().then(r => { if (r.isSuccessful && r.data) setAccounts(r.data) })
  }, [])

  // Pre-select account from URL param (e.g. coming from dashboard card)
  useEffect(() => {
    const urlAccountId = searchParams.get('accountId') ?? undefined
    setSelectedAccountId(urlAccountId)
    accountIdRef.current = urlAccountId
  }, [searchParams])

  const columnDefs: ColDef<TransactionListItem>[] = [
    { field: 'transactionDate', headerName: 'Date', valueFormatter: (p) => formatDate(p.value), sortable: true, flex: 1 },
    { field: 'description',     headerName: 'Description', sortable: true, flex: 2 },
    { field: 'amount',          headerName: 'Amount', valueFormatter: (p) => formatCurrency(p.value), sortable: true, flex: 1 },
    { field: 'category',        headerName: 'Category', flex: 1 },
    {
      field: 'accountName', headerName: 'Account', flex: 1.2,
      hide: !!selectedAccountId,
    },
    {
      field: 'isDisputed', headerName: 'Status', flex: 1,
      cellRenderer: (p: { value: boolean }) => p.value == null ? null : (
        <Chip color={p.value ? 'warning' : 'success'} size="sm" variant="soft">
          {p.value ? 'Disputed' : 'Clear'}
        </Chip>
      ),
    },
    {
      headerName: 'Action', flex: 1, sortable: false,
      cellRenderer: (p: { data: TransactionListItem | undefined }) => {
        if (!p.data) return null
        return (
          <Button
            size="sm" variant="outlined" color="danger"
            disabled={p.data.isDisputed}
            onClick={(e) => { e.stopPropagation(); setDisputeTarget(p.data!); setModalOpen(true) }}
          >
            Dispute
          </Button>
        )
      },
    },
  ]

  const datasource: IDatasource = {
    getRows: async (params: IGetRowsParams) => {
      const { startRow, endRow, sortModel } = params
      const pageSize = endRow - startRow
      const page = Math.floor(startRow / pageSize) + 1
      const sort = sortModel[0]
      const result = await getTransactions({
        page,
        pageSize,
        sortField: sort?.colId,
        sortDir: sort?.sort ?? 'desc',
        search: searchRef.current || undefined,
        accountId: accountIdRef.current,
      })
      if (!result.isSuccessful || !result.data) { params.failCallback(); return }
      params.successCallback(result.data.items, result.data.totalCount)
    },
  }
  const datasourceRef = useRef(datasource)

  const handleSearchChange = useCallback((text: string) => {
    searchRef.current = text
    gridRef.current?.purgeCache()
  }, [])

  const handleAccountChange = (id: string | undefined) => {
    setSelectedAccountId(id)
    accountIdRef.current = id
    gridRef.current?.purgeCache()
  }

  return (
    <Box>
      {/* Account filter bar */}
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 2 }}>
        <Typography level="body-sm" fontWeight={600} textColor="neutral.600">Filter by account:</Typography>
        <Select
          size="sm"
          value={selectedAccountId ?? ''}
          onChange={(_, v) => handleAccountChange(v === '' ? undefined : (v ?? undefined))}
          sx={{ minWidth: 220 }}
        >
          <Option value="">All Accounts</Option>
          {accounts.map(a => (
            <Option key={a.id} value={a.id}>
              {a.accountName} — {a.accountNumber.slice(-4).padStart(a.accountNumber.length, '•')}
            </Option>
          ))}
        </Select>
      </Box>

      <GridTable<TransactionListItem>
        ref={gridRef}
        title="My Transactions"
        columnDefs={columnDefs}
        datasource={datasourceRef.current}
        cacheBlockSize={20}
        height={560}
        showSearch
        searchPlaceholder="Search description…"
        onSearchChange={handleSearchChange}
        showExport
        noRowsMessage="No transactions found"
      />

      <RaiseDisputeModal
        open={modalOpen}
        transaction={disputeTarget}
        onClose={() => setModalOpen(false)}
        onSuccess={() => {
          setModalOpen(false)
          gridRef.current?.purgeCache()
        }}
      />
    </Box>
  )
}
