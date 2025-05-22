import { get_api, post_api, put_api, delete_api } from "./Methods";

export function getTags(name = "", pageSize = 10, pageNumber = 1) {
    return get_api(`https://localhost:7239/api/tags?Name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}`);
}

export function getTagById(id) {
    return get_api(`https://localhost:7239/api/api/tags/${id}`);
}

export function addTag(tag) {
    return post_api(`https://localhost:7239/api/api/tags`, tag);
}

export function updateTag(id, tag) {
    return put_api(`https://localhost:7239/api/api/tags/${id}`, tag);
}

export function deleteTag(id) {
    return delete_api(`https://localhost:7239/api/api/tags/${id}`);
}
