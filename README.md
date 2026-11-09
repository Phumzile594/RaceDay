
# RaceDay

## Project Overview

RaceDay is an event management system designed to manage running, walking and cycling events.

The system allows Organisers to create and manage events, define event categories, manage participant enrolments and capture race results.

Participants can register for an account, maintain their profile, view available events and categories, enrol in events and view their results.

This repository contains the planning, database design and documentation completed for Part 1 of the RaceDay assignment.

---

## System Roles

### Organiser

Organisers are responsible for managing RaceDay events.

Organiser functionality includes:

- Create events
- Update events
- Delete events
- Define event categories
- View participant enrolments
- Capture participant results

### Participant

Participants use the system to participate in RaceDay events.

Participant functionality includes:

- Register an account
- Login
- View and update their profile
- View available events
- View event categories
- Enrol in events
- View their own results

---

## Part 1 Deliverables

The following Part 1 deliverables are included in this repository:

- Entity Relationship Diagram (ERD)
- API Endpoint Plan
- SQL Server database script
- Database tables and relationships
- Database constraints
- Realistic sample data
- GitHub Actions CI workflow
- Project documentation
- Part 1 presentation video

---

## Database Design

The RaceDay database contains six main entities:

1. Users
2. Profile
3. Events
4. Categories
5. Enrolments
6. Results

### ERD Relationships and Cardinality

The main relationships are:

- Users → Profile: one-to-one
- Users → Events: one-to-many
- Events → Categories: one-to-many
- Profile → Enrolments: one-to-many
- Categories → Enrolments: one-to-many
- Enrolments → Results: one-to-zero-or-one

Primary keys and foreign keys are used to maintain relationships and data integrity.

The detailed ERD relationship documentation is available in:

`ERD_Relationships.md`

---

## API Endpoint Plan

The RaceDay REST API plan contains 14 endpoints covering authentication, profiles, events, categories, enrolments and results.

| # | HTTP Method | Endpoint | Role |
|---|---|---|---|
| 1 | POST | `/api/auth/register` | Public |
| 2 | POST | `/api/auth/login` | Public |
| 3 | GET | `/api/profile` | Authenticated User |
| 4 | PUT | `/api/users/me` | Participant / Organiser |
| 5 | POST | `/api/events` | Organiser |
| 6 | GET | `/api/events` | Public |
| 7 | PUT | `/api/events/{id}` | Organiser |
| 8 | DELETE | `/api/events/{id}` | Organiser |
| 9 | POST | `/api/events/{id}/categories` | Organiser |
| 10 | GET | `/api/events/{id}/categories` | Public |
| 11 | POST | `/api/events/{id}/enrolments` | Participant |
| 12 | GET | `/api/events/{id}/enrolments` | Organiser |
| 13 | POST | `/api/enrolments/{id}/results` | Organiser |
| 14 | GET | `/api/results/my-results` | Participant |

The complete API Endpoint Plan is available at:

`docs/RaceDay Part 1/Raceday API Endpoint Plan.pdf`

Additional API documentation is also included in the repository.

---

## Authentication

The authentication functionality allows users to register and log in.

### Register

`POST /api/auth/register`

Allows a new Organiser or Participant to create an account.

### Login

`POST /api/auth/login`

Authenticates an existing user and returns an authentication token.

Passwords are intended to be securely hashed before being stored in the database.

---

## User Profiles

Authenticated Organisers and Participants can manage their own profile information.

### View Profile

`GET /api/profile`

Retrieves the currently logged-in user's profile.

### Update Profile

`PUT /api/users/me`

Updates the currently logged-in user's profile information.

---

## Events

Organisers are responsible for managing RaceDay events.

### Create Event

`POST /api/events`

Creates a new event.

### View Events

`GET /api/events`

Retrieves available events.

### Update Event

`PUT /api/events/{id}`

Updates an existing event.

### Delete Event

`DELETE /api/events/{id}`

Deletes an existing event.

Events include information such as:

- Event name
- Description
- Date
- Start time
- Location
- Distance
- Event type
- Status

Supported event types are:

- Run
- Walk
- Cycle

---

## Categories

Organisers can create categories for their events.

### Create Category

`POST /api/events/{id}/categories`

Creates a category for a specific event.

### View Categories

`GET /api/events/{id}/categories`

Retrieves categories belonging to an event.

Categories include:

- Category name
- Distance
- Maximum participants
- Entry fee

---

## Enrolments

Participants can enrol in event categories.

### Create Enrolment

`POST /api/events/{id}/enrolments`

Allows a Participant to enrol in an event category.

### View Enrolments

`GET /api/events/{id}/enrolments`

Allows an Organiser to view enrolments for an event.

The database prevents duplicate enrolments for the same participant and category.

---

## Results

Organisers can record race results and Participants can view their own results.

### Record Result

`POST /api/enrolments/{id}/results`

Records a participant's race result.

### View My Results

`GET /api/results/my-results`

Allows a Participant to view their own race results.

Results include:

- Finish time
- Position
- Result status
- Recorded date

Supported result statuses include:

- Finished
- DNF
- DNS

---

## SQL Server Database

The RaceDay database was designed and tested using Microsoft SQL Server and SQL Server Management Studio (SSMS).

The database contains six tables:

- Users
- Profile
- Events
- Categories
- Enrolments
- Results

The SQL script includes:

- Primary keys
- Foreign keys
- Unique constraints
- Check constraints
- Default values
- Identity columns
- Realistic sample data

The SQL database script is available at:

`docs/RaceDay Part 1/RaceDay_Database.sql`

---

## Database Constraints

The database uses constraints to maintain data integrity and consistency.

These include:

- Primary key constraints
- Foreign key constraints
- Unique constraints
- Check constraints
- Default constraints
- Identity columns

Examples include:

- Unique user email addresses
- Valid Organiser and Participant roles
- Valid Run, Walk and Cycle event types
- Valid enrolment statuses
- Valid result statuses
- Positive distances and participant limits
- Non-negative entry fees
- Positive result positions

Additional database constraint documentation is available in:

`Database_Constraints.md`

---

## Sample Data

The database contains realistic sample data including:

- 2 Organisers
- 2 Participants
- 3 Events
- 6 Event Categories
- 5 Participant Enrolments
- 3 Race Results

The sample events include:

- Johannesburg City Run
- Cape Town Charity Walk
- Pretoria Cycle Challenge

The SQL script was successfully executed and tested using SQL Server Management Studio.

---

## How to Run the Database

### Requirements

- Microsoft SQL Server
- SQL Server Management Studio (SSMS)

### Steps

1. Open SQL Server Management Studio.
2. Open `RaceDay_Database.sql` from the `docs/RaceDay Part 1` folder.
3. Execute the SQL script.
4. The script creates the `RaceDay` database if it does not already exist.
5. The script creates all six required tables.
6. The script inserts the sample data.
7. Verify the tables and sample records in SSMS.

The database script has been tested successfully in SQL Server Management Studio.

---

## Entity Relationship Diagram

The RaceDay ERD contains six entities:

- Users
- Profile
- Events
- Categories
- Enrolments
- Results

The ERD shows:

- Entity attributes
- Primary keys
- Foreign keys
- Relationships
- Cardinality

The ERD is available at:

`docs/RaceDay Part 1/RaceDay_ERD.png`

---

## Continuous Integration

GitHub Actions is used to verify the required RaceDay Part 1 project files.

The workflow is located at:

`.github/workflows/ci.yml`

The workflow runs when changes are pushed to the `main` branch or when a pull request targets the `main` branch.

The CI workflow verifies the presence of:

- README
- RaceDay ERD
- API Endpoint Plan
- SQL Server database script

The latest CI workflow completed successfully.

### CI Verification Screenshot

![RaceDay CI](docs/CI_Screenshot.png)

---

## Project Structure

```text
RaceDay/
│
├── .github/
│   └── workflows/
│       └── ci.yml
│
├── docs/
│   ├── RaceDay Part 1/
│   │   ├── RaceDay_ERD.png
│   │   ├── Raceday API Endpoint Plan.pdf
│   │   └── RaceDay_Database.sql
│   │
│   └── CI_Screenshot.png
│
├── API_Authentication.md
├── API_Categories.md
├── API_Enrolments.md
├── API_Events.md
├── API_Profile.md
├── API_Results.md
├── Database_Constraints.md
├── Database_Tables.md
├── ERD_Relationships.md
├── Part_1_Completion.md
│
└── README.md
---

# RaceDay – Part 2: ASP.NET Core Web API

## Part 2 Overview

Part 2 focuses on developing the RaceDay backend using ASP.NET Core Web API, Entity Framework Core, and Microsoft SQL Server.

The API is designed to support event management, user authentication, participant enrolments, and race results.

## Technologies Used

- C#
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- Visual Studio 2022
- Swagger / OpenAPI
- Docker Desktop
- GitHub and GitHub Actions

## API Project Structure

The `RaceDay.API` project contains the backend application.

The project uses the following components:

- **Controllers:** Handle incoming HTTP requests.
- **Models:** Represent the database entities.
- **Data:** Contains the Entity Framework Core database context.
- **Program.cs:** Configures services and middleware.
- **appsettings.json:** Stores application configuration.
- **Dockerfile:** Defines the API container build.

## Database Integration

Entity Framework Core is used to connect the RaceDay API to Microsoft SQL Server.

The database context includes the following entities:

1. Users
2. Profile
3. Events
4. Categories
5. Enrolments
6. Results

The database connection is configured through the `RaceDayConnection` connection string.

Database credentials should not be committed to GitHub.

## User Authentication

The API includes registration and login endpoints:

| HTTP Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Auth/register` | Register a user |
| POST | `/api/Auth/login` | Log in using an existing account |

Passwords are hashed before being stored in the database.

The API also includes session middleware configuration.

## API Testing

Swagger was used to test the authentication endpoints.

The following tests were completed successfully:

- Participant registration
- Participant login
- API build using the .NET CLI
- Accessing Swagger through the HTTPS launch profile

Other planned API endpoints are documented in the Part 1 API Endpoint Plan.

## Running the API

### Requirements

- Visual Studio 2022
- .NET 8 SDK
- SQL Server or SQL Server Express
- SQL Server Management Studio

### Setup Instructions

1. Clone the RaceDay GitHub repository.
2. Open the project in Visual Studio.
3. Restore NuGet packages.
4. Configure the `RaceDayConnection` connection string.
5. Ensure the RaceDay SQL Server database is available.
6. Build the `RaceDay.API` project.
7. Run the API using the HTTPS launch profile.
8. Open Swagger in your browser.

### Terminal Commands

```bash
dotnet restore RaceDay.API/RaceDay.API.csproj
dotnet build RaceDay.API/RaceDay.API.csproj
dotnet run --project RaceDay.API/RaceDay.API.csproj --launch-profile https
```

### Swagger

```text
https://localhost:7094/swagger
```

The actual port may vary depending on the launch configuration.

## Docker Support

A Dockerfile is included in the API project.

Docker Desktop was used to run the API in a container during development.

Additional database networking configuration may be required for SQL Server connectivity from Docker.

## Continuous Integration

GitHub Actions is used for project validation.

The CI workflow is located in:

`.github/workflows/ci.yml`

A successful Part 2 build and test run should be verified in GitHub Actions before submission.

## Part 2 Video Demonstration

**YouTube Video Link:** PASTE YOUR PART 2 UNLISTED YOUTUBE LINK HERE

The demonstration should show the running API and explain the functionality implemented in Part 2.

## Part 2 Conclusion

Part 2 extends the RaceDay database design by introducing an ASP.NET Core Web API.

The project includes database integration, registration and login functionality, Swagger testing, and Docker support.

Further development will focus on completing the remaining event management functionality and integrating a web-based user interface.
# RaceDay – Part 3: ASP.NET Core MVC Website

## Part 3 Overview

Part 3 focuses on developing a web-based interface for the RaceDay event management system using ASP.NET Core MVC.

The website connects to the existing RaceDay REST API developed in Part 2. It allows Organisers and Participants to access different features according to their roles.

The aim of Part 3 is to provide a user-friendly website for managing running, walking and cycling events, participant enrolments and race results.

## Technologies Used

- C#
- ASP.NET Core MVC (.NET 8)
- ASP.NET Core Web API
- Entity Framework Core
- Microsoft SQL Server Express
- Visual Studio 2026
- HTML, CSS, Bootstrap and Razor Views
- xUnit for unit testing
- Git and GitHub
- GitHub Actions

## Project Structure

The RaceDay solution contains three main projects:

**RaceDay.API**

The backend application responsible for authentication, database operations, event management, enrolments and race results.

**RaceDay.Web**

The ASP.NET Core MVC website containing controllers, models, services and Razor views.

**RaceDay.Tests**

The unit testing project used to test selected controller functionality, role restrictions and API service behaviour.

## User Roles

### Organiser

Organisers can:

- Log in to the website.
- Access the Organiser Dashboard.
- Create new events.
- Edit existing events they own.
- Delete their own events using a confirmation page.
- Record participant race results for their own events.

### Participant

Participants can:

- Register an account and log in.
- Access the Participant Dashboard.
- View available running, walking and cycling events.
- View event categories.
- Complete their participant profile information.
- Enrol in event categories.
- View their own enrolments and enrolment statuses.
- View recorded race results, finishing times and positions.

## Authentication and Authorisation

RaceDay uses session-based authentication for the REST API and cookie authentication for the MVC website.

Passwords are hashed before being stored in the database.

Role-based access is implemented to separate Organiser and Participant functionality.

Organisers cannot access Participant-only actions, and Participants cannot access Organiser-only actions.

The API also checks event ownership before allowing an Organiser to update, delete or record results for an event.

## MVC Features Implemented

### Registration and Login

Users can register as Participants and log in using their email addresses and passwords.

After successful login, users are redirected to the dashboard associated with their role.

### Event Management

Organisers can create events by entering the event name, description, date, start time, location, distance, event type and status.

Existing events can be edited, and deletion requires confirmation.

### Participant Enrolment

Participants can select an event category and register for an event.

The enrolment process collects required profile information, including a phone number, date of birth and emergency contact details.

The API checks for duplicate enrolments and category capacity.

### My Enrolments

Participants can view their registered events, categories, distances, entry fees and enrolment statuses.

### Race Results

Organisers can record finishing times, positions and result statuses for participants enrolled in their events.

Participants can view their own recorded race results from the My Results page.

## API Integration

The MVC website communicates with the RaceDay REST API using HttpClient.

The API base URL is configured in `RaceDay.Web/appsettings.json`.

The API handles database operations and validates the user's session and permissions.

Examples of API endpoints used:

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/auth/register` | Register Participant |
| POST | `/api/auth/login` | Log in |
| GET | `/api/events` | View events |
| POST | `/api/events` | Create event |
| PUT | `/api/events/{id}` | Update event |
| DELETE | `/api/events/{id}` | Delete event |
| GET | `/api/events/{id}/categories` | View categories |
| POST | `/api/events/{id}/enrolments` | Enrol Participant |
| GET | `/api/enrolments/my-enrolments` | View personal enrolments |
| POST | `/api/enrolments/{id}/results` | Record race result |
| GET | `/api/results/my-results` | View personal results |

## Database Integration

RaceDay uses Microsoft SQL Server and Entity Framework Core.

The database contains six main entities:

1. Users
2. Profile
3. Events
4. Categories
5. Enrolments
6. Results

Primary keys and foreign keys maintain relationships between the entities.

## How to Run RaceDay Part 3

### Requirements

- Visual Studio 2026 or a compatible development environment
- .NET 8 SDK
- SQL Server Express
- SQL Server Management Studio
- A local RaceDay database

### Setup Instructions

1. Clone the RaceDay GitHub repository.
2. Open `RaceDay.slnx` in Visual Studio.
3. Restore the NuGet packages.
4. Create the RaceDay database using the provided SQL script if it has not already been created.
5. Configure `RaceDay.API/appsettings.json` with the correct SQL Server connection string.
6. Ensure `RaceDay.Web/appsettings.json` contains the correct API base URL.
7. Build the solution.
8. Start `RaceDay.API` using its HTTPS launch profile.
9. Start `RaceDay.Web` using its HTTPS launch profile.
10. Open the MVC website in a browser.

### Local Development URLs

**RaceDay API Swagger:**

`https://localhost:7094/swagger`

**RaceDay MVC Website:**

`https://localhost:7037`

Actual port numbers may vary depending on the launch configuration.

### Build and Test Commands

```bash
dotnet restore RaceDay.slnx
dotnet build RaceDay.slnx
dotnet test RaceDay.Tests/RaceDay.Tests.csproj
```

The MVC website requires the REST API and SQL Server database to be available for database-dependent features.

## Unit Testing

Unit tests were implemented using xUnit.

The tests cover selected functionality including:

- Participant Dashboard controller behaviour
- Organiser Dashboard controller behaviour
- Role-based authorisation attributes
- API service behaviour when authentication is missing

The unit tests were executed locally using Visual Studio Test Explorer and passed.

## Testing Completed

The following functionality was manually tested:

- Participant registration and login
- Organiser login
- Role-based access restrictions
- Viewing available events
- Creating an event
- Editing an existing event
- Deleting a temporary event
- Participant enrolment
- Viewing personal enrolments
- Recording a race result
- Viewing the recorded result as a Participant

## Continuous Integration

The GitHub repository includes a GitHub Actions workflow in:

`.github/workflows/ci.yml`

GitHub Actions provides automated checks when changes are pushed to the repository.

The successful workflow status should be verified on the GitHub Actions page. The existing workflow may require additional configuration to run all Part 3 unit tests automatically.

**GitHub Repository:**

https://github.com/Phumzile594/RaceDay

**GitHub Actions:**

https://github.com/Phumzile594/RaceDay/actions

## Part 3 Video Demonstration

**YouTube Video Link:** ADD YOUR PART 3 UNLISTED YOUTUBE VIDEO LINK HERE

The video demonstration should show:

- The RaceDay website
- Registration and login
- Organiser and Participant dashboards
- Event creation, editing and deletion
- Participant enrolment
- Recording and viewing race results
- Successful unit tests
- GitHub repository and CI status

## Part 3 Conclusion

Part 3 extends the RaceDay REST API by introducing an ASP.NET Core MVC website.

The system provides separate interfaces for Organisers and Participants, allowing users to manage events, register for event categories and access race results.

The project demonstrates MVC architecture, REST API integration, SQL Server database operations, role-based access control and unit testing.
