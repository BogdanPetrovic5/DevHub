
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


## Features

- Landing page
- Authentication (register / login) with form validation
- JWT access token (15min) + refresh token (7 days) stored in HttpOnly cookies
- Dashboard with sidebar, repository list, stats, and activity feed
- Shared navbar with responsive drawer on mobile
- Repository creation and file upload via ZIP
- Repository file tree browser with folder navigation and last commit messages
- File viewer (blob) with line numbers and language detection

## API Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /api/auth/register | No | Register new user, sets auth cookies |
| POST | /api/auth/login | No | Login, sets auth cookies |
| POST | /api/auth/refresh | No | Refreshes access token via HttpOnly cookie |
| DELETE | /api/auth/logout | No | Logout, clears auth cookies and revokes refresh token |
| POST | /api/auth/cli-login | CLI only* | Login for CLI, returns token in response body |
| POST | /api/repo/new | 🔒 | Create a new repository |
| GET | /api/repo/user | 🔒 | Get all repositories for the authenticated user |
| POST | /api/repo/{repoId}/upload | 🔒 | Upload a ZIP file as a new commit |
| PUT | /api/repo/{repoId}/push | 🔒 | Push changes with change detection |
| GET | /api/repo/{username}/{repoName} | Public* | Get repository details and file tree |
| GET | /api/repo/{username}/{repoName}/blob | Public* | Get file content |
| GET | /api/repo/{username}/{repoName}/commits | Public* | Get commit history |
| GET | /api/repo/{username}/{repoName}/commits/{commitId} | Public* | Get commit details with changed files |
| GET | /api/repo/activity | 🔒 | Get recent commit activity grouped by repo |
| GET | /api/auth/me | 🔒 | Get current authenticated user (username, email) |
| GET | /api/user/{username} | Public* | Get public profile with repos and stats |

> `*` Public = accessible without token, but private repositories require authentication
> `*` CLI only = requires `User-Agent: DevHubCLI`

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

### POST /api/auth/cli-login `CLI only`
Login for DevHub CLI. Returns tokens in response body instead of cookies.

**Header required:** `User-Agent: DevHubCLI`

**Body**
```json
{
  "email": "string",
  "password": "string"
}
```

**Response `200 OK`**
```json
{
  "accessToken": "string",
  "refreshToken": "string"
}
```

**Response `401 Unauthorized`**
```json
{ "success": false, "message": "Invalid credentials" }
```

**Response `403 Forbidden`** — missing or invalid `User-Agent` header

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
{ "success": true, "message": "Upload successful" }
```

---

### PUT /api/repo/{repoId}/push 🔒
Push local files to a repository. Compares SHA-256 hashes — only modified, added, or deleted files are recorded. No commit is created if nothing changed.

**Body**
```json
{
  "message": "string",
  "files": [
    { "path": "string", "content": "string" }
  ]
}
```

**Response `200 OK`**
```json
{ "success": true, "message": "Push successful" }
```

**Response `200 OK`** — nothing changed
```json
{ "success": true, "message": "Nothing to push, everything up to date." }
```

**Response `400 Bad Request`**
```json
{ "success": false, "message": "Repository not found." }
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
      "lastCommitMessage": "string",
      "lastCommitAt": "2026-06-19T00:00:00"
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

### GET /api/repo/{username}/{repoName}/commits `Public*`
Get commit history for a repository.

**Response `200 OK`**
```json
[
  {
    "id": "guid",
    "message": "string",
    "authorUsername": "string",
    "createdAt": "2026-06-19T00:00:00"
  }
]
```

**Response `403 Forbidden`** — private repo, not the owner

---

### GET /api/repo/{username}/{repoName}/commits/{commitId} `Public*`
Get details for a specific commit including all changed files.

**Response `200 OK`**
```json
{
  "authorUsername": "string",
  "commitMessage": "string",
  "createdAt": "2026-06-19T00:00:00",
  "files": [
    {
      "path": "string",
      "content": "string",
      "changeType": "Added | Modified | Deleted"
    }
  ]
}
```

**Response `404 Not Found`** — commit not found  
**Response `403 Forbidden`** — private repo, not the owner

---

---

### GET /api/repo/activity 🔒
Get recent commit activity for the authenticated user, grouped by repository.

**Response `200 OK`**
```json
[
  {
    "type": "push",
    "message": "string",
    "repoName": "string",
    "createdAt": "2026-06-19T00:00:00Z",
    "commits": [
      { "shortHash": "string", "message": "string" }
    ]
  }
]
```

---

### GET /api/auth/me 🔒
Get the currently authenticated user's basic info from JWT claims.

**Response `200 OK`**
```json
{
  "username": "string",
  "email": "string"
}
```

---

### GET /api/user/{username} `Public`
Get a user's public profile.

**Response `200 OK`**
```json
{
  "username": "string",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "repoCount": 0,
  "commitCount": 0,
  "repositories": []
}
```

**Response `404 Not Found`** — user not found

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
└── Backend/           # .NET 10 Web API
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
