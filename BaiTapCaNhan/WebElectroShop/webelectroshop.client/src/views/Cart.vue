<template>
  <div>
    <h2>Giỏ hàng</h2>
    <ul>
      <li v-for="item in cart" :key="item.id">
        {{ item.name }} - {{ item.quantity }} x {{ item.price.toLocaleString() }} VND
      </li>
    </ul>
    <p><strong>Tổng tiền: {{ total.toLocaleString() }} VND</strong></p>
    <button @click="checkout">Đặt hàng và thanh toán Momo</button>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted } from 'vue'
  import { createOrder } from '../services/orderService'

  const cart = ref([])

  onMounted(() => {
    cart.value = JSON.parse(localStorage.getItem('cart') || '[]')
  })

  const total = computed(() =>
    cart.value.reduce((sum, item) => sum + item.price * item.quantity, 0)
  )

  async function checkout() {
    const order = {
      customerName: "Khách hàng demo",
      items: cart.value.map(item => ({
        productId: item.id,
        quantity: item.quantity
      }))
    }

    try {
      const res = await createOrder(order)

      // Gọi API backend tạo URL thanh toán Momo (giả định backend trả về paymentUrl)
      const response = await fetch('/api/payments/momo', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ orderId: res.id, amount: total.value })
      })

      const data = await response.json()
      if (data.paymentUrl) {
        window.location.href = data.paymentUrl
      } else {
        alert("Không lấy được URL thanh toán Momo.")
      }
    } catch (err) {
      console.error(err)
      alert("Đặt hàng thất bại.")
    }
  }
</script>
