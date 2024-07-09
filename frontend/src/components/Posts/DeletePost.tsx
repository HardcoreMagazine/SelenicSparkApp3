import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { IPost } from './IPost'
import { formatDtString } from '../Shared/FuncFormatDtString'
import { Link } from 'react-router-dom'

function DeletePost() {
  const { id } = useParams();

  const [post, setPost] = useState<IPost>();

  useEffect(() => {
    populateData();
  }, []);

  const mainPageContent = post === undefined ? (
    <h1>Loading, please wait</h1>
  ) : (
    <div>
      <div className="space-y-4">
        <h1 className="text-4xl">
          Are you sure you want to delete this post?
        </h1>
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
            <Link to={`/post/${post.id}`} className="bg-indigo-700 border rounded-md px-5 py-1">
              Cancel
            </Link>
            <Link to={`/post/${post.id}/edit`} className="bg-indigo-700 border rounded-md px-5 py-1">
              Edit
            </Link>
            <button onClick={commitDeletion} className="bg-indigo-700 border rounded-md px-5 py-1 hover:invert hover:text-white">
              Yes, I'm sure
            </button>
          </div>
        </div>
      </div>
    </div>
  );

  return (
    <>
      {mainPageContent}
    </>
  );

  async function populateData() {
    const res = await fetch(`https://localhost:46801/post/${id}`, {
      method: "GET"
    });
    const data: IPost = await res.json();
    data.dateCreated = formatDtString(data.dateCreated);
    setPost(data);
  }

  async function commitDeletion() {
    const res = await fetch(`https://localhost:46801/post/?id=${id}`, {
      method: "DELETE",
    });
    const data: number = await res.json();
    if (data == 0) {
      window.location.replace("/posts")
    }
    else {
      // error handler, maybe?
    }
    // note: not sure if such checks are really needed
  }
}

export default DeletePost;