
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
- Repository creation and file upload via ZIP
- Repository file tree browser with folder navigation and last commit messages
- File viewer (blob) with line numbers and language detection
- Clickjacking protection (X-Frame-Options + CSP)

## API Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /api/auth/register | No | Register new user, sets auth cookies |
| POST | /api/auth/login | No | Login, sets auth cookies |
| POST | /api/auth/refresh | No | Refreshes access token via HttpOnly cookie |
| DELETE | /api/auth/logout | No | Logout, clears auth cookies and revokes refresh token |
| POST | /api/repo/new | 🔒 | Create a new repository |
| GET | /api/repo/user | 🔒 | Get all repositories for the authenticated user |
| POST | /api/repo/{repoId}/upload | 🔒 | Upload a ZIP file as a new commit |
| GET | /api/repo/{username}/{repoName} | Public* | Get repository details and file tree |
| GET | /api/repo/{username}/{repoName}/blob | Public* | Get file content |

> \* Public repos are accessible without authentication. Private repos require the owner's token.

# DevHub API

Base URL: `http://localhost:5207/api`

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

### POST /api/repo/{repoId}/upload 🔒
Upload a ZIP file as a new commit. Extracts files, ignores `.git` and `node_modules`.

**Form Data**
- `File` — `.zip` file
- `Message` — commit message string

**Response `200 OK`**
```json
{ "success": true, "message": "..." }
```

---

### GET /api/repo/{username}/{repoName} `Public*`
Get repository details and file tree for the given path.

**Query Params**
- `path` (optional) — folder path to browse, defaults to root

**Response `200 OK`**
```json
{
  "id": "guid",
  "name": "string",
  "ownerUsername": "string",
  "ownerId": "guid",
  "isPrivate": false,
  "tree": [
    {
      "name": "string",
      "path": "string",
      "type": "tree | blob",
      "lastCommitMessage": "string"
    }
  ]
}
```

---

### GET /api/repo/{username}/{repoName}/blob `Public*`
Get file content for the blob viewer.

**Query Params**
- `path` — full file path (e.g. `src/app/app.ts`)

**Response `200 OK`**
```json
{
  "path": "string",
  "content": "string",
  "language": "typescript | csharp | plaintext | ..."
}
```

**Response `404 Not Found`** — file or repo not found  
**Response `403 Forbidden`** — private repo, not the owner

---

> 🔒 — requires valid `accessToken` cookie  
> \* — public repos accessible without auth; private repos require owner token

## Project Structure

```
DevHub/
├── Frontend/          # Angular 17+ app
│   └── src/app/
│       ├── core/      # services, models, guards
│       ├── features/  # landing, auth, dashboard, repository
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

Frontend runs on http://localhost:4200, backend on http://localhost:5207.

## Environment

Create `Frontend/src/environments/environment.ts`:

```typescript
export const environment = {
  apiUrl: 'http://localhost:5207'
};
```
