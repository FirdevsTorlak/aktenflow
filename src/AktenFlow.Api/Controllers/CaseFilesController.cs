using AktenFlow.Api.Dtos;
using AktenFlow.Api.Mappings;
using AktenFlow.Api.Domain.Entities;
using AktenFlow.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AktenFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaseFilesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CaseFilesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseFileDto>>> Get()
            => Ok((await _db.CaseFiles.AsNoTracking().ToListAsync()).Select(cf => cf.ToDto()));

        public record CreateCaseFileRequest(string Title, string ReferenceCode, int Confidentiality = 1, string? AssignedTo = null);

        [HttpPost]
        public async Task<ActionResult<CaseFileDto>> Create(CreateCaseFileRequest req)
        {
            var cf = new CaseFile
            {
                Title = req.Title,
                ReferenceCode = req.ReferenceCode,
                Confidentiality = (Domain.ConfidentialityLevel)req.Confidentiality,
                AssignedTo = req.AssignedTo
            };
            _db.CaseFiles.Add(cf);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = cf.Id }, cf.ToDto());
        }
    }
}
