
# HelpDesk 

## 1. Technology Stack

- **Frontend:** Angular
- **Backend:** ASP.NET Core Web API (C#)
- **Database:** Microsoft SQL Server

## 2. Project Overview

HelpDesk is a web-based IT support ticket management system that allows customers to raise support tickets and support teams to manage and resolve them.

## 3. Versions Used

- Node.js: 20.17.0
- npm: 10.8.2
- Angular CLI: 19.2.27
- .NET SDK: 10.0.404

---

# 4. Initial Project Setup

## Step 1 – Creating the Backend

### Create an ASP.NET Core Web API project

```
dotnet new webapi -n HelpDesk.Api
```

This creates a new ASP.NET Core Web API project named `HelpDesk.Api`.

### Restore dependencies

```
dotnet restore
```

This restores the required NuGet packages for the project.

### Run the backend

```
dotnet run
```

This runs the Web API application.

### Create a solution file

```
dotnet new sln -n HelpDesk
```

This creates a .NET solution named `HelpDesk.sln` (or the newer solution format supported by the installed SDK).

A solution groups multiple related .NET projects together.

### Add the existing project to the solution

```
dotnet sln add HelpDesk.Api/HelpDesk.Api.csproj
```

This adds the existing Web API project to the solution.

### Create folders for different functionalities

We created folders such as:

- Controllers
- Models
- Data
- DTOs

These folders help organize the project and support a layered architecture.

## Step 2 – Installing Entity Framework Core

Entity Framework Core (EF Core) is an Object-Relational Mapper (ORM). It allows us to interact with the database using C# objects instead of writing SQL queries for every operation.

### Install the SQL Server provider

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.15
```

This package allows EF Core to connect to Microsoft SQL Server.

### Install the EF Core Design package

```
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.15
```

This package provides design-time functionality required for operations such as creating database migrations.

## Step 3 – Configuring the Database Connection

We add the database connection string inside `appsettings.json`.

The connection string contains the information required by the backend to connect to SQL Server.

### Example connection string

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=HelpDeskDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Connection string properties:**

- `Server` – SQL Server instance name.
- `Database` – Name of the database we want to create or use.
- `Trusted_Connection=True` – Uses Windows Authentication.
- `TrustServerCertificate=True` – Trusts the server certificate for the connection.

Replace `YOUR_SERVER_NAME` with the actual SQL Server instance name.

## Step 4 – Connecting the Database to the Backend

### 1. Create ApplicationDbContext

We create a `Data` folder and add an `ApplicationDbContext.cs` file inside it.

ApplicationDbContext is the main class used by Entity Framework Core to interact with the database. It acts as a bridge between our C# application and SQL Server.

**Example:**

```
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
```

### 2. Register DbContext in Program.cs

We register `ApplicationDbContext` in the dependency injection container so that different parts of our application can use it.

```
using HelpDesk.Api.Data;
using Microsoft.EntityFrameworkCore;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
```

This tells the application to use SQL Server as the database provider and retrieves the connection string from `appsettings.json`.

### 3. Install EF Core command-line tools

```
dotnet tool install --global dotnet-ef --version 10.0.15
```

This installs the Entity Framework Core CLI tool, which is used to manage database migrations.

### 4. Create the first migration

```
dotnet ef migrations add InitialCreate --project HelpDesk.Api
```

A migration records changes made to the database structure.

`InitialCreate` is the name of our first migration.

### 5. Apply the migration to the database

```
dotnet ef database update --project HelpDesk.Api
```

This applies the migration and creates the database and its tables in SQL Server.

Whenever we make changes to our models or database structure, we can create a new migration and update the database again.

## Step 5 – Creating the Frontend

### Create an Angular project

```
ng new helpdesk-client --routing --style=scss
```

This creates a new Angular application named `helpdesk-client`.

- `--routing` enables routing in the Angular application.
- `--style=scss` configures SCSS for styling.

### Run the frontend

```
ng serve
```

This starts the Angular development server.

## Step 6 – Connecting Angular with ASP.NET Core

To communicate with the backend APIs, we create an Angular service.

### Generate an API service

```
ng generate service services/api
```

This generates two files:

- `api.service.ts` – Contains the API communication logic.
- `api.service.spec.ts` – The generated test file for the service.

The API service handles HTTP requests from Angular to our ASP.NET Core Web API.

---

# 5. General Development Flow

For a feature that requires database changes, we generally follow this sequence:

1. Create or update the entity/model in the Models folder.
2. Update ApplicationDbContext:
   - Add the required DbSet.
   - Configure keys, relationships, constraints, or indexes in `OnModelCreating` when needed.
3. Create a migration.
4. Apply the migration to SQL Server.
5. Implement the required DTOs, interfaces, services, and controllers.
6. Register the services in `Program.cs` when dependency injection is required.
7. Connect the frontend if the feature has a user interface.
8. Test the API and verify the database changes.

## Common Migration Commands

**Create a migration:**

```
dotnet ef migrations add MigrationName --project HelpDesk.Api
```

**Apply migrations to the database:**

```
dotnet ef database update --project HelpDesk.Api
```

For example, the authentication migration was created using:

```
dotnet ef migrations add InitialAuthentication --project HelpDesk.Api
```

---

# 6. Module 1 – Authentication

Authentication allows users to register and log in to the HelpDesk application.

## 6.1 Registration API

### Create the User Entity

- Created the User entity/model.
- Added Users to ApplicationDbContext.
- Configured the primary key and required fields.
- Enforced unique email addresses.
- Generated the InitialAuthentication migration.
- Applied the migration to SQL Server.
- Verified that the Users table was created successfully in SQL Server Management Studio (SSMS).

### Create Registration DTOs

Created the required DTOs to define the data accepted by the registration API.

### Backend Implementation Flow

**Models → DTOs → Interfaces → Services → Program.cs (Service Registration) → Controllers**

Each layer has a responsibility:

- **Models:** Represent the application's data.
- **DTOs:** Define the data transferred between the client and API.
- **Interfaces:** Define the operations that a service must provide.
- **Services:** Contain the business logic.
- **Program.cs:** Registers services with dependency injection.
- **Controllers:** Handle incoming HTTP requests and return API responses.

## 6.2 Login API

Implemented the login API to authenticate registered users.

## 6.3 JWT Configuration

Added the JWT configuration in `appsettings.json` and implemented token generation as part of the authentication flow.

JWT (JSON Web Token) is used to represent an authenticated user's identity when making authorized requests to the backend.

## 6.4 Angular Authentication Frontend

Followed this frontend implementation flow:

**Model (TypeScript Interface) → App Services → Component → app.routes.ts → App Component**

- Created TypeScript interfaces for the authentication data.
- Created Angular services to communicate with the authentication APIs.
- Created the required registration and login components.
- Added routes in `app.routes.ts`.
- Integrated the components into the application.

### Generate an Authentication Guard

```
ng generate guard guards/auth --functional
```

This generates a functional route guard that can be used to control access to frontend routes.

---

# 7. Module 2 – Authorization

Authorization determines what an authenticated user is allowed to access or do in the application.

## 7.1 Role-Based Permissions

| Role     | Permissions                                                |
| -------- | ---------------------------------------------------------- |
| Customer | Create tickets and view their own tickets.                 |
| Agent    | View assigned tickets and update assigned tickets.         |
| Manager  | View all tickets, assign tickets, and manage the workflow. |
| Admin    | Full system access, including user and role management.    |

## 7.2 Define User Roles

Created constants for the user roles so that role names can be reused consistently throughout the application.

## 7.3 Apply Role-Based Authorization

Used the role constants in controllers to protect API routes and restrict access based on the user's role.

## 7.4 Configure Initial Admin Credentials

Used .NET User Secrets to store the initial administrator's details outside the source code.

```
dotnet user-secrets set "InitialAdmin:FullName" "HelpDesk Admin"

dotnet user-secrets set "InitialAdmin:Email" "admin@helpdesk.local"

dotnet user-secrets set "InitialAdmin:Password" "YOUR_STRONG_PASSWORD"
```

Replace `YOUR_STRONG_PASSWORD` with a strong password.

**Important:** Do not commit real credentials or secrets to Git.

## 7.5 Create the Initial Admin Seeder

Created an initial admin seeder to create the administrator account using the configured User Secrets.

## 7.6 Create DTOs for Assigning Roles

Created DTOs to define the data required when assigning roles to users.

## 7.7 Implement Role Assignment

Followed the implementation flow:

**DTOs → Interfaces → Controllers → Service Registration (if required)**

## 7.8 Test Authorization

Tested the authorization APIs using Postman to verify that access is controlled according to user roles.

---

# 8. Global Error Handling

Implemented global error handling to manage exceptions centrally.

### Swagger Package Installation

```
dotnet add package Swashbuckle.AspNetCore --version 10.0.0
```

---

# 9. Module 3 – Ticket Management

## 9.1 Backend Implementation Flow

For ticket management, follow this sequence:

1. Create Models.
2. Create Enums (fixed values).
3. Update ApplicationDbContext.
4. Create and apply database migrations to SQL Server.

### General Implementation Flow

**DTOs → Interfaces → Services → Controllers**

Continue with the remaining required implementation and testing steps.

---

# 10. Module 4 – Ticket Management UI

## 10.1 Objective

Develop the Angular frontend for ticket management and integrate it with the existing ASP.NET Core Web APIs.

The application provides different dashboards and functionalities based on the logged-in user's role.

## 10.2 Frontend Implementation Flow

Follow this sequence:

**Ticket Model → Angular Services → Components → Routing → API Integration → UI Testing**

### Step 1 – Create Ticket Model

Create TypeScript interfaces to represent ticket data transferred between Angular and the backend.

File: `models/ticket.model.ts`

### Step 2 – Create Ticket Service

Create an Angular service to communicate with the ticket-management APIs.

The service handles HTTP requests such as:

- Creating tickets.
- Fetching customer tickets.
- Fetching assigned agent tickets.
- Fetching all tickets.
- Assigning tickets to agents.
- Updating ticket status.

### Step 3 – Create Role-Based UI Components

Create separate dashboards based on the user's role.

#### 1. Customer Dashboard

Functionalities:

- Create new tickets using a form.
- View tickets created by the logged-in customer.
- View ticket details.
- View ticket status and priority.

#### 2. Agent Dashboard

Functionalities:

- View tickets assigned to the logged-in agent.
- View ticket details.
- Update the status of assigned tickets.

#### 3. Manager Dashboard

Functionalities:

- View all tickets.
- Assign tickets to agents.
- Monitor ticket status and priority.
- Manage ticket assignments.

#### 4. Admin Access

Admin can access the overall ticket-management functionality.

Existing manager-level ticket APIs can be reused where appropriate, with Admin included in the backend authorization rules.

## 10.3 Backend APIs Required

Before frontend integration, verify that the required APIs are implemented and tested.

| Functionality                 | Authorized Role |
| ----------------------------- | --------------- |
| Create ticket                 | Customer        |
| View own tickets              | Customer        |
| View assigned tickets         | Agent           |
| Update assigned ticket status | Agent           |
| View all tickets              | Manager, Admin  |
| Assign tickets to agents      | Manager, Admin  |

**Note:** Reuse the existing APIs wherever possible instead of creating duplicate endpoints.

## 10.4 Application Flow

### Customer Ticket Creation Flow

**Customer → Angular Form → ASP.NET Core API → SQL Server → Success Response**

### Overall Application Flow

1. User logs in.
2. Backend authenticates the user and generates a JWT.
3. Angular identifies the user's role.
4. The user is redirected to the appropriate dashboard.
5. Angular services send HTTP requests to the backend APIs.
6. The backend validates the request and performs the required operations.
7. SQL Server stores or retrieves the ticket information.
8. The backend returns the response to Angular.
9. Angular updates the UI accordingly.

## 10.5 Testing

Test all role-based functionalities:

- Test ticket creation using the Customer account.
- Verify that customers can only view their own tickets.
- Verify that agents can only view their assigned tickets.
- Test ticket status updates by agents.
- Test ticket assignment by the Manager.
- Verify Admin access.
- Test unauthorized access and expired JWT tokens.

## 10.6 Git Workflow

After completing the module:

1. Test all frontend functionalities.
2. Verify backend integration.
3. Commit the changes.
4. Push the feature branch.
5. Create a Pull Request.
6. Merge after reviewing the implementation.

---

# 11. Overall Project Workflow

The overall HelpDesk application follows this workflow:

1. **Authentication:** Users register and log in.
2. **Authorization:** Access is controlled based on user roles.
3. **Ticket Creation:** Customers create support tickets.
4. **Ticket Assignment:** Managers assign tickets to agents.
5. **Ticket Management:** Agents view and update their assigned tickets.
6. **Ticket Monitoring:** Managers and Admins monitor tickets across the system.

The frontend and backend communicate through REST APIs, while SQL Server stores the application data.

Complete the ticket-management backend section