<template>
  <div>
    <h2>Danh sách sản phẩm</h2>
    <ul v-if="products.length">
      <li v-for="product in products" :key="product.id">
        {{ product.name }} - {{ product.price }} VND
      </li>
    </ul>
    <span v-if="product.stock === 0" class="text-red-600">Hết hàng</span>
    <p v-else>Không có sản phẩm nào.</p>
  </div>
</template>

<CategoryFilter @filtered="filterByCategory" />

<script setup>
  import CategoryFilter from './CategoryFilter.vue'
  import axios from 'axios'
  import { ref, onMounted } from 'vue'
  import axios from 'axios'
import { getProducts } from '../services/productService'

  const keyword = ref('')
  const products = ref([])
  const page = ref(1)
  const totalPages = ref(1)

  async function fetchProducts() {
    const res = await axios.get('/api/products/search', {
      params: { keyword: keyword.value, page: page.value }
    })
    products.value = res.data.items
    totalPages.value = res.data.totalPages
  }

  async function filterByCategory(categoryId) {
    const res = await axios.get(`/api/products/category/${categoryId}`)
    products.value = res.data
  }

onMounted(async () => {
  products.value = await getProducts()
})
</script>
