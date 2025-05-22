import { get_api, post_api, put_api, delete_api } from "./Methods";

export function getAuthors() {
    return get_api("https://localhost:7239/api/authors?PageSize=10&PageNumber=1");
}

export function getAuthorById(id) {
    return get_api(`https://localhost:7239/api/authors/${id}`);
}

export function addAuthor(data) {
    return post_api("https://localhost:7239/api/authors", data);
}

export function updateAuthor(id, data) {
    return put_api(`https://localhost:7239/api/authors/${id}`, data);
}

export function deleteAuthor(id) {
    return delete_api(`https://localhost:7239/api/authors/${id}`);
}
