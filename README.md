
# DevHub

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
| POST | `/api/auth/register` | No | Register new user, sets auth cookies |
| POST | `/api/auth/login` | No | Login, sets auth cookies |
| POST | `/api/auth/refresh` | No | Refreshes access token via HttpOnly cookie. Returns 401 if token is missing or invalid |
| POST | `/api/auth/logout` | No | Logout, clears auth cookies and revokes refresh token |

## Project Structure

DevHub/
├── Frontend/          # Angular 17+ app
│   └── src/app/
│       ├── core/      # services, models
│       ├── features/  # landing, authentication, dashboard
│       └── shared/    # reusable components (navbar)
└── Backend/           # .NET 10 Web API
└── Backend/
├── Controllers/
├── Services/
├── Repositories/
├── Models/
└── Interfaces/



## Getting Started

**Backend**
```bash
cd Backend
dotnet restore
dotnet ef database update
dotnet run
Frontend


cd Frontend
npm install
ng serve
Frontend runs on http://localhost:4200, backend on https://localhost:7081.

Environment
Create Frontend/src/environments/environment.ts:


export const environment = {
  apiUrl: 'https://localhost:7081'
};
