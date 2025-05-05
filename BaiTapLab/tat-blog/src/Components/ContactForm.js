import React, { useState } from "react";
import { Form, Button, Alert } from "react-bootstrap";

const ContactForm = () => {
    const [formData, setFormData] = useState({
        fullName: "",
        email: "",
        subject: "",
        message: ""
    });

    const [submitted, setSubmitted] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prevState => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        // TODO: Xử lý gửi dữ liệu về server tại đây
        console.log("Dữ liệu gửi đi:", formData);
        setSubmitted(true);
        setFormData({
            fullName: "",
            email: "",
            subject: "",
            message: ""
        });
    };

    return (
        <>
            <h1 className="text-success mb-4">Liên hệ với chúng tôi</h1>
            {submitted && (
                <Alert variant="success" onClose={() => setSubmitted(false)} dismissible>
                    Gửi liên hệ thành công! Cảm ơn bạn đã phản hồi.
                </Alert>
            )}
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3" controlId="fullName">
                    <Form.Label>Họ tên</Form.Label>
                    <Form.Control
                        type="text"
                        name="fullName"
                        value={formData.fullName}
                        onChange={handleChange}
                        placeholder="Nhập họ tên"
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="email">
                    <Form.Label>Địa chỉ Email</Form.Label>
                    <Form.Control
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        placeholder="Nhập email"
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="subject">
                    <Form.Label>Chủ đề</Form.Label>
                    <Form.Control
                        type="text"
                        name="subject"
                        value={formData.subject}
                        onChange={handleChange}
                        placeholder="Nhập chủ đề"
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="message">
                    <Form.Label>Nội dung</Form.Label>
                    <Form.Control
                        as="textarea"
                        name="message"
                        value={formData.message}
                        onChange={handleChange}
                        placeholder="Nhập nội dung liên hệ"
                        rows={5}
                        required
                    />
                </Form.Group>

                <Button variant="success" type="submit">
                    Gửi đi
                </Button>
            </Form>
        </>
    );
};

export default ContactForm;
