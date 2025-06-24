import axios from 'axios'

const API_URL = 'https://localhost:7114/api/orders'

export async function createOrder(order) {
  return (await axios.post(API_URL, order)).data
}

export async function getAllOrders() {
  return (await axios.get('/api/orders')).data
}

export async function updateOrderStatus(id, status) {
  return (await axios.put(`/api/orders/${id}/status`, status)).data
}
