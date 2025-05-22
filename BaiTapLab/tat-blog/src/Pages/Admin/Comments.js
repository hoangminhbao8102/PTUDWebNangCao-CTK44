import React, { useEffect, useState } from "react";
import { Table, Button } from "react-bootstrap";
import { getComments, approveComment, deleteComment } from "../../Services/CommentRepository";

const Comments = () => {
    const [comments, setComments] = useState([]);

    const fetchComments = () => {
        getComments().then(res => {
            if (res.isSuccess) setComments(res.result);
        });
    };

    useEffect(() => {
        document.title = "Quản lý bình luận";
        fetchComments();
    }, []);

    const handleApprove = (id) => {
        approveComment(id).then(() => fetchComments());
    };

    const handleDelete = (id) => {
        if (window.confirm("Bạn có chắc muốn xóa bình luận này?")) {
            deleteComment(id).then(() => fetchComments());
        }
    };

    return (
        <div>
            <h3>Danh sách bình luận</h3>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Người gửi</th>
                        <th>Nội dung</th>
                        <th>Trạng thái</th>
                        <th>Thao tác</th>
                    </tr>
                </thead>
                <tbody>
                    {comments.map(comment => (
                        <tr key={comment.id}>
                            <td>{comment.userName}</td>
                            <td>{comment.content}</td>
                            <td>{comment.approved ? "Đã duyệt" : "Chờ duyệt"}</td>
                            <td>
                                {!comment.approved && (
                                    <Button variant="success" size="sm" onClick={() => handleApprove(comment.id)}>
                                        Duyệt
                                    </Button>
                                )}
                                <Button variant="danger" size="sm" className="ms-2" onClick={() => handleDelete(comment.id)}>
                                    Xóa
                                </Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default Comments;
