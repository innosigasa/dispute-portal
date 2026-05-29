import { useMemo } from 'react'
import { useNavigate } from 'react-router-dom'
import type { ColDef, IDatasource, IGetRowsParams, RowClickedEvent } from 'ag-grid-community'
import { GridTable } from '../../../components/GridTable'
import { getAllDisputes } from '../services/agent.service'
import type { DisputeListItem, DisputeStatus } from '../../disputes/models/dispute.model'
import { formatCurrency, formatDate } from '../../../utils/formatters'
import { StatusBadge } from '../../../components/StatusBadge'

export function AgentDisputeQueuePage() {
  const navigate = useNavigate()

  const columnDefs: ColDef<DisputeListItem>[] = [
    { field: 'referenceNumber', headerName: 'Reference No', flex: 1 },
    { field: 'customerName',    headerName: 'Customer', flex: 1.5 },
    { field: 'accountType',     headerName: 'Account', flex: 0.8 },
    { field: 'transactionDate', headerName: 'Txn Date', valueFormatter: (p) => formatDate(p.value), flex: 1 },
    { field: 'amount',          headerName: 'Amount', valueFormatter: (p) => formatCurrency(p.value), flex: 1 },
    { field: 'reason',          headerName: 'Reason', flex: 1.5 },
    { field: 'status',          headerName: 'Status', flex: 1, cellRenderer: (p: { value: DisputeStatus }) => p.value ? <StatusBadge status={p.value} /> : null },
    { field: 'submittedAt',     headerName: 'Submitted', valueFormatter: (p) => formatDate(p.value), flex: 1 },
    { field: 'updatedAt',       headerName: 'Last Updated', valueFormatter: (p) => formatDate(p.value), flex: 1 },
  ]

  const datasource = useMemo<IDatasource>(() => ({
    getRows: async (params: IGetRowsParams) => {
      const { startRow, endRow } = params
      const pageSize = endRow - startRow
      const page = Math.floor(startRow / pageSize) + 1
      const result = await getAllDisputes({ page, pageSize })
      if (!result.isSuccessful || !result.data) { params.failCallback(); return }
      params.successCallback(result.data.items, result.data.totalCount)
    },
  }), [])

  return (
    <GridTable<DisputeListItem>
      title="Dispute Queue"
      columnDefs={columnDefs}
      datasource={datasource}
      cacheBlockSize={20}
      height={600}
      showSearch={false}
      noRowsMessage="No disputes match your filters."
      onRowClicked={(e: RowClickedEvent<DisputeListItem>) => navigate(`/agent/disputes/${e.data?.id}`)}
    />
  )
}
