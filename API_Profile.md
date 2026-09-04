# RaceDay Profile API

## Overview

The RaceDay profile functionality allows authenticated users to view and update their own profile information.

Both Organisers and Participants can manage their own profile.

## Endpoints

### 1. Get My Profile

**HTTP Method:** GET

**Endpoint:**

`/api/profile`

**Role Required:** Authenticated User

**Description:**

Retrieves the profile information of the currently logged-in user.

**Request Body:**

None

**Expected Responses:**

- `200 OK` - Profile retrieved successfully
- `401 Unauthorized` - User is not authenticated
- `404 Not Found` - Profile does not exist

---

### 2. Update My Profile

**HTTP Method:** PUT

**Endpoint:**

`/api/users/me`

**Role Required:** Participant / Organiser

**Description:**

Updates the profile information of the currently logged-in user.

**Request Body:**

```json
{
  "name": "John Smith",
  "email": "john@example.com",
  "phone": "0712345678"
}
