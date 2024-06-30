import { useState, useEffect } from 'react'
import { IPost } from './IPost'
import { format, parseISO } from 'date-fns'

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
    data.forEach(post => post.dateCreated = formatDtString(post.dateCreated));
    setPosts(data); 
  }

  function formatDtString(dts: string): string {
    const parsedDt = parseISO(dts);
    return format(parsedDt, "dd.MM.yyyy HH:mm 'UTC'")
  }
}

export default Posts;