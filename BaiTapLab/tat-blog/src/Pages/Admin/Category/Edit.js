import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getCategoryById, addCategory, updateCategory } from "../../../Services/CategoryRepository";
import { Form, Button } from "react-bootstrap";

const initialState = {
    name: "",
    urlSlug: "",
    description: "",
    showOnMenu: false,
};

const Edit = () => {
    const [category, setCategory] = useState(initialState);
    const { id } = useParams();
    const navigate = useNavigate();
    const isEdit = Boolean(id);

    useEffect(() => {
        document.title = isEdit ? "Cập nhật chủ đề" : "Thêm chủ đề";
        if (isEdit) {
            getCategoryById(id).then((res) => {
                if (res.isSuccess) setCategory(res.result);
            });
        }
    }, [id, isEdit]); // ✅ đã thêm isEdit

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setCategory({
            ...category,
            [name]: type === "checkbox" ? checked : value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const apiCall = isEdit ? updateCategory(id, category) : addCategory(category);
        const res = await apiCall;
        if (res.isSuccess) navigate("/admin/categories");
    };

    return (
        <Form onSubmit={handleSubmit}>
            <h3>{isEdit ? "Cập nhật" : "Thêm"} Chủ đề</h3>

            <Form.Group className="mb-3">
                <Form.Label>Tên chủ đề</Form.Label>
                <Form.Control
                    type="text"
                    name="name"
                    value={category.name}
                    onChange={handleChange}
                    required
                />
            </Form.Group>

            <Form.Group className="mb-3">
                <Form.Label>Slug</Form.Label>
                <Form.Control
                    type="text"
                    name="urlSlug"
                    value={category.urlSlug}
                    onChange={handleChange}
                    required
                />
            </Form.Group>

            <Form.Group className="mb-3">
                <Form.Label>Mô tả</Form.Label>
                <Form.Control
                    as="textarea"
                    rows={3}
                    name="description"
                    value={category.description}
                    onChange={handleChange}
                />
            </Form.Group>

            <Form.Group className="mb-3">
                <Form.Check
                    type="checkbox"
                    name="showOnMenu"
                    label="Hiển thị trên menu"
                    checked={category.showOnMenu}
                    onChange={handleChange}
                />
            </Form.Group>

            <Button variant="primary" type="submit">
                {isEdit ? "Cập nhật" : "Thêm mới"}
            </Button>
        </Form>
    );
};

export default Edit;
