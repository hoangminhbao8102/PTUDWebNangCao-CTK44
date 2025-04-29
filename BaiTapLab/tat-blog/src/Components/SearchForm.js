import { useRef } from "react"; // <-- đã bỏ useState
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";

const SearchForm = () => {
    const keyword = useRef("");

    const handleSubmit = (e) => {
        e.preventDefault();
        window.location = `/blog/?k=${keyword.current.value}`;
    };

    return (
        <div className="pt-4 ps-2">
            <Form method="get" onSubmit={handleSubmit}>
                <Form.Group className="input-group mb-3">
                    <h3 className="text-success mb-2">Tìm kiếm bài viết</h3>
                    <Form.Control
                        type="text"
                        name="k"
                        ref={keyword}
                        aria-label="Enter keyword"
                        aria-describedby="btnSearchPost"
                        placeholder="Enter keyword"
                    />
                    <Button
                        id="btnSearchPost"
                        variant="outline-secondary"
                        type="submit">
                        <FontAwesomeIcon icon={faSearch} />
                    </Button>
                </Form.Group>
            </Form>
        </div>
    );
}

export default SearchForm;
