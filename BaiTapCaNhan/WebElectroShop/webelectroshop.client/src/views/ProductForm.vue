<template>
  <div>
    <h3>{{ product.id ? "Cập nhật" : "Thêm" }} sản phẩm</h3>
    <form @submit.prevent="save">
      <input v-model="product.name" placeholder="Tên sản phẩm" required />
      <input v-model="product.price" placeholder="Giá" type="number" required />
      <input v-model="product.imageUrl" placeholder="Hình ảnh" />
      <textarea v-model="product.description" placeholder="Mô tả"></textarea>
      <button type="submit">Lưu</button>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { addProduct, updateProduct } from '../services/productService'

const props = defineProps(['initialProduct'])
const emit = defineEmits(['saved'])

const product = ref({ ...props.initialProduct })

async function save() {
  if (product.value.id) {
    await updateProduct(product.value)
  } else {
    await addProduct(product.value)
  }
  emit('saved')
}
</script>
