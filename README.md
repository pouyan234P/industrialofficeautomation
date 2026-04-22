# Industrial Office Automation System

A microservices-based office automation platform built with **.NET 8** and **Clean Architecture**. The system handles official correspondence (letters), document workflows (referrals), full-text search, and user identity — all orchestrated through a central API gateway.

---

## Architecture Overview

```
┌─────────────┐      ┌──────────────────┐
│   Web API   │─────▶│  Ocelot Gateway  │
│  (BFF/Facade│      │  (Port 5003)     │
│  Port 5xxx) │      └────────┬─────────┘
└─────────────┘               │
                    ┌─────────┼─────────────────┐
                    ▼         ▼                 ▼
         ┌──────────────┐  ┌──────────┐  ┌──────────────┐
         │Correspondence│  │ Workflow │  │    Search    │
         │Core Service  │  │ Service  │  │   Service    │
         │  (Port 5001) │  │(Port 5191│  │ (Port 5273)  │
         └──────┬───────┘  └────┬─────┘  └──────┬───────┘
                │               │               │
             SQL DB          MongoDB        Elasticsearch
                    ▲
         ┌──────────────┐
         │Authentication│
         │   Service    │
         │  (Port 5230) │
         └──────────────┘

         RabbitMQ (async messaging between services)
```

All services follow **Clean Architecture** with `Domain → Application → Infrastructure → API` layering and use **CQRS with MediatR** for request handling.

---

## Services

### 1. `industrialGateway` — API Gateway
The single entry point for all client traffic. Built on **Ocelot**, it routes upstream requests to the appropriate downstream microservice.

**Routes proxied:**

| Upstream Path | Downstream Service | Port |
|---|---|---|
| `/api/Letter/*` | Correspondence Core | 5001 |
| `/api/attachment/*` | Correspondence Core | 5001 |
| `/api/generateNextNumber/*` | Correspondence Core | 5001 |
| `/api/referral/*` | Workflow Service | 5191 |
| `/api/letterelsi/*` | Search Service | 5273 |

---

### 2. `automationCorrespondencecoreService` — Correspondence Core
Manages the lifecycle of official **letters** and their **attachments**, including auto-generating sequential letter numbers per department per year.

**Key features:**
- Create, retrieve, and update letters with rich metadata (subject, abstract, HTML body, priority, confidentiality, type)
- Manage file attachments linked to letters
- Auto-generate sequential letter numbers (`generateNextNumber`) using a per-department, per-year `IndicatorBook` table with a composite unique index
- Store rich HTML letter bodies separately in **MongoDB**
- Enum-based fields: `Priority`, `Confidentiality`, `Type`, `FileType`

**Dual-database design:**
| Store | What it holds |
|---|---|
| SQL Server (EF Core) | Letters, Attachments, IndicatorBook (sequential numbering) |
| MongoDB | HTML body content of letters |

**Stack:** .NET 8 · Clean Architecture · CQRS/MediatR · SQL Server (EF Core) · MongoDB

---

### 3. `automationWorkflowservice` — Workflow / Referral Service
Handles the **routing and referral** of letters between organizational positions and departments.

**Key features:**
- Create referrals linking a letter to sender/receiver positions
- Query referrals by position or receiver
- Async updates via **RabbitMQ** (`myreferral`, `updatereferral` queues)
- Enum-based fields: `ActionType`, `Priority`, `ReferralStatus`, `Type`

**Stack:** .NET 8 · Clean Architecture · CQRS/MediatR · **MongoDB**

---

### 4. `automationSearchservice` — Search Engine Service
Provides **full-text search** over letter documents.

**Key features:**
- Index letter documents (letter number, subject, abstract, plain-text body, type, department, creator position)
- Full-text search across indexed letters
- Listens to RabbitMQ (`myels` queue) for async indexing triggered by other services

**Stack:** .NET 8 · Clean Architecture · CQRS/MediatR · **Elasticsearch**

---

### 5. `Authentication` — Identity Service
Manages users, roles, departments, and positions. Issues **JWT tokens** consumed by the rest of the platform.

**Key features:**
- User registration and login (JWT)
- Role management
- Department and position CRUD
- Signature image upload/retrieval per user

**Stack:** .NET 8 · Clean Architecture · CQRS/MediatR · ASP.NET Core Identity · JWT Bearer

---

### 6. `webapi` — BFF / Facade API
The public-facing Web API that clients interact with directly. It acts as a **Backend-for-Frontend**, calling the downstream microservices via HTTP and publishing events to RabbitMQ.

**Controllers:**

| Controller | Responsibility |
|---|---|
| `authController` | Register, login, create roles |
| `departmentController` | Department management |
| `positionController` | Position management |
| `userController` | User profile management |
| `pictureController` | Signature image upload |
| `LetterController` | Letter CRUD (calls Correspondence Core) |
| `referralController` | Create & manage referrals (calls Workflow Service + publishes to RabbitMQ) |
| `letterelsiController` | Trigger search indexing & run searches |

---

## Technology Stack

| Category | Technology |
|---|---|
| Framework | .NET 8 / ASP.NET Core |
| Architecture | Clean Architecture + CQRS + MediatR |
| API Gateway | Ocelot |
| Auth | ASP.NET Core Identity + JWT Bearer |
| Relational DB | SQL Server (EF Core) |
| Document DB | MongoDB |
| Search | Elasticsearch (NEST/Elastic client) |
| Messaging | RabbitMQ |
| Caching | Redis (StackExchange.Redis) |
| Validation | FluentValidation |
| API Docs | Swagger / Swashbuckle |

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server
- MongoDB
- Elasticsearch
- RabbitMQ
- Redis

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/your-repo-name.git
cd your-repo-name
```

### 2. Configure each service

Each service has its own `appsettings.json`. Update connection strings and settings before running:

**Gateway** (`Gateway/industrialGateway/appsettings.json`):
```json
{
  "Ocelot": {
    "Routes": [ ... ]   // update Host/Port per your environment
  }
}
```

**Web API** (`webapi/appsettings.json`):
```json
{
  "ServiceUrls": {
    "gatewayApi": "http://localhost:5003",
    "IdentityApi": "http://localhost:5230"
  },
  "appSetting": {
    "Token": "YOUR_SECRET_JWT_KEY_HERE"
  },
  "TopicAndQueueNames": {
    "myreferral": "myreferral",
    "myels": "myels",
    "updatereferral": "updatereferral"
  }
}
```

Update the corresponding `appsettings.json` in the Correspondence, Workflow, Search, and Authentication services with your database and RabbitMQ connection strings.

### 3. Run the services

Start each service independently. The recommended startup order is:

```bash
# 1. Authentication Service
cd Authentication && dotnet run

# 2. Correspondence Core Service
cd automationCorrespondencecoreService && dotnet run

# 3. Workflow Service
cd automationWorkflowservice && dotnet run

# 4. Search Service
cd automationSearchservice && dotnet run

# 5. API Gateway
cd Gateway/industrialGateway && dotnet run

# 6. Web API (BFF)
cd webapi && dotnet run
```

### 4. Explore the API

Once running, Swagger UI is available at:

```
http://localhost:{port}/swagger
```

---

## Project Structure

```
├── Authentication/                    # Identity & user management service
│   └── Core/
│       ├── Application/               # CQRS handlers, DTOs, features
│       └── Domain/                    # Domain entities
├── automationCorrespondencecoreService/   # Letters & attachments service
│   ├── core/
│   │   ├── Application/
│   │   └── Domain/
│   └── infrastructure/
├── automationWorkflowservice/         # Referral/routing workflow service
│   ├── core/
│   │   ├── Application/
│   │   └── Domain/
│   └── infrastructure/                # MongoDB persistence
├── automationSearchservice/           # Elasticsearch search service
│   ├── Core/
│   │   ├── Application/
│   │   └── Domain/
│   └── infrastructure/                # Elasticsearch persistence
├── Gateway/
│   └── industrialGateway/             # Ocelot API gateway
└── webapi/                            # BFF / public-facing Web API
    ├── Controllers/
    │   └── api/
    │       ├── authentication/
    │       ├── correspondece/
    │       ├── search/
    │       └── workflow/
    └── Services/
```

---

## Messaging (RabbitMQ)

The platform uses RabbitMQ for asynchronous, event-driven communication between services:

| Queue / Topic | Published by | Consumed by |
|---|---|---|
| `myreferral` | `referralController` (WebAPI) | Workflow Service |
| `updatereferral` | `referralController` (WebAPI) | Workflow Service |
| `myels` | `referralController` / `letterelsiController` | Search Service |

---

## License

This project is licensed under the [MIT License](LICENSE).
