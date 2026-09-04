# RaceDay Results API

## Overview

The RaceDay Results API allows Organisers to record participant race results.

Participants can view their own results after completing an event.

## Endpoints

### 1. Record Race Result

**HTTP Method:** POST

**Endpoint:**

`/api/enrolments/{id}/results`

**Role Required:** Organiser

**Description:**

Records the race result for a participant enrolment.

**Request Body:**

```json
{
  "finishTime": "00:52:35",
  "position": 12,
  "resultStatus": "Finished"
}
