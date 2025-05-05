import React, { useEffect, useState } from "react";
import axios from "axios";

const Archives = () => {
  const [archives, setArchives] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7239/api/posts/archives/60")
      .then(res => setArchives(res.data.result))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="mb-4">
      <h3 className="text-success mb-3">Bài viết gần nhất</h3>
      <ul className="list-group">
        {archives.map((archive, index) => (
          <li key={index} className="list-group-item d-flex justify-content-between align-items-center">
            <a href={`/blog/archives/${archive.year}/${archive.month}`}>
              {archive.monthName} {archive.year}
            </a>
            <span className="badge bg-success rounded-pill">
              {archive.postCount}
            </span>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Archives;
