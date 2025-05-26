# Simple .NET 9 WebApi project with EF Core, CQRS and SqlServer

This is a simple .NET 9 WebApi project that demonstrates the use of Entity Framework Core, Clean Architecture (Domain Driven Design), CQRS (Command Query Responsibility Segregation), and SqlServer. It is designed to be a starting point for building web applications using these technologies.

This project is built using the latest features of the framework. It is a simple web API that allows you to perform CRUD operations on a database using Entity Framework Core. The project is structured using Clean Architecture principles, which helps to separate concerns and make the code more maintainable.

## Its structure is based on the following layers:

- [x]  **Domain**: Contains the core business logic and domain entities. This layer is independent of any frameworks or external libraries.
- [x]  **Application**: Contains the application logic, including commands and queries. This layer is responsible for orchestrating the flow of data between the domain and the presentation layers.
- [x]  **Persistence**: Contains the implementation of the data access layer using Entity Framework Core. This layer is responsible for interacting with the database and providing data to the application layer.
- [x]  **WebApi**: Contains the presentation layer, which exposes the API endpoints. This layer is responsible for handling HTTP requests and responses.

### Domain Layer
In this layer, we do the following:

- [x]  Define a simple Employee entity with properties such as Id, Name, Position and Salary.
- [x]  Define a custom exception to be thrown when an employee is not found.
- [x]  Define a repository interface for the Employee entity.

### Application Layer
In application layer, we do the following:

- [x]  Define the interface for the DbContext where we define the DbSet for the Employee entity.
- [x]  Define the base interfaces for the commands and queries that will be used to interact with the Employee entity.
- [x]  Define the command and query handlers for the Employee entity CRUD. These handlers implement the logic for creating, updating, deleting, and retrieving employees from the database.
- [x]  Define the responses and request models for the Employee entity.
- [x]  Inject the command and query handlers into the ServiceCollection using dependency injection.

### Infrastructure/Persistence Layer
In this layer, we do the following:

- [x]  Implement the DbContext and the repository interfaces defined in the application layer.
- [x]  Configure the DbContext to use SqlServer as the database provider, extracting the connection string from the appsettings.json file.
- [x]  Define the Employee entity configuration using Fluent API to configure the properties of the Employee entity, such as the table name and column names.
- [x]  Define the DbContextFactory to allow migrations to be run from the command line.
- [x]  Inject the DbContext and the repository implementations into the ServiceCollection.
- [x]  Create a migration to create the database and the Employee table.

### Presentation Layer
In this layer, we do the following:

- [x]  Create the WebApi project and add the necessary dependencies for Entity Framework Core and SqlServer.
- [x]  Define the API controllers for the Employee entity, including endpoints for creating, updating, deleting, and retrieving employees.
- [x]  Use the extensions methods created in the application and persistence layers to register the services and repositories in the ServiceCollection.

## Good Practices, Principles and Patterns Used

- [x]  **Clean Architecture**: The project is structured using Clean Architecture principles, which helps to separate concerns and make the code more maintainable.
- [x]  **CQRS**: The project uses CQRS to separate the command and query responsibilities.
- [x]  **Domain Driven Design**: The project follows Domain Driven Design principles to ensure that the domain logic is separated from the application logic.
- [x]  **Entity Framework Core**: The project uses Entity Framework Core as the ORM to interact with the database.
- [x]  **Migrations**: The project uses EF Core migrations to create and update the database schema.
- [x]  **Fluent API**: The project uses Fluent API to configure the properties of the Employee entity, such as the table name and column names.
- [x]  **Extension Methods**: The project uses extension methods to register the services and repositories in the ServiceCollection. This helps to keep the Startup class clean and organized.
- [x]  **Dependency Injection**: The project uses dependency injection to manage the dependencies between the different layers of the application.
- [x]  **FluentValidation**: The project uses FluentValidation to validate the request models for the API endpoints.
- [x]  **SOLID**: The project follows the SOLID principles to ensure that the code is maintainable and extensible.
- [x]  **Unit Testing**: The project includes unit tests, it is recommended to tests the command and query handlers to ensure that the business logic is working as expected.
- [x]  **AAA Pattern**: The project uses the Arrange-Act-Assert (AAA) pattern to structure the unit tests. This helps to make the tests more readable and maintainable.
- [x]  **Factory Method**: The project uses the factory method pattern to create instances of the DbContext and the repository implementations. This allows for better separation of concerns and makes it easier to test the code.
- [x]  **Repository Pattern**: The project uses the repository pattern to abstract the data access layer from the application layer.
- [x]  **Async/Await**: The project uses async/await to improve the performance of the application and to avoid blocking the main thread.
- [x]  **Global Exception Handling**: The project uses global exception handling to handle exceptions that occur in the application and to return appropriate HTTP responses.
- [x]  **FastLogger**: The project implements a simple logger using the FastLogger library to log messages to the console. This allows for better debugging and monitoring of the application.
- [x]  **Memory Cache**: The project uses memory cache to cache the results of the queries. This helps to improve the performance of the application by reducing the number of database calls.
- [x]  **Api Versioning**: The project uses API versioning to allow for multiple versions of the API to coexist. This helps to ensure that the API is backward compatible and allows for future changes without breaking existing clients.
 
## Dependencies

```
"FluentValidation" Version="12.0.0"
"Microsoft.EntityFrameworkCore" Version="9.0.5"
"Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.5"
"Microsoft.Extensions.Configuration.Json" Version="9.0.5"
"coverlet.collector" Version="6.0.4"
"Microsoft.EntityFrameworkCore.InMemory" Version="9.0.5"
"Microsoft.NET.Test.Sdk" Version="17.14.0"
"xunit" Version="2.9.3"
"xunit.runner.visualstudio" Version="3.1.0"
"Microsoft.AspNetCore.OpenApi" Version="9.0.5"
"Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.22.1-Preview.1"
"Swashbuckle.AspNetCore" Version="8.1.1"
"Microsoft.EntityFrameworkCore.Tools" Version="9.0.5"
"Asp.Versioning.Http" Version="8.1.0"
"Asp.Versioning.Mvc.ApiExplorer" Version="8.1.0"
```
---------

[**YouTube**](https://www.youtube.com/@hectorgomez-backend-dev/featured) - 
[**LinkedIn**](https://www.linkedin.com/in/hectorgomez-backend-dev/) - 
[**GitHub**](https://github.com/MoonDoDev/WebApiEFCoreSqlServer)

