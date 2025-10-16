using AktenFlow.Api.Dtos;
using AktenFlow.Api.Mappings;
using AktenFlow.Api.Persistence;
using AktenFlow.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AktenFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IDocumentService _svc;
        public DocumentsController(AppDbContext db, IDocumentService svc)
        {
            _db = db; _svc = svc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> List([FromQuery] Guid caseFileId)
        {
            if (!await _db.CaseFiles.AnyAsync(x => x.Id == caseFileId)) return NotFound("CaseFile not found");
            var docs = await _svc.ListAsync(caseFileId);
            return Ok(docs.Select(d => d.ToDto()));
        }

        [HttpPost("{caseFileId:guid}/upload")]
        [RequestSizeLimit(100_000_000)]
        public async Task<ActionResult<DocumentDto>> Upload([FromRoute] Guid caseFileId, [FromForm] IFormFile file, [FromForm] string title, [FromForm] Guid? supersedes)
        {
            if (!await _db.CaseFiles.AnyAsync(x => x.Id == caseFileId)) return NotFound("CaseFile not found");
            if (file == null || file.Length == 0) return BadRequest("file is required");
            await using var stream = file.OpenReadStream();
            var doc = await _svc.SaveAsync(caseFileId, title, file.FileName, file.ContentType ?? "application/octet-stream", stream, createdBy: "demo", supersedes: supersedes);
            return Created($"/api/documents/{doc.Id}", doc.ToDto());
        }
    }
}
