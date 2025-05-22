import React, { useEffect, useState } from "react";
import { Button, Table } from "react-bootstrap";
import { getTags, deleteTag } from "../../../Services/TagRepository";
import { useNavigate } from "react-router-dom";

const Tags = () => {
    const [tags, setTags] = useState([]);
    const navigate = useNavigate();

    const fetchTags = () => {
        getTags().then(res => {
            if (res.isSuccess) setTags(res.result.items);
        });
    };

    useEffect(() => {
        document.title = "Danh sách thẻ";
        fetchTags();
    }, []);

    const handleDelete = (id) => {
        if (window.confirm("Xác nhận xóa thẻ?")) {
            deleteTag(id).then(res => {
                if (res.isSuccess) fetchTags();
            });
        }
    };

    return (
        <div className="container py-3">
            <h2>Danh sách thẻ</h2>
            <Button className="mb-3" onClick={() => navigate("/admin/tags/add")}>Thêm thẻ</Button>
            <Table striped bordered>
                <thead>
                    <tr>
                        <th>Tên</th>
                        <th>Slug</th>
                        <th>Mô tả</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {tags.map(tag => (
                        <tr key={tag.id}>
                            <td>{tag.name}</td>
                            <td>{tag.urlSlug}</td>
                            <td>{tag.description}</td>
                            <td>
                                <Button size="sm" onClick={() => navigate(`/admin/tags/edit/${tag.id}`)}>Sửa</Button>{' '}
                                <Button size="sm" variant="danger" onClick={() => handleDelete(tag.id)}>Xóa</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default Tags;
