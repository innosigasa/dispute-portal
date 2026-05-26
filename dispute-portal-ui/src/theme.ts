import { extendTheme as extendedJoyTheme, shouldSkipGeneratingVar, type Theme as JoyTheme } from '@mui/joy/styles';
import {
  extendTheme as muiExtendTheme,
  shouldSkipGeneratingVar as muiShouldSkipGeneratingVar,
} from '@mui/material/styles';
import { deepmerge } from '@mui/utils';
import colors from '@mui/joy/colors';

declare module '@mui/material/styles' {
  interface Theme {
    vars: JoyTheme['vars'];
  }
}


const { unstable_sxConfig: muiSxConfig, ...muiTheme } = muiExtendTheme({
  cssVarPrefix: 'joy',
  colorSchemes: {
    light: {
      palette: {
        primary: {
          main: colors.blue[500],
          light: colors.blue[300],
          dark: colors.blue[700],
          contrastText: '#ffffff',
        },
        success: {
          main: colors.green[500],
          light: colors.green[300],
          dark: colors.green[700],
        },
        warning: {
          main: colors.yellow[500],
          light: colors.yellow[300],
          dark: colors.yellow[700],
        },
        error: {
          main: colors.red[500],
          light: colors.red[300],
          dark: colors.red[700],
        },
        grey: colors.grey,
        // background: {
        //   default: '#f8fafc',
        //   paper: '#ffffff',
        // },
        // text: {
        //   primary: '#1e293b',
        //   secondary: '#475569',
        // },
        divider: '#e2e8f0',
        // common: {
        //   black: '#000000',
        //   white: '#ffffff',
        // },
        // action: {
        //   active: '#475569',
        //   hover: 'rgba(71, 85, 105, 0.08)',
        //   selected: 'rgba(71, 85, 105, 0.16)',
        //   disabled: 'rgba(71, 85, 105, 0.3)',
        //   disabledBackground: 'rgba(71, 85, 105, 0.12)',
        // },
        // TableCell: {
        //   border: '#e2e8f0',
        // },
        // Button: {
        //   inheritContainedBg: '#e2e8f0',
        //   inheritContainedHoverBg: '#d1d5db',
        // },
      },
    },
    dark: {
      palette: {
        primary: {
          main: colors.blue[500],
          light: colors.blue[300],
          dark: colors.blue[700],
          contrastText: '#ffffff',
        },
        success: {
          main: colors.green[500],
          light: colors.green[300],
          dark: colors.green[700],
        },
        warning: {
          main: colors.yellow[500],
          light: colors.yellow[300],
          dark: colors.yellow[700],
        },
        error: {
          main: colors.red[500],
          light: colors.red[300],
          dark: colors.red[700],
        },
        grey: colors.grey,
        // background: {
        //   default: '#0b1020',
        //   paper: '#151b2d',
        // },
        // text: {
        //   primary: '#f8fafc',
        //   secondary: '#cbd5e1',
        // },
        divider: '#2a3447',
        // common: {
        //   black: '#000000',
        //   white: '#ffffff',
        // },
        // action: {
        //   active: '#cbd5e1',
        //   hover: 'rgba(203, 213, 225, 0.08)',
        //   selected: 'rgba(203, 213, 225, 0.16)',
        //   disabled: 'rgba(203, 213, 225, 0.3)',
        //   disabledBackground: 'rgba(203, 213, 225, 0.12)',
        // },
        // TableCell: {
        //   border: '#2a3447',
        // },
        // Button: {
        //   inheritContainedBg: '#2a3447',
        //   inheritContainedHoverBg: '#374056',
        // },
      },
    },
  },
  shape: {
    borderRadius: 8,
  },
  typography: {
    fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
    fontSize: 14,
  },
});

// Medical Practice Color Palette
const medicalColors = {
  // Primary - Medical Blue/Teal
  primary: {
    50: '#e0f7fa',
    100: '#b2ebf2',
    200: '#80deea',
    300: '#4dd0e1',
    400: '#26c6da',
    500: '#00bcd4', // Main medical teal
    600: '#00acc1',
    700: '#0097a7',
    800: '#00838f',
    900: '#006064',
  },
  // Success - Medical Green
  success: {
    50: '#e8f5e8',
    100: '#c8e6c9',
    200: '#a5d6a7',
    300: '#81c784',
    400: '#66bb6a',
    500: '#4caf50', // Main medical green
    600: '#43a047',
    700: '#388e3c',
    800: '#2e7d32',
    900: '#1b5e20',
  },
  // Warning - Medical yellow/Coral
  warning: {
    50: '#ffe8e0',
    100: '#ffc7b3',
    200: '#ffa380',
    300: '#ff7e4d',
    400: '#ff6326',
    500: '#ff5722', // Main coral/yellow
    600: '#f4511e',
    700: '#e64a19',
    800: '#d84315',
    900: '#bf360c',
  },
  // Danger - Medical Red
  danger: {
    50: '#ffebee',
    100: '#ffcdd2',
    200: '#ef9a9a',
    300: '#e57373',
    400: '#ef5350',
    500: '#f44336', // Main medical red
    600: '#e53935',
    700: '#d32f2f',
    800: '#c62828',
    900: '#b71c1c',
  },
  // Neutral - Medical Gray
  neutral: {
    50: '#fafafa',
    100: '#f5f5f5',
    200: '#eeeeee',
    300: '#e0e0e0',
    400: '#bdbdbd',
    500: '#9e9e9e',
    600: '#757575',
    700: '#616161',
    800: '#424242',
    900: '#212121',
  },
};


// Medical Practice Theme
export const {unstable_sxConfig: joySxConfig , ...medicalTheme} = extendedJoyTheme({
  colorSchemes: {
    light: {
      palette: {
        primary: medicalColors.primary,
        success: medicalColors.success,
        warning: medicalColors.warning,
        danger: medicalColors.danger,
        neutral: medicalColors.neutral,

        background: {
          body: '#f8fafc',
          surface: '#ffffff',
        },
        // text: {
        //   primary: 'red', // slightly darker for clarity
        //   secondary: '#475569',
        //   tertiary: '#64748b',
        // },

        divider: '#e5e7eb',
      },
    },

    dark: {
      palette: {
        primary: medicalColors.primary,
        success: medicalColors.success,
        warning: medicalColors.warning,
        danger: medicalColors.danger,
        neutral: medicalColors.neutral,

        // background: {
        //   body: '#0b1020',
        //   surface: '#151b2d',
        //   popup: '#1e2738',
        //   level1: '#1e2738',
        //   level2: '#2a3447',
        //   level3: '#374056',
        // },

        // text: {
        //   primary: '#f8fafc',
        //   secondary: '#cbd5e1',
        //   tertiary: '#94a3b8',
        // },

        divider: '#2a3447',
      },
    },
  },

  fontFamily: {
    body: '"Inter", system-ui, sans-serif',
    display: '"Inter", system-ui, sans-serif',
    code: '"Fira Code", monospace',
  },

  // radius: {
  //   xs: '4px',
  //   sm: '8px',
  //   md: '10px',
  //   lg: '14px',
  //   xl: '18px',
  // },

  // shadow: {
  //   xs: '0 1px 2px rgba(0,0,0,0.04)',
  //   sm: '0 2px 4px rgba(0,0,0,0.06)',
  //   md: '0 4px 8px rgba(0,0,0,0.08)',
  //   lg: '0 10px 20px rgba(0,0,0,0.10)',
  //   xl: '0 20px 40px rgba(0,0,0,0.12)',
  // },

  // components: {
  //   JoyCard: {
  //     styleOverrides: {
  //       root: {
  //         borderRadius: '14px',
  //         border: '1px solid var(--joy-palette-divider)',
  //         boxShadow: 'var(--joy-shadow-sm)',
  //         backgroundColor: 'var(--joy-palette-background-surface)',
  //       },
  //     },
  //   },

  //   JoyButton: {
  //     styleOverrides: {
  //       root: {
  //         borderRadius: '10px',
  //         fontWeight: 500,
  //         textTransform: 'none',
  //         boxShadow: 'none',
  //         '&:hover': {
  //           boxShadow: 'var(--joy-shadow-sm)',
  //         },
  //       },
  //     },
  //   },

  //   JoyInput: {
  //     styleOverrides: {
  //       root: {
  //         borderRadius: '10px',
  //         backgroundColor: 'var(--joy-palette-background-surface)',
  //         '--Input-focusedThickness': '2px',
  //         '--Input-radius': '10px',
  //       },
  //     },
  //   },

  //   JoyTextarea: {
  //     styleOverrides: {
  //       root: {
  //         borderRadius: '10px',
  //       },
  //     },
  //   },

  //   JoyListItemButton: {
  //     styleOverrides: {
  //       root: {
  //         borderRadius: '10px',
  //         '&.Mui-selected': {
  //           backgroundColor: 'var(--joy-palette-primary-softBg)',
  //           color: 'var(--joy-palette-primary-solidColor)',
  //           fontWeight: 600,
  //         },
  //       },
  //     },
  //   },

  //   JoyChip: {
  //     styleOverrides: {
  //       root: {
  //         borderRadius: '8px',
  //         fontWeight: 500,
  //       },
  //     },
  //   },}
});

//export const theme = deepmerge(muiTheme, medicalTheme);

export const theme = {
  ...muiTheme,
  ...medicalTheme,
  colorSchemes: deepmerge(muiTheme.colorSchemes, medicalTheme.colorSchemes),
  typography: {
    ...muiTheme.typography,
    ...medicalTheme.typography,
  },
  // shape: {
  //   ...muiTheme.shape,
  // },
  // components: {
  //   ...muiTheme.components,
  // },
} as unknown as ReturnType<typeof extendedJoyTheme>;

theme.shouldSkipGeneratingVar = (...params) => shouldSkipGeneratingVar(params[0]) || muiShouldSkipGeneratingVar(params[0]);

theme.generateCssVars = (colorScheme) => ({
  css: {
    ...medicalTheme.generateCssVars(colorScheme).css,
    ...muiTheme.generateStyleSheets().flatMap((sheet) => sheet).values,
  },
  vars: deepmerge(medicalTheme.generateCssVars(colorScheme).vars, muiTheme.generateThemeVars()),
});

// theme.unstable_sxConfig = {
//   ...joySxConfig,
//   ...muiSxConfig,
// };


































// import {
//   extendTheme as extendJoyTheme,
//   shouldSkipGeneratingVar,
//   type Theme as JoyTheme,
// } from '@mui/joy/styles'
// import {
//   extendTheme as muiExtendTheme,
//   shouldSkipGeneratingVar as muiShouldSkipGeneratingVar,
// } from '@mui/material/styles'
// import { deepmerge } from '@mui/utils'

// declare module '@mui/material/styles' {
//   interface Theme {
//     vars: JoyTheme['vars']
//   }
// }

// const { unstable_sxConfig: muiSxConfig, ...muiTheme } = muiExtendTheme({
//   cssVarPrefix: 'joy',
//   colorSchemes: {
//     light: {
//       palette: {
//         primary: { main: '#3b6bc4', light: '#7799d4', dark: '#0A2463', contrastText: '#ffffff' },
//         success: { main: '#1B998B', light: '#4cc5b9', dark: '#0d6b60' },
//         warning: { main: '#f59e0b', light: '#fcd34d', dark: '#b45309' },
//         error:   { main: '#ef4444', light: '#f87171', dark: '#b91c1c' },
//         divider: '#dce1ea',
//       },
//     },
//     dark: {
//       palette: {
//         primary: { main: '#3b6bc4', light: '#7799d4', dark: '#0A2463', contrastText: '#ffffff' },
//         success: { main: '#1B998B', light: '#4cc5b9', dark: '#0d6b60' },
//         warning: { main: '#f59e0b', light: '#fcd34d', dark: '#b45309' },
//         error:   { main: '#ef4444', light: '#f87171', dark: '#b91c1c' },
//         divider: '#364455',
//       },
//     },
//   },
//   shape: { borderRadius: 8 },
//   typography: { fontFamily: '"Inter", "system-ui", sans-serif', fontSize: 14 },
// })

// const { unstable_sxConfig: joySxConfig, ...joyTheme } = extendJoyTheme({
//   colorSchemes: {
//     light: {
//       palette: {
//         primary: {
//           50:  '#e8eef7',
//           100: '#c5d3ec',
//           200: '#9eb6e0',
//           300: '#7799d4',
//           400: '#5982cc',
//           500: '#3b6bc4',
//           600: '#2d5aad',
//           700: '#1e4491',
//           800: '#0f2d75',
//           900: '#0A2463',
//           solidBg: '#0A2463',
//           solidHoverBg: '#1e4491',
//           solidActiveBg: '#0f2d75',
//           outlinedColor: '#0A2463',
//           outlinedBorderColor: '#0A2463',
//           outlinedHoverBg: '#e8eef7',
//           softBg: '#e8eef7',
//           softColor: '#0A2463',
//         },
//         neutral: {
//           50:  '#f8f9fb',
//           100: '#edf0f4',
//           200: '#dce1ea',
//           300: '#c4cdd9',
//           400: '#97a6b8',
//           500: '#6b7d93',
//           600: '#4e5f74',
//           700: '#364455',
//           800: '#1f2d3d',
//           900: '#111c2b',
//         },
//         success: {
//           solidBg: '#1B998B',
//           solidHoverBg: '#158a7d',
//           softBg: '#e6f6f4',
//           softColor: '#0d6b60',
//         },
//         background: {
//           body: '#f8f9fb',
//           surface: '#ffffff',
//         },
//         divider: '#dce1ea',
//       },
//     },
//     dark: {
//       palette: {
//         primary: {
//           solidBg: '#1e4491',
//           solidHoverBg: '#2d5aad',
//         },
//         divider: '#364455',
//       },
//     },
//   },
//   fontFamily: {
//     body: '"Inter", "system-ui", sans-serif',
//     display: '"Inter", "system-ui", sans-serif',
//   },
//   components: {
//     JoyCard: {
//       styleOverrides: {
//         root: {
//           borderRadius: '12px',
//           boxShadow: '0 1px 3px rgba(0,0,0,0.08), 0 1px 2px rgba(0,0,0,0.04)',
//         },
//       },
//     },
//     JoyChip: {
//       styleOverrides: {
//         root: { fontWeight: 600, letterSpacing: '0.01em' },
//       },
//     },
//     JoyButton: {
//       styleOverrides: {
//         root: { borderRadius: '8px', fontWeight: 600 },
//       },
//     },
//     JoyInput: {
//       styleOverrides: {
//         root: { borderRadius: '8px' },
//       },
//     },
//   },
// })

// export const theme = {
//   ...muiTheme,
//   ...joyTheme,
//   colorSchemes: deepmerge(muiTheme.colorSchemes, joyTheme.colorSchemes),
//   typography: {
//     ...muiTheme.typography,
//     ...joyTheme.typography,
//   },
// } as unknown as ReturnType<typeof extendJoyTheme>

// theme.shouldSkipGeneratingVar = (...params) =>
//   shouldSkipGeneratingVar(params[0]) || muiShouldSkipGeneratingVar(params[0])

// theme.generateCssVars = (colorScheme) => ({
//   css: {
//     ...joyTheme.generateCssVars(colorScheme).css,
//   },
//   vars: deepmerge(
//     joyTheme.generateCssVars(colorScheme).vars,
//     muiTheme.generateThemeVars(),
//   ),
// })
