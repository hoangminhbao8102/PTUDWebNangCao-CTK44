<template>
  <div>
    Xin chào {{ auth.user.fullName }} |
    <button @click="auth.logout()">Đăng xuất</button>
    <router-view />

    <router-link to="/change-password">🔒 Đổi mật khẩu</router-link>

    <!-- Liên kết giỏ hàng -->
    <router-link to="/cart">🛒 Giỏ hàng ({{ cart.totalQuantity }})</router-link>

    <!-- Liên kết quản lý đơn hàng chỉ hiển thị nếu là admin -->
    <router-link to="/admin/orders" v-if="auth.isAdmin">⚙️ Quản lý đơn hàng</router-link>
  </div>
</template>
