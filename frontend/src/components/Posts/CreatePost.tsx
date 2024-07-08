import { useEffect, useRef, useState } from "react"
import { IPost } from './IPost'
// import { ErrorPopup } from '../Shared/ErrorPopup'
// import { StatusCodes } from "../Shared/EStatusCodes"
import { Navigate } from "react-router-dom"

function CreatePost() {
  //const [errorMessage, setErrorMessage] = useState<string | null>(null);
  // this will allow to auto-resize textarea HTML element
  // kudos to github.com/codewgi
  const textRef = useRef("");
  const [txt, setTxt] = useState("");
  // needed by law, otherwise browser renders read-only input
  const txtChangeHandler = (e) => {
    setTxt(e.target.value);
  };
  
  useEffect(() => {
    textRef.current.style.height = "auto";
    textRef.current.style.height = textRef.current.scrollHeight + "px";
  }, [txt]);

  const createHandler = async (e) => {
    e.preventDefault();
    
    // unsafe but should work in absolute majority of cases
    const formData: IPost = {} as IPost;
    // dirty
    // len-1 because <button> also counts as element as gets written in JSON    
    for (let i=0; i < e.target.length - 1; i++) {
      formData[e.target[i].name] = e.target[i].value;
    }
    const jsonData = JSON.stringify(formData);
    
    const res = await fetch("https://localhost:46801/post", {
      method: "POST",
      headers: {
        'Content-Type': 'application/json'
      },
      body: jsonData
    });
    
    const data = await res.json();
    afterCreateHandler(data);
  }

  const afterCreateHandler = (responseCode: number) => {
    if (responseCode > 0) {
      window.location.replace(`/post/${responseCode}`);
    }
    // else {
    //   if (responseCode === StatusCodes.ServerFail) {
    //     setErrorMessage("Server is unavailable or busy. Please, try again later.");
    //   }
    //   else {
    //     setErrorMessage("Bad request");
    //   }
    // }
  }
  
  // const handleCloseError = () => {
  //   setErrorMessage(null);
  // }

  return (
    <>
      <div className="space-y-6">
        <h1 className="text-4xl mt-8">
          Create a post
        </h1>
        <form method="post" onSubmit={createHandler}>
          <div className="grid mx-16 mb-4 space-y-3">
            <input name="id" value="0" hidden readOnly/>
            <input name="author" value={"developer_TODO"} hidden readOnly />
            <input name="dateCreated" value={new Date().toISOString()} hidden readOnly />
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
//insert between closing tags 'form' and 'div':
//{errorMessage && <ErrorPopup message={errorMessage} onClose={handleCloseError} />}
export default CreatePost;