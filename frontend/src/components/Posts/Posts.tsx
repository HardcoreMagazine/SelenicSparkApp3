import { useState, useEffect } from 'react'
import { IPost } from './IPost'

function Posts() {
  const [posts, setPosts] = useState<IPost[]>();

  useEffect(() => { 
    populateCollection();
  });

  const pageContent = posts === undefined ? <h1>Loading, please wait</h1> : (
    <ul>
      {posts.map(post => 
        <li key={post.id}>
          {post.dateCreated} {post.author} {post.title}
        </li>
      )}
    </ul>
  );

  return pageContent;

  async function populateCollection() {
    const res = await fetch('https://localhost:46801/post');
    const data: IPost[] = await res.json();
    setPosts(data); 
  }
}

export default Posts;