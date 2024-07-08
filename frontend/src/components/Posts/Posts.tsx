import { useState, useEffect } from 'react'
import { IPost } from './IPost'
import { Link } from 'react-router-dom'
import { formatDtString } from '../Shared/FuncFormatDtString'

function Posts() {
  const [posts, setPosts] = useState<IPost[]>();

  useEffect(() => {
    populateCollection();
  }, []); // when "[]" is not specified - program fetches data every 0.5s

  const mainPageContent = posts === undefined ? (
    <h1>Loading, please wait</h1>
  ) : (
    <ul className="space-x-10 mx-16">
      {posts.length > 0 && (
        posts.map(post => 
          <Link to={`/post/${post.id}`} key={post.id} target="_blank">
            <li key={post.id} className="border-2 border-indigo-800 py-6 bg-opacity-10 bg-slate-500 rounded-md">
              <h2 className="break-all mx-10 text-start text-xl">
                {post.title}
              </h2>
              <span className="text-xs font-light flex justify-between mx-32 mt-6">
                <p>
                  On <span className="font-medium">{post.dateCreated}</span> @ by <span className="font-medium">{post.author}</span>
                </p>              
                <p className="font-medium">
                  &lt;user reactions placeholder&gt; 
                  &lt;user comments placeholder&gt;
                </p>
              </span>
            </li>
          </Link>
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
        <Link to="/post/create" target="_blank" className="">Create new</Link>
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
}

export default Posts;