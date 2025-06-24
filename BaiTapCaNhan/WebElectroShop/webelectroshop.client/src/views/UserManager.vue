<template>
  <div>
    <h2>Quản lý người dùng</h2>
    <table>
      <tr><th>Username</th><th>Role</th><th>Cập nhật</th></tr>
      <tr v-for="user in users" :key="user.id">
        <td>{{ user.username }}</td>
        <td>
          <select v-model="user.role">
            <option>Admin</option>
            <option>Customer</option>
          </select>
        </td>
        <td><button @click="update(user)">Lưu</button></td>
      </tr>
    </table>
  </div>
</template>

<script setup>
import axios from 'axios'
import { ref, onMounted } from 'vue'

const users = ref([])

onMounted(async () => {
  users.value = await axios.get('/api/users')
})

async function update(user) {
  await axios.put(`/api/users/${user.id}`, `"${user.role}"`, {
    headers: { 'Content-Type': 'application/json' }
  })
}
</script>
