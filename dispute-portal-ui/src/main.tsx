import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import {
  CssVarsProvider as MaterialCssVarsProvider,
  THEME_ID as MATERIAL_THEME_ID,
  extendTheme as muiExtendTheme,
} from '@mui/material/styles'
import { CssVarsProvider } from '@mui/joy/styles'
import CssBaseline from '@mui/joy/CssBaseline'
import { theme } from './theme'
import { Toaster } from 'react-hot-toast'
import { BrowserRouter } from 'react-router-dom'
import App from './App'
import { AuthProvider } from './store/AuthProvider'

const materialTheme = muiExtendTheme()

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <MaterialCssVarsProvider theme={{ [MATERIAL_THEME_ID]: materialTheme }}>
      <CssVarsProvider disableTransitionOnChange theme={theme}>
        <CssBaseline />
        <BrowserRouter>
          <AuthProvider>
            <App />
            <Toaster position="top-right" />
          </AuthProvider>
        </BrowserRouter>
      </CssVarsProvider>
    </MaterialCssVarsProvider>
  </StrictMode>,
)
