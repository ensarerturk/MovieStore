# MovieStore API
MovieStore is a sample ASP.NET Core Web API application that allows users to manage movies, directors, actors, and customers. It also provides authentication and authorization features for customer-related operations.

## Getting Started
### Prerequisites
- .NET 7.0 SDK
- PostgreSQL
- Your favorite code editor

### Installation
1. Clone the repository
```bash
git clone https://github.com/ensarerturk/MovieStore.git
```
2. Navigate to the project directory:
```bash
cd MovieStore
```
3. Create a PostgreSQL database and update the connection string in the `appsettings.json` file:
```bash
"Default": "Host=localhost;Database=YourDatabaseName;Username=YourUsername;Password=YourPassword"
```
4. Run the following commands to apply migrations and seed the initial data:
```bash
dotnet ef database update
```
5. Open the project in your preferred code editor.
6. Launch the application:
```bash
dotnet run
```

The API will be available at https://localhost:5001.


### Swagger API Documentation
Swagger has been integrated to provide interactive API documentation. You can access it by navigating to:
```bash
https://localhost:5001/swagger
```
## API Endpoints
### Actors
- `GET /actors: Get a list of all actors.`

- `GET /actors/{id}: Get details of a specific actor by ID.`

- `POST /actors: Create a new actor.`

- `PUT /actors/{id}: Update information for a specific actor by ID.`

- `DELETE /actors/{id}: Delete a specific actor by ID.`

### Directors
- `GET /directors: Get a list of all directors.`

- `GET /directors/{id}: Get details of a specific director by ID.`

- `POST /directors: Create a new director.`

- `PUT /directors/{id}: Update information for a specific director by ID.`

- `DELETE /directors/{id}: Delete a specific director by ID.`

### Movies
- `GET /movies: Get a list of all movies.`

- `GET /movies/{id}: Get details of a specific movie by ID.`

- `POST /movies: Create a new movie.`

- `PUT /movies/{id}: Update information for a specific movie by ID.`

- `DELETE /movies/{id}: Delete a specific movie by ID.`

- `POST /movies/{id}/actors: Add an actor to a movie.`

- `POST /movies/{id}/buy: Purchase a movie (authentication required).`

### Customers
- `GET /customers: Get a list of all customers.`

- `GET /customers/{id}: Get details of a specific customer by ID.`

- `POST /customers: Create a new customer.`

- `POST /customers/connect/token: Generate a new authentication token.`

- `GET /customers/refreshToken: Refresh the authentication token.`

- `DELETE /customers/{id}: Delete a specific customer by ID (authentication required).`

## Authentication
For customer-related operations, authentication is required. To obtain an authentication token, use the `/customers/connect/token` endpoint with valid credentials.

## Error Handling
Custom exception handling middleware has been implemented to provide detailed error messages. In case of an error, the API will return a JSON response with the error details.

## Logging
Request and response details, along with errors, are logged for better traceability. Logs can be viewed in the console.

## Development
The application is built using ASP.NET Core, Entity Framework Core, AutoMapper, FluentValidation, and PostgreSQL.