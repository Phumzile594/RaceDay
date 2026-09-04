# RaceDay Authentication API

## Overview

The RaceDay authentication functionality allows users to register an account and securely log in to the system.

The system supports two roles:

- Organiser
- Participant

## Endpoints

### 1. Register User

**HTTP Method:** POST

**Endpoint:**

`/api/auth/register`

**Role Required:** Public

**Description:**

Registers a new RaceDay user account.

**Request Body:**

```json
{
  "firstName": "John",
  "lastName": "Smith",
  "email": "john@example.com",
  "password": "Password123",
  "role": "Participant"
}
