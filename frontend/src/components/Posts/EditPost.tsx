import { useEffect, useRef, useState } from "react"
import { IPost } from './IPost'
import { useParams } from "react-router-dom";
import { formatDtString } from "../Shared/FuncFormatDtString";

function EditPost() {
  const { id } = useParams();
  const [post, setPost] = useState<IPost>();
  useEffect(() => {
    populateData();
  }, []);

  const createHandler = async (e) => {
    e.preventDefault();
    const formData: IPost = {} as IPost;
    // dirty
    // len-1 because <button> also counts as element as gets written in JSON    
    for (let i = 0; i < e.target.length - 1; i++) {
      formData[e.target[i].name] = e.target[i].value;
    }
    const jsonData = JSON.stringify(formData);
    // error in reponse, code 400/bad request
    // caused because we somehow fetch ALL the data at once, i.e. ID=1, ID=11 (we trying to update), ID=26 and etc - 
    // server has no idea how to process it so it throws an error
    const res = await fetch("https://localhost:46801/post", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: jsonData
    });
    const data = await res.json();
    afterCreateHandler(data);
  };
  const afterCreateHandler = (responseCode: number) => {
    if (responseCode == 0) {
      window.location.replace(`/post/${responseCode}`);
    }
  };

  const textRef = useRef<HTMLTextAreaElement>(null);
  const textChangeHandler = (e) => {
    setPost((prevPost) => ({
      ...prevPost,
      text: e.target.value,
    }));
  };
  useEffect(() => {
    if (textRef.current && post) {
      textRef.current.style.height = "auto";
      textRef.current.style.height = textRef.current.scrollHeight + "px";
    }
  }, [post?.text]);

  const mainPageContent = post === undefined ? (
    <h1>Loading, please wait</h1>
  ) : (
    <>
      <div className="space-y-6">
        <h1 className="text-4xl mt-8">
          Post editing mode
        </h1>
        <form onSubmit={createHandler} className="py-12 rounded-lg bg-opacity-50 bg-neutral-800">
          <div className="grid mx-16 mb-4 space-y-3">
            <input name="id" value={post.id} hidden readOnly />
            <input name="author" value={post.author} hidden readOnly />
            <input name="dateCreated" value={post.dateCreated} hidden readOnly />
            <input name="title" value={post.title} readOnly placeholder="Title" maxLength="256" minLength="15" autoComplete="off" className="p-2 rounded-lg break-all border bg-stone-900 border-stone-500" />
            <textarea name="text" placeholder="Text (optional)" autoComplete="off" maxLength="24000" value={post?.text || ""} onChange={textChangeHandler} ref={textRef} className="resize-none pb-6 flex flex-col p-2 rounded-lg border bg-stone-900 border-stone-500" />
          </div>
          <button type="submit" className="py-1 px-32 rounded-lg text-lg border select-none bg-indigo-800 hover:bg-indigo-700">
            Publish changes
          </button>
        </form>
      </div>
    </>
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

export default EditPost;