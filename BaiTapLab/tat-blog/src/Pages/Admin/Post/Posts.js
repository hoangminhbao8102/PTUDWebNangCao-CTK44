import React, { useEffect, useState, useCallback } from 'react';
import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button';
import { Link } from 'react-router-dom';
import {
    getPostsFilter,
    deletePostById,
    updatePostPublishStatus
} from '../../../Services/BlogRepository';
import Loading from '../../../Components/Loading';
import PostFilterPane from '../../../Components/Admin/PostFilterPane';
import { useSelector } from 'react-redux';

const Posts = () => {
    const [postsList, setPostsList] = useState([]);
    const [isVisibleLoading, setIsVisibleLoading] = useState(true);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalPages, setTotalPages] = useState(0);

    const postFilter = useSelector(state => state.postFilter);
    const pageSize = 10;

    // ✅ Dùng useCallback để tránh lỗi missing dependency
    const loadPosts = useCallback(() => {
        setIsVisibleLoading(true);

        getPostsFilter(
            postFilter.keyword,
            postFilter.authorId,
            postFilter.categoryId,
            postFilter.year,
            postFilter.month,
            pageSize,
            pageNumber,
            "", "", // sortColumn, sortOrder
            postFilter.unpublishedOnly
        ).then(data => {
            if (data) {
                setPostsList(data.items);
                setTotalPages(data.metadata.totalPages);
            } else {
                setPostsList([]);
                setTotalPages(0);
            }
            setIsVisibleLoading(false);
        });
    }, [postFilter, pageNumber]);

    useEffect(() => {
        document.title = "Danh sách bài viết";
        loadPosts();
    }, [loadPosts]); // ✅ không còn cảnh báo vì loadPosts dùng useCallback

    const handleDelete = async (id) => {
        if (window.confirm("Bạn có thật sự muốn xóa bài viết này không?")) {
            await deletePostById(id);
            loadPosts();
        }
    };

    const handleTogglePublish = async (id) => {
        await updatePostPublishStatus(id);
        loadPosts();
    };

    const renderPagination = () => (
        <div className="my-3">
            {Array.from({ length: totalPages }, (_, i) => (
                <Button
                    key={i}
                    className={`mx-1 ${i + 1 === pageNumber ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setPageNumber(i + 1)}
                >
                    {i + 1}
                </Button>
            ))}
        </div>
    );

    return (
        <>
            <h1>Danh sách bài viết</h1>
            <PostFilterPane />
            {isVisibleLoading ? <Loading /> :
                <>
                    <Table striped responsive bordered>
                        <thead>
                            <tr>
                                <th>Tiêu đề</th>
                                <th>Tác giả</th>
                                <th>Chủ đề</th>
                                <th>Xuất bản</th>
                                <th>Hành động</th>
                            </tr>
                        </thead>
                        <tbody>
                            {postsList.length > 0 ? postsList.map((item, index) =>
                                <tr key={index}>
                                    <td>
                                        <Link to={`/admin/posts/edit/${item.id}`} className='text-bold'>
                                            {item.title}
                                        </Link>
                                        <p className='text-muted'>{item.shortDescription}</p>
                                    </td>
                                    <td>{item.author.fullName}</td>
                                    <td>{item.category.name}</td>
                                    <td>
                                        <Button
                                            size="sm"
                                            variant={item.published ? "success" : "secondary"}
                                            onClick={() => handleTogglePublish(item.id)}
                                        >
                                            {item.published ? "Có" : "Không"}
                                        </Button>
                                    </td>
                                    <td>
                                        <Button
                                            size="sm"
                                            variant="danger"
                                            onClick={() => handleDelete(item.id)}
                                        >
                                            Xóa
                                        </Button>
                                    </td>
                                </tr>
                            ) : (
                                <tr>
                                    <td colSpan={5}>
                                        <h4 className='text-danger text-center'>Không tìm thấy bài viết nào</h4>
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </Table>
                    {renderPagination()}
                </>
            }
        </>
    );
};

export default Posts;
