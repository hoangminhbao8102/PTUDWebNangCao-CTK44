import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { addTag, getTagById, updateTag } from "../../../Services/TagRepository";
import { Form, Button } from "react-bootstrap";

const initialState = {
    name: "",
    urlSlug: "",
    description: "",
};

const Edit = () => {
    const [tag, setTag] = useState(initialState);
    const { id } = useParams();
    const isEdit = Boolean(id);
    const navigate = useNavigate();

    useEffect(() => {
        document.title = isEdit ? "Cập nhật thẻ" : "Thêm thẻ";
        if (isEdit) {
            getTagById(id).then(res => {
                if (res.isSuccess) setTag(res.result);
            });
        }
    }, [id, isEdit]); // <-- thêm isEdit

    const handleChange = (e) => {
        const { name, value } = e.target;
        setTag(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const request = isEdit ? updateTag(id, tag) : addTag(tag);
        request.then(res => {
            if (res.isSuccess) navigate("/admin/tags");
        });
    };

    return (
        <div className="container py-3">
            <h2>{isEdit ? "Cập nhật thẻ" : "Thêm thẻ mới"}</h2>
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Label>Tên thẻ</Form.Label>
                    <Form.Control
                        type="text"
                        name="name"
                        value={tag.name}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Slug</Form.Label>
                    <Form.Control
                        type="text"
                        name="urlSlug"
                        value={tag.urlSlug}
                        onChange={handleChange}
                    />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Mô tả</Form.Label>
                    <Form.Control
                        type="text"
                        name="description"
                        value={tag.description}
                        onChange={handleChange}
                    />
                </Form.Group>
                <Button type="submit">{isEdit ? "Cập nhật" : "Thêm mới"}</Button>
            </Form>
        </div>
    );
};

export default Edit;
