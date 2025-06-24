<template>
  <div>
    <h2>Danh sách đơn hàng</h2>
    <table>
      <tr>
        <th>ID</th>
        <th>Khách hàng</th>
        <th>Trạng thái</th>
        <th>Sản phẩm</th>
        <th>Cập nhật</th>
      </tr>
      <tr v-for="order in orders" :key="order.id">
        <td>{{ order.id }}</td>
        <td>{{ order.customerName }}</td>
        <td>
          <select v-model="order.status">
            <option>Pending</option>
            <option>Confirmed</option>
            <option>Shipping</option>
            <option>Delivered</option>
            <option>Canceled</option>
          </select>
        </td>
        <td>
          <ul>
            <li v-for="item in order.items" :key="item.id">
              {{ item.product.name }} x {{ item.quantity }}
            </li>
          </ul>
        </td>
        <td><button @click="update(order)">Lưu</button></td>
      </tr>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getAllOrders, updateOrderStatus } from '../services/orderService'

const orders = ref([])

onMounted(async () => {
  orders.value = await getAllOrders()
})

async function update(order) {
  await updateOrderStatus(order.id, order.status)
  alert("Đã cập nhật!")
}
</script>
