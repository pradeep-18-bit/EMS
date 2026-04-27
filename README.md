# Employee Management System

Production-oriented full-stack EMS repository with a React/Vite frontend and a .NET 8 Web API backend.

## Structure

```text
EMS/
  backend/
    Dockerfile
    EmployeeManagementSystem.sln
    EmployeeManagementSystem/
      Controllers/
      DTOs/
      Data/
      Interfaces/
      Models/
      Services/
      Templates/
  frontend/
    Dockerfile
    nginx.conf
    package.json
    src/
  docker-compose.yml
  .env.example
```

Only source code, templates, Docker files, and configuration examples belong in Git. Build output, IDE state, generated payslips/letters, uploads, and local `.env` files are ignored.

## Local Setup

1. Copy `.env.example` to `.env`.
2. Fill in the database, JWT, SMTP, and CORS values.
3. Start the stack:

```powershell
docker compose up --build
```

Frontend: `http://localhost:8080`

Backend API: `http://localhost:5007`

## Development Checks

```powershell
dotnet build backend/EmployeeManagementSystem.sln
```

```powershell
cd frontend
npm ci
npm run build
```
