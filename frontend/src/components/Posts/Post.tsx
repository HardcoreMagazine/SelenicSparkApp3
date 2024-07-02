import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { IPost } from './IPost'
import { formatDtString } from '../Shared/FuncFormatDtString'

function Post() {
  const { id } = useParams();

  const [post, setPost] = useState<IPost>();

  useEffect(() => {
    populateData();
  }, []);

  const mainPageContent = post === undefined ? (
    <h1>Loading, please wait</h1>
  ) : (
    <div className="space-y-4 mx-20 text-start">
      <h1 className="break-words text-2xl">
        {post.title}
      </h1>
      <p className="text-sm font-light text-indigo-400">
        By <span className="font-medium">{post.author}</span> @ on <span className="font-medium">{post.dateCreated}</span>
      </p>
      <p className="break-words">
        {post.text}
      </p>
    </div>
  );

  return (
    <>
      {mainPageContent}
    </>
  );

  async function populateData() {
    const res = await fetch(`https://localhost:46801/post/${id}`);
    const data: IPost = await res.json();
    data.dateCreated = formatDtString(data.dateCreated);
    setPost(data);
  }
}

export default Post;