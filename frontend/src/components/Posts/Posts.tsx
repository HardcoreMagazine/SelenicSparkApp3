import { useState, useEffect } from 'react'
import { IPost } from './IPost'
import { format, parseISO } from 'date-fns'

function Posts() {
  const [posts, setPosts] = useState<IPost[]>();

  useEffect(() => {
    populateCollection();
  }, []); // when "[]" is not specified - program fetches data every 0.5s

  const mainPageContent = posts === undefined ? (
    <h1>Loading, please wait</h1>
  ) : (
    <ul>
      {posts.length > 0 && (
        posts.map(post => 
          <li key={post.id} className="border border-blue-400">
            {post.dateCreated} {post.author} {post.title}
          </li>
        )
      )}
      {(posts.length === 0) && (
        <li>Nope, nothing here</li>
      )}
    </ul>
  );

  return (
    <>
      <div>
        <button className="">Create new placeholder</button>
      </div>
      <div>
        {mainPageContent}
      </div>
      <div>
        &lt; pagination placeholder &gt;
      </div>
    </>
  );

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