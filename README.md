# TaskTracker API

A task management REST API built with **ASP.NET Core 9** — projects, tasks and labels with JWT authentication. Built as a backend learning/portfolio project.

## Stack

- **.NET 9 / ASP.NET Core** Web API (controllers)
- **EF Core** + **PostgreSQL** (Npgsql)
- **JWT** Bearer authentication, **BCrypt** password hashing
- **FluentValidation** for request validation
- **Docker** + docker-compose (API + PostgreSQL)

## Features

- CRUD for **Projects**, **Tasks** and **Labels**
- **Many-to-many** between tasks and labels (attach / detach)
- Pagination, search and sorting on list endpoints
- JWT auth with `[Authorize]`; resource owner is taken from the token, not the request body
- Global error handling via `ProblemDetails` (RFC 7807)
- Declarative validation (auto-validation, returns `400` with details)

## Run with Docker (recommended)

```bash
docker compose up --build
```
API → `http://localhost:8080`, PostgreSQL → `localhost:5433`. Migrations are applied automatically on startup.

## Run locally

Prerequisites: .NET 9 SDK, PostgreSQL on `localhost:5432` (db `tasktracker_db`).

```bash
# JWT signing key is NOT stored in the repo — provide it via user-secrets:
dotnet user-secrets set "Jwt:Key" "<your-32+char-secret>" --project TaskTracker

dotnet run --project TaskTracker
```

## Auth flow

```
POST /api/auth/register   { "username", "password" }      → создаёт пользователя
POST /api/auth/login      { "username", "password" }      → { "token": "<JWT>" }
# дальше — заголовок:  Authorization: Bearer <token>
```

## Main endpoints

| Method | Route | Описание |
|---|---|---|
| `POST` | `/api/auth/register` · `/api/auth/login` | регистрация / логин |
| `GET/POST/PUT/DELETE` | `/api/projects` | проекты (CRUD, пагинация/поиск/сортировка) |
| `GET/POST/PUT/DELETE` | `/api/taskitems` | задачи |
| `GET/POST/DELETE` | `/api/labels` | метки |
| `POST/DELETE` | `/api/taskitems/{taskId}/labels/{labelId}` | привязать / отвязать метку |

> Все эндпоинты кроме `auth` требуют JWT. Готовые запросы — в `TaskTracker/TaskTracker.http`.

## Security notes

Secrets (JWT key, DB password) are never committed — supplied via **user-secrets** (dev) or **environment variables** (Docker / prod).
