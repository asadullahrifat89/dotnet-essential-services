# Introduction

Within this project, a commanding monolith stands tall, exemplifying the principles of domain-driven design and embodying a clean architecture. This architectural marvel houses indispensable modules essential for the development of even the most rudimentary software applications.

![image](https://github.com/asadullahrifat89/dotnet-essential-services/assets/25480176/6460194d-76b2-489b-bea2-3562232d1067)

# Project Structure
The architectural design of this solution is deeply influenced by the principles of clean architecture, ensuring a structured and organized development process.

### Domain Layer: 
At the core of our solution lies the Domain layer. It serves as the bedrock upon which modules are constructed, defining fundamental elements such as entities, value objects, aggregates, domain events, exceptions, and repository interfaces.

### Application Layer: 
Positioned directly above the Domain layer, the Application layer orchestrates the business logic and vital use cases of the application. It encompasses the definition of commands, queries, DTOs (Data Transfer Objects), services, extensions, providers, constants, attributes, and middlewares, along with their corresponding interfaces and implementations. This solution follows the CQRS pattern, hence, you will find commands and queries, complete with their definitions, handlers, and validators in dedicated folders.

### Infrastructure Layer: 
The Infrastructure layer serves as the bridge between repositories and database connections. This layer primarily houses repository implementations and database contexts.

### Presentation Layer: 
As the outermost layer, the Presentation layer serves as the entry point to the system. It comprises controllers that facilitate the execution of commands and queries. In line with the CQRS pattern, you will find command and query controllers within this layer, each action adorned with relevant attributes and endpoint routes meticulously derived from a constant class.

# Modules

### CommonModule: 
This module serves as a repository for files and components shared across all other modules. It is the ideal place to store constants, classes, or any elements intended for widespread use throughout your application.

### BaseModule: 
BaseModule plays the role of a foundation for constructing other modules. It typically contains standard request and response classes, base classes for defining entities, and foundational database repositories, including MongoDB or SQL implementations. Additionally, you can find valuable extensions for service collection, string manipulation, validation, and more within this module.

### IdentityModule: 
Within this module, we have implemented a fundamental OAuth2 system. This empowers you to create users, roles, and claims while establishing intricate relationships among them. The module supports user authentication, token generation, token validation, and refresh token management.

### BlobModule: 
BlobModule introduces a basic yet robust blob storage solution using GridFS. This facilitates the storage of binary large objects within a MongoDB database using structured buckets. Files are efficiently chunked and stored as byte arrays, ensuring optimal performance.

### EmailModule: 
The EmailModule offers a comprehensive email management solution powered by MailKit. Users can create custom email templates in both plain text and HTML formats. These templates can be employed to dispatch emails to recipients. Furthermore, the module allows for email dispatch without templates. It operates seamlessly with a hosted service, efficiently queuing and processing email-sending tasks in batches.

### LanguageModule: 
LanguageModule serves as a cornerstone for implementing user interface language features, particularly valuable in React or Angular applications. Here, you can create applications and set multi-lingual resource values for specific labels within your front-end application.

### NotificationModule: 
The NotificationModule introduces a fundamental notification system using SignalR. Users can subscribe to events via WebSocket connections and receive prompt responses upon the successful execution of lengthy commands.

In summary, this project combines domain-driven design and clean architecture principles to provide a robust foundation for building software applications. The modular structure, spanning from the core domain to various specialized modules, ensures a structured and efficient development process, catering to a wide range of application requirements.
