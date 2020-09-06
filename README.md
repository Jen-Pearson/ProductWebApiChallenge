# ProductWebApiChallenge

After downloading be sure to restore all nuget packages for the solution.

Running this site will load a swagger page for testing the API.
eg: https://localhost:<port>/

NOTE: The API uses an in memory database, as configured in the Startup class.

The solution also contains API integration tests and service unit tests.

A client app has been started and can be found in the ProductWebApi.UI directory.  If running the API through IIS Express then the base url for the api may need to be updated in the Constants.js file. Take care as error handling and delete confirmation has not yet been implemented.
 
Once the API is running, you can run the client app by navigating to the productsclient subfolder from a command prompt and running 'npm start'.  The API has been configured for Cors policy to work with localhost:3000.
