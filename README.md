# 🎙️ TalkPulse

<!-- Badges -->
![.NET 10](https://img.shields.io/badge/.NET-10.0_LTS-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-21-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-Real--Time-00ADEF?style=for-the-badge&logo=microsoft&logoColor=white)
![Aspire](https://img.shields.io/badge/.NET_Aspire-13-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-EF_Core_10-336791?style=for-the-badge&logo=postgresql&logoColor=white)
![Google ADK](https://img.shields.io/badge/Google_ADK-AI_Insights-4285F4?style=for-the-badge&logo=google&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-22c55e?style=for-the-badge)
![Built Live](https://img.shields.io/badge/Built-Live_on_YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)

> **A real-time audience feedback and live polling platform for tech speakers — built entirely in public during live coding sessions on YouTube.**

**TalkPulse** solves a real problem for conference speakers and livestreamers: collecting structured, meaningful audience feedback during and after a talk — and running live polls that update in real-time without any page refresh.

---

## 📺 Built Live on YouTube

Every line of code, every architectural decision, every bug — built on stream.

| | Schedule | Link |
|---|---|---|
| 🎥 **Live Coding** | Every **Wednesday & Friday** | [Join the Stream →](https://www.youtube.com/watch?v=1nD8oRCNcPI&list=PLc2Ziv7051bZmovEL_XL4THcelJGjFxj3) |
| 📼 **VODs & Tutorials** | Uploaded after each stream | [YouTube Channel →](https://www.youtube.com/@letsprogram30) |

---

## ⚠️ The Problem

As a tech speaker who gives regular talks at meetups and conferences, there are two painful moments:

**1. During the talk:**
> You ask the audience a question or want a quick temperature check — but there is no structured way to do it. Raise of hands is awkward. Chat is chaotic on a livestream.

**2. After the talk:**
> You want real, structured feedback. But Google Forms links get ignored, and the chat has already scrolled away. You never find out what actually resonated.

---

## 💡 The Solution

**TalkPulse** is a dedicated platform that solves both problems:

### 🗳️ Live Polls (During the Talk)
- Speaker creates polls before the session (e.g., *"Which topic should I cover first?"*).
- Audience scans a QR code and their phone syncs to the session instantly.
- Speaker activates a poll — it **pops up on every audience device at the same time**.
- Vote results update **live on the speaker's screen** as the audience votes — no refresh needed.
- Speaker can see results in a **live animated bar chart** and share it on screen.

### 📝 Structured Feedback (During & After the Talk)
- Audience can submit **free-text feedback** at any point during the session.
- After the talk ends, a **dedicated feedback form** appears on audience devices automatically.
- All feedback is collected, stored, and timestamped.

### 🤖 AI-Powered Insights (After the Session)
- A background AI agent (Google ADK) processes all submitted feedback.
- It groups similar comments, extracts key themes, and generates a clean summary.
- The speaker gets an **"Insights Panel"** — e.g., *"80% of your audience loved the live demo. Most questions were about Clean Architecture trade-offs."*
- Topics are auto-tagged and ranked.

---

## 🏗️ Architecture Overview

TalkPulse uses **Vertical Slice Architecture (VSA)** — every feature is a self-contained slice from HTTP endpoint to database. No cross-feature services. No shared repositories for business logic.

---

## 🛠️ Full Tech Stack

| Layer | Technology | Why |
|---|---|---|
| **Runtime** | .NET 10 LTS | Long-term support, C# 14, performance improvements |
| **Orchestration** | .NET Aspire 13 | Code-first infra — no docker-compose needed |
| **API Style** | ASP.NET Core Minimal APIs | Lightweight, fast, one endpoint per slice |
| **Architecture** | Vertical Slice Architecture | Feature-first, self-contained, easy to maintain |
| **CQRS** | MediatR 14 | Commands + Queries + Pipeline Behaviors |
| **Validation** | FluentValidation 12 | Strongly-typed rules per slice |
| **Database** | EF Core 10 + PostgreSQL | Optimistic concurrency for live voting |
| **Real-Time** | SignalR + Redis Backplane | Zero-latency vote and poll broadcasting |
| **Messaging** | RabbitMQ + Worker Service | Decoupled async AI processing |
| **AI** | Google Agent Development Kit | Feedback grouping, summarization, topic tagging |
| **Frontend** | Angular 21 | Signal Forms, Zoneless, `@defer`, standalone |
| **UI Library** | `ng-oat` | Your own OSS component library — dogfooded here |
| **Logging** | Serilog + Seq | Structured logs, readable in dev |
| **Observability** | OpenTelemetry + Aspire Dashboard | Traces, metrics, logs — out of the box |
| **Testing** | xUnit + TestContainers + Playwright | Unit, integration, E2E |
| **CI/CD** | GitHub Actions + `azd` CLI | Build, test, deploy on every push |
| **Deployment** | Azure Container Apps | Aspire-native cloud target |

---

## 📦 NuGet Packages — Complete Install Reference

Use this during the stream as your **copy-paste cheat sheet**.

### `TalkPulse.AppHost` (Aspire Orchestrator)

```bash
dotnet add package Aspire.Hosting.AppHost
dotnet add package Aspire.Hosting.PostgreSQL
dotnet add package Aspire.Hosting.Redis
dotnet add package Aspire.Hosting.RabbitMQ
```

---

### `TalkPulse.ServiceDefaults` (Shared Telemetry)

```bash
dotnet add package Microsoft.Extensions.ServiceDiscovery
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol
dotnet add package OpenTelemetry.Extensions.Hosting
dotnet add package OpenTelemetry.Instrumentation.AspNetCore
dotnet add package OpenTelemetry.Instrumentation.Http
```

---

### `TalkPulse.Api` (Main API — All Slices)

```bash
# EF Core + PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Aspire.Npgsql.EntityFrameworkCore.PostgreSQL

# CQRS
dotnet add package MediatR

# Validation
dotnet add package FluentValidation

# SignalR (built into ASP.NET Core 10 — no extra package needed)
# Redis backplane for SignalR
dotnet add package Microsoft.AspNetCore.SignalR.StackExchangeRedis

# Auth / JWT
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

# QR Code generation
dotnet add package QRCoder

# Logging
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.Seq

# OpenAPI / Swagger
dotnet add package Microsoft.AspNetCore.OpenApi
dotnet add package Scalar.AspNetCore

# Redis (Aspire integration)
dotnet add package Aspire.StackExchange.Redis

# RabbitMQ publisher
dotnet add package RabbitMQ.Client
```

---

### `TalkPulse.WorkerService` (Background AI Processor)

```bash
dotnet add package RabbitMQ.Client
dotnet add package Microsoft.Extensions.Http
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

---

### `TalkPulse.Api.Tests`

```bash
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Testcontainers.PostgreSQL
dotnet add package FluentAssertions
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
```

---

### Angular 21 Frontend — npm packages

```bash
# Inside /frontend/talkpulse-client
ng new talkpulse-client --standalone --routing --style=scss

# SignalR client
npm install @microsoft/signalr

# QR code display
npm install angularx-qrcode

# Charts (for live poll results)
npm install chart.js ng2-charts

# Icons (optional)
npm install @ng-icons/core @ng-icons/heroicons
```

---

## 🗂️ Folder Structure

```text
TalkPulse/
├── .github/
│   └── workflows/
│       ├── ci.yml                          # PR: build + lint + test
│       └── deploy.yml                      # main: deploy to Azure
│
├── backend/
│   ├── TalkPulse.AppHost/                  # .NET Aspire orchestrator
│   │   └── Program.cs                      # Provisions all containers
│   │
│   ├── TalkPulse.ServiceDefaults/          # Shared: OTel, health checks, logging
│   │
│   ├── TalkPulse.Api/                      # 🔑 Single API project (all VSA slices)
│   │   │
│   │   ├── Common/                         # Shared infrastructure (NOT business logic)
│   │   │   ├── Domain/
│   │   │   │   ├── Session.cs              # EF Core entity
│   │   │   │   ├── Poll.cs                 # RowVersion for optimistic concurrency
│   │   │   │   ├── PollOption.cs
│   │   │   │   ├── Vote.cs
│   │   │   │   ├── Feedback.cs
│   │   │   │   └── Speaker.cs
│   │   │   ├── Persistence/
│   │   │   │   ├── AppDbContext.cs
│   │   │   │   └── Migrations/
│   │   │   ├── Behaviors/
│   │   │   │   ├── ValidationBehavior.cs   # MediatR pipeline
│   │   │   │   └── LoggingBehavior.cs
│   │   │   ├── Errors/
│   │   │   │   └── Error.cs               # Result<T, Error> pattern
│   │   │   └── Extensions/
│   │   │       └── EndpointExtensions.cs  # IEndpoint auto-registration
│   │   │
│   │   ├── Features/                       # 🍕 All vertical slices
│   │   │   │
│   │   │   ├── Sessions/
│   │   │   │   ├── CreateSession/
│   │   │   │   ├── GetSession/
│   │   │   │   ├── GoLive/                 # Broadcasts via SignalR
│   │   │   │   └── EndSession/             # Publishes to RabbitMQ + SignalR
│   │   │   │
│   │   │   ├── Polls/
│   │   │   │   ├── CreatePoll/
│   │   │   │   ├── ActivatePoll/           # Broadcasts PollActivated to audience
│   │   │   │   └── GetPollResults/
│   │   │   │
│   │   │   ├── Voting/
│   │   │   │   └── SubmitVote/             # Concurrency-safe + SignalR broadcast
│   │   │   │       ├── SubmitVoteCommand.cs
│   │   │   │       ├── SubmitVoteHandler.cs
│   │   │   │       ├── SubmitVoteValidator.cs
│   │   │   │       ├── SubmitVoteEndpoint.cs
│   │   │   │       └── SubmitVoteResponse.cs
│   │   │   │
│   │   │   ├── Feedback/
│   │   │   │   ├── SubmitFeedback/         # Publishes FeedbackSubmitted to RabbitMQ
│   │   │   │   └── GetFeedback/
│   │   │   │
│   │   │   ├── Auth/
│   │   │   │   ├── RegisterSpeaker/
│   │   │   │   ├── LoginSpeaker/
│   │   │   │   └── JoinAsAudience/         # Anonymous JWT on QR scan
│   │   │   │
│   │   │   └── AiInsights/
│   │   │       └── GetInsights/            # Streams tokens via SignalR
│   │   │
│   │   ├── Hubs/
│   │   │   └── SessionHub.cs              # SignalR hub
│   │   │
│   │   └── Program.cs
│   │
│   ├── TalkPulse.WorkerService/            # Background AI processor
│   │   ├── Features/
│   │   │   └── ProcessFeedback/
│   │   │       ├── ProcessFeedbackConsumer.cs
│   │   │       ├── FeedbackAiAgent.cs      # Google ADK HTTP call
│   │   │       └── SaveInsightsHandler.cs
│   │   └── Program.cs
│   │
│   ├── TalkPulse.Api.Tests/
│   │   └── Features/                       # Tests mirror feature slices
│   │       ├── Voting/SubmitVoteTests.cs
│   │       ├── Sessions/CreateSessionTests.cs
│   │       └── Feedback/SubmitFeedbackTests.cs
│   │
│   └── TalkPulse.Application.Tests/
│
├── frontend/
│   └── talkpulse-client/                   # Angular 21 SPA
│       └── src/app/
│           ├── core/
│           │   ├── services/
│           │   │   ├── auth.service.ts
│           │   │   └── signalr.service.ts  # SignalR connection manager
│           │   ├── guards/
│           │   │   └── speaker.guard.ts
│           │   └── interceptors/
│           │       └── auth.interceptor.ts
│           ├── shared/
│           │   └── components/             # ng-oat components live here
│           └── features/
│               ├── speaker/
│               │   ├── create-session/
│               │   ├── manage-polls/
│               │   └── dashboard/          # Live chart + AI insights
│               ├── audience/
│               │   ├── join-session/       # QR scan → anonymous JWT
│               │   ├── live-poll/          # Touch-friendly voting UI
│               │   └── feedback-form/      # Auto-appears on session end
│               └── ai-insights/
│                   └── insights-panel/     # Summary + topic tags
│
├── docker-compose.override.yml             # Local dev overrides only
└── README.md
```

---

## 🚀 Getting Started (Local Dev)

### Prerequisites

| Tool | Version | Install |
|---|---|---|
| .NET SDK | 10.0+ | [dot.net](https://dot.net) |
| .NET Aspire workload | 13.x | `dotnet workload install aspire` |
| Node.js | 20+ | [nodejs.org](https://nodejs.org) |
| Angular CLI | 21 | `npm install -g @angular/cli` |
| Docker Desktop | Latest | [docker.com](https://docker.com) |

> ⚠️ **Docker Desktop must be running.** Aspire uses it to spin up PostgreSQL, Redis, and RabbitMQ automatically.

---

### 1️⃣ Clone the repo

```bash
git clone https://github.com/yshashi/TalkPulse.git
cd TalkPulse
```

### 2️⃣ Start the backend

```bash
cd backend
dotnet run --project TalkPulse.AppHost
```

Aspire will automatically:
- 🐘 Start **PostgreSQL** container
- ⚡ Start **Redis** container (SignalR backplane)
- 🐰 Start **RabbitMQ** container
- 🚀 Start **TalkPulse.Api** → `https://localhost:5001`
- 🤖 Start **TalkPulse.WorkerService**
- 📊 Open **Aspire Dashboard** → `https://localhost:15888`

### 3️⃣ Apply migrations

```bash
cd backend/TalkPulse.Api
dotnet ef database update
```

### 4️⃣ Start the frontend

```bash
cd frontend/talkpulse-client
npm install
ng serve
# → http://localhost:4200
```

### 5️⃣ Explore

| URL | What it is |
|---|---|
| `https://localhost:5001/scalar` | API docs (Scalar UI) |
| `https://localhost:15888` | Aspire dashboard — traces, logs, metrics |
| `http://localhost:4200` | Angular frontend |

---

## 🧩 VSA — The 3 Rules We Follow

```
✅ Each slice owns everything — endpoint → handler → DB call.
✅ Duplication across slices is acceptable — wrong abstraction is worse.
✅ Share infrastructure, not business logic.
      AppDbContext     ✅     IFeedbackRepository  ❌
      MediatR Behaviors ✅    Shared feature services ❌
```

Every feature looks like this:

```
Features/Voting/SubmitVote/
    SubmitVoteCommand.cs       ← the request
    SubmitVoteHandler.cs       ← ALL logic: DB + SignalR broadcast
    SubmitVoteValidator.cs     ← FluentValidation rules
    SubmitVoteEndpoint.cs      ← Minimal API route registration
    SubmitVoteResponse.cs      ← response DTO
```

---

## 🗺️ 12-Week Build Roadmap

Checked off live as streams happen:

- [ ] **Week 1** — Aspire scaffolding, domain entities, `IEndpoint` auto-registration
- [ ] **Week 2** — `CreateSession`, `GetSession` slices + Angular session feature
- [ ] **Week 3** — Auth slices: `RegisterSpeaker`, `LoginSpeaker`, `JoinAsAudience` (anonymous JWT)
- [ ] **Week 4** — SignalR `SessionHub` + `GoLive` / `ActivatePoll` slices
- [ ] **Week 5** — `SubmitVote` with optimistic concurrency + live Angular chart
- [ ] **Week 6** — QR code, `EndSession`, auto-trigger feedback form on audience devices
- [ ] **Week 7** — `SubmitFeedback` slice + RabbitMQ publishing + WorkerService setup
- [ ] **Week 8** — Google ADK AI agent + `ProcessFeedback` slice
- [ ] **Week 9** — Streaming AI insights via `IAsyncEnumerable` + SignalR token streaming
- [ ] **Week 10** — Testing: TestContainers, WebApplicationFactory, Playwright E2E
- [ ] **Week 11** — Observability: OpenTelemetry traces, Aspire dashboard, production hardening
- [ ] **Week 12** — CI/CD, Azure Container Apps deploy + **live dogfood on stream** 🎉

---

## 🤝 Contributing

1. **Fork** the repo
2. **Create a branch**: `git checkout -b feat/your-slice-name`
3. **Follow VSA** — one slice per PR under `Features/`
4. **Add a test** in `TalkPulse.Api.Tests/Features/`
5. **Open a PR** with a clear description

### Branch naming

| Type | Format |
|---|---|
| Feature | `feat/slice-name` e.g. `feat/activate-poll` |
| Bug | `fix/description` e.g. `fix/duplicate-vote` |
| Stream week | `stream/week-N` e.g. `stream/week-4` |
| Docs | `docs/topic` |

### Commit style (Conventional Commits)

```bash
feat(voting): add optimistic concurrency retry to SubmitVote
fix(signalr): handle audience disconnect on session end
docs(readme): update architecture diagram
test(feedback): add TestContainers test for SubmitFeedback slice
```

---

## 📄 License

MIT — see [LICENSE](LICENSE)

---

<p align="center">
  Built live with ❤️ on YouTube · Every Wednesday & Friday
  <br/><br/>
  <a href="#">▶️ Watch Live</a> &nbsp;·&nbsp;
  <a href="../../issues">🐛 Report a Bug</a> &nbsp;·&nbsp;
  <a href="../../discussions">💬 Join the Discussion</a>
</p>
