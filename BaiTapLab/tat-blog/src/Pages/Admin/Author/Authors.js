import React, { useEffect, useState } from "react";
import { Button, Table } from "react-bootstrap";
import { Link } from "react-router-dom";
import { deleteAuthor, getAuthors } from "../../../Services/AuthorRepository";

const Authors = () => {
    const [authors, setAuthors] = useState([]);

    const fetchAuthors = async () => {
        const res = await getAuthors();
        if (res && res.items) setAuthors(res.items);
    };

    const handleDelete = async (id) => {
        if (window.confirm("Bạn có chắc chắn muốn xóa tác giả này không?")) {
            await deleteAuthor(id);
            fetchAuthors();
        }
    };

    useEffect(() => {
        fetchAuthors();
    }, []);

    return (
        <div>
            <h2>Danh sách tác giả</h2>
            <Link to="/admin/authors/add" className="btn btn-primary mb-2">
                Thêm tác giả
            </Link>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Họ tên</th>
                        <th>Email</th>
                        <th>Ghi chú</th>
                        <th>Hành động</th>
                    </tr>
                </thead>
                <tbody>
                    {authors.map((author) => (
                        <tr key={author.id}>
                            <td>{author.fullName}</td>
                            <td>{author.email}</td>
                            <td>{author.notes}</td>
                            <td>
                                <Link to={`/admin/authors/edit/${author.id}`} className="btn btn-warning btn-sm me-2">Sửa</Link>
                                <Button variant="danger" size="sm" onClick={() => handleDelete(author.id)}>Xóa</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
}

export default Authors;