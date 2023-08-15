# dotnet-essential-services

A majestic monolith with Domain Driven Design and Clean Architecture containing some essential modules required to build any simple piece of software.

# Folder Structure
The solution follows a clean architecture inspired folder structure.

### Domain
The Domain layer sits at the core of the solution. Here we define things that are the base foundation of a module. i.e entities, value objects, aggregates, domain events, exceptions, repositories, etc.

### Application
The Application layer sits right above the Domain layer. It acts as an orchestrator for the Domain layer, containing the most important use cases in your application.
Here you can define things like commands, queries, DTOs etc. This solution follows CQRS pattern so you will find commands and queries with their definitions, handlers, and validators.

### Infrastructure
The Infrastructure layer contains things that facilitate and enable technical parts of a module. i.e extensions, services, constants, attributes, middlewares etc.
This layer can contain things like email providers, database providers, storage providers, identity providers, jwt middlewares, extension functions etc.

### Presentation
The Presentation layer is the entry point to our system. This layer contains controllers. As this solutions follows CQRS patterns you will find command and query controllers here.
You will see that each controller action is decorated with attributes and the endpoint routes are derived from a constant class.

# Modules

### CommonModule
### BaseModule

### IdentityModule
### BlobModule
### EmailModule
### LanguageModule
### NotificationModule



