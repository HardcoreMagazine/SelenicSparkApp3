import { Link, Outlet } from "react-router-dom";

function NavWrapper() {
  return (
    <>
      <div className="min-h-screen flex flex-col">
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
                <li>
                  <Link to="/signin">Sign In</Link>
                </li>
              </ul>
            </div>
          </div>
        </nav>
        <main className="flex-grow content-center">
          <Outlet />
        </main>
        <footer>
          <div className="w-full flex flex-wrap items-center p-4 bg-gradient-to-t from-slate-950 to-indigo-900 justify-between">
            <ul className="mx-6 inline-flex space-x-10">
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
      </div>
    </>
  );
}

export default NavWrapper;