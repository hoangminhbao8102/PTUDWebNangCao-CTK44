import React, { useEffect, useState } from "react";
import { getCategories, deleteCategory } from "../../../Services/CategoryRepository";
import { Table, Button } from "react-bootstrap";
import { Link } from "react-router-dom";

const Categories = () => {
    const [categories, setCategories] = useState([]);

    const fetchCategories = async () => {
        const res = await getCategories();
        if (res && res.isSuccess) setCategories(res.result);
    };

    const handleDelete = async (id) => {
        if (window.confirm("Bạn có chắc muốn xóa chủ đề này?")) {
            await deleteCategory(id);
            fetchCategories();
        }
    };

    useEffect(() => {
        document.title = "Danh sách chủ đề";
        fetchCategories();
    }, []);

    return (
        <div>
            <h3>Danh sách Chủ đề</h3>
            <Link to="/admin/categories/add" className="btn btn-primary mb-3">Thêm chủ đề</Link>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Tên</th>
                        <th>Slug</th>
                        <th>Hiện menu</th>
                        <th>Hành động</th>
                    </tr>
                </thead>
                <tbody>
                    {categories.map((cat) => (
                        <tr key={cat.id}>
                            <td>{cat.id}</td>
                            <td>{cat.name}</td>
                            <td>{cat.urlSlug}</td>
                            <td>{cat.showOnMenu ? "✔️" : "❌"}</td>
                            <td>
                                <Link to={`/admin/categories/edit/${cat.id}`} className="btn btn-warning btn-sm me-2">Sửa</Link>
                                <Button variant="danger" size="sm" onClick={() => handleDelete(cat.id)}>Xóa</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
}

export default Categories;