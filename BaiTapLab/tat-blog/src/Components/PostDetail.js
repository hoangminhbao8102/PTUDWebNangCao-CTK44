import { useParams, Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { getPostBySlug } from "../Services/BlogRepository";
import { FacebookShareButton, TwitterShareButton, LinkedinShareButton, FacebookIcon, TwitterIcon, LinkedinIcon } from "react-share";
import TagList from "./TagList";
import CommentForm from "./CommentForm";
import CommentList from "./CommentList";
import { isEmptyOrSpaces } from "../Utils/Utils";

const PostDetail = () => {
    const { year, month, day, slug } = useParams();
    const [post, setPost] = useState(null);
    const [comments, setComments] = useState([]);

    useEffect(() => {
        document.title = "Chi tiết bài viết";

        getPostBySlug(year, month, day, slug).then(data => {
            if (data) setPost(data);
            else setPost(null);
        });
    }, [year, month, day, slug]);

    const handleAddComment = (comment) => {
        setComments([comment, ...comments]);
    };

    if (!post) {
        return <p className="text-center mt-4">Không tìm thấy bài viết.</p>;
    }

    // Quan trọng: khai báo biến ở đây (sau khi đảm bảo post tồn tại)
    const postUrl = `${window.location.origin}/blog/post/${year}/${month}/${day}/${slug}`;
    const imageUrl = isEmptyOrSpaces(post.imageUrl)
        ? process.env.PUBLIC_URL + "/images/image_1.jpg"
        : post.imageUrl;

    return (
        <div className="post-detail p-4">
            <h1>{post.title}</h1>
            <p>
                <strong>Tác giả:</strong>{" "}
                <Link to={`/blog/author/${post.author.urlSlug}`}>
                    {post.author.fullName}
                </Link>{" "}
                | <strong>Chuyên mục:</strong>{" "}
                <Link to={`/blog/category/${post.category.urlSlug}`}>
                    {post.category.name}
                </Link>{" "}
                | <strong>Ngày đăng:</strong>{" "}
                {new Date(post.postedDate).toLocaleDateString()}
            </p>

            <img src={imageUrl} alt={post.title} className="img-fluid mb-4" />

            <div dangerouslySetInnerHTML={{ __html: post.content }} />

            <div className="mt-4">
                <TagList tags={post.tags} />
            </div>

            {/* Chia sẻ bài viết */}
            <div className="d-flex align-items-center mt-4 mb-4">
                <span className="me-2">Chia sẻ:</span>
                <FacebookShareButton url={postUrl} quote={post.title} className="me-2">
                    <FacebookIcon size={32} round />
                </FacebookShareButton>
                <TwitterShareButton url={postUrl} title={post.title} className="me-2">
                    <TwitterIcon size={32} round />
                </TwitterShareButton>
                <LinkedinShareButton url={postUrl} title={post.title} summary={post.shortDescription} className="me-2">
                    <LinkedinIcon size={32} round />
                </LinkedinShareButton>
            </div>

            {/* Thảo luận và đánh giá */}
            <div className="mt-5">
                <h3>Thảo luận và đánh giá</h3>
                <CommentForm onSubmit={handleAddComment} />
                <CommentList comments={comments} />
            </div>
        </div>
    );
};

export default PostDetail;
