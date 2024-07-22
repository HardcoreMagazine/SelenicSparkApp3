import { Link } from 'react-router-dom'

function Signin() {
  return (
    <>
      <div>
        <div className="flex flex-col items-center justify-center">
          <div className="w-full max-w-md bg-gray-800 border-gray-700 rounded-md">
            <div className="p-9 space-y-4">
              <form className="space-y-4 md:space-y-6">
                <input type="email" className="rounded-md focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5" placeholder="E-mail" required="true" />
                <input type="password" placeholder="Password" className="rounded-md focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5" required="true" />
                <div className="flex justify-between">
                  <div className="inline-flex">
                    <label className="flex items-center h-5">
                      <input type="checkbox" className="w-4 h-4 border rounded focus:ring-primary-600" />
                      <span className="ml-2 text-sm select-none">Remember me</span>
                    </label>
                  </div>
                  <Link to="/user/resetpassword" className="text-sm font-medium">Reset password</Link>
                </div>
                <button type="submit" className="w-full rounded-md px-5 py-2.5 text-center bg-indigo-900 hover:bg-indigo-800 font-semibold">
                  Sign in
                </button>
              </form>
            </div>
          </div>
        </div>
        <div className="inline">
          New to SelenicSpark?&nbsp;
          <Link to="/user/register" className="font-semibold">Create an account</Link>
        </div>
      </div>
    </>
  );
}

export default Signin;