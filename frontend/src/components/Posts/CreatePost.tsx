//import { IPost } from './IPost'
import { SetStateAction, useEffect, useRef, useState } from "react"

function CreatePost() {
  /*
  // TODO: fetch data to webAPI server
  function postNew(data) {
    const id = data.get("title");
    window.alert("used title = '" + id + "'");
  }*/

  // this will allow to auto-resize textarea HTML element
  // kudos to github.com/codewgi
  const textRef = useRef("");
  const [txt, setTxt] = useState("");
  
  // needed by law, otherwise browser renders read-only input
  const txtChangeHandler = (e: { target: { value: SetStateAction<string>; }; }) => {
    setTxt(e.target.value);
  };

  useEffect(() => {
    textRef.current.style.height = "auto";
    textRef.current.style.height = textRef.current.scrollHeight + "px";
  }, [txt]);

  return (
    <>
      <div className="space-y-6">
        <h1 className="text-4xl mt-8">
          Create a post
        </h1>
        <form>
          <div className="grid mx-16 mb-4 space-y-3">
            <input name="id" hidden />
            <input name="author" hidden />
            <input name="dateCreated" hidden />
            <input name="title" placeholder="Title" maxLength="256" minLength="15" autoComplete="off" className="p-2 rounded-lg break-all border bg-stone-900 border-stone-500" />
            <textarea name="text" placeholder="Text (optional)" autoComplete="off" maxLength="24000" value={txt} onChange={txtChangeHandler} ref={textRef} className="flex flex-col p-2 rounded-lg border bg-stone-900 border-stone-500" />
          </div>
          <button type="submit" className="py-1 px-32 mb-8 rounded-lg text-lg border border-indigo-400 bg-rose-800 hover:bg-rose-700 hover:text-indigo-300 select-none">
            PUBLISH
          </button>
        </form>
      </div>
    </>
  );
}

export default CreatePost;