import React from 'react';
import { withRouter, Link } from "react-router-dom";
import Constants from '../Constants'

class ProductDetails extends React.Component {

    constructor(props) {
        super(props);
        this.product = {};
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    componentDidMount() {
        const id = this.props.match.params.id;
        if (!!id) {
            this.loadEvent(id);
        }
    }

    async loadEvent(id) {
        try {
            const response = await fetch(Constants.baseAPIUrl + '/' + id);
            const data = await response.json();
            this.setState({ product: data, loading: false });
            // this would be replaced on by onchange associated to input fields
            document.getElementById('id').value = data.id;
            document.getElementById('description').value = data.description;
            document.getElementById('model').value = data.model;
            document.getElementById('brand').value = data.brand;
        } catch (error) {
            // would be replaced with something more meaningful
            console.log(error);
        }
    }

    delete() {
        this.deleteEvent();
    }

    async deleteEvent() {
        try {
            if (!!this.state?.product) {
                const id = this.state.product.id;
                await fetch(Constants.baseAPIUrl+'/' + id, {
                    method: 'delete'
                });
                this.props.history.push('/');
            }
        }
        catch (error) {
            console.log(error);
        }
    }

    handleSubmit = async event => {
        event.preventDefault();

        const formData = new FormData(event.target);
        const id = formData.get('id').trim();
        const description = formData.get('description').trim();
        const model = formData.get('model').trim();
        const brand = formData.get('brand').trim();

        var product = { "id": id, "description": description, "model": model, "brand": brand };

        try {
            if (!!this.state?.product) {
                await fetch(Constants.baseAPIUrl+'/' + id, {
                    method: 'put',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(product)
                });
            } else {
                await fetch(Constants.baseAPIUrl, {
                    method: 'post',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(product)
                });
            }
            this.props.history.push('/');
        } catch (error) {
            console.log(error);
        }
    }

    render() {
        return (
            <div><h2>Product Details</h2>
                <form onSubmit={this.handleSubmit}>
                    <div className="form-group">
                        <label className="form-label" htmlFor="id">Id</label>
                        <input className="form-control" id="id" type="text" name="id" placeholder="Id... " />
                        <label className="form-label" htmlFor="description">Description</label>
                        <input className="form-control" id="description" type="text" name="description" placeholder="Description... " />
                        <label className="form-label" htmlFor="model">Model</label>
                        <input className="form-control" id="model" type="text" name="model" placeholder="Model... " />
                        <label className="form-label" htmlFor="brand">Brand</label>
                        <input className="form-control" id="brand" type="text" name="brand" placeholder="Brand... " />
                    </div>
                    <div className="form-group">
                        <button type="submit" className="btn btn-primary">Save</button>&nbsp;
                        {!!this.state?.product ? <button onClick={() => this.delete(this)} className="btn btn-danger">Delete</button> : null }
                        &nbsp;<Link to='/' className="btn btn-secondary">Cancel</Link>
                    </div>
                </form>
            </div>
        );
    }
}
export default withRouter(ProductDetails);