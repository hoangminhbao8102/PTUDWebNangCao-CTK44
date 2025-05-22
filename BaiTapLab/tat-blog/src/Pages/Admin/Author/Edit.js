import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Button, Form } from "react-bootstrap";
import { addAuthor, getAuthorById, updateAuthor } from "../../../Services/AuthorRepository";

const initialState = {
    fullName: "",
    email: "",
    notes: ""
};

const Edit = () => {
    const { id } = useParams();
    const isEdit = Boolean(id);
    const [author, setAuthor] = useState(initialState);
    const navigate = useNavigate();

    useEffect(() => {
        if (isEdit) {
            getAuthorById(id).then(res => {
                if (res) setAuthor(res);
            });
        }
    }, [id, isEdit]); // <-- thêm isEdit

    const handleChange = (e) => {
        const { name, value } = e.target;
        setAuthor({ ...author, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (isEdit) {
            await updateAuthor(id, author);
        } else {
            await addAuthor(author);
        }
        navigate("/admin/authors");
    };

    return (
        <Form onSubmit={handleSubmit}>
            <h2>{isEdit ? "Cập nhật" : "Thêm"} tác giả</h2>
            <Form.Group className="mb-3">
                <Form.Label>Họ tên</Form.Label>
                <Form.Control name="fullName" value={author.fullName} onChange={handleChange} required />
            </Form.Group>
            <Form.Group className="mb-3">
                <Form.Label>Email</Form.Label>
                <Form.Control name="email" type="email" value={author.email} onChange={handleChange} required />
            </Form.Group>
            <Form.Group className="mb-3">
                <Form.Label>Ghi chú</Form.Label>
                <Form.Control name="notes" value={author.notes} onChange={handleChange} />
            </Form.Group>
            <Button type="submit" variant="primary">Lưu</Button>
        </Form>
    );
};

export default Edit;
