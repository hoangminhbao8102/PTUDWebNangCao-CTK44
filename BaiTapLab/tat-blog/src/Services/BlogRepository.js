import { get_api, post_api } from "./Methods";

export function getPosts(keyword = "", pageSize = 10, pageNumber = 1, sortColumn = "", sortOrder = "") {
    return get_api(`https://localhost:7239/api/posts?Keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}

export function getAuthors(name = "", pageSize = 10, pageNumber = 1, sortColumn = "", sortOrder = "") {
    return get_api(`https://localhost:7239/api/authors?Name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}

export function getFilter() {
    return get_api(`https://localhost:7239/api/posts/get-filter`);
}

export function getPostsFilter(
    keyword = "",
    authorId = "",
    categoryId = "",
    year = "",
    month = "",
    pageSize = 10,
    pageNumber = 1,
    sortColumn = "",
    sortOrder = "",
    unpublishedOnly = false // ✅ thêm tham số
) {
    let url = new URL(`https://localhost:7239/api/posts/get-posts-filter`);
    
    if (keyword) url.searchParams.append("Keyword", keyword);
    if (authorId) url.searchParams.append("AuthorId", authorId);
    if (categoryId) url.searchParams.append("CategoryId", categoryId);
    if (year) url.searchParams.append("Year", year);
    if (month) url.searchParams.append("Month", month);
    if (sortColumn) url.searchParams.append("SortColumn", sortColumn);
    if (sortOrder) url.searchParams.append("SortOrder", sortOrder);
    if (unpublishedOnly) url.searchParams.append("Published", false); // ✅ truyền Published=false

    url.searchParams.append("PageSize", pageSize);
    url.searchParams.append("PageNumber", pageNumber);

    return get_api(url.href);
}

export function getPostById(id = 0) {
    if (id > 0) {
        return get_api(`https://localhost:7239/api/posts/${id}`);
    }
    // Trả về Promise resolve null để tránh lỗi .then
    return Promise.resolve(null);
}

export function getPostBySlug(year, month, day, slug) {
    return get_api(`https://localhost:7239/api/posts/${year}/${month}/${day}/${slug}`);
}

export function addOrUpdatePost(formData) {
    return post_api(`https://localhost:7239/api/posts`, formData);
}

export function deletePostById(id) {
    return fetch(`https://localhost:7239/api/posts/${id}`, {
        method: "DELETE"
    }).then(response => response.ok);
}

export function updatePostPublishStatus(id) {
    return fetch(`https://localhost:7239/api/posts/toggle-publish/${id}`, {
        method: "POST"
    }).then(response => response.ok);
}
