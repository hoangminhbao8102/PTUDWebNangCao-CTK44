import { useState } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import axios from "axios";

const SubscribeForm = () => {
    const [email, setEmail] = useState("");
    const [message, setMessage] = useState("");
    const [isSuccess, setIsSuccess] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!email) {
            setMessage("Vui lòng nhập email.");
            setIsSuccess(false);
            return;
        }

        try {
            const response = await axios.post("https://localhost:7239/api/subscribers/subscribe", {
                email: email,
            });

            if (response.data.isSuccess) {
                setMessage("Đăng ký thành công!");
                setIsSuccess(true);
                setEmail("");
            } else {
                setMessage(response.data.errors?.join(", ") || "Đăng ký thất bại!");
                setIsSuccess(false);
            }
        } catch (error) {
            console.error("Error subscribing:", error);
            setMessage("Đã xảy ra lỗi, vui lòng thử lại sau.");
            setIsSuccess(false);
        }
    };

    return (
        <div className="p-3 border rounded bg-light mt-4">
            <h5 className="text-success mb-3">Đăng ký nhận bài viết mới</h5>
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3">
                    <Form.Control
                        type="email"
                        placeholder="Nhập địa chỉ email của bạn"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                </Form.Group>
                <Button type="submit" variant="success" className="w-100">
                    Đăng ký
                </Button>
            </Form>
            {message && (
                <div className={`mt-3 alert ${isSuccess ? "alert-success" : "alert-danger"}`} role="alert">
                    {message}
                </div>
            )}
        </div>
    );
};

export default SubscribeForm;
