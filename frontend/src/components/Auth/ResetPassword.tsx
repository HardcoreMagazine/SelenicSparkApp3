function ResetPassword() {
  return (
    <>
      <div>
        <div className="flex flex-col items-center justify-center">
          <div className="w-full max-w-md bg-gray-800 border-gray-700 rounded-md">
            <div className="p-9 space-y-4">
              <form className="space-y-4 md:space-y-6">
                <input type="email" className="rounded-md focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5" placeholder="E-mail" required="true" />
                <button type="submit" className="w-full rounded-md text-sm px-5 py-2.5 text-center bg-pink-900 hover:bg-pink-800 font-semibold">
                  Send the recovery link
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default ResetPassword;