import axios from 'axios'

export async function login(username, password) {
  const res = await axios.post('/api/auth/login', { username, password })
  localStorage.setItem('token', res.data.token)
  return res.data
}

export async function register(username, password, role = "Customer") {
  return (await axios.post('/api/auth/register', { username, password, role })).data
}

// ✅ Hàm đăng nhập Google thêm vào
export async function loginWithGoogle(googleToken) {
  const res = await axios.post('/google-login', googleToken, {
    headers: { 'Content-Type': 'application/json' }
  })
  localStorage.setItem('token', res.data.token) // ✅ Lưu token như login thường
  return res.data
}
