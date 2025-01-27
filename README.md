# Inventory Management System

Architectural Decisions
Clean Architecture
Clean Architecture is chosen to maintain a clear separation of concerns and to ensure that the system is easy to understand, test, and maintain. This architecture divides the system into layers, each with its own responsibility:

Entities: Represents the core business logic and rules. These are the most stable elements of the application.

Use Cases: Contains application-specific business rules. This layer orchestrates the flow of data to and from the entities and manages the business logic.

Interface Adapters: Adapts the data from the use cases to a format suitable for the framework or user interface layers.

Frameworks and Drivers: Contains the frameworks and tools such as the user interface, database, and external APIs. This layer is the least stable and most susceptible to change.
