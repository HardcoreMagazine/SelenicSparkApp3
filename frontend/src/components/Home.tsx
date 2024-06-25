import { Link } from "react-router-dom";

function Home() {
    return (
    <>
      <h1>Homepage</h1>
      <div>
        <Link to="/">Home</Link>
        <Link to="/posts">Posts</Link>
      </div>
    </>
  );
}

export default Home;