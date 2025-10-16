# Runbook – AktenFlow (Dev)

## Start
- `dotnet run` from `src/AktenFlow.Api`
- Health: GET `/health`

## Data
- SQLite file at `./data/aktenflow.db`
- Files at `./data/storage/{caseFileId}/...`

## Backup/Restore (Dev)
- Backup: copy `./data` directory
- Restore: stop API → replace `./data` → start API
