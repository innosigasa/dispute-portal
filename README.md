# Transactions Dispute Portal

A production-grade full-stack web application where bank customers can view their transactions, raise disputes, and track dispute history. Back-office agents can review and resolve disputes through a dedicated agent dashboard.

---

## Table of Contents

- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Running the Project](#running-the-project)
  - [Option A — Full Docker (recommended for reviewers)](#option-a--full-docker-recommended-for-reviewers)
  - [Option B — PostgreSQL in Docker, API & UI on your machine](#option-b--postgresql-in-docker-api--ui-on-your-machine)
  - [Option C — Everything local (no Docker)](#option-c--everything-local-no-docker)
- [Environment Variables](#environment-variables)
- [Test Credentials](#test-credentials)
- [Seed Data](#seed-data)
- [Running Tests](#running-tests)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [Known Limitations](#known-limitations)

---

## Architecture

The backend follows **Clean Architecture** with strict layer separation:

```
┌─────────────────────────────────────────┐
│            DisputePortal.Api            │  Controllers · Auth · JWT · Swagger
│         (ASP.NET Core Web API)          │
├─────────────────────────────────────────┤
│       DisputePortal.Application         │  Domain · Entities · DTOs · Validators
│         (Business Logic Layer)          │  State Machine · Service Interfaces
├─────────────────────────────────────────┤
│       DisputePortal.Persistence         │  EF Core · Repositories · Migrations
│         (Data Access Layer)             │  Seed Data · DbContext
├─────────────────────────────────────────┤
│          DisputePortal.Tests            │  xUnit · FluentAssertions · Moq
│            (Unit Tests)                 │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│           dispute-portal-ui             │  React 19 · TypeScript · Vite
│              (Frontend)                 │  MUI Joy UI · AG Grid · Formik
└─────────────────────────────────────────┘
```

### Dispute State Machine

```
Submitted ──► UnderReview ──► Resolved
                          └──► Rejected
```

No backward transitions. No skipping steps. Enforced by `DisputeStatusTransition.EnsureValid()`.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | .NET 10 ASP.NET Core Web API |
| ORM | EF Core 10 + Npgsql (PostgreSQL 16) |
| Validation | FluentValidation 12 |
| Logging | Serilog (console + structured) |
| Auth | JWT HS256 — `customer` and `agent` roles |
| API Docs | Swashbuckle 7.x (Swagger UI) |
| Frontend | React 19 + TypeScript (Vite) |
| UI Library | MUI Joy UI |
| Data Tables | AG Grid Community (infinite scroll & client-side row models) |
| Forms | Formik + Yup |
| HTTP Client | native `fetch` API with JWT interceptors (`httpHelper`) |
| Routing | React Router v7 |
| Database | PostgreSQL 16 |
| Infrastructure | Docker + Docker Compose |

---

## Prerequisites

| Tool | Version | Notes |
|------|---------|-------|
| Docker Desktop | 24+ | Required for Option A & B |
| Docker Compose | v2 (bundled with Docker Desktop) | |
| .NET SDK | 10.0 | Required for Option B & C |
| Node.js | 20+ | Required for Option B & C |
| npm | 10+ | Bundled with Node.js |
| PostgreSQL | 16 | Required for Option C only |

---

## Running the Project

### Option A — Full Docker (recommended for reviewers)

Everything — database, API, and frontend — runs in containers. No local tooling required beyond Docker.

**1. Clone the repository**

```bash
git clone https://github.com/<your-username>/dispute-portal.git
cd dispute-portal
```

**2. Create your environment file**

```bash
cp .env.example .env
```

The defaults in `.env.example` work out of the box for a local Docker run. If you want to change the JWT secret or database credentials, edit `.env` before starting.

**3. Build and start all services**

```bash
docker compose up --build
```

On first run this will:
- Pull the PostgreSQL 16 image
- Build the .NET API image (multi-stage)
- Build the React UI image (Vite → Nginx)
- Run EF Core migrations automatically
- Seed the database with demo data

| Service | URL |
|---------|-----|
| Frontend | http://localhost:3000 |
| API | http://localhost:5000 |
| Swagger UI | http://localhost:5000/swagger |
| PostgreSQL | localhost:5432 |

**4. Stop the stack**

```bash
docker compose down          # stop containers, keep data volume
docker compose down -v       # stop containers AND delete database volume
```

---

### Option B — PostgreSQL in Docker, API & UI on your machine

This is the recommended setup for **active development**. The database runs in Docker; the API and UI run natively for fast rebuild times and debugger support.

**1. Start only the database**

```bash
docker compose up db
```

PostgreSQL will be available at `localhost:5432`.

**2. Configure the API**

Open [DisputePortal.Api/appsettings.Development.json](DisputePortal.Api/appsettings.Development.json) and verify (or update) the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=dispute_portal;Username=postgres;Password=postgres"
  }
}
```

If you changed the database credentials in `.env`, update this connection string to match.

**3. Run the API**

```bash
cd DisputePortal.Api
dotnet run
```

The API starts at **http://localhost:5000** (or **https://localhost:5001**).  
Swagger UI is at **http://localhost:5000/swagger**.

On first launch (Development environment), EF Core migrations and seed data are applied automatically.

**4. Run the frontend**

Open a second terminal:

```bash
cd dispute-portal-ui
npm install
npm run dev
```

The frontend starts at **http://localhost:3000** (configured in `dispute-portal-ui/vite.config.ts`).

`vite.config.ts` proxies all `/api` requests to `http://localhost:5000`, so no extra environment variables are needed as long as the API is on its default port. If you change the API port, update the proxy target in `vite.config.ts`.

---

### Option C — Everything local (no Docker)

Use this if you have PostgreSQL installed directly on your machine.

**1. Create the database**

Connect to your local PostgreSQL instance and create the database:

```sql
CREATE DATABASE dispute_portal;
```

**2. Set the connection string**

Edit [DisputePortal.Api/appsettings.Development.json](DisputePortal.Api/appsettings.Development.json):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=dispute_portal;Username=<your-pg-user>;Password=<your-pg-password>"
  }
}
```

**3. Apply migrations (optional)**

Migrations run automatically on startup, but you can also apply them manually:

```bash
cd DisputePortal.Api
dotnet ef database update
```

**4. Run the API**

```bash
cd DisputePortal.Api
dotnet run
```

**5. Run the frontend**

```bash
cd dispute-portal-ui
npm install
npm run dev
```

---

## Environment Variables

The `.env` file (copied from `.env.example`) controls Docker Compose configuration.

| Variable | Default | Description |
|----------|---------|-------------|
| `POSTGRES_USER` | `postgres` | PostgreSQL username |
| `POSTGRES_PASSWORD` | `postgres` | PostgreSQL password |
| `POSTGRES_DB` | `dispute_portal` | PostgreSQL database name |
| `JWT_SECRET_KEY` | `your-super-secret-key-at-least-32-characters-long` | HS256 signing secret — **change in production** |
| `JWT_ISSUER` | `DisputePortalApi` | JWT issuer claim |
| `JWT_AUDIENCE` | `DisputePortalClients` | JWT audience claim |
| `JWT_ACCESS_TOKEN_EXPIRY_MINUTES` | `60` | Access token lifetime |
| `JWT_REFRESH_TOKEN_EXPIRY_DAYS` | `7` | Refresh token lifetime |
| `VITE_API_BASE_URL` | `/api` | API base URL baked into the UI at build time. Defaults to `/api` (proxied by nginx in Docker). Set to `https://localhost:7062/api` for local dev with `npm run dev`. |

> **Security note:** Never commit a `.env` file with real secrets. The `.env.example` file is safe to commit — it contains only placeholder values.

For the API when running locally (Options B & C), JWT settings are read from `DisputePortal.Api/appsettings.Development.json` or overridden via environment variables using the `Jwt__SecretKey` double-underscore convention.

---

## Test Credentials

These accounts are created automatically by the seed process on first run:

| Role | Email | Password |
|------|-------|----------|
| Customer 1 | alice@example.com | Password1! |
| Customer 2 | bob@example.com | Password1! |
| Agent | agent@bank.com | Password1! |

Customers can only see their own transactions and disputes. The agent account can see and manage all disputes across all customers.

---

## Seed Data

On first run in the `Development` environment the API automatically seeds:

- 2 customer accounts + 1 agent account
- 50 transactions per customer (~100 total) with randomised amounts, categories, and dates
- 7 pre-existing disputes in various states (Submitted, UnderReview, Resolved, Rejected)
- Corresponding `DisputeStatusHistory` audit trail for each dispute
- All `DisputeReason` and `TransactionCategoryLookup` reference data

Re-seeding is idempotent — it checks for existing data before inserting.

---

## Running Tests

The test suite covers the Application layer (business logic, validators, state machine):

```bash
# From the repository root
dotnet test DisputePortal.Tests/DisputePortal.Tests.csproj

# With detailed output
dotnet test DisputePortal.Tests/DisputePortal.Tests.csproj --logger "console;verbosity=detailed"

# With code coverage report
dotnet test DisputePortal.Tests/DisputePortal.Tests.csproj --collect:"XPlat Code Coverage"
```

Or run all projects in the solution:

```bash
dotnet test DisputePortal.slnx
```

### Frontend type check

```bash
cd dispute-portal-ui
npx tsc --noEmit       # TypeScript type check (no output files)
npm run build          # Full production build (fails on type errors)
npm run lint           # ESLint
```

---

## API Documentation

Swagger UI is available at `/swagger` in the `Development` environment.

All endpoints except `/api/auth/login` and `/api/auth/refresh` require a `Bearer` JWT token in the `Authorization` header.

### Quick API test flow

1. `POST /api/auth/login` — authenticate and receive `accessToken` + `refreshToken`
2. Copy the `accessToken` value
3. Click **Authorize** in Swagger UI and paste: `Bearer <token>`
4. All protected endpoints are now accessible

---

## Project Structure

```
dispute-portal/
├── DisputePortal.Api/           # ASP.NET Core 10 Web API
│   ├── Controllers/             # Thin controllers — validation + one service call
│   ├── Services/                # Auth, JWT, Notification, Lookup services
│   ├── Middleware/              # Global exception handler
│   ├── Dockerfile               # Multi-stage Docker build
│   └── Program.cs               # DI wiring, JWT, Swagger, CORS, seed trigger
│
├── DisputePortal.Application/   # Business logic (no infrastructure dependencies)
│   ├── Domain/
│   │   ├── Entities/            # Customer, Transaction, Dispute, AppUser, …
│   │   ├── Enums/               # DisputeStatus, DisputeReason, …
│   │   └── StateMachine/        # DisputeStatusTransition.EnsureValid()
│   ├── DTOs/                    # Request/response contracts
│   ├── Interfaces/              # IDisputeService, ITransactionRepository, …
│   └── Validators/              # FluentValidation validators
│
├── DisputePortal.Persistence/   # Data access
│   ├── DbContext/               # AppDbContext
│   ├── Configurations/          # IEntityTypeConfiguration per entity
│   ├── Migrations/              # EF Core migration files
│   ├── Repositories/            # Projection-based repository implementations
│   └── Seed/                    # DataSeeder (dev environment only)
│
├── DisputePortal.Tests/         # xUnit unit tests
│   ├── Services/                # Service-layer tests
│   ├── Validators/              # Validator tests
│   └── StateMachine/            # State machine transition tests
│
├── dispute-portal-ui/           # React 19 + TypeScript (Vite)
│   ├── src/
│   │   ├── modules/             # Feature modules (auth, transactions, disputes, agent, dashboard, accounts)
│   │   │   └── <feature>/
│   │   │       ├── pages/       # Route-level page components
│   │   │       ├── services/    # API calls via httpHelper
│   │   │       ├── models/      # TypeScript interfaces
│   │   │       └── requests/    # Request shape types
│   │   ├── components/          # Shared UI components (GridTable, StatusBadge, …)
│   │   ├── layouts/             # AppLayout (authenticated shell), AuthLayout
│   │   ├── store/               # Auth context — AuthProvider, useAuth hook
│   │   ├── domain/              # Shared domain types (RequestResult<T>)
│   │   ├── utils/               # httpHelper (fetch wrapper + JWT refresh), formatters
│   │   └── api/                 # client.ts — legacy Axios stub (unused)
│   ├── Dockerfile               # Vite build → Nginx
│   ├── nginx.conf               # SPA routing + /api reverse-proxy
│   └── vite.config.ts           # Dev server on :3000; proxies /api → :5000
│
├── docker-compose.yml           # db + api + ui services
├── .env.example                 # Environment variable template
└── README.md
```

---

## Known Limitations

- **Document upload** — simulated (file name recorded, no actual binary storage)
- **Email/SMS notifications** — not real; status change events are persisted to the `notifications` table and logged via Serilog
- **Bulk status update** — marked as nice-to-have; not implemented
- **JWT algorithm** — HS256 is used; RS256 is out of scope
- **HTTPS in Docker** — the container exposes HTTP on port 8080; TLS termination would sit at a reverse proxy (e.g., Nginx or a cloud load balancer) in a real deployment
