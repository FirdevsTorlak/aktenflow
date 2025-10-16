using AktenFlow.Api.Domain.Entities;

namespace AktenFlow.Api.Services
{
    public interface IDocumentService
    {
        Task<Document> SaveAsync(Guid caseFileId, string title, string fileName, string mimeType, Stream content, string? createdBy = null, Guid? supersedes = null);
        Task<IReadOnlyList<Document>> ListAsync(Guid caseFileId);
    }
}
