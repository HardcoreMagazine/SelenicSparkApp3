import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Home from './components/Shared/Home'
import Privacy from './components/Shared/Privacy'
import NavWrapper from './components/Shared/NavWrapper'
import TermsOfService from './components/Shared/TermsOfService'

import Signin from './components/Auth/SignIn'
import Register from './components/Auth/Register'
import ResetPassword from './components/Auth/ResetPassword'

import Posts from './components/Posts/Posts'
import Post from './components/Posts/Post'
import CreatePost from './components/Posts/CreatePost'
import DeletePost from './components/Posts/DeletePost'
import EditPost from './components/Posts/EditPost'

function App() {
  return (
    <>
      <Router>
        <Routes>
          <Route element={<NavWrapper />}>
            <Route path="*" element={<Home />} />
            <Route path="/about" element={<Home />} />
            <Route path="/privacy" element={<Privacy />} />
            <Route path="/tos" element={<TermsOfService />} />

            <Route path="/posts" element={<Posts />} />
            <Route path="/post/:id" element={<Post />}   />
            <Route path="/post/create" element={<CreatePost />}   />
            <Route path="/post/:id/edit" element={<EditPost />} />
            <Route path="/post/:id/delete" element={<DeletePost />} />

            <Route path="/register" element={<Register />} />
            <Route path="/signin" element={<Signin />} />
            <Route path="resetpassword" element={<ResetPassword />} />
          </Route>
        </Routes>
      </Router>
    </>
  );
}

export default App
