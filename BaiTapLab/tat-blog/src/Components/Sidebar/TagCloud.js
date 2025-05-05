import React, { useEffect, useState } from "react";
import axios from "axios";

const TagCloud = () => {
  const [tags, setTags] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7239/api/tags?pageNumber=1&pageSize=10")
      .then(res => {
        if (res.data.isSuccess) {
          setTags(res.data.result.items);
        } else {
          console.error("API error:", res.data);
        }
      })
      .catch(err => console.error("Request error:", err));
  }, []);

  return (
    <div className="mb-4">
      <h3 className="text-success mb-3">Tag Cloud</h3>
      <div>
        {tags.map(tag => (
          <a
            key={tag.id}
            className="btn btn-outline-success btn-sm m-1"
            href={`/blog/tag/${tag.urlSlug}`}
          >
            {tag.name}
          </a>
        ))}
      </div>
    </div>
  );
};

export default TagCloud;
