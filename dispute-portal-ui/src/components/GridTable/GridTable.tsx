import {
  forwardRef,
  useCallback,
  useImperativeHandle,
  useMemo,
  useRef,
  useState,
  type CSSProperties,
  type ReactNode,
} from 'react'
import { AgGridReact } from 'ag-grid-react'
import {
  AllCommunityModule,
  ModuleRegistry,
  themeQuartz,
  type ColDef,
  type GridReadyEvent,
  type GridSizeChangedEvent,
  type IDatasource,
  type RowClickedEvent,
  type SortChangedEvent,
  type GetRowIdParams,
  type RowClassParams,
} from 'ag-grid-community'
import Box from '@mui/joy/Box'
import Chip from '@mui/joy/Chip'
import CircularProgress from '@mui/joy/CircularProgress'
import IconButton from '@mui/joy/IconButton'
import Input from '@mui/joy/Input'
import Tooltip from '@mui/joy/Tooltip'
import Typography from '@mui/joy/Typography'
import {
  FileDownloadRounded,
  RefreshRounded,
  SearchRounded,
  TableRowsRounded,
  ViewColumnRounded,
} from '@mui/icons-material'
import './GridTable.css'

ModuleRegistry.registerModules([AllCommunityModule])

const gridTheme = themeQuartz

export type GridDensity = 'compact' | 'standard' | 'comfortable'

export interface GridTableHandle {
  purgeCache: () => void
  exportCsv: (filename?: string) => void
}

export interface GridTableProps<TData = object> {
  columnDefs: ColDef<TData>[]
  /** Client-side row data. Provide either this or `datasource`, not both. */
  rowData?: TData[] | null
  /** Infinite-scroll datasource. Takes priority over `rowData` when provided. */
  datasource?: IDatasource
  /** Number of rows fetched per block in infinite mode (default 20). */
  cacheBlockSize?: number
  height?: number | string
  title?: string
  density?: GridDensity
  striped?: boolean
  showToolbar?: boolean
  showSearch?: boolean
  searchPlaceholder?: string
  /** Called with the current search text after a 400 ms debounce. In infinite
   *  mode call `gridTableRef.current?.purgeCache()` inside this callback. */
  onSearchChange?: (text: string) => void
  showRowCount?: boolean
  showExport?: boolean
  toolbarActions?: ReactNode
  onRefresh?: () => void
  loading?: boolean
  noRowsMessage?: string
  onGridReady?: (event: GridReadyEvent<TData>) => void
  onRowClicked?: (event: RowClickedEvent<TData>) => void
  onSortChanged?: (event: SortChangedEvent<TData>) => void
  getRowId?: (params: GetRowIdParams<TData>) => string
  getRowClass?: (params: RowClassParams<TData>) => string | string[] | undefined
  defaultColDef?: ColDef<TData>
  animateRows?: boolean
  domLayout?: 'normal' | 'autoHeight' | 'print'
  sx?: CSSProperties
  className?: string
}

const SEARCH_DEBOUNCE_MS = 400

function GridTableInner<TData extends object = object>(
  {
    columnDefs,
    rowData,
    datasource,
    cacheBlockSize = 20,
    height = 520,
    title,
    density = 'standard',
    striped = false,
    showToolbar = true,
    showSearch = true,
    searchPlaceholder = 'Search…',
    onSearchChange,
    showRowCount = true,
    showExport = false,
    toolbarActions,
    onRefresh,
    loading = false,
    noRowsMessage = 'No rows to display',
    onGridReady,
    onRowClicked,
    onSortChanged,
    getRowId,
    getRowClass,
    defaultColDef: defaultColDefOverride,
    animateRows = true,
    domLayout = 'normal',
    sx,
    className,
  }: GridTableProps<TData>,
  ref: React.Ref<GridTableHandle>,
) {
  const gridRef = useRef<AgGridReact<TData>>(null)
  const [searchText, setSearchText] = useState('')
  const [displayedRowCount, setDisplayedRowCount] = useState(0)
  const searchTimerRef = useRef<ReturnType<typeof setTimeout> | null>(null)

  const isInfinite = datasource != null
  const rowModelType = isInfinite ? 'infinite' : 'clientSide'

  // ─── Imperative handle ───
  useImperativeHandle(ref, () => ({
    purgeCache: () => gridRef.current?.api?.purgeInfiniteCache(),
    exportCsv: (filename) =>
      gridRef.current?.api?.exportDataAsCsv({
        fileName: filename ?? (title ? `${title.toLowerCase().replace(/\s+/g, '_')}_export` : 'export'),
      }),
  }))

  const defaultColDef = useMemo<ColDef<TData>>(
    () => ({
      sortable: true,
      resizable: true,
      minWidth: 80,
      flex: 1,
      ...defaultColDefOverride,
    }),
    [defaultColDefOverride],
  )

  // ─── Grid ready ───
  const handleGridReady = useCallback(
    (event: GridReadyEvent<TData>) => {
      event.api.sizeColumnsToFit()
      if (!isInfinite) {
        setDisplayedRowCount(event.api.getDisplayedRowCount())
      }
      onGridReady?.(event)
    },
    [isInfinite, onGridReady],
  )

  // ─── Auto-resize on container resize ───
  const handleGridSizeChanged = useCallback((event: GridSizeChangedEvent<TData>) => {
    event.api.sizeColumnsToFit()
  }, [])

  // ─── Row count (client-side only) ───
  const handleModelUpdated = useCallback(() => {
    if (!isInfinite && gridRef.current?.api) {
      setDisplayedRowCount(gridRef.current.api.getDisplayedRowCount())
    }
  }, [isInfinite])

  // ─── Search ───
  const handleSearchChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      const value = e.target.value
      setSearchText(value)

      if (!isInfinite) {
        gridRef.current?.api?.setGridOption('quickFilterText', value)
        return
      }

      if (searchTimerRef.current) clearTimeout(searchTimerRef.current)
      searchTimerRef.current = setTimeout(() => {
        onSearchChange?.(value)
      }, SEARCH_DEBOUNCE_MS)
    },
    [isInfinite, onSearchChange],
  )

  // ─── Export ───
  const handleExportCsv = useCallback(() => {
    gridRef.current?.api?.exportDataAsCsv({
      fileName: title ? `${title.toLowerCase().replace(/\s+/g, '_')}_export` : 'export',
    })
  }, [title])

  // ─── Auto-size columns ───
  const handleAutoSizeAll = useCallback(() => {
    gridRef.current?.api?.autoSizeAllColumns()
  }, [])

  // ─── CSS classes ───
  const densityClass = density === 'compact' ? 'grid-table--dense' : density === 'comfortable' ? 'grid-table--comfortable' : ''
  const stripedClass = striped ? 'grid-table--striped' : ''
  const clickableClass = onRowClicked ? 'grid-table--clickable' : ''
  const gridHeight = typeof height === 'number' ? `${height}px` : height

  // ─── Overlays ───
  const noRowsOverlayComponent = useCallback(
    () => (
      <Box className="grid-table-empty">
        <TableRowsRounded style={{ fontSize: 48 }} />
        <Typography level="body-md">{noRowsMessage}</Typography>
      </Box>
    ),
    [noRowsMessage],
  )

  const loadingOverlayComponent = useCallback(
    () => (
      <Box className="grid-table-loading">
        <CircularProgress size="md" />
        <Typography level="body-sm">Loading…</Typography>
      </Box>
    ),
    [],
  )

  return (
    <Box className={`grid-table-wrapper ${className ?? ''}`} style={sx}>
      {/* ─── Toolbar ─── */}
      {showToolbar && (
        <Box className="grid-table-toolbar">
          <Box className="grid-table-toolbar-left">
            {title && (
              <Typography level="title-md" className="grid-table-title">
                {title}
              </Typography>
            )}
            {showRowCount && !isInfinite && rowData != null && (
              <Chip size="sm" variant="soft" color="neutral">
                {displayedRowCount} {displayedRowCount === 1 ? 'row' : 'rows'}
              </Chip>
            )}
            {showSearch && (
              <Input
                size="sm"
                placeholder={searchPlaceholder}
                value={searchText}
                onChange={handleSearchChange}
                startDecorator={<SearchRounded style={{ fontSize: 18 }} />}
                className="grid-table-search"
                sx={{ '--Input-focusedThickness': '1.5px', borderRadius: '8px', fontSize: '0.85rem' }}
              />
            )}
          </Box>
          <Box className="grid-table-toolbar-right">
            {toolbarActions}
            {showExport && (
              <Tooltip title="Export CSV" variant="soft" size="sm">
                <IconButton size="sm" variant="plain" color="neutral" onClick={handleExportCsv}>
                  <FileDownloadRounded style={{ fontSize: 18 }} />
                </IconButton>
              </Tooltip>
            )}
            <Tooltip title="Auto-size columns" variant="soft" size="sm">
              <IconButton size="sm" variant="plain" color="neutral" onClick={handleAutoSizeAll}>
                <ViewColumnRounded style={{ fontSize: 18 }} />
              </IconButton>
            </Tooltip>
            {onRefresh && (
              <Tooltip title="Refresh" variant="soft" size="sm">
                <IconButton size="sm" variant="plain" color="neutral" onClick={onRefresh}>
                  <RefreshRounded style={{ fontSize: 18 }} />
                </IconButton>
              </Tooltip>
            )}
          </Box>
        </Box>
      )}

      {/* ─── Grid ─── */}
      <Box
        className={`grid-table-container ag-theme-grid-table ${densityClass} ${stripedClass} ${clickableClass}`}
        style={{ height: gridHeight, width: '100%' }}
      >
        <AgGridReact<TData>
          ref={gridRef}
          theme={gridTheme}
          columnDefs={columnDefs}
          defaultColDef={defaultColDef}
          rowModelType={rowModelType as 'clientSide' | 'infinite'}
          {...(isInfinite
            ? { datasource, cacheBlockSize }
            : { rowData: rowData ?? [] })}
          animateRows={animateRows}
          domLayout={domLayout}
          loading={loading}
          getRowId={getRowId}
          getRowClass={getRowClass}
          noRowsOverlayComponent={noRowsOverlayComponent}
          loadingOverlayComponent={loadingOverlayComponent}
          onGridReady={handleGridReady}
          onGridSizeChanged={handleGridSizeChanged}
          onModelUpdated={handleModelUpdated}
          onRowClicked={onRowClicked}
          onSortChanged={onSortChanged}
          enableCellTextSelection
          ensureDomOrder
          tooltipShowDelay={500}
        />
      </Box>
    </Box>
  )
}

export const GridTable = forwardRef(GridTableInner) as <TData extends object = object>(
  props: GridTableProps<TData> & { ref?: React.Ref<GridTableHandle> },
) => React.ReactElement

export default GridTable
