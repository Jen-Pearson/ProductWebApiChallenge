import React, { Component } from 'react';
import {
    Link
} from "react-router-dom";
import Constants from '../Constants'

class ProductList extends Component {
    static displayName = ProductList.name;

    constructor(props) {
        super(props);
        this.state = { products: [], loading: true };
    }

    componentDidMount() {
        this.populateProductData();
    }

    static renderProductsTable(products) {
        return (
            <table className='table striped bordered' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Description</th>
                        <th>Brand</th>
                        <th>Model</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {products.map(product =>
                        <tr key={product.id}>
                            <td>{product.description}</td>
                            <td>{product.brand}</td>
                            <td>{product.model}</td>
                            <td><Link className="btn btn-link" to={`/product/${product.id}`}>Edit</Link>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : ProductList.renderProductsTable(this.state.products);

        return (
            <div>
                <h1 id="tableLabel" >Products</h1>
                {contents}
            </div>
        );
    }

    async populateProductData() {
        const response = await fetch(Constants.baseAPIUrl);
        const data = await response.json();
        this.setState({ products: data, loading: false });
    }
}
export default ProductList;