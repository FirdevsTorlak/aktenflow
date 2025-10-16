# Business Requirements – AktenFlow (MVP)

## Purpose
Provide a lightweight eFile/DMS to manage case files, documents, and a simple workflow (Incoming → Review → Approval → Archived).

## Users & Roles
- Clerk: create case files, upload documents, move to Review
- Lead: approve/reject; finalize to Archived
- Admin: manage roles/policies, configure storage

## Core Entities
- CaseFile: title, referenceCode, confidentiality, assignedTo, status
- Document: title, filename, mimeType, version, isLatest, audit (createdAt, createdBy)
- CaseItem: small workflow tasks

## Success Criteria (Acceptance level)
- Upload a document and create a new version → previous version marked not latest
- List documents for a case file
- Move case file status through the simple workflow
