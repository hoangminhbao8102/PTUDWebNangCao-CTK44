import { get_api, post_api, delete_api } from "./Methods";

export function getComments() {
    return get_api("https://localhost:7239/api/comments");
}

export function approveComment(id) {
    return post_api(`https://localhost:7239/api/comments/${id}/approve`);
}

export function deleteComment(id) {
    return delete_api(`https://localhost:7239/api/comments/${id}`);
}
