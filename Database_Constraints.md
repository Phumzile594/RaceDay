# RaceDay Database Constraints

## Overview

The RaceDay database uses SQL Server constraints to maintain data integrity, consistency and valid relationships between records.

## Primary Keys

Each table has a primary key that uniquely identifies every record.

- Users: UserID
- Profile: ProfileID
- Events: EventID
- Categories: CategoryID
- Enrolments: EnrolmentID
- Results: ResultID

## Foreign Keys

Foreign keys maintain relationships between related tables.

- Profile.UserID references Users.UserID
- Events.UserID references Users.UserID
- Categories.EventID references Events.EventID
- Enrolments.ProfileID references Profile.ProfileID
- Enrolments.CategoryID references Categories.CategoryID
- Results.EnrolmentID references Enrolments.EnrolmentID

## Unique Constraints

Unique constraints prevent duplicate records where necessary.

Examples include:

- Users.Email must be unique.
- Profile.UserID must be unique.
- Enrolments prevent duplicate enrolment for the same participant and category.
- Results.EnrolmentID is unique so an enrolment can have at most one result.

## Check Constraints

Check constraints restrict invalid values.

Examples include:

- User roles are limited to Organiser or Participant.
- Event types are limited to Run, Walk or Cycle.
- Event statuses use valid predefined values.
- Enrolment statuses use valid predefined values.
- Result statuses use Finished, DNF or DNS.
- Distance and participant limits must be greater than zero.
- Entry fees cannot be negative.
- Result positions must be greater than zero.

## Default Values

Default constraints automatically provide values where appropriate.

Examples include:

- CreatedAt
- EnrolmentDate
- RecordedAt
- Event status
- Enrolment status
- Result status

## Identity Columns

Identity columns are used for automatically generated primary key values.

This reduces the risk of duplicate IDs and simplifies record creation.

## Data Integrity

The combination of primary keys, foreign keys, unique constraints, check constraints and default values helps ensure that the RaceDay database stores reliable and consistent information.
