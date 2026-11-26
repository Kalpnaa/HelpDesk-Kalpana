# HelpDesk - IT Support Ticket Management System

## Technology Stack

- Frontend: Angular
- Backend: ASP.NET Core Web API (C#)
- Database: Microsoft SQL Server

## Project Overview

HelpDesk is a web-based IT support ticket management system that allows customers to raise support tickets and support teams to manage and resolve them.

## Versions Used

- Node.js: 20.17.0
- npm: 10.8.2
- Angular CLI: 19.2.27
- .NET SDK: 10.0.404

# How I Approach

## Step 1 - Creating the Backend

**Create an ASP.NET Core Web API project**

```bash
dotnet new webapi -n HelpDesk.Api
```

This creates a new ASP.NET Core Web API project named `HelpDesk.Api`.

**Restore dependencies**

```bash
dotnet restore
```

This restores all the required NuGet packages for the project.

**Run the backend**

```bash
dotnet run
```

This runs the Web API application.

**Create a solution file**

```bash
dotnet new sln -n HelpDesk
```

This creates a .NET solution named `HelpDesk.sln` (or the newer solution format supported by the SDK).

A solution groups multiple related .NET projects together.

**Add the existing project to the solution**

```bash
dotnet sln add HelpDesk.Api/HelpDesk.Api.csproj
```

This adds our existing Web API project to the solution.

**Create folders for different functionalities**

We created multiple folders such as:

- Controllers
- Models
- Data
- DTOs

These folders help organize the project and follow a layered architecture.

## Step 2 - Installing Entity Framework Core

Entity Framework Core (EF Core) is an ORM that allows us to interact with the database using C# objects instead of writing SQL queries for every operation.

**SQL Server provider**

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.15
```

This package allows EF Core to connect with Microsoft SQL Server.

**EF Core Design package**

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.15
```

This package provides design-time functionality required for operations like creating database migrations.

## Step 3 - Configuring the Database Connection

We add the database connection string inside `appsettings.json`.

The connection string contains the information required by the backend to connect to SQL Server.

**Example connection string**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=HelpDeskDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

- `Server` - SQL Server instance name.
- `Database` - Name of the database we want to create/use.
- `Trusted_Connection=True` - Uses Windows Authentication.
- `TrustServerCertificate=True` - Allows the connection to trust the server certificate.

We replace `YOUR_SERVER_NAME` with our actual SQL Server instance name.

## Step 4 - Connecting the Database to the Backend

**1. Create ApplicationDbContext**

We create a `Data` folder and add an `ApplicationDbContext.cs` file inside it.

`ApplicationDbContext` is the main class used by Entity Framework Core to interact with the database.

It acts as a bridge between our C# application and SQL Server.

Example:

```csharp
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
```

**2. Register DbContext in Program.cs**

We register `ApplicationDbContext` in the dependency injection container so that different parts of our application can use it.

```csharp
using HelpDesk.Api.Data;
using Microsoft.EntityFrameworkCore;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
```

This tells the application to use SQL Server as the database provider and fetches the connection string from `appsettings.json`.

**3. Install EF Core command-line tools**

```bash
dotnet tool install --global dotnet-ef --version 10.0.15
```

This installs the Entity Framework Core CLI tool, which is used to manage database migrations.

**4. Create the first migration**

```bash
dotnet ef migrations add InitialCreate --project HelpDesk.Api
```

A migration keeps track of changes made to our database structure.

`InitialCreate` is the name of our first migration.

**5. Apply migration to the database**

```bash
dotnet ef database update --project HelpDesk.Api
```

This applies the migration and creates the database and its tables in SQL Server.

Whenever we make changes to our models or database structure, we can create a new migration and update the database again.

## Step 5 - Creating the Frontend

**Create an Angular project**

```bash
ng new helpdesk-client --routing --style=scss
```

This creates a new Angular application named `helpdesk-client`.

- `--routing` enables routing in the Angular application.
- `--style=scss` configures SCSS for styling.

**Run the frontend**

```bash
ng serve
```

This starts the Angular development server.

## Step 6 - Connecting Angular with ASP.NET Core

To communicate with the backend APIs, we create an Angular service.

**Generate an API service**

```bash
ng generate service services/api
```

This generates two files:

- `api.service.ts` - Where we write the API communication logic.
- `api.service.spec.ts` - The generated test file for the service.

The API service will handle HTTP requests from Angular to our ASP.NET Core Web API.


genral flow:- create in models -> update in applicationdbcontect ( configure - make method name modelcraeting to add contriants)
apply migration to sql server - dotnet ef database update
migration creating dotnet ef migrations add InitialAuthentication
Module 1. authication

1- sign in api
Created the User entity.



Added Users to ApplicationDbContext.



Configured primary key and required fields.



Enforced unique email addresses.



Generated InitialAuthentication migration.



Applied the migration to SQL Server.



Verified the Users table in SSMS.


craete the registartion dtos

models->interface->services ->program.cs(register) -> contollers

2.login in api

3.add jwt configuration
 - in appseettings.json

 4. frotend
 model(create interface) ->app/services -> component -> add route in app.rout.ts -> integrate in app.ts

 ng generate guard guards/auth --functional
