import Card from "react-bootstrap/Card";

const CommentList = ({ comments }) => {
    if (comments.length === 0) {
        return <p>Chưa có bình luận nào. Hãy là người đầu tiên!</p>;
    }

    return (
        <div className="mt-4">
            <h5>Các bình luận</h5>
            {comments.map((comment, index) => (
                <Card key={index} className="mb-3">
                    <Card.Body>
                        <Card.Title>{comment.author}</Card.Title>
                        <Card.Subtitle className="mb-2 text-muted">
                            Đánh giá: {comment.rating} sao
                        </Card.Subtitle>
                        <Card.Text>{comment.content}</Card.Text>
                    </Card.Body>
                </Card>
            ))}
        </div>
    );
};

export default CommentList;
