AktenFlow – Mini eFile/DMS (Coordination‑Focused)

**A coordination‑focused mini eFile/DMS prototype demonstrating the end‑to‑end flow from business requirements and technical specification to workflow/versioning, acceptance testing, runbook, and CI. Built with ASP.NET Core, EF Core, and SQLite.**

---

1) Overview

**AktenFlow** is a compact document and case file management API intended to showcase **IT coordination** skills for public‑sector software: requirements capture, stakeholder‑friendly specs, operational runbooks, vendor/SLA considerations, and build/test automation.
The prototype is intentionally minimal but complete enough to demonstrate the core lifecycle and hand‑offs typical in government DMS/eFile projects.

**Key capabilities**
- **Entities:** `CaseFile`, `CaseItem`, `Document` with basic **versioning** and audit fields.
- **Workflow:** `Incoming → Review → Approved → Archived` (kept simple and explicit).
- **Authorization hooks:** Roles (`Clerk`, `Lead`, `Admin`) planned; policy‑based auth can be enabled later.
- **Storage:** SQLite file DB (`./data/aktenflow.db`) + on‑disk file storage (`./data/storage/{caseFileId}`).
- **CI:** GitHub Actions workflow for build and test.
- **Docs:** Business Requirements, Technical Specification, Acceptance Test Plan, Runbook, Authorization/Privacy notes, and realistic placeholders for Budget/Offer and Vendor/SLA matrix.

**Non‑goals (MVP)**
- No full‑text OCR, no complex records management (retention schedules, Aktenplan trees) yet.
- No enterprise SSO/IDP integration; endpoints are open for demo (add auth when needed).
- No production hardening (HA, backups schedule, observability stack) in the MVP.

---

2) Architecture

- **API:** ASP.NET Core 8 Web API
- **Persistence:** EF Core + SQLite
- **File Storage:** Local file system (under `./data/storage`)
- **Container:** Optional Dockerfile + Docker Compose
- **CI/CD:** `.github/workflows/ci.yml` (restore → build → test)

@startuml
skinparam componentStyle rectangle
package "AktenFlow" {
  [API \n ASP.NET Core] --> [Persistence \n EF Core + SQLite]
  [API \n ASP.NET Core] --> [File Storage \n ./data/storage]
}
actor Clerk
actor Lead
actor Admin
Clerk --> [API \n ASP.NET Core]
Lead --> [API \n ASP.NET Core]
Admin --> [API \n ASP.NET Core]
@enduml

---

3) Features

- **Case Files:** Create and list case files with reference codes, confidentiality level, status, and assignee.
- **Documents:** Upload PDFs as new versions; previous version is flagged as non‑latest.
- **Simple Workflow:** Move a case file across `Incoming → Review → Approved → Archived` (expandable).
- **Auditability:** Timestamps and creator fields on documents; deterministic storage paths.
- **Health Check:** `GET /health` for a lightweight liveness probe.
- **API Contract:** Swagger UI available in Development for easy manual testing.

---

4) API Quickstart

**Run locally (Development):**
dotnet restore && dotnet build && dotnet test
cd src/AktenFlow.Api && dotnet run
Swagger (Development): http://localhost:5086/swagger

**Create a Case File (PowerShell):**
$cf = Invoke-RestMethod -Method Post `
  -Uri http://localhost:5086/api/casefiles `
  -ContentType application/json `
  -Body (@{ title="Demo Case"; referenceCode="DEMO-1" } | ConvertTo-Json)
$cf

**Upload a PDF document:**
curl.exe -F "title=First PDF" ^
  -F "file=@C:	emp\demo.pdf;type=application/pdf" ^
  http://localhost:5086/api/documents/$($cf.id)/upload

**List documents for a case file:**
Invoke-RestMethod -Method Get `
  -Uri "http://localhost:5086/api/documents?caseFileId=$($cf.id)"

---

5) Project Structure
aktenflow/
├─ README.md
├─ LICENSE
├─ .github/workflows/ci.yml
├─ docs/
│  ├─ 01_business-requirements.md
│  ├─ 02_technical-specification.md
│  ├─ 03_architecture.puml
│  ├─ 04_acceptance-test-plan.md
│  ├─ 05_runbook.md
│  ├─ 06_authorization-privacy.md
│  ├─ 07_budget_offer.xlsx          # placeholder
│  └─ 08_vendor_sla_matrix.xlsx     # placeholder
├─ infra/
│  └─ docker-compose.yml
└─ src/
   ├─ AktenFlow.Api/                # ASP.NET Core Web API
   └─ AktenFlow.Tests/              # xUnit test project

**Notable files**
- `Program.cs` – DB init, health endpoint, Swagger in Development.
- `Persistence/AppDbContext.cs` – EF Core DbContext & basic model config.
- `Services/DocumentService.cs` – Versioned document save & disk storage.
- `Controllers/*` – REST endpoints for case files and documents.

---

6) Configuration

- **Environment:** Development by default (via `launchSettings.json`).
- **Database:** SQLite file path is calculated under `./data/aktenflow.db` at runtime.
- **Ports:** Configured in `launchSettings.json` or via `--urls`. Example: `--urls "http://localhost:5086"`.
- **Docker:** See `infra/docker-compose.yml` (maps host `5000` to container `8080` by default; change as needed).

**Data directory layout**
/data
├─ aktenflow.db
└─ storage/
   └─ <CASEFILE_ID>/
      └─ {documentId}_v{version}_{originalFileName}

---

7) Security & Privacy (MVP Notes)

- **AuthN/AuthZ:** Planned policy‑based authorization with roles (`Clerk`, `Lead`, `Admin`). Endpoints are open for demo.
- **Audit fields:** Tracking `CreatedAtUtc` and `CreatedBy` for documents.
- **PII/Content:** Only store metadata required for demo; avoid unnecessary personal data.
- **Retention:** Out of scope for MVP; recommend adding delete/retention endpoints per policy.
- **Transport security:** HTTPS recommended in production; use reverse proxy or Kestrel TLS config.

---

8) Operations & Runbook (Summary)

- **Start:** `dotnet run` from `src/AktenFlow.Api` (Development).
- **Health:** `GET /health` (liveness).
- **Backups (Dev):** Stop API → copy the `./data` directory → start API.
- **Restore (Dev):** Stop API → replace `./data` with a backup copy → start API.
- **Logs:** Console logs via ASP.NET Core; integrate Serilog/structured logging for prod.

See `docs/05_runbook.md` for more details.

---

9) Testing & Quality

- **Unit tests:** `src/AktenFlow.Tests` (xUnit + FluentAssertions).
- **Acceptance tests (manual):** `docs/04_acceptance-test-plan.md` includes scenario‑driven checks.
- **CI:** GitHub Actions `ci.yml` executes restore → build → test on push/PR to `main`.

**Next:** Add ASP.NET integration tests with `WebApplicationFactory` once auth policies are introduced.

---

10) Roadmap (Suggested)

- Add policy‑based authorization & role claims.
- Introduce full‑text search (SQLite FTS or external OCR with SLA).
- Add retention & deletion policy endpoints.
- Switch to PostgreSQL and a multi‑container setup for realism.
- Observability: request logging, metrics, tracing (OpenTelemetry), dashboards.
- Blue/green or canary deployment examples; rollout & rollback playbooks.

---

11) License

This project is licensed under the **MIT License** – see the [LICENSE](LICENSE) file for details.