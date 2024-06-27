import { Link } from "react-router-dom";

function Home() {
  return (
    <>
      <nav className="bg-gradient-to-b from-slate-950 to-indigo-900">
        <div className="flex items-center justify-between p-3 mx-8">
          <Link to="/">
            <div className="inline-flex">
              <img src="./public/logo.png" className="h-8" alt="SelenicSpark3 Logo" />
              &nbsp;
              <span className="text-2xl font-semibold">SelenicSpark3</span>
            </div>
          </Link>
          <div>
            <ul className="font-medium flex space-x-10">
              <li>
                <Link to="/">Home</Link>
              </li>
              <li>
                <Link to="/posts">Posts</Link>
              </li>
            </ul>
          </div>
        </div>
      </nav>
      <main className="flex justify-center items-center">
        <div>
          <ul>
            <li className="mt-3">
              A work-in-progress .NET 8 + ReactTS application created with microservice architecture in mind
            </li>
            <li className="mt-3">
              Initial goal of the SelenicSpark project was to create a simple blog web app to log all of my batshit crazy thoughts (get retrospective!)
            </li>
            <li>
              ...but feel free to repurpose the project for your own needs.
            </li>
            <li className="mt-3">
              License type: Apache 2.0
            </li>
            <li className="mt-3">
              <a href='https://github.com/HardcoreMagazine/SelenicSparkApp3' target='_blank'>
                Project Github page
              </a>
            </li>
          </ul>
        </div>
      </main>
      <footer>
        <div className="w-full flex flex-wrap items-center p-4 bg-gradient-to-t from-slate-950 to-indigo-900 justify-between">
          <ul className="inline-flex space-x-10">
            <li>
              <Link to="/">
                  <span className="self-center whitespace-nowrap">&copy; 2024 SelenicSpark3</span>
              </Link>
            </li>
            <li>
              <Link to="/">Home</Link>
            </li>
            <li>
              <Link to="/">About</Link>
            </li>
            <li>
              <Link to="/privacy">Privacy</Link>
            </li>
          </ul>
        </div>
      </footer>
    </>
  );
}

export default Home;