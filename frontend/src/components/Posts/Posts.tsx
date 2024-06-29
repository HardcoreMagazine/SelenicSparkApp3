import { Link } from 'react-router-dom'
import { useState, useEffect } from 'react';
import { Post } from './IPost'

function Posts() {
  const [posts, setPosts] = useState<Post[]>();

  useEffect(() => {
    populateCollection();
  }, []);

  const someContent = posts === undefined ? <h1>Loading, please wait</h1> : (
    <table className="table table-striped" aria-labelledby="tableLabel">
      <thead>
        <tr>
          <th>ID</th>
          <th>Title</th>
          <th>Author</th>
          <th>DateCreated</th>
        </tr>
      </thead>
      <tbody>
        {posts.map(p =>
          <Link to={"/post/" + p.ID}>
            <tr key={p.ID}>
              <td>{p.Title}</td>
              <td>{p.Author}</td>
              <td>{p.DateCreated}</td>
            </tr>
          </Link>
        )}
      </tbody>
    </table>
  );

  return someContent;

  async function populateCollection() {
    const res = await fetch('https://localhost:46801/post');
    const data = await res.json();
    setPosts(data);
    //fetch('/post')
    //  .then(res => res.json())
    //  .then(json => setPosts(json))
    //  .catch(error => console.error(error));
  }
}

export default Posts;