import { get_api, post_api, put_api, delete_api } from './Methods';

const API_URL = 'https://localhost:7239/api/categories';

export function getCategories() {
    return get_api(API_URL);
}

export function getCategoryById(id) {
    return get_api(`${API_URL}/${id}`);
}

export function addCategory(category) {
    return post_api(API_URL, category);
}

export function updateCategory(id, category) {
    return put_api(`${API_URL}/${id}`, category);
}

export function deleteCategory(id) {
    return delete_api(`${API_URL}/${id}`);
}
