# Acceptance Test Plan – AktenFlow

## AT-01 Upload first document
- Pre: CaseFile exists
- Action: POST /api/documents/{caseId}/upload with a PDF
- Verify: 201 Created, response `version=1`, `isLatest=true`

## AT-02 Upload new version
- Pre: AT-01 done
- Action: POST upload with `supersedes={docId}`
- Verify: new doc `version=2`, previous doc `isLatest=false`

## AT-03 List documents by CaseFile
- Action: GET /api/documents?caseFileId={caseId}
- Verify: returns array with latest first
