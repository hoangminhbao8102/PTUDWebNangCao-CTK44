<template>
  <nav>
    <router-link to="/">Trang chủ</router-link>
    <router-link v-if="isAdmin" to="/admin">Quản lý</router-link>
    <router-link v-if="isAdmin" to="/admin/dashboard">Admin</router-link>
    <router-link v-if="!isAdmin" to="/cart">Giỏ hàng</router-link>
  </nav>
</template>

<script setup>
  const isAdmin = localStorage.getItem('role') === 'Admin';
</script>
