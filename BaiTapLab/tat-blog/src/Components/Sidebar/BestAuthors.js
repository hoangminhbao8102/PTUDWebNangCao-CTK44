import React, { useEffect, useState } from "react";
import axios from "axios";

const BestAuthors = () => {
  const [authors, setAuthors] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7239/api/authors/best/4")
      .then(res => setAuthors(res.data.result))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="mb-4">
      <h3 className="text-success mb-3">Tác giả nổi bật</h3>
      <ul className="list-group">
        {authors.map(author => (
          <li key={author.id} className="list-group-item">
            <a href={`/blog/author/${author.urlSlug}`}>
              {author.fullName}
            </a>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default BestAuthors;
