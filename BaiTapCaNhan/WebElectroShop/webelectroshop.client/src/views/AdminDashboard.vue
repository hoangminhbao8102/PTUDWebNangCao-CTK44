<template>
  <div class="admin-dashboard">
    <h2>Thống kê</h2>
    <ul>
      <li>Tổng đơn hàng: {{ stats.totalOrders }}</li>
      <li>Tổng sản phẩm: {{ stats.totalProducts }}</li>
      <li>Tổng người dùng: {{ stats.totalUsers }}</li>
      <li>Đơn hàng chờ xác nhận: {{ stats.pendingOrders }}</li>
    </ul>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import axios from 'axios'

const stats = ref({})

onMounted(async () => {
  const res = await axios.get('/api/admin/dashboard')
  stats.value = res.data
})
</script>
