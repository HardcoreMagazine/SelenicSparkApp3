import { Link } from 'react-router-dom'

function Login() {
  return (
    <>
      <div>
        placeholder
      </div>
      <div className="inline">
        New to SelenicSpark?&nbsp;
        <Link to="/register">Create an account</Link>
      </div>
    </>
  );
}

export default Login;