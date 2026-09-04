https://youtube.com/shorts/d6DyYUiBvHw?si=2zpXgxWNhW7e2Vgn
# RaceDay

## Project Overview

RaceDay is an event management system designed to manage running, walking and cycling events.

The system allows Organisers to create and manage events, define event categories, manage participant enrolments and capture race results.

Participants can register for an account, maintain their profile, view available events and categories, enrol in events and view their results.

This repository contains the planning and database implementation completed for Part 1 of the RaceDay assignment.

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
- Realistic sample database data
- GitHub Actions workflow
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

### Main Relationships

- Users have profiles
- Organisers create events
- Events contain categories
- Participants have enrolments
- Categories contain participant enrolments
- Enrolments can have results

Primary keys and foreign keys are used to maintain relationships between the database entities.

### ERD Relationships and Cardinality

The main relationships represented in the ERD are:

- Users → Profile: one-to-one
- Users → Events: one-to-many
- Events → Categories: one-to-many
- Profile → Enrolments: one-to-many
- Categories → Enrolments: one-to-many
- Enrolments → Results: one-to-zero-or-one

These relationships allow users, events, categories, participant enrolments and race results to be connected correctly.

---

## Database Constraints

The RaceDay database uses several constraints to maintain data integrity and consistency.

These include:

- Primary keys to uniquely identify records
- Foreign keys to maintain relationships between tables
- Unique constraints to prevent duplicate records
- Check constraints to restrict invalid values
- Default values for automatically generated dates and statuses
- Identity columns for automatically generated IDs

These constraints help ensure that valid and consistent data is stored in the RaceDay database.

---

## API Endpoint Plan

The planned RaceDay REST API contains 14 endpoints covering authentication, profiles, events, categories, enrolments and results.

### API Endpoint Summary

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

### Authentication

The authentication functionality allows users to create accounts and log in.

- `POST /api/auth/register` registers a new Organiser or Participant.
- `POST /api/auth/login` authenticates an existing user.

### Profiles

Authenticated users can view and manage their own profile.

- `GET /api/profile` retrieves the logged-in user's profile.
- `PUT /api/users/me` updates the logged-in user's profile.

Both Organisers and Participants can manage their own profile information.

### Events

Organisers manage RaceDay events, while users can view available events.

- `POST /api/events` creates a new event.
- `GET /api/events` retrieves available events.
- `PUT /api/events/{id}` updates an existing event.
- `DELETE /api/events/{id}` deletes an existing event.

### Categories

Organisers can define categories for their events.

- `POST /api/events/{id}/categories` creates a category for an event.
- `GET /api/events/{id}/categories` retrieves categories for a specific event.

Categories contain information such as category name, distance, maximum participants and entry fee.

### Enrolments

Participants can enrol in event categories, while Organisers can view participant enrolments.

- `POST /api/events/{id}/enrolments` allows a Participant to enrol in an event category.
- `GET /api/events/{id}/enrolments` allows an Organiser to view event enrolments.

The database prevents duplicate enrolments for the same participant and category.

### Results

Organisers can record race results, while Participants can view their own results.

- `POST /api/enrolments/{id}/results` records a participant's race result.
- `GET /api/results/my-results` allows a Participant to view their own results.

Results include information such as finish time, position and result status.

The complete API endpoint plan is available at:

`docs/RaceDay Part 1/Raceday API Endpoint Plan.pdf`

---

## SQL Server Database

The database was designed and tested using Microsoft SQL Server and SQL Server Management Studio (SSMS).

The database script creates the following tables:

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

The database script is available at:

`docs/RaceDay Part 1/RaceDay_Database.sql`

---

## Database Table Descriptions

### Users

Stores RaceDay user account information and user roles.

### Profile

Stores additional profile information associated with a user.

### Events

Stores information about running, walking and cycling events.

### Categories

Stores categories associated with each event.

### Enrolments

Stores participant enrolments in event categories.

### Results

Stores race results associated with participant enrolments.

---

## Sample Data

The database contains realistic sample data including:

- 2 Organisers
- 2 Participants
- 3 Events
- 6 Event Categories
- 5 Participant Enrolments
- 3 Race Results

The sample events include running, walking and cycling activities.

The SQL script was successfully executed and tested using SQL Server Management Studio.

---

## Entity Relationship Diagram

The RaceDay ERD shows the six database entities, their attributes, primary keys, foreign keys and relationships.

The ERD is available at:

`docs/RaceDay Part 1/RaceDay_ERD.png`

---

## Continuous Integration

GitHub Actions is used to verify that the required RaceDay Part 1 project files are present in the repository.

The CI workflow is located at:

`.github/workflows/ci.yml`

The workflow runs automatically when changes are pushed to the `main` branch.

The workflow verifies the presence of:

- README documentation
- RaceDay ERD
- API Endpoint Plan
- SQL Server database script

The latest GitHub Actions workflow completed successfully.

---

## Project Structure

```text
RaceDay/
│
├── docs/
│   └── RaceDay Part 1/
│       ├── RaceDay_ERD.png
│       ├── Raceday API Endpoint Plan.pdf
│       └── RaceDay_Database.sql
│
├── .github/
│   └── workflows/
│       └── ci.yml
│
└── README.md
