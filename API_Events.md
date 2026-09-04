# RaceDay Events API

## Overview

The RaceDay Events API allows Organisers to create, update and delete events.

Both Organisers and Participants can view available events.

## Endpoints

### 1. Create Event

**HTTP Method:** POST

**Endpoint:**

`/api/events`

**Role Required:** Organiser

**Description:**

Creates a new RaceDay event.

**Request Body:**

```json
{
  "name": "Johannesburg City Run",
  "description": "Annual city running event",
  "date": "2026-10-10",
  "location": "Johannesburg",
  "distance": 10,
  "eventType": "Run"
}
