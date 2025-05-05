import React, { useEffect, useState } from "react";
import axios from "axios";

const FeaturedPosts = () => {
  const [posts, setPosts] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7239/api/posts/featured/3")
      .then(res => setPosts(res.data.result))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="mb-4">
      <h3 className="text-success mb-3">Bài viết nổi bật</h3>
      <ul className="list-group">
        {posts.map(post => (
          <li key={post.id} className="list-group-item">
            <a href={`/blog/post/${post.postedDate.split('T')[0].replace(/-/g, '/')}/${post.urlSlug}`}>
              {post.title}
            </a>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default FeaturedPosts;
