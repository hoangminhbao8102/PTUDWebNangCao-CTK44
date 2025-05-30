import './App.css';
import Footer from './Components/Footer';
import Layout from './Pages/Layout';
import AdminLayout from './Pages/Admin/Layout';
import AdminIndex from './Pages/Admin/Index';
import Authors from './Pages/Admin/Author/Authors';
import Categories from './Pages/Admin/Category/Categories';
import Comments from './Pages/Admin/Comments';
import Posts from './Pages/Admin/Post/Posts';
import Tags from './Pages/Admin/Tag/Tags';
import Subscribers from './Pages/Admin/Subscribers';
import NotFound from './Pages/NotFound';
import BadRequest from './Pages/BadRequest';
import Index from './Pages/Index';
import About from './Pages/About';
import Contact from './Pages/Contact';
import RSS from './Pages/RSS';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import PostDetail from './Components/PostDetail';
import Edit from './Pages/Admin/Post/Edit';
import CategoryEdit from './Pages/Admin/Category/Edit';
import AuthorEdit from './Pages/Admin/Author/Edit';
import TagEdit from "./Pages/Admin/Tag/Edit";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Layout />} >
          <Route path="/" element={<Index />} />
          <Route path="blog" element={<Index />} />
          <Route path="blog/About" element={<About />} />
          <Route path="blog/Contact" element={<Contact />} />
          <Route path="blog/RSS" element={<RSS />} />
          <Route path="blog/post/:year/:month/:day/:slug" element={<PostDetail />} />
        </Route>
        <Route path="/admin" element={<AdminLayout />} >
          <Route path="/admin" element={<AdminIndex />} />
          <Route path="/admin/authors" element={<Authors />} />
          <Route path="/admin/authors/add" element={<AuthorEdit />} />
          <Route path="/admin/authors/edit/:id" element={<AuthorEdit />} />
          <Route path="/admin/categories" element={<Categories />} />
          <Route path="/admin/categories/add" element={<CategoryEdit />} />
          <Route path="/admin/categories/edit/:id" element={<CategoryEdit />} />
          <Route path="/admin/comments" element={<Comments />} />
          <Route path="/admin/posts" element={<Posts />} />
          <Route path="/admin/posts/edit" element={<Edit />} />
          <Route path="/admin/posts/edit/:id" element={<Edit />} />
          <Route path="/admin/tags" element={<Tags />} />
          <Route path="/admin/tags/add" element={<TagEdit />} />
          <Route path="/admin/tags/edit/:id" element={<TagEdit />} />
          <Route path="/admin/subscribers" element={<Subscribers />} />
        </Route>
        <Route path="/400" element={<BadRequest />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
      <Footer />
    </Router>
  );
}

export default App;
