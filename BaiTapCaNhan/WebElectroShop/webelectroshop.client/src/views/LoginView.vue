<template>
  <div>
    <h2>Đăng nhập</h2>
    <input v-model="username" placeholder="Username" />
    <input v-model="password" placeholder="Password" type="password" />
    <button @click="handleLogin">Đăng nhập</button>

    <hr />

    <button @click="handleGoogleLogin">Đăng nhập bằng Google</button>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import { login, loginWithGoogle } from '../services/authService'
  import { googleTokenLogin } from 'vue3-google-login'

  const username = ref('')
  const password = ref('')

  async function handleLogin() {
    try {
      const result = await login(username.value, password.value)
      alert(`Chào mừng ${result.username}`)
      // Lưu token nếu muốn: localStorage.setItem('token', result.token)
    } catch (err) {
      alert('Sai tài khoản hoặc mật khẩu')
    }
  }

  async function handleGoogleLogin() {
    try {
      const googleResponse = await googleTokenLogin()
      if (googleResponse && googleResponse.credential) {
        const result = await loginWithGoogle(googleResponse.credential)
        alert(`Đăng nhập Google thành công: ${result.username}`)
        // Lưu token nếu muốn: localStorage.setItem('token', result.token)
      }
    } catch (err) {
      alert('Đăng nhập Google thất bại')
    }
  }
</script>
