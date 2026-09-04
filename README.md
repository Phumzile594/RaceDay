https://youtube.com/shorts/d6DyYUiBvHw?si=2zpXgxWNhW7e2Vgn
# RaceDay

## Project Overview

RaceDay is an event management system designed to manage running, walking and cycling events.

The system allows organisers to create and manage events, define event categories, manage participant enrolments and capture race results.

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

Participants use the system to participate in events.

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
- Sample database data
- GitHub Actions workflow
- Project documentation

---

## Database Design

The RaceDay database contains the following six main entities:

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

---

## API Endpoint Plan

The planned REST API supports the following functionality:

### Authentication

- Register a new user
- Login an existing user

### Profiles

- View the logged-in user's profile
- Update the logged-in user's profile

### Events

- Create an event
- View events
- Update an event
- Delete an event

### Categories

- Create event categories
- View event categories

### Enrolments

- Enrol a participant in an event category
- Allow organisers to view event enrolments

### Results

- Allow organisers to record participant results
- Allow participants to view their own results

The complete API endpoint plan is available at:

`docs/RaceDay Part 1/Raceday API Endpoint Plan.pdf`

---

## SQL Server Database

The database was designed and tested using Microsoft SQL Server and SQL Server Management Studio (SSMS).

The database script creates:

- Users table
- Profile table
- Events table
- Categories table
- Enrolments table
- Results table

The script also includes:

- Primary keys
- Foreign keys
- Unique constraints
- Check constraints
- Default values
- Sample data

The database script is available at:

`docs/RaceDay Part 1/RaceDay_Database.sql`

---

## Sample Data

The database contains sample data including:

- 2 Organisers
- 2 Participants
- 3 Events
- 6 Event Categories
- Sample Participant Enrolments
- Sample Race Results

The SQL script was executed successfully in SQL Server Management Studio.

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
