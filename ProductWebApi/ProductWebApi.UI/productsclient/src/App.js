import React from "react";
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";

import ProductList from './components/ProductList';
import ProductDetails from './components/ProductDetails';
import 'bootstrap/dist/css/bootstrap.min.css'

export default function App() {
    return (
        <Router>
            <div className="container-fluid">
                <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
                    <ul className="navbar-nav mr-auto">
                        <li className="nav-item">
                            <Link to="/" className="nav-link">Products</Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/product" className="nav-link">Add Product</Link>
                        </li>
                    </ul>
                </nav>

                <Switch>
                    <Route path="/product/:id">
                        <ProductDetails />
                    </Route>
                    <Route path="/product">
                        <ProductDetails />
                    </Route>
                    <Route path="/">
                        <ProductList />
                    </Route>
                </Switch>
            </div>
        </Router>
    );
}
