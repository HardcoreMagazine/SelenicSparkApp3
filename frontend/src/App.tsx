import './App.css'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Home from './components/Home'
import Posts from './components/Posts/Posts'
import Privacy from './components/Privacy'
import NavWrapper from './components/NavWrapper'
import Register from './components/Auth/Register'
import Login from './components/Auth/Login'

function App() {
  return (
    <>
      <Router>
        <Routes>
          <Route element={<NavWrapper />}>
            <Route path="*" element={<Home />} />
            <Route path="/about" element={<Home />} />
            <Route path="/posts" element={<Posts />} />
            <Route path="/privacy" element={<Privacy />} />

            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />
          </Route>
        </Routes>
      </Router>
    </>
  );
}

export default App
