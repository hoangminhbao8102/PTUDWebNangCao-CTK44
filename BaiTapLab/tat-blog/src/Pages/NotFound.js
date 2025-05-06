import React, { useEffect } from "react";
import { Link } from "react-router-dom";
import { Container, Row, Col, Button } from "react-bootstrap";

const NotFound = () => {
    useEffect(() => {
        document.title = "404 - Không tìm thấy trang";
    }, []);

    return (
        <Container className="text-center py-5">
            <Row className="justify-content-center">
                <Col md={8}>
                    <h1 className="display-1">404</h1>
                    <p className="lead">
                        <span style={{ color: "red", fontWeight: "bold" }}>Chà!</span> Không tìm thấy trang rồi.
                    </p>
                    <p>Trang mà bạn đang tìm không tồn tại.</p>
                    <Link to="/">
                        <Button variant="outline-primary">Về trang chủ thôi!</Button>
                    </Link>
                </Col>
            </Row>
        </Container>
    );
};

export default NotFound;
