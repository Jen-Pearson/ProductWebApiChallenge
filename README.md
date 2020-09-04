# ProductWebApiChallenge

Currently the API can be tested using postman.
To create a new product:
* Set HTTP Method to POST
* Set URI to https://localhost:5001/Products
* Select Body tab, select raw radio button, set type to JSON
* In request body enter JSON for a product:
    {
        "id": "1",
        "description": "Product 1",
        "model": "Model 1",
        "brand": "Brand Awesome"
    }
* Select Send

To view the list of products 
* Load https://localhost:5001/Products
* To use Postman Use the GET verb.
