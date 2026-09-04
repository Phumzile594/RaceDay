# RaceDay ERD Relationships and Cardinality

## Overview

The RaceDay Entity Relationship Diagram (ERD) represents the relationships between the six main database entities.

The relationships use primary keys and foreign keys to connect related records.

## 1. Users to Profile

**Relationship:** One-to-One

One user can have one profile.

The Profile table uses `UserID` as a foreign key and unique value to associate a profile with a specific user.

**Cardinality:**

`Users 1 : 1 Profile`

## 2. Users to Events

**Relationship:** One-to-Many

An Organiser can create multiple events.

Each event is associated with the Organiser who created it through `UserID`.

**Cardinality:**

`Users 1 : M Events`

## 3. Events to Categories

**Relationship:** One-to-Many

An event can contain multiple categories.

Each category belongs to one specific event through `EventID`.

**Cardinality:**

`Events 1 : M Categories`

## 4. Profile to Enrolments

**Relationship:** One-to-Many

A participant profile can have multiple enrolments.

Each enrolment belongs to one participant profile through `ProfileID`.

**Cardinality:**

`Profile 1 : M Enrolments`

## 5. Categories to Enrolments

**Relationship:** One-to-Many

A category can have multiple participant enrolments.

Each enrolment belongs to one category through `CategoryID`.

**Cardinality:**

`Categories 1 : M Enrolments`

## 6. Enrolments to Results

**Relationship:** One-to-Zero-or-One

An enrolment may have no result if the participant has not completed the event yet.

Once a result is recorded, that enrolment can have one result.

The `EnrolmentID` in the Results table is unique.

**Cardinality:**

`Enrolments 1 : 0..1 Results`

## Summary

The ERD relationships allow RaceDay to connect:

- Users with their profiles
- Organisers with events
- Events with categories
- Participants with enrolments
- Categories with enrolments
- Enrolments with race results

These relationships provide the database structure required to support the RaceDay event management system.
