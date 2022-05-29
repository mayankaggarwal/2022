import React, { Component, Fragment } from 'react';
import {BrowserRouter as Router, Routes, Route, Link, useParams} from 'react-router-dom';
import Movies from './components/Movies';
import Home from './components/Home';
import Admin from './components/Admin';

export default function App() {
  return (
    <Router>
    <div className="container">

      <div className="row">
        <h1 className="mt-3">
          Go Watch a Movie!
        </h1>
        <hr className="mb-3"></hr>
      </div>

      <div className="row">
        <div className="col-md-2">
          <nav>
            <ul className="list-group">
              <li className="list-group-item">
                <Link to="/">Home</Link>
              </li>
              <li className="list-group-item">
                <Link to="/movies">Movies</Link>
              </li>
              <li className="list-group-item">
                <Link to="/admin">Manage Catalogue</Link>
              </li>
            </ul>
          </nav>
        </div>

        <div className="col-md-10">
          <Routes>
            <Route path="/movies/:id" element={<Movie />}></Route>
            <Route path="/movies" element={<Movies />}>
            </Route>
            <Route path="/admin" element={<Admin />}>
            </Route>
            <Route path="/" element={<Home />}>
            </Route>
          </Routes>
        </div>
      </div>
    </div>
    </Router>
  );
}

function Movie() {
  let { id } = useParams();
  return <h2>Movie Id {id}</h2>
}