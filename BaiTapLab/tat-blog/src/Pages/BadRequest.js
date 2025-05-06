import { Link } from "react-router-dom";
import { useQuery } from "../Utils/Utils";
import { Container, Row, Col, Button } from "react-bootstrap";
import { useEffect } from "react";

const BadRequest = () => {
    let query = useQuery(), redirect = query.get("redirectTo") ?? "/";

    useEffect(() => {
        document.title = "400 - Yêu cầu không hợp lệ";
    }, []);

    return (
        <Container className="text-center py-5">
            <Row className="justify-content-center">
                <Col md={8}>
                    <h1 className="display-1">400</h1>
                    <p className="lead">
                        <span style={{ color: "red", fontWeight: "bold" }}>Chà!</span> Yêu cầu không hợp lệ.
                    </p>
                    <p>Có vẻ tham số trong URL của bạn chưa đúng yêu cầu.</p>
                    <Link to={redirect}>
                        <Button variant="outline-primary">Về trang chủ thôi!</Button>
                    </Link>
                </Col>
            </Row>
        </Container>
    );
}

export default BadRequest;
