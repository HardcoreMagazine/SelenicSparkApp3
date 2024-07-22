import { Link } from 'react-router-dom'

function Register() {
  return (
    <div>
      <div className="flex flex-col items-center justify-center">
        <div className="w-full max-w-md bg-gray-800 border-gray-700 rounded-md">
          <div className="p-9 space-y-4">
            <form className="space-y-4 md:space-y-6">
              <input type="text" className="rounded-md focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5" placeholder="Username" required={true} />
              <input type="email" className="rounded-md focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5" placeholder="E-mail" required={true} />
              <input type="password" placeholder="Password" className="rounded-md focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5" required={true} />
              <input type="date" className="rounded-md focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5" required={true} />
              <div className="flex justify-between">
                  <div className="inline-flex">
                    <label className="flex items-center h-5">
                    <input type="checkbox" className="w-4 h-4 border rounded focus:ring-primary-600" required={true} />
                      <span className="ml-2 text-sm select-none">
                        I agree to <Link to="/privacy" target="_blank">privacy terms</Link> and <Link to="/tos" target="_blank">terms of service</Link>
                      </span>
                    </label>
                  </div>
              </div>
              <button type="submit" className="w-full rounded-md px-5 py-2.5 text-center bg-indigo-900 hover:bg-indigo-800 font-semibold">
                Register
              </button>
            </form>
          </div>
        </div>
      </div>
      <div className="inline">
        Already have an account?&nbsp;
        <Link to="/user/signin" className="font-semibold">Sign In instead</Link>
      </div>
    </div>
  );
}

export default Register;