using AktenFlow.Api.Domain.Entities;
using AktenFlow.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AktenFlow.Api.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly AppDbContext _db;
        private readonly string _storageRoot;

        public DocumentService(AppDbContext db)
        {
            _db = db;
            _storageRoot = Path.Combine(AppContext.BaseDirectory, "data", "storage");
            Directory.CreateDirectory(_storageRoot);
        }

        public async Task<IReadOnlyList<Document>> ListAsync(Guid caseFileId)
        {
            return await _db.Documents.Where(d => d.CaseFileId == caseFileId)
                                      .OrderByDescending(d => d.CreatedAtUtc)
                                      .ToListAsync();
        }

        public async Task<Document> SaveAsync(Guid caseFileId, string title, string fileName, string mimeType, Stream content, string? createdBy = null, Guid? supersedes = null)
        {
            // bump version if superseding existing
            int version = 1;
            if (supersedes.HasValue)
            {
                var prev = await _db.Documents.FindAsync(supersedes.Value) ?? throw new InvalidOperationException("Superseded document not found");
                prev.IsLatest = false;
                version = prev.Version + 1;
            }
            else
            {
                var last = await _db.Documents.Where(d => d.CaseFileId == caseFileId && d.IsLatest)
                                              .OrderByDescending(d => d.Version)
                                              .FirstOrDefaultAsync();
                if (last != null) version = last.Version + 1;
            }

            var id = Guid.NewGuid();
            var storageName = $"{id}_v{version}_{fileName}";
            var caseDir = Path.Combine(_storageRoot, caseFileId.ToString());
            Directory.CreateDirectory(caseDir);
            var path = Path.Combine(caseDir, storageName);

            using (var fs = File.Create(path))
            {
                await content.CopyToAsync(fs);
            }

            var doc = new Document
            {
                Id = id,
                CaseFileId = caseFileId,
                Title = title,
                FileName = fileName,
                MimeType = mimeType,
                StoragePath = path,
                Version = version,
                IsLatest = true,
                SupersedesDocumentId = supersedes,
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            _db.Documents.Add(doc);
            await _db.SaveChangesAsync();
            return doc;
        }
    }
}
