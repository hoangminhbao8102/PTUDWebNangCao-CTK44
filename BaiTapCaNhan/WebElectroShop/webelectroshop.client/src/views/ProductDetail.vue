<template>
  <div v-if="product">
    <h2>{{ product.name }}</h2>
    <img :src="product.imageUrl" width="150" />
    <p>{{ product.description }}</p>
    <p><strong>{{ product.price }} VND</strong></p>
    <button @click="addToCart">Thêm vào giỏ</button>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getProduct } from '../services/productService'

const route = useRoute()
const product = ref(null)

onMounted(async () => {
  product.value = await getProduct(route.params.id)
})

function addToCart() {
  const cart = JSON.parse(localStorage.getItem('cart') || '[]')
  cart.push({ ...product.value, quantity: 1 })
  localStorage.setItem('cart', JSON.stringify(cart))
  alert("Đã thêm vào giỏ")
}
</script>
