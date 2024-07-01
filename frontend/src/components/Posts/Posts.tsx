import { useState, useEffect } from 'react'
import { IPost } from './IPost'
import { format, parseISO } from 'date-fns'
import { Link } from 'react-router-dom'

function Posts() {
  const [posts, setPosts] = useState<IPost[]>();

  useEffect(() => {
    populateCollection();
  }, []); // when "[]" is not specified - program fetches data every 0.5s

  const mainPageContent = posts === undefined ? (
    <h1>Loading, please wait</h1>
  ) : (
    <ul className="space-y-6">
      {posts.length > 0 && (
        posts.map(post => 
          <li key={post.id} className="border-2 border-indigo-800 py-6 mx-12 bg-opacity-10 bg-slate-500 rounded-md">
            <Link to={"/post/"+post.id} className="break-all flex mx-10">
              {post.title}
            </Link>
            <span className="text-xs font-light flex justify-between mx-32 mt-6">
              <p>
                {post.dateCreated} @ by <span className="text-blue-400">{post.author}</span>
              </p>              
              <p>
                &lt;user reactions placeholder&gt; 
                &lt;user comments placeholder&gt;
              </p>
            </span>
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
      <div className="mt-12">
        <button className="">Create new placeholder</button>
      </div>
      <div className="my-7">
        {mainPageContent}
      </div>
      <div className="mb-12">
        &lt;pagination placeholder&gt;
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