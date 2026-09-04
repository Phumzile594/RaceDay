# RaceDay Categories API

## Overview

The RaceDay Categories API allows Organisers to create categories for events.

Participants and Organisers can view the categories available for each event.

## Endpoints

### 1. Create Event Category

**HTTP Method:** POST

**Endpoint:**

`/api/events/{id}/categories`

**Role Required:** Organiser

**Description:**

Creates a category for a specific RaceDay event.

**Request Body:**

```json
{
  "name": "10km Open",
  "distance": 10,
  "maximumParticipants": 100,
  "entryFee": 150.00
}
