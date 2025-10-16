# Technical Specification – AktenFlow (MVP)

## Architecture
- ASP.NET Core 8 Web API
- EF Core + SQLite (file DB under ./data)
- File storage on disk under ./data/storage/{caseFileId}

## API (selected)
- GET /api/casefiles
- POST /api/casefiles
- GET /api/documents?caseFileId={id}
- POST /api/documents/{caseFileId}/upload (multipart)

## Versioning Strategy
- New upload → increment integer version; previous document `IsLatest=false`.

## Security (MVP)
- Keep endpoints open for demo; add policy‑based authorization later (roles Clerk/Lead/Admin).

## Observability (MVP)
- Basic health endpoint `/health`; extend later with structured logging.
