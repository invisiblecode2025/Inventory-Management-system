
# Inventory Management System

This Inventory Management System is designed with the Clean Architecture and MVVM (Model-View-ViewModel) pattern to ensure a robust, scalable, and maintainable codebase. 
The following sections provide an overview of the architectural decisions and chosen technologies.
# Future
- items
- Suppliers
- Category
- inventory
## Architectural Decisions

### Clean Architecture
Clean Architecture is chosen to maintain a clear separation of concerns and to ensure that the system is easy to understand, test, and maintain. This architecture divides the system into layers, each with its own responsibility:
- **Entities:** Represents the core business logic and rules. These are the most stable elements of the application.
- **Use Cases:** Contains application-specific business rules. This layer orchestrates the flow of data to and from the entities and manages the business logic.
- **Interface Adapters:** Adapts the data from the use cases to a format suitable for the framework or user interface layers.
- **Frameworks and Drivers:** Contains the frameworks and tools such as the user interface, database, and external APIs. This layer is the least stable and most susceptible to change.

### MVVM Pattern
The MVVM pattern is used to facilitate the separation of the UI from the business logic and data binding. It enhances testability and maintainability.
- **Model:** Represents the data and business logic.
- **View:** Represents the UI components and their interactions with the user.
- **ViewModel:** Acts as a mediator between the Model and the View. It handles the presentation logic, data binding, and commands.

## Chosen Technologies

### Backend
- **.NET 8:** The primary framework for building the backend services, chosen for its performance, cross-platform capabilities, and rich ecosystem.
- **Entity Framework Core:** An ORM (Object-Relational Mapper) to simplify database interactions.
- **SQL Server:** The database management system used for storing and retrieving data.


### Frontend
- **WPF (Windows Presentation Foundation):** The UI framework for building rich desktop applications. WPF is chosen for its powerful data binding capabilities and flexibility in creating complex user interfaces.
- **MVVM Light Toolkit:** A toolkit that provides essential components and helpers for implementing the MVVM pattern in WPF applications.

### Dependency Injection
- **Microsoft.Extensions.DependencyInjection:** The built-in dependency injection framework provided by .NET, used to manage dependencies and promote loose coupling between components.

### Logging and Exception Handling
- **Serilog:** A logging library used for capturing and storing application logs. It provides structured logging and various sinks for outputting logs to different targets.

### Unit Testing
- **xUnit:** A popular unit testing framework for .NET applications. It is used to write and execute test cases to ensure the correctness of the application's logic.
- **Moq:** A mocking framework used in conjunction with xUnit to create mock objects and isolate the units under test.

## Folder Structure

The project follows a modular folder structure to organize the codebase effectively:

```
InventoryManagementSystem/
├── src/
│   ├── InventoryManagementSystem.Application/
│   ├── Inventory.DomainModels/
│   ├── Inventory.Infrastructure
│   ├── Inventory ManagementUI
│   ├── Inventory.ManagementDto
│   ├── Inventory.Services
│   ├── Inventory.Shared.Core.Enum
│   ├── InventoryManagementSqlDb
├── tests/
│   ├── InventoryManagementSystem.Application.Tests/
│   ├── InventoryManagementSystem.Domain.Tests/
│   ├── InventoryManagementSystem.Infrastructure.Tests/
│   ├── InventoryManagementSystem.Presentation.Tests/
```

- **Application:** Contains use cases and application-specific logic.
- **Domain:** Contains entities and core business logic.
- **Infrastructure:** Contains database context, repositories, and external service implementations.
- **Presentation:** Contains WPF views, view models, and UI-specific logic.
- **Tests:** Contains unit tests for each layer of the application.


## Conclusion

This Inventory Management System leverages Clean Architecture and the MVVM pattern to ensure a clean, maintainable, and scalable codebase. The chosen technologies and tools further enhance the system's robustness and ease of development.


