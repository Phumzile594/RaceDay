# RaceDay Database Tables

## Overview

The RaceDay database is designed using Microsoft SQL Server.

The database contains six main tables that support user accounts, profiles, events, categories, participant enrolments and race results.

## 1. Users

The Users table stores account information for RaceDay users.

Main fields include:

- UserID - Primary Key
- FirstName
- LastName
- Email
- PasswordHash
- Role
- CreatedAt

The Role field identifies whether the user is an Organiser or Participant.

## 2. Profile

The Profile table stores additional information for a user.

Main fields include:

- ProfileID - Primary Key
- UserID - Foreign Key
- PhoneNumber
- DateOfBirth
- EmergencyContactName
- EmergencyContactPhone

Each profile is associated with a user.

## 3. Events

The Events table stores RaceDay event information.

Main fields include:

- EventID - Primary Key
- UserID - Foreign Key
- EventName
- Description
- EventDate
- StartTime
- Location
- DistanceKm
- EventType
- Status
- CreatedAt

An Organiser creates and manages events.

## 4. Categories

The Categories table stores categories belonging to an event.

Main fields include:

- CategoryID - Primary Key
- EventID - Foreign Key
- CategoryName
- DistanceKm
- MaximumParticipants
- EntryFee

Each category belongs to an event.

## 5. Enrolments

The Enrolments table stores participant enrolments in event categories.

Main fields include:

- EnrolmentID - Primary Key
- ProfileID - Foreign Key
- CategoryID - Foreign Key
- EnrolmentDate
- Status

An enrolment connects a participant profile to an event category.

## 6. Results

The Results table stores race results for participant enrolments.

Main fields include:

- ResultID - Primary Key
- EnrolmentID - Foreign Key
- FinishTime
- Position
- ResultStatus
- RecordedAt

Each enrolment can have zero or one recorded result.

## Database Design

The six tables work together to provide the foundation for the RaceDay event management system.

Primary keys uniquely identify records, while foreign keys connect related tables and maintain referential integrity.
