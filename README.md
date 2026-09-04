https://youtube.com/shorts/d6DyYUiBvHw?si=2zpXgxWNhW7e2Vgn
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
