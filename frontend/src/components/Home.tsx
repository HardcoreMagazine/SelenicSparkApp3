import { Link } from "react-router-dom";

function Home() {
  return (
    <>
      <nav className="dark:bg-gray-600">
        <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
          <Link to="/">
            <div className="inline-flex">
              <img src="./public/logo.png" className="h-8" alt="SelenicSpark3 Logo" />
              &nbsp;
              <span className="self-center text-2xl font-semibold whitespace-nowrap dark:text-white">SelenicSpark3</span>
            </div>
          </Link>
          <div className="hidden w-full md:block md:w-auto">
            <ul className="font-medium flex flex-col p-4 md:p-0 mt-4 border border-gray-100 rounded-lg bg-gray-50 md:flex-row md:space-x-8 rtl:space-x-reverse md:mt-0 md:border-0 md:bg-white dark:bg-gray-800 md:dark:bg-gray-900 dark:border-gray-700">
              <li>
                <Link to="/">Home</Link>
              </li>
              <li>
                <Link to="/posts">
                  <div className="block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent">
                    Posts
                  </div>
                </Link>
              </li>
            </ul>
          </div>
        </div>
      </nav>
      <main className="flex h-screen justify-center items-center">
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
              <a href='https://github.com/HardcoreMagazine/SelenicSparkApp3' target='_blank'>Project Github page</a>
            </li>
          </ul>
        </div>
      </main>
    </>
  );
}

export default Home;