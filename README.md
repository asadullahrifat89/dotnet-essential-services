# dotnet-essential-services

A majestic monolith with domain-driven design and clean architecture containing some essential modules required to build any simple piece of software.

![image](https://github.com/asadullahrifat89/dotnet-essential-services/assets/25480176/484cdb75-da2d-4adb-b0ba-ff4ca763e87c)


# Folder Structure
The solution follows a clean architecture-inspired folder structure.

### Domain
The Domain layer sits at the core of the solution. Here we define things that are the base foundation of a module. i.e. entities, value objects, aggregates, domain events, exceptions, repository interfaces, etc.

### Application
The Application layer sits right above the Domain layer. It acts as an orchestrator for the Domain layer, containing business logic and the most important use cases in your application.
Here you can define things like commands, queries, DTOs, services, extensions, providers, constants, attributes, middlewares, with their interfaces and implementations, etc.
This solution follows the CQRS pattern so you will find commands and queries with their definitions, handlers, and validators in their respective folders.

### Infrastructure
The Infrastructure layer contains persistence that normally bridges the gap between repositories and database connections. This layer should contain the repository implementations and db contexts.

### Presentation
The Presentation layer is the entry point of the system and is the outermost layer. This layer contains controllers. As this solution follows CQRS patterns you will find command and query controllers here.
You will see that each controller action is decorated with attributes and the endpoint routes are derived from a constant class.

# Modules

### CommonModule
This module contains files that are shared in all other modules. Normally if you have some constants, classes, or anything that you want to share across all your modules you can put them here.
### BaseModule
This module contains files that act as bases or starting points for building other modules. Normally things like standard request and response classes, base classes for defining entities, and base database repositories such as MongoDB or SQL can be put here. Also, some handy extensions for service collection, string manipulation, validation, etc can reside here too.
### IdentityModule
This module contains a basic OAuth2 implementation. This module lets you create users, roles & claims and establish relationships among them. This module lets you authenticate a user and generates auth tokens, validate auth tokens, and generate refresh tokens.
### BlobModule
This module contains a basic blob storage implementation using GridFS. This stores binary large objects in the MongoDB database using buckets. It chunks a file and stores them as byte arrays.
### EmailModule
This module contains a basic email implementation using MailKit. Here you can create custom email templates with text or HTML. Use those templates to send emails to people. Also email sending without templates is also possible. This module works with a hosted service where if you enqueue a mail-sending job it will gracefully pick it up and execute tasks in batches.
### LanguageModule
This module contains a basic user interface language implementation that facilitates i18n functionality normally used in React or Angular applications. Here you can create an app and set multi-lingual resource values for certain labels for your front-end application.
### NotificationModule
This module contains a basic notification system using SignalR. Here you can subscribe to an event using web socket connection and expect a return message upon completion of a long-running command.



