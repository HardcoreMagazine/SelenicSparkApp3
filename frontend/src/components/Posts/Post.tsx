import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { IPost } from './IPost'
import { formatDtString } from '../Shared/Scriprs/FuncFormatDtString'
import { Link } from 'react-router-dom'
import { sendReq } from '../Shared/Scriprs/FuncApiCallHandler'

function Post() {
  const { id } = useParams();

  const [post, setPost] = useState<IPost>();

  useEffect(() => {
    populateData();
  }, []);

  const mainPageContent = post === undefined ? (
    <h1>Loading, please wait</h1>
  ) : (
    <div className="space-y-4 text-start p-8 rounded-lg bg-opacity-50 bg-neutral-800">
      <h1 className="break-words text-2xl font-bold">
        {post.title}
      </h1>
      <p className="text-sm font-light text-indigo-400">
        By <span className="font-medium">{post.author}</span> @ on <span className="font-medium">{post.dateCreated}</span>
      </p>
      <p className="break-words">
        {post.text}
      </p>
      <div className="space-x-16 py-5 px-12 rounded-lg bg-opacity-50 bg-neutral-900">
        <Link to={`/post/${post.id}/edit`} className="bg-indigo-700 border  rounded-md px-5 py-1">
          Edit
        </Link>
        <Link to={`/post/${post.id}/delete`} className="bg-indigo-700 border rounded-md px-5 py-1">
          Delete
        </Link>
      </div>
    </div>
  );

  return (
    <>
      {mainPageContent}
    </>
  );

  async function populateData() {
    const data: IPost = await sendReq(`https://localhost:46801/post/${id}`);
    data.dateCreated = formatDtString(data.dateCreated);
    setPost(data);
  }
}

export default Post;