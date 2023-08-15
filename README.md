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
This module contains files that are shared in all other modules. Normally if you some constants, classes or anything that you want to share across all your modules you can put them here.
### BaseModule
This module contains files that act as bases or starting points for building other modules. Normally things like standard request and reponse classes, base classes for defining entities, base database repositories such as MongoDB or SQL can be put here. Also some handly extensions for service collection, string manipulation, validation etc can reside here too.
### IdentityModule
This module contains a basic OAuth2 implementation. This module lets you create users, roles & claims and lets you establish relationships among them. This module lets you authenticate a user and generates auth tokens, validates auth tokens, and generates refresh tokens.
### BlobModule
This module contains a basic blob storage implementation using GridFS. This stores binary large objects in MongoDB database using buckets. It chunks a file and stores them as byte arrays.
### EmailModule
This module contains a basic email implementation using MailKit. Here you can create custom email templates with text or HTML. Use those templates to send emails to people. Also email sending without templates is also possible. This module works with a hosted service where if you enqueue a mail sending job it will gracefully pick it up and execute tasks in batches.
### LanguageModule
This modules contains a basic user interface language implemnentation that facilitates i18n functionality normally used in React or Angular applications. Here you can create an app and set multi lingual resource values for certain labels for you front end application.
### NotificationModule
This module contains a basic notification system using SignalR. Here you can subscribe to an event using websocket connection and expect a return message upon completion of a long running command.



