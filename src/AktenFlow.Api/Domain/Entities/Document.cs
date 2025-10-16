using System.ComponentModel.DataAnnotations;

namespace AktenFlow.Api.Domain.Entities
{
    public class Document
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CaseFileId { get; set; }
        public CaseFile? CaseFile { get; set; }

        [MaxLength(200)] public string Title { get; set; } = string.Empty;
        [MaxLength(260)] public string FileName { get; set; } = string.Empty;
        [MaxLength(100)] public string MimeType { get; set; } = "application/pdf";

        public int Version { get; set; } = 1;
        public bool IsLatest { get; set; } = true;
        public Guid? SupersedesDocumentId { get; set; }

        // stored on disk under /data/storage
        public string StoragePath { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
    }
}
