# HelpDesk 

## 1. Technology Stack

- **Frontend:** Angular
- **Backend:** ASP.NET Core Web API (C#)
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **API Testing:** Swagger
- **Authentication:** JWT (JSON Web Token)
- **Version Control:** Git and GitHub

## 2. Project Overview

HelpDesk is a web-based IT support ticket management system that allows customers to raise support tickets and support teams to manage and resolve them.

The application follows a role-based approach where different users have different permissions and responsibilities.

### User Roles

| Role | Responsibilities |
|---|---|
| Customer | Create support tickets and view their own tickets. |
| Agent | View assigned tickets and update their status. |
| Manager | View all tickets, assign tickets to agents and monitor ticket progress. |
| Admin | Manage users and access overall ticket-management functionality. |

## 3. Versions Used

- Node.js: 20.17.0
- npm: 10.8.2
- Angular CLI: 19.2.27
- .NET SDK: 10.0.404
- Entity Framework Core: 10.0.15
- SQL Server: Microsoft SQL Server

---

# 4. Initial Project Setup

## Step 1 – Creating the Backend

### Create an ASP.NET Core Web API project

```bash
dotnet new webapi -n HelpDesk.Api
```

This creates a new ASP.NET Core Web API project named `HelpDesk.Api`.

### Restore dependencies

```bash
dotnet restore
```

This restores the required NuGet packages for the project.

### Run the backend

```bash
dotnet run
```

This starts the Web API application.

### Create a solution file

```bash
dotnet new sln -n HelpDesk
```

This creates a .NET solution that groups related .NET projects together.

### Add the existing project to the solution

```bash
dotnet sln add HelpDesk.Api/HelpDesk.Api.csproj
```

### Create folders for different functionalities

We organized the backend using folders such as:

- Controllers
- Models
- Data
- DTOs
- Interfaces
- Services
- Repositories
- Middleware
- Constants
- Enums

These folders help maintain a clean and organized application structure.

## Step 2 – Installing Entity Framework Core

Entity Framework Core (EF Core) is an Object-Relational Mapper (ORM). It allows us to interact with the database using C# objects instead of writing SQL queries for every operation.

### Install the SQL Server provider

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.15
```

This package allows EF Core to connect to Microsoft SQL Server.

### Install the EF Core Design package

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.15
```

This package provides design-time functionality required for operations such as creating database migrations.

## Step 3 – Configuring the Database Connection

We configure the database connection string in `appsettings.json`.

### Example connection string

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=HelpDeskDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Connection string properties

- `Server` – SQL Server instance name.
- `Database` – Name of the database.
- `Trusted_Connection=True` – Uses Windows Authentication.
- `TrustServerCertificate=True` – Trusts the server certificate for the connection.

Replace `YOUR_SERVER_NAME` with the actual SQL Server instance name.

## Step 4 – Connecting the Database to the Backend

### 1. Create ApplicationDbContext

Create a `Data` folder and add an `ApplicationDbContext.cs` file.

ApplicationDbContext is the main class used by Entity Framework Core to interact with the database. It acts as a bridge between our C# application and SQL Server.

### Example

```csharp
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

Register `ApplicationDbContext` in the dependency injection container.

```csharp
using HelpDesk.Api.Data;
using Microsoft.EntityFrameworkCore;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
```

This configures EF Core to use SQL Server.

### 3. Install EF Core command-line tools

```bash
dotnet tool install --global dotnet-ef --version 10.0.15
```

This installs the EF Core CLI tool used to manage database migrations.

### 4. Create the first migration

```bash
dotnet ef migrations add InitialCreate --project HelpDesk.Api
```

A migration records changes made to the database structure.

### 5. Apply the migration

```bash
dotnet ef database update --project HelpDesk.Api
```

This applies the migration and creates or updates the database tables in SQL Server.

Whenever database models or relationships change, a new migration can be created and applied.

## Step 5 – Creating the Frontend

### Create an Angular project

```bash
ng new helpdesk-client --routing --style=scss
```

This creates a new Angular application named `helpdesk-client`.

- `--routing` enables routing.
- `--style=scss` configures SCSS for styling.

### Run the frontend

```bash
ng serve
```

This starts the Angular development server.

## Step 6 – Connecting Angular with ASP.NET Core

Angular communicates with the backend through HTTP requests.

We create Angular services to handle API communication.

### Generate an API service

```bash
ng generate service services/api
```

This generates:

- `api.service.ts` – Contains API communication logic.
- `api.service.spec.ts` – Generated service test file.

The API service handles HTTP requests between Angular and ASP.NET Core.

---

# 5. General Development Flow

For features involving database changes, we generally follow this sequence:

1. Create or update the entity/model in the Models folder.
2. Update ApplicationDbContext:
   - Add the required DbSet.
   - Configure primary keys and relationships.
   - Configure constraints and indexes when required.
3. Create a database migration.
4. Apply the migration to SQL Server.
5. Create request and response DTOs.
6. Create interfaces for the required operations.
7. Implement business logic inside services.
8. Register services in Program.cs when dependency injection is required.
9. Implement API endpoints in controllers.
10. Add authentication and role-based authorization.
11. Connect the frontend if the feature requires a UI.
12. Test the APIs and verify database operations.

## 5.1 Backend Architecture

The backend follows a layered architecture.

**Implementation Flow:**

`Models → DTOs → Interfaces → Services → Repositories (when required) → Controllers`

### Responsibilities

| Layer | Responsibility |
|---|---|
| Models | Represent database entities. |
| DTOs | Define the data transferred between frontend and backend. |
| Interfaces | Define service operations. |
| Services | Contain business logic. |
| Repositories | Handle data access when repository abstraction is used. |
| Controllers | Handle HTTP requests and return API responses. |
| Middleware | Handle cross-cutting concerns such as global exceptions. |
| Program.cs | Configures services, middleware and application startup. |

### Important Development Standards

- Never return database entities directly from API endpoints.
- Use DTOs to control the information exposed by the API.
- Validate incoming request DTOs.
- Keep business logic inside services instead of controllers.
- Use global exception-handling middleware.
- Protect endpoints using `[Authorize]`.
- Apply role restrictions using `[Authorize(Roles = "...")]`.
- Test each endpoint in Swagger while implementing it.

## 5.2 Common Migration Commands

### Create a migration

```bash
dotnet ef migrations add MigrationName --project HelpDesk.Api
```

### Apply migrations

```bash
dotnet ef database update --project HelpDesk.Api
```

### Example authentication migration

```bash
dotnet ef migrations add InitialAuthentication --project HelpDesk.Api
```

---

# 6. Module 1 – Authentication

Authentication allows users to register and log in to the HelpDesk application.

## 6.1 Registration API

### Create the User Entity

Created the User entity/model with the required user information.

The User entity includes:

- Id
- FullName
- Email
- PasswordHash
- Role
- CreatedAt

Added the Users DbSet to ApplicationDbContext.

Configured a unique constraint on email addresses to prevent duplicate registrations.

Created and applied the authentication migration to SQL Server.

Verified that the Users table was created successfully using SQL Server Management Studio (SSMS).

### Create Registration DTOs

Created DTOs to define the information accepted by the registration API and the response returned to the client.

### Backend Implementation Flow

`Models → DTOs → Interfaces → Services → Program.cs (Service Registration) → Controllers`

### Registration Logic

1. Receive registration details.
2. Validate the incoming request.
3. Check whether the email already exists.
4. Hash the password before storing it.
5. Assign the Customer role by default.
6. Save the user in SQL Server.
7. Return a suitable response DTO.

**Important:** Public registration must not allow users to select privileged roles such as Agent, Manager or Admin.

## 6.2 Login API

Implemented the login API to authenticate registered users.

### Login Flow

1. Receive email and password.
2. Find the user using the registered email.
3. Verify the supplied password against the stored password hash.
4. Generate a JWT token when authentication succeeds.
5. Return the token and relevant user information.
6. Return an appropriate error response when authentication fails.

## 6.3 Password Hashing

Implemented password hashing using ASP.NET Core password hashing functionality.

Passwords are never stored as plain text.

The stored password hash is used to verify passwords during login.

## 6.4 JWT Configuration

Implemented JWT token generation and validation.

JWT (JSON Web Token) represents the authenticated user's identity when making authorized requests to the backend.

The token contains relevant claims such as:

- User ID
- Name
- Email
- Role

The frontend stores the token for authenticated API communication.

**Security:** JWT signing keys and sensitive credentials must not be committed to Git. Development secrets are stored using .NET User Secrets.

## 6.5 Angular Authentication Frontend

Followed this frontend implementation flow:

`TypeScript Models → Angular Services → Components → Routing → API Integration`

Implemented:

- Authentication models.
- AuthService for registration and login API calls.
- Login component.
- Registration component.
- Authentication interceptor.
- Authentication guard.
- Dashboard component.
- Application routing.

### Authentication Interceptor

The HTTP interceptor automatically attaches the JWT token to authenticated API requests.

### Authentication Guard

The authentication guard prevents unauthenticated users from accessing protected frontend routes.

## 6.6 Authentication Testing

Verified:

- Successful registration.
- Duplicate email handling.
- Password hashing.
- Successful login.
- Invalid login handling.
- JWT generation.
- Access to protected endpoints.
- Logout and session clearing.

---

# 7. Module 2 – Authorization

Authorization determines what an authenticated user is allowed to access or perform.

## 7.1 Role-Based Permissions

| Role | Permissions |
|---|---|
| Customer | Create tickets and view their own tickets. |
| Agent | View assigned tickets and update assigned tickets. |
| Manager | View all tickets, assign tickets and monitor ticket progress. |
| Admin | Manage users and access overall ticket-management functionality. |

## 7.2 Define User Roles

Created centralized role constants so role names can be reused consistently throughout the application.

Roles:

- Customer
- Agent
- Manager
- Admin

## 7.3 Apply Role-Based Authorization

Used role constants in backend controllers to protect API routes.

Examples:

```csharp
[Authorize]
```

Allows only authenticated users.

```csharp
[Authorize(Roles = UserRoles.Admin)]
```

Allows only Admin users.

Role-based authorization is enforced by the backend, not just by Angular route guards.

## 7.4 Configure Initial Admin Credentials

Used .NET User Secrets to store the initial administrator's credentials outside the source code.

```bash
dotnet user-secrets set "InitialAdmin:FullName" "HelpDesk Admin"

dotnet user-secrets set "InitialAdmin:Email" "admin@helpdesk.local"

dotnet user-secrets set "InitialAdmin:Password" "YOUR_STRONG_PASSWORD"
```

Replace `YOUR_STRONG_PASSWORD` with a strong password.

**Important:** Never commit real credentials or secrets to Git.

## 7.5 Create the Initial Admin Seeder

Implemented an initial admin seeder to create the administrator account using the configured User Secrets.

The seeder checks whether an Admin account already exists before creating one.

## 7.6 User Creation and Role Assignment

Implemented the user-management functionality required for privileged account creation.

Created:

- CreateUserDto
- IUserService
- UserService
- UserController

The Admin can create Manager and Agent accounts.

The backend hashes the password before storing the account and checks for duplicate email addresses.

Public registration remains restricted to the Customer role.

## 7.7 Angular Role-Based Routing

Implemented a role guard to protect frontend routes based on the logged-in user's role.

Separate dashboard routes are configured for:

- Customer
- Agent
- Manager
- Admin

The frontend redirects users to the appropriate dashboard after successful login.

## 7.8 Authorization Testing

Tested role-based access using Swagger and the Angular application.

Verified:

- Customer cannot access Agent-only endpoints.
- Agent cannot access Manager-only endpoints.
- Manager cannot access Admin-only endpoints.
- Admin can access authorized administrative endpoints.
- Unauthenticated requests receive 401 Unauthorized.
- Authenticated users without sufficient permissions receive 403 Forbidden.

---

# 8. Global Error Handling and Swagger

## 8.1 Global Exception Handling

Implemented centralized exception handling using custom middleware.

The middleware:

1. Catches unhandled exceptions.
2. Logs the exception details.
3. Returns a standardized ProblemDetails response.
4. Includes a trace ID to help identify errors.

This avoids repeating exception-handling logic in individual controllers.

The middleware is registered in Program.cs after building the application and before mapping endpoints.

## 8.2 Swagger Configuration

Installed Swagger using:

```bash
dotnet add package Swashbuckle.AspNetCore --version 10.0.0
```

Configured Swagger UI to document and test API endpoints.

JWT authentication is configured in Swagger so protected endpoints can be tested by providing a valid Bearer token.

## 8.3 API Testing Approach

For every new endpoint:

1. Start the backend.
2. Open Swagger UI.
3. Authenticate using the appropriate user account.
4. Execute the endpoint.
5. Verify the HTTP status code.
6. Verify the response DTO.
7. Test invalid input.
8. Test unauthorized access.
9. Verify database changes where applicable.

Common HTTP status codes:

| Status Code | Meaning |
|---|---|
| 200 | OK |
| 201 | Created |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 409 | Conflict |
| 500 | Internal Server Error |

---

# 9. Module 3 – Ticket Management Backend

## 9.1 Objective

Implement the backend functionality required for customers to create support tickets and for support teams to manage, assign and resolve them.

The backend uses ASP.NET Core Web API, EF Core and SQL Server.

## 9.2 Define Ticket Enums

Created enums to represent fixed ticket priorities and statuses.

### Ticket Priority

- Low
- Medium
- High
- Critical

### Ticket Status

- New
- Open
- Pending
- Resolved
- Closed

Enums ensure that the application uses consistent values for priority and status.

## 9.3 Create Ticket and Category Models

### Category Entity

The Category entity contains:

- Id
- Name
- Description
- IsActive

Categories help classify support tickets.

### Ticket Entity

The Ticket entity contains:

- Id
- TicketNumber
- Title
- Description
- CategoryId
- Priority
- Status
- CreatedById
- AssignedToId
- CreatedAt
- UpdatedAt

The Ticket entity maintains relationships with Category and User entities.

`CreatedById` identifies the customer who created the ticket.

`AssignedToId` identifies the Agent assigned to handle the ticket. It can be null when a ticket is not yet assigned.

## 9.4 Update ApplicationDbContext

Added the required DbSets:

- Tickets
- Categories

Configured:

- Primary keys.
- Foreign key relationships.
- Enum-to-string conversions.
- Unique category names.
- Unique ticket numbers.
- Appropriate delete behavior for related records.

## 9.5 Create and Apply the Migration

Created a migration for ticket management.

```bash
dotnet ef migrations add AddTicketManagement --project HelpDesk.Api
```

Applied the migration:

```bash
dotnet ef database update --project HelpDesk.Api
```

Verified the database tables and relationships in SQL Server Management Studio.

## 9.6 Category Management

Implemented category-management functionality.

### Category DTOs

- CreateCategoryDto
- UpdateCategoryDto
- CategoryResponseDto

### Category Service

Implemented the business logic for:

- Creating categories.
- Retrieving active categories.
- Retrieving a category by ID.
- Updating category information.
- Deactivating categories.

Category responses are mapped to DTOs rather than exposing database entities.

### Category Controller

Implemented category API endpoints.

| HTTP Method | Endpoint | Authorized Role |
|---|---|---|
| GET | `/api/Categories` | Authenticated users |
| GET | `/api/Categories/{id}` | Authenticated users |
| POST | `/api/Categories` | Admin |
| PUT | `/api/Categories/{id}` | Admin |
| DELETE | `/api/Categories/{id}` | Admin |

Category deletion is handled through deactivation so existing ticket references can be preserved.

## 9.7 Ticket DTOs

Created DTOs to define the data accepted and returned by ticket endpoints.

### CreateTicketDto

Defines the information required when a customer creates a ticket.

### TicketResponseDto

Defines the ticket information returned to the frontend.

Includes:

- Ticket ID and number.
- Title and description.
- Category information.
- Priority and status.
- Customer information.
- Assigned Agent information.
- Creation and update timestamps.

### UpdateTicketStatusDto

Defines the status information required when an Agent updates a ticket.

### AssignTicketDto

Defines the Agent ID required when a Manager or Admin assigns a ticket.

## 9.8 Ticket Service

Created ITicketService and TicketService to contain ticket-management business logic.

Implemented the following operations:

- CreateTicketAsync
- GetMyTicketsAsync
- GetAssignedTicketsAsync
- GetAllTicketsAsync
- GetTicketByIdAsync
- UpdateTicketStatusAsync
- AssignTicketAsync

### Ticket Creation Logic

1. Receive the ticket details from the Customer.
2. Validate the request.
3. Identify the logged-in Customer from the authenticated user claims.
4. Generate a unique ticket number.
5. Set the initial status to New.
6. Set the default priority to Medium if no priority is supplied.
7. Save the ticket in SQL Server.
8. Return the ticket response DTO.

### Ticket Assignment Logic

1. Manager or Admin selects a ticket.
2. The selected Agent is validated.
3. The ticket's AssignedToId is updated.
4. The updated ticket is saved.
5. The response DTO is returned.

### Ticket Status Update Logic

1. Agent opens an assigned ticket.
2. Agent selects a new status.
3. The backend verifies that the Agent is assigned to the ticket.
4. The ticket status is updated.
5. UpdatedAt is refreshed.
6. The updated ticket DTO is returned.

## 9.9 Ticket Controller

Created TicketsController to expose the ticket-management operations through REST API endpoints.

### Ticket API Endpoints

| HTTP Method | Endpoint | Authorized Role |
|---|---|---|
| POST | `/api/Tickets` | Customer |
| GET | `/api/Tickets/my-tickets` | Customer |
| GET | `/api/Tickets/assigned` | Agent |
| GET | `/api/Tickets` | Manager, Admin |
| GET | `/api/Tickets/{id}` | Authorized ticket owner/assignee or Manager/Admin |
| PUT | `/api/Tickets/{id}/status` | Assigned Agent |
| PUT | `/api/Tickets/{id}/assign` | Manager, Admin |

### Access Control

- Customers can create tickets and view their own tickets.
- Agents can view assigned tickets and update their status.
- Managers and Admins can view all tickets.
- Managers and Admins can assign tickets to Agents.
- Ticket-level access checks prevent users from viewing or modifying tickets outside their permissions.

## 9.10 Backend Testing

Tested ticket-management APIs using Swagger.

Verified:

- Successful category creation.
- Duplicate category handling.
- Category retrieval.
- Category update and deactivation.
- Successful ticket creation.
- Ticket number generation.
- Customer ticket listing.
- Agent assigned-ticket listing.
- Manager and Admin all-ticket access.
- Ticket assignment.
- Agent status updates.
- Unauthorized access.
- Role-based restrictions.

---

# 10. Module 4 – Ticket Management UI

## 10.1 Objective

Develop the Angular frontend for ticket management and integrate it with the existing ASP.NET Core Web APIs.

The application provides separate dashboards and functionalities based on the logged-in user's role.

## 10.2 Frontend Implementation Flow

`TypeScript Models → Angular Services → Components → Routing → API Integration → UI Testing`

## 10.3 Global Styling

Created a global SCSS file to maintain consistent styling throughout the application.

Configured:

- Global font family.
- Color variables.
- Background and text colors.
- Button styles.
- Card styles.
- Form input styles.
- Table styles.
- Priority and status colors.
- Common spacing and visual elements.

This provides a consistent design across the different pages.

## 10.4 Public Landing Page

Created the public landing page for HelpDesk.

The page includes:

- Application branding.
- Navigation.
- Project introduction.
- Key benefits.
- Login and registration navigation.
- Call-to-action sections.
- Responsive styling.

The landing page is accessible without authentication.

## 10.5 Login and Registration Pages

### Login Page

Implemented:

- Email and password fields.
- Form validation.
- Password visibility toggle.
- API integration with AuthService.
- Loading and error states.
- Role-based redirection after login.

### Registration Page

Implemented:

- Full name.
- Email.
- Password.
- Form validation.
- Registration API integration.
- Success and error messages.

Public registration creates Customer accounts only.

## 10.6 Create Ticket Page

Created a ticket form for Customers.

The form includes:

- Ticket title.
- Description.
- Category.
- Priority.

When submitted:

1. Angular validates the form.
2. TicketService sends the request to the backend.
3. The backend creates the ticket.
4. SQL Server stores the ticket.
5. Angular displays the result.

## 10.7 Customer Dashboard

Created a separate Customer Dashboard.

### Functionalities

- View total tickets.
- View new tickets.
- View in-progress tickets.
- View resolved and closed tickets.
- View recent tickets.
- Navigate to Create Ticket.
- Navigate to My Tickets.
- Logout.

### My Tickets Page

Allows Customers to:

- View tickets created by their account.
- See ticket numbers.
- View ticket titles.
- View category and priority.
- View ticket status.
- Track ticket creation dates.

## 10.8 Agent Dashboard

Created a separate Agent Dashboard.

### Functionalities

- View total assigned tickets.
- View new tickets.
- View open and pending tickets.
- View resolved tickets.
- View high-priority and critical tickets.
- View recent assigned tickets.
- Navigate to Assigned Tickets.
- Logout.

### Assigned Tickets Page

Allows Agents to:

- View tickets assigned to them.
- View customer information.
- View ticket details.
- Update ticket status.
- Monitor ticket progress.

## 10.9 Manager Dashboard

Created a separate Manager Dashboard.

### Functionalities

- View total tickets.
- View assigned and unassigned tickets.
- View active tickets.
- View resolved tickets.
- View customer activity.
- View total Agents.
- View recent tickets.
- Navigate to All Tickets.
- Logout.

### Manager Ticket Management Page

Allows Managers to:

- View all tickets.
- View customer information.
- View ticket priority and status.
- Assign tickets to Agents.
- Monitor ticket assignments.

## 10.10 Admin Dashboard

Created a separate Admin Dashboard.

### Functionalities

- View overall ticket statistics.
- View assigned and unassigned tickets.
- View active and resolved tickets.
- View critical-priority tickets.
- View customer activity.
- View total Agents.
- Navigate to all tickets.
- Navigate to User Management.
- Logout.

Admin can access the overall ticket-management functionality through the appropriate backend permissions.

## 10.11 Admin User Management

Implemented user-management functionality for Admin.

### User Directory

Allows Admin to:

- View all registered users.
- View Manager details.
- View Agent details.
- View Customer details.
- View total user count.
- View separate Manager, Agent and Customer counts.
- Search users by name, email or role.
- View account registration dates.

### Create User Page

Allows Admin to create new Manager and Agent accounts.

The form includes:

- Full Name.
- Email.
- Password.
- Role selection.

The frontend connects to the existing `POST /api/User` endpoint.

The backend validates the request, checks for duplicate email addresses, hashes the password and saves the new user.

Public registration remains restricted to Customer accounts.

## 10.12 Frontend Routing and Guards

Configured application routes in `app.routes.ts`.

Implemented:

- Public landing route.
- Login route.
- Registration route.
- Authentication-protected routes.
- Role-protected dashboard routes.
- Ticket-management routes.
- Admin user-management routes.
- Unauthorized page.
- Wildcard route handling.

Authentication and role guards control frontend navigation.

**Important:** Frontend guards improve navigation and user experience, but backend authorization remains the actual security enforcement.

## 10.13 API Integration

Created Angular services to communicate with the ASP.NET Core backend.

### AuthService

Handles:

- Registration.
- Login.
- Current user information.
- Logout.
- Token management.

### TicketService

Handles:

- Category retrieval.
- Ticket creation.
- Customer ticket retrieval.
- Agent assigned-ticket retrieval.
- All-ticket retrieval.
- Ticket status updates.
- Agent retrieval.
- Ticket assignment.

### UserService

Handles:

- Retrieving all users.
- Creating Manager and Agent accounts.

### HTTP Interceptor

Automatically attaches the JWT token to authenticated HTTP requests.

This allows protected API endpoints to identify and authorize the current user.

## 10.14 Frontend Testing

Tested the frontend with the backend running locally.

### Authentication Testing

- Landing page navigation.
- Customer registration.
- Login.
- Role-based dashboard redirection.
- Logout.
- Protected route access.

### Customer Testing

- Create a ticket.
- View own tickets.
- Verify ticket details and status.

### Agent Testing

- View assigned tickets.
- Update ticket status.
- Verify that unassigned tickets are not accessible.

### Manager Testing

- View all tickets.
- Assign tickets to Agents.
- Verify ticket assignment.

### Admin Testing

- View overall dashboard.
- View all users.
- Search users.
- Create Manager accounts.
- Create Agent accounts.
- Verify updated user counts.
- Access authorized ticket-management functionality.

### Security Testing

- Verify that unauthorized users cannot access protected pages.
- Verify backend role restrictions.
- Verify invalid or expired JWT handling.
- Verify that passwords and password hashes are not exposed in API responses.

---

# 11. Module 5 – Angular UI Integration and Application Completion

## 11.1 Objective

Complete the Angular frontend by integrating all implemented pages and connecting them with the existing backend APIs.

The objective is to ensure that all application modules work together as one complete HelpDesk system.

## 11.2 Frontend Integration Approach

Follow this sequence:

1. Review all existing Angular components and pages.
2. Verify that all routes are correctly configured.
3. Verify authentication and role guards.
4. Verify that Angular services connect to the correct backend endpoints.
5. Verify that the HTTP interceptor attaches JWT tokens.
6. Integrate the four separate role dashboards.
7. Connect all dashboard navigation links.
8. Connect ticket-management pages with backend APIs.
9. Connect Admin user-management pages with backend APIs.
10. Test complete user workflows.
11. Fix UI, API integration and navigation issues.
12. Verify responsive behavior.

## 11.3 Application Navigation Structure

The application provides separate navigation experiences for different user roles.

### Public Routes

- Landing Page
- Login
- Registration

### Customer Routes

- Customer Dashboard
- Create Ticket
- My Tickets

### Agent Routes

- Agent Dashboard
- Assigned Tickets

### Manager Routes

- Manager Dashboard
- All Tickets

### Admin Routes

- Admin Dashboard
- All Tickets
- User Management
- Create User

Each role has its own dashboard and navigation.

## 11.4 End-to-End Application Flow

### Customer Workflow

1. Customer opens the HelpDesk landing page.
2. Customer registers or logs in.
3. Backend authenticates the user.
4. Angular redirects the user to Customer Dashboard.
5. Customer creates a support ticket.
6. Backend validates and saves the ticket.
7. Customer views the ticket under My Tickets.
8. Customer tracks the ticket status.

### Agent Workflow

1. Agent logs in.
2. Angular redirects the Agent to Agent Dashboard.
3. Agent views assigned tickets.
4. Agent opens a ticket.
5. Agent updates the ticket status.
6. Backend verifies the assignment and saves the update.

### Manager Workflow

1. Manager logs in.
2. Angular redirects the Manager to Manager Dashboard.
3. Manager views all tickets.
4. Manager checks unassigned tickets.
5. Manager selects an Agent.
6. Backend saves the ticket assignment.
7. Agent can view the assigned ticket.

### Admin Workflow

1. Admin logs in.
2. Angular redirects the Admin to Admin Dashboard.
3. Admin views overall ticket statistics.
4. Admin opens User Management.
5. Admin views all registered users.
6. Admin creates Manager and Agent accounts.
7. New users are saved in SQL Server.
8. Admin can use the authorized ticket-management functionality.

## 11.5 End-to-End Testing

Test the complete application using separate accounts for Customer, Agent, Manager and Admin.

Verify that data created by one user is correctly reflected in the relevant dashboards and pages.

Test the complete ticket lifecycle:

`Customer Creates Ticket → Manager Assigns Ticket → Agent Updates Status → Manager/Admin Monitors Progress`

Also verify:

- Database persistence.
- Correct ticket ownership.
- Correct Agent assignment.
- Correct user roles.
- Correct dashboard statistics.
- Error messages.
- Loading states.
- Empty states.
- Responsive layout.
- Unauthorized access handling.

## 11.6 Final Git Workflow

After completing the frontend pages and connecting them with the backend:

1. Review all changed files.
2. Build the backend.
3. Build the Angular frontend.
4. Test the complete application.
5. Verify that no sensitive credentials are included.
6. Commit the completed changes.
7. Push the feature branch.
8. Create a Pull Request.
9. Review the changes.
10. Merge the Pull Request into the main branch.

---

# 12. Overall Project Workflow

The complete HelpDesk application follows this workflow:

1. **Initial Setup:** Configure Angular, ASP.NET Core Web API and SQL Server.
2. **Authentication:** Implement registration, login, password hashing and JWT.
3. **Authorization:** Implement Customer, Agent, Manager and Admin roles.
4. **Global Error Handling:** Configure centralized exception handling and Swagger.
5. **Ticket Management Backend:** Implement categories, ticket creation, retrieval, assignment and status updates.
6. **Public Frontend:** Develop landing, login and registration pages.
7. **Customer UI:** Develop Customer Dashboard, Create Ticket and My Tickets.
8. **Agent UI:** Develop Agent Dashboard and Assigned Tickets.
9. **Manager UI:** Develop Manager Dashboard and All Tickets.
10. **Admin UI:** Develop Admin Dashboard, User Management and Create User.
11. **API Integration:** Connect all Angular pages to the backend APIs.
12. **Testing:** Verify complete role-based workflows and application behavior.
13. **Git Workflow:** Commit, push, review and merge the completed feature branch.

## 12.1 Overall System Architecture

```text
                 HelpDesk Application
                         |
             Angular Frontend (Port 4200)
                         |
       -----------------------------------------
       |           |           |               |
    Customer     Agent       Manager          Admin
    Dashboard   Dashboard   Dashboard       Dashboard
       |           |           |               |
       -----------------------------------------
                         |
                  Angular Services
                         |
                 HTTP Interceptor
                         |
                  REST API Requests
                         |
             ASP.NET Core Web API
                  (Port 5029)
                         |
       -----------------------------------------
       |           |           |               |
   Controllers   Services   Middleware       DTOs
                         |
                  Entity Framework Core
                         |
                   SQL Server
                    HelpDeskDb
```

## 12.2 Important Development Practices

Throughout implementation, follow these practices:

- Maintain separation of concerns using the layered architecture.
- Use DTOs instead of returning database entities directly.
- Validate incoming request data.
- Keep business logic inside services.
- Apply authentication and role-based authorization to protected APIs.
- Use centralized exception handling.
- Test every API endpoint in Swagger.
- Verify database changes after migrations.
- Reuse existing APIs instead of creating duplicate endpoints.
- Keep frontend components and services organized.
- Use feature branches and meaningful commit messages.
- Test complete workflows before merging changes.

---

# 13. Final Project Outcome

HelpDesk provides a centralized IT support ticket management system where:

- Customers can raise and track support tickets.
- Agents can manage tickets assigned to them.
- Managers can assign tickets and monitor support operations.
- Admins can manage users and access overall ticket information.

The Angular frontend communicates with the ASP.NET Core Web API through REST APIs, while SQL Server stores application data.

The application uses JWT authentication, role-based authorization, Entity Framework Core, DTO-based API communication and centralized error handling to maintain a structured and secure implementation.