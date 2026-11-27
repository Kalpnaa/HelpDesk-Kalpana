HelpDesk 
Technology Stack
- Frontend: Angular
- Backend: ASP.NET Core Web API (C#)
- Database: Microsoft SQL Server
Project Overview
HelpDesk is a web-based IT support ticket management system that allows
customers to raise support tickets and support teams to manage and
resolve them.
Versions Used
- Node.js: 20.17.0
- npm: 10.8.2
- Angular CLI: 19.2.27
- .NET SDK: 10.0.404
How I Approach
Step 1 -- Creating the Backend
Create an ASP.NET Core Web API project
dotnet new webapi -n HelpDesk.Api
This creates a new ASP.NET Core Web API project named HelpDesk.Api.
Restore dependencies
dotnet restore
This restores the required NuGet packages for the project.
Run the backend
dotnet run
This runs the Web API application.
Create a solution file
dotnet new sln -n HelpDesk
This creates a .NET solution named HelpDesk.sln (or the newer solution
format supported by the installed SDK).

A solution groups multiple related .NET projects together.
Add the existing project to the solution
dotnet sln add HelpDesk.Api/HelpDesk.Api.csproj
This adds the existing Web API project to the solution.
Create folders for different functionalities
We created folders such as:
- Controllers
- Models
- Data
- DTOs
These folders help organize the project and support a layered
architecture.
Step 2 -- Installing Entity Framework Core
Entity Framework Core (EF Core) is an Object-Relational Mapper (ORM). It
allows us to interact with the database using C# objects instead of
writing SQL queries for every operation.
Install the SQL Server provider
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.15
This package allows EF Core to connect to Microsoft SQL Server.
Install the EF Core Design package
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.15
This package provides design-time functionality required for operations
such as creating database migrations.
Step 3 -- Configuring the Database Connection
We add the database connection string inside appsettings.json.

The connection string contains the information required by the backend
to connect to SQL Server.
Example connection string
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=HelpDeskDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
- Server -- SQL Server instance name.
- Database -- Name of the database we want to create or use.
- Trusted_Connection=True -- Uses Windows Authentication.
- TrustServerCertificate=True -- Trusts the server certificate for
  the connection.
Replace YOUR_SERVER_NAME with the actual SQL Server instance name.
Step 4 -- Connecting the Database to the Backend
1. Create ApplicationDbContext
We create a Data folder and add an ApplicationDbContext.cs file
inside it.

ApplicationDbContext is the main class used by Entity Framework Core
to interact with the database. It acts as a bridge between our C#
application and SQL Server.

Example:
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
2. Register DbContext in Program.cs
We register ApplicationDbContext in the dependency injection container
so that different parts of our application can use it.
using HelpDesk.Api.Data;
using Microsoft.EntityFrameworkCore;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
This tells the application to use SQL Server as the database provider
and retrieves the connection string from appsettings.json.
3. Install EF Core command-line tools
dotnet tool install --global dotnet-ef --version 10.0.15
This installs the Entity Framework Core CLI tool, which is used to
manage database migrations.
4. Create the first migration
dotnet ef migrations add InitialCreate --project HelpDesk.Api
A migration records changes made to the database structure.

InitialCreate is the name of our first migration.
5. Apply the migration to the database
dotnet ef database update --project HelpDesk.Api
This applies the migration and creates the database and its tables in
SQL Server.

Whenever we make changes to our models or database structure, we can
create a new migration and update the database again.
Step 5 -- Creating the Frontend
Create an Angular project
ng new helpdesk-client --routing --style=scss
This creates a new Angular application named helpdesk-client.
- --routing enables routing in the Angular application.
- --style=scss configures SCSS for styling.
Run the frontend
ng serve
This starts the Angular development server.
Step 6 -- Connecting Angular with ASP.NET Core
To communicate with the backend APIs, we create an Angular service.
Generate an API service
ng generate service services/api
This generates two files:
- api.service.ts -- Contains the API communication logic.
- api.service.spec.ts -- The generated test file for the service.
The API service handles HTTP requests from Angular to our ASP.NET Core
Web API.
General Development Flow
For a feature that requires database changes, we generally follow this
sequence:
1. Create or update the entity/model in the Models folder.
2. Update ApplicationDbContext:
   - Add the required DbSet.
   - Configure keys, relationships, constraints, or indexes in
     OnModelCreating when needed.
3. Create a migration.
4. Apply the migration to SQL Server.
5. Implement the required DTOs, interfaces, services, and controllers.
6. Register the services in Program.cs when dependency injection is
   required.
7. Connect the frontend if the feature has a user interface.
8. Test the API and verify the database changes.
Common migration commands
Create a migration:
dotnet ef migrations add MigrationName --project HelpDesk.Api
Apply migrations to the database:
dotnet ef database update --project HelpDesk.Api
For example, the authentication migration was created using:
dotnet ef migrations add InitialAuthentication --project HelpDesk.Api
Module 1 -- Authentication
Authentication allows users to register and log in to the HelpDesk
application.
1. Registration API
Create the User entity
- Created the User entity/model.
- Added Users to ApplicationDbContext.
- Configured the primary key and required fields.
- Enforced unique email addresses.
- Generated the InitialAuthentication migration.
- Applied the migration to SQL Server.
- Verified that the Users table was created successfully in SQL
  Server Management Studio (SSMS).
Create registration DTOs
Created the required DTOs to define the data accepted by the
registration API.
Follow the backend implementation flow
Models → DTOs → Interfaces → Services → Program.cs (service registration) → Controllers
Each layer has a responsibility:
- Models: Represent the application's data.
- DTOs: Define the data transferred between the client and API.
- Interfaces: Define the operations that a service must provide.
- Services: Contain the business logic.
- Program.cs: Registers services with dependency injection.
- Controllers: Handle incoming HTTP requests and return API
  responses.
2. Login API
Implemented the login API to authenticate registered users.
3. JWT Configuration
Added the JWT configuration in appsettings.json and implemented token
generation as part of the authentication flow.

JWT (JSON Web Token) is used to represent an authenticated user's
identity when making authorized requests to the backend.
4. Angular Authentication Frontend
Followed this frontend implementation flow:
Model (TypeScript interface) → App Services → Component → app.routes.ts → App Component
- Created TypeScript interfaces for the authentication data.
- Created Angular services to communicate with the authentication
  APIs.
- Created the required registration and login components.
- Added routes in app.routes.ts.
- Integrated the components into the application.
Generate an authentication guard
ng generate guard guards/auth --functional
This generates a functional route guard that can be used to control
access to frontend routes.
Module 2 -- Authorization
Authorization determines what an authenticated user is allowed to access
or do in the application.
Role-based permissions
  Role                                Permissions
  Customer                        Create tickets and view their own
tickets.

  Agent                           View assigned tickets and update
assigned tickets.

  Manager                         View all tickets, assign tickets,
and manage the workflow.
Admin                           Full system access, including user
and role management.
1. Define user roles
Created constants for the user roles so that role names can be reused
consistently throughout the application.
2. Apply role-based authorization
Used the role constants in controllers to protect API routes and
restrict access based on the user's role.
3. Configure initial admin credentials
Used .NET User Secrets to store the initial administrator's details
outside the source code.
dotnet user-secrets set "InitialAdmin:FullName" "HelpDesk Admin"
dotnet user-secrets set "InitialAdmin:Email" "admin@helpdesk.local"
dotnet user-secrets set "InitialAdmin:Password" "YOUR_STRONG_PASSWORD"
Replace YOUR_STRONG_PASSWORD with a strong password. Do not commit
real credentials or secrets to Git.
4. Create the initial admin seeder
Created an initial admin seeder to create the administrator account
using the configured User Secrets.
5. Create DTOs for assigning roles
Created DTOs to define the data required when assigning roles to users.
6. Implement role assignment
Followed the implementation flow:
DTOs → Interfaces → Controllers → Service Registration (if required)
7. Test authorization
Tested the authorization APIs using Postman to verify that access is
controlled according to user roles.


feature - global error handling
dotnet add package Swashbuckle.AspNetCore --version 10.0.0