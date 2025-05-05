import { useState } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

const CommentForm = ({ onSubmit }) => {
    const [author, setAuthor] = useState("");
    const [content, setContent] = useState("");
    const [rating, setRating] = useState(5);

    const handleSubmit = (e) => {
        e.preventDefault();
        if (author && content) {
            onSubmit({ author, content, rating });
            setAuthor("");
            setContent("");
            setRating(5);
        }
    };

    return (
        <div className="mt-5">
            <h5>Thảo luận và đánh giá</h5>
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3" controlId="formAuthor">
                    <Form.Label>Tên bạn</Form.Label>
                    <Form.Control 
                        type="text" 
                        placeholder="Nhập tên" 
                        value={author}
                        onChange={(e) => setAuthor(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formContent">
                    <Form.Label>Nội dung bình luận</Form.Label>
                    <Form.Control 
                        as="textarea" 
                        rows={3}
                        placeholder="Nhập nội dung"
                        value={content}
                        onChange={(e) => setContent(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formRating">
                    <Form.Label>Đánh giá</Form.Label>
                    <Form.Select
                        value={rating}
                        onChange={(e) => setRating(Number(e.target.value))}
                    >
                        {[5,4,3,2,1].map((star) => (
                            <option key={star} value={star}>
                                {star} sao
                            </option>
                        ))}
                    </Form.Select>
                </Form.Group>

                <Button variant="primary" type="submit">
                    Gửi bình luận
                </Button>
            </Form>
        </div>
    );
};

export default CommentForm;
