import { useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import type { ColDef, IDatasource, IGetRowsParams, RowClickedEvent } from 'ag-grid-community'
import { GridTable } from '../../../components/GridTable'
import { getMyDisputes } from '../services/disputes.service'
import type { DisputeListItem, DisputeStatus } from '../models/dispute.model'
import { formatCurrency, formatDate } from '../../../utils/formatters'
import { StatusBadge } from '../../../components/StatusBadge'

export function MyDisputesPage() {
  const navigate = useNavigate()

  const columnDefs: ColDef<DisputeListItem>[] = [
    { field: 'referenceNumber', headerName: 'Reference No', flex: 1 },
    { field: 'transactionDate', headerName: 'Transaction Date', valueFormatter: (p) => formatDate(p.value), flex: 1 },
    { field: 'amount', headerName: 'Amount', valueFormatter: (p) => formatCurrency(p.value), flex: 1 },
    { field: 'reason', headerName: 'Reason', flex: 1.5 },
    { field: 'status', headerName: 'Status', flex: 1, cellRenderer: (p: { value: DisputeStatus }) => p.value ? <StatusBadge status={p.value} /> : null },
    { field: 'submittedAt', headerName: 'Submitted', valueFormatter: (p) => formatDate(p.value), flex: 1 },
    { field: 'updatedAt', headerName: 'Last Updated', valueFormatter: (p) => formatDate(p.value), flex: 1 },
  ]

  const datasourceRef = useRef<IDatasource>({
    getRows: async (params: IGetRowsParams) => {
      const { startRow, endRow } = params
      const pageSize = endRow - startRow
      const page = Math.floor(startRow / pageSize) + 1
      const result = await getMyDisputes({ page, pageSize })
      if (!result.isSuccessful || !result.data) { params.failCallback(); return }
      params.successCallback(result.data.items, result.data.totalCount)
    }
  })

  return (
    <GridTable<DisputeListItem>
      title="My Disputes"
      columnDefs={columnDefs}
      datasource={datasourceRef.current}
      cacheBlockSize={20}
      height={580}
      showSearch={false}
      noRowsMessage="You have not raised any disputes."
      onRowClicked={(e: RowClickedEvent<DisputeListItem>) => navigate(`/disputes/${e.data?.id}`)}
    />
  )
}
