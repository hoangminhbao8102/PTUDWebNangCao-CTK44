import React, { useEffect } from "react";
import ContactForm from "../Components/ContactForm";
import { Container, Row, Col } from "react-bootstrap";

const Contact = () => {
    useEffect(() => {
        document.title = "Liên hệ";
    }, []);

    return (
        <Container className="py-4">
            <Row className="justify-content-center">
                <Col md={8}>
                    <h1 className="text-success mb-4">Đây là trang liên hệ</h1>
                    <ContactForm />
                </Col>
            </Row>
        </Container>
    );
};

export default Contact;
