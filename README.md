
# DevHub
> UI design generated with AI assistance, implemented and customized manually.

A GitHub-inspired developer platform built with Angular 17+ and .NET 10.

## Tech Stack

**Frontend**
- Angular 17+ (standalone components, signals, built-in control flow)
- SCSS with BEM naming convention
- Reactive Forms with custom validators
- Angular Router

**Backend**
- .NET 10 Web API
- Entity Framework Core
- JWT authentication (HttpOnly cookies)
- BCrypt password hashing

## Features

- Landing page
- Authentication (register / login) with form validation
- JWT access token (15min) + refresh token (7 days) stored in HttpOnly cookies
- Dashboard with sidebar, repository list, stats, and activity feed
- Shared navbar with responsive drawer on mobile

## API Endpoints ( for now )

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /api/auth/register | No | Register new user, sets auth cookies |
| POST | /api/auth/login | No | Login, sets auth cookies |
| POST | /api/auth/refresh | No | Refreshes access token via HttpOnly cookie. Returns 401 if token is missing or invalid |
| DELETE | /api/auth/logout | No | Logout, clears auth cookies and revokes refresh token |
| POST | /api/repo/new | ✅ | Create a new repository |
| GET | /api/repo/user | ✅ | Get all repositories for the authenticated user |

# DevHub API

Base URL: `https://localhost:7081/api`

Auth: JWT via HttpOnly cookie (`accessToken`). Protected endpoints require a valid token.

## Authentication

### POST /api/auth/register
Register a new user.

**Body**
```json
{
  "firstName": "string",
  "lastName": "string",
  "username": "string",
  "email": "string",
  "password": "string"
}
```

**Response `200 OK`**
```json
{ "success": true, "message": "User registered successfully" }
```
Sets `accessToken` (15min) and `refreshToken` (7d) cookies.

**Response `400 Bad Request`**
```json
{ "success": false, "message": "An error occurred while registering the user" }
```

---

### POST /api/auth/login
Login with email and password.

**Body**
```json
{
  "email": "string",
  "password": "string",
  "rememberMe": false
}
```

**Response `200 OK`**
```json
{ "success": true, "message": "string" }
```
Sets `accessToken` (15min) and `refreshToken` (7d / 30d if `rememberMe`) cookies.

**Response `401 Unauthorized`**
```json
{ "success": false, "message": "Invalid credentials" }
```

---

### POST /api/auth/refresh
Rotate refresh token and issue a new access token.

**Cookie required:** `refreshToken`

**Response `200 OK`**
```json
{ "success": true }
```
Sets new `accessToken` and `refreshToken` cookies. Old refresh token is revoked.

**Response `401 Unauthorized`**
```json
{ "success": false, "message": "Invalid or expired refresh token" }
```

> ⚠️ Token reuse detected (reusing a revoked token) revokes all tokens for that user.

---

### DELETE /api/auth/logout
Revoke refresh token and clear auth cookies.

**Cookie required:** `refreshToken`

**Response `200 OK`**

---

## Repositories

### POST /api/repo/new 🔒
Create a new repository.

**Body**
```json
{
  "name": "string",
  "description": "string | null",
  "isPrivate": false
}
```

**Response `200 OK`**
```json
{ "success": true, "message": "Successfully created a new repository!" }
```

**Response `400 Bad Request`**
```json
{ "success": false, "message": "You already have a repository with that name" }
```

---

### GET /api/repo/user 🔒
Get all repositories for the authenticated user.

**Response `200 OK`**
```json
[
  {
    "id": "guid",
    "name": "string",
    "description": "string | null",
    "isPrivate": false,
    "createdAt": "2026-06-19T00:00:00"
  }
]
```

---

> 🔒 — requires valid `accessToken` cookie
## Project Structure

```
DevHub/
├── Frontend/          # Angular 17+ app
│   └── src/app/
│       ├── core/      # services, models
│       ├── features/  # landing, authentication, dashboard
│       └── shared/    # reusable components (navbar)
├── Backend/           # .NET 10 Web API
    ├── Controllers/
    ├── Data/
    ├── Dto/
    ├── Interfaces/
    ├── Migrations/
    ├── Models/
    ├── Repositories/
    ├── Responses/
    ├── Security/
    ├── Services/
    └── Utility/
```


## Getting Started

**Backend**
```bash
cd Backend
dotnet restore
dotnet ef database update
dotnet run
```
**Frontend**
```bash
cd Frontend
npm install
ng serve
```

Frontend runs on http://localhost:4200, backend on https://localhost:7081.

## Environment

Create `Frontend/src/environments/environment.ts`:

```typescript
export const environment = {
  apiUrl: 'https://localhost:7081'
};
```
