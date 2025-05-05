import { Card } from "react-bootstrap";
import { Link } from "react-router-dom";
import TagList from "./TagList";
import { isEmptyOrSpaces } from "../Utils/Utils";


const PostItem = ({ postItem }) => {
    let imageUrl = isEmptyOrSpaces(postItem.imageUrl)
        ? process.env.PUBLIC_URL + "/images/image_1.jpg"
        : postItem.imageUrl;

    let postedDate = new Date(postItem.postedDate);

    // Tạo URL cho bài viết
    const postUrl = `/blog/post/${postedDate.getFullYear()}/${postedDate.getMonth() + 1}/${postedDate.getDate()}/${postItem.urlSlug}`;
    
    // Tạo URL cho tác giả
    const authorUrl = `/blog/author/${postItem.author.urlSlug}`;

    // Tạo URL cho chuyên mục
    const categoryUrl = `/blog/category/${postItem.category.urlSlug}`;

    return (
        <article className="blog-entry mb-4">
            <Card>
                <div className="row g-0">
                    <div className="col-md-4">
                        <Card.Img variant="top" src={imageUrl} alt={postItem.title} />
                    </div>
                    <div className="col-md-8">
                        <Card.Body>
                            {/* Tiêu đề bài viết */}
                            <Card.Title>
                                <Link to={postUrl} className="text-decoration-none">
                                    {postItem.title}
                                </Link>
                            </Card.Title>

                            {/* Tên tác giả và chuyên mục */}
                            <Card.Text>
                                <small className="text-muted">Tác giả: </small>
                                <Link to={authorUrl} className="text-primary m-1 text-decoration-none">
                                    {postItem.author.fullName}
                                </Link>
                                <small className="text-muted">Chủ đề: </small>
                                <Link to={categoryUrl} className="text-primary m-1 text-decoration-none">
                                    {postItem.category.name}
                                </Link>
                            </Card.Text>

                            <Card.Text>
                                {postItem.shortDescription}
                            </Card.Text>

                            {/* Các tag */}
                            <div className="tag-list">
                                <TagList tags={postItem.tags} />
                            </div>

                            <div className="text-end mt-2 mb-4">
                                <Link to={postUrl} className="btn btn-primary" title={postItem.title}>
                                    Xem chi tiết
                                </Link>
                            </div>
                        </Card.Body>
                    </div>
                </div>
            </Card>
        </article>
    );
};

export default PostItem;
