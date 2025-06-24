<template>
  <div>
    <h2>Quản lý danh mục</h2>
    <form @submit.prevent="save">
      <input v-model="category.name" placeholder="Tên danh mục" />
      <button type="submit">Lưu</button>
    </form>
    <ul>
      <li v-for="c in categories" :key="c.id">
        {{ c.name }}
        <button @click="edit(c)">Sửa</button>
        <button @click="remove(c.id)">Xóa</button>
      </li>
    </ul>
  </div>
</template>

<script setup>
import axios from 'axios'
import { ref, onMounted } from 'vue'

const categories = ref([])
const category = ref({ name: "" })

onMounted(async () => {
  categories.value = (await axios.get('/api/categories')).data
})

async function save() {
  if (category.value.id) {
    await axios.put(`/api/categories/${category.value.id}`, category.value)
  } else {
    await axios.post('/api/categories', category.value)
  }
  category.value = { name: "" }
  categories.value = (await axios.get('/api/categories')).data
}

function edit(c) {
  category.value = { ...c }
}

async function remove(id) {
  await axios.delete(`/api/categories/${id}`)
  categories.value = (await axios.get('/api/categories')).data
}
</script>
