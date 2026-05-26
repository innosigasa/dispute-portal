import Box from '@mui/joy/Box'
import Button from '@mui/joy/Button'
import Card from '@mui/joy/Card'
import FormControl from '@mui/joy/FormControl'
import FormHelperText from '@mui/joy/FormHelperText'
import FormLabel from '@mui/joy/FormLabel'
import Input from '@mui/joy/Input'
import Typography from '@mui/joy/Typography'
import AccountBalanceIcon from '@mui/icons-material/AccountBalance'
import LockIcon from '@mui/icons-material/Lock'
import EmailIcon from '@mui/icons-material/Email'
import { useFormik } from 'formik'
import * as Yup from 'yup'
import { useNavigate } from 'react-router-dom'
import toast from 'react-hot-toast'
import { login } from '../services/auth.service'
import { useAuth } from '../../../store/authStore'

const schema = Yup.object({
  email: Yup.string().email('Invalid email').required('Email is required'),
  password: Yup.string().required('Password is required'),
})

export function LoginPage() {
  const { login: storeLogin } = useAuth()
  const navigate = useNavigate()

  const formik = useFormik({
    initialValues: { email: '', password: '' },
    validationSchema: schema,
    onSubmit: async (values, helpers) => {
      const result = await login(values.email, values.password)
      if (!result.isSuccessful || !result.data) {
        toast.error(result.error ?? result.message ?? 'Invalid email or password.')
        helpers.setSubmitting(false)
        return
      }
      storeLogin(result.data.accessToken, result.data.refreshToken, result.data.role)
      navigate(result.data.role === 'agent' ? '/agent/disputes' : '/dashboard', { replace: true })
    },
  })

  return (
    <Box
      sx={{
        display: 'flex',
        minHeight: '100vh',
        width: '100%',
      }}
    >
      {/* Left brand panel */}
      <Box
        sx={{
          display: { xs: 'none', md: 'flex' },
          flex: 1,
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          background: 'linear-gradient(145deg, #0A2463 0%, #1e4491 60%, #1B998B 100%)',
          p: 6,
          gap: 3,
        }}
      >
        <AccountBalanceIcon sx={{ color: '#fff', fontSize: 64, opacity: 0.9 }} />
        <Typography level="h2" sx={{ color: '#fff', fontWeight: 800, textAlign: 'center', lineHeight: 1.2 }}>
          Dispute Portal
        </Typography>
        <Typography level="body-md" sx={{ color: 'rgba(255,255,255,0.7)', textAlign: 'center', maxWidth: 300 }}>
          Manage your banking disputes with transparency and ease.
        </Typography>
        <Box sx={{ mt: 4, display: 'flex', flexDirection: 'column', gap: 1.5 }}>
          {['View all your accounts in one place', 'Raise and track disputes', 'Real-time status updates'].map(text => (
            <Box key={text} sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <Box sx={{ width: 6, height: 6, borderRadius: '50%', bgcolor: '#1B998B', flexShrink: 0 }} />
              <Typography level="body-sm" sx={{ color: 'rgba(255,255,255,0.8)' }}>{text}</Typography>
            </Box>
          ))}
        </Box>
      </Box>

      {/* Right form panel */}
      <Box
        sx={{
          flex: { xs: 1, md: '0 0 440px' },
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          bgcolor: '#F4F6F9',
          p: 4,
        }}
      >
        <Card sx={{ width: '100%', maxWidth: 380, p: 4, gap: 0 }}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, mb: 0.5 }}>
            <LockIcon sx={{ color: 'primary.600', fontSize: 22 }} />
            <Typography level="h4" fontWeight={800}>Sign In</Typography>
          </Box>
          <Typography level="body-sm" textColor="neutral.500" mb={3}>
            Enter your credentials to continue
          </Typography>

          <form onSubmit={formik.handleSubmit}>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <FormControl error={formik.touched.email && !!formik.errors.email}>
                <FormLabel>Email address</FormLabel>
                <Input
                  type="email"
                  startDecorator={<EmailIcon sx={{ fontSize: 18 }} />}
                  {...formik.getFieldProps('email')}
                />
                {formik.touched.email && formik.errors.email && (
                  <FormHelperText>{formik.errors.email}</FormHelperText>
                )}
              </FormControl>

              <FormControl error={formik.touched.password && !!formik.errors.password}>
                <FormLabel>Password</FormLabel>
                <Input
                  type="password"
                  startDecorator={<LockIcon sx={{ fontSize: 18 }} />}
                  {...formik.getFieldProps('password')}
                />
                {formik.touched.password && formik.errors.password && (
                  <FormHelperText>{formik.errors.password}</FormHelperText>
                )}
              </FormControl>

              <Button type="submit" loading={formik.isSubmitting} fullWidth sx={{ mt: 1 }}>
                Sign In
              </Button>
            </Box>
          </form>
        </Card>

        <Typography level="body-xs" textColor="neutral.400" mt={3}>
          Powered by DisputePortal © {new Date().getFullYear()}
        </Typography>
      </Box>
    </Box>
  )
}
