import axios from 'axios';

const API_URL = 'https://localhost:7114/api/products';

export async function getProducts() {
  const res = await axios.get(API_URL);
  return res.data;
}

export async function getProduct(id) {
  return (await axios.get(`${API_URL}/${id}`)).data;
}

export async function addProduct(product) {
  return (await axios.post(API_URL, product)).data;
}

export async function updateProduct(product) {
  return (await axios.put(`${API_URL}/${product.id}`, product)).data;
}

export async function deleteProduct(id) {
  return (await axios.delete(`${API_URL}/${id}`)).data;
}
