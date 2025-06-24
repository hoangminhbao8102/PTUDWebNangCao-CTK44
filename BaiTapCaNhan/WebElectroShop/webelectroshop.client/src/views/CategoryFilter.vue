<template>
  <div>
    <label>Lọc danh mục:</label>
    <select v-model="selected" @change="filter">
      <option v-for="c in categories" :key="c.id" :value="c.id">
        {{ c.name }}
      </option>
    </select>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'

const selected = ref()
const categories = ref([])

const emit = defineEmits(['filtered'])

onMounted(async () => {
  categories.value = (await axios.get('/api/categories')).data
})

function filter() {
  emit('filtered', selected.value)
}
</script>
