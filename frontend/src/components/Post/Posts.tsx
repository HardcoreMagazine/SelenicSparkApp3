import { useState, useEffect, useCallback } from 'react'
import { IPost } from './IPost'
import { Link } from 'react-router-dom'
import { formatDtString } from '../Shared/Scripts/FuncFormatDtString'
import { ApiService } from '../Shared/Scripts/ApiService'
import { ApiEndpoints } from '../Shared/Scripts/ApiEndpoints'
import { ApiEndpointActions } from '../Shared/Scripts/ApiEndpointActions'
import { BasicHttpMethods } from '../Shared/Scripts/HttpMethods'

function Posts() {
  const [posts, setPosts] = useState<IPost[]>([]);

  const populateCollection = useCallback(async () => {
    const data: IPost[] = await ApiService.handleRequest({
      endpoint: ApiEndpoints.Post,
      action: ApiEndpointActions.PostGetAll,
      method: BasicHttpMethods.GET,
    });

    data.forEach((post) => (post.dateCreated = formatDtString(post.dateCreated)));
    setPosts(data);
  }, []);


  useEffect(() => {
    // set initial state
    populateCollection();

    // set update interval
    const timer = setInterval(() => {
      populateCollection();
    }, 30*1000);
    return () => clearInterval(timer);
  }, [populateCollection]); // update state after each interval

  const mainPageContent = posts === undefined ? (
    <span className="opacity-70">Loading, please wait...</span>
  ) : (
    <ul className="space-x-10">
      {posts.length > 0 && (
        posts.map(post => 
          <Link to={`/post/${post.id}`} key={post.id}>
            <li className="border-2 border-indigo-800 py-6 bg-opacity-10 bg-slate-500 rounded-md">
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
        <li>
          Empty! Try to post something
        </li>
      )}
    </ul>
  );

  return (
    <>
      <div>
        <Link to="/post/create" className="rounded-lg px-8 py-2 border font-semibold bg-indigo-700 hover:bg-indigo-600">
          Create new
        </Link>
      </div>
      <div className="my-7">
        {mainPageContent}
      </div>
      <div>
        &lt;pagination placeholder&gt;
      </div>
    </>
  );

  // async function populateCollection() {
  //   const data: IPost[] = await ApiService.handleRequest({
  //     endpoint: ApiEndpoints.Post,
  //     action: ApiEndpointActions.PostGetAll,
  //     method: BasicHttpMethods.GET
  //   });
    
  //   data.forEach(post => post.dateCreated = formatDtString(post.dateCreated));
  //   setPosts(data);
  // }

}

export default Posts;