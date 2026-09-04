# RaceDay Enrolments API

## Overview

The RaceDay Enrolments API allows Participants to enrol in event categories.

Organisers can view the enrolments for their events.

## Endpoints

### 1. Enrol in an Event

**HTTP Method:** POST

**Endpoint:**

`/api/events/{id}/enrolments`

**Role Required:** Participant

**Description:**

Allows a Participant to enrol in a category for a specific RaceDay event.

**Request Body:**

```json
{
  "categoryId": 1
}
