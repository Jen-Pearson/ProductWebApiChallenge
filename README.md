# ProductWebApiChallenge

The solution is based on a .net core 3.1 api with an in memory database. 

After downloading be sure to restore all nuget packages for the solution.

Running this site will load a swagger page for testing the API.
eg: https://localhost:44323/

You may need to install/trust an IIS local certificate for SSL purposes.
The solution also contains API integration tests and service unit tests.

A very basic React based client app can be found in the ProductWebApi.UI directory. Please ensure you are using Node v14.8.0. If the base url for the API deviates from the above localhost example url then the Constants.js file will need to be updated. Take care as error handling and delete confirmation has not yet been implemented.
 
Once the API is running, you can run the client app by navigating to the productsclient subfolder from a command prompt and running 'npm install' to install the required node modules.  You can then run the client app with 'npm start'.  The API has been configured for Cors policy to work with localhost:3000.
