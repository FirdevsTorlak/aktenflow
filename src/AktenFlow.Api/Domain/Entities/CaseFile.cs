using System.ComponentModel.DataAnnotations;
using AktenFlow.Api.Domain;

namespace AktenFlow.Api.Domain.Entities
{
    public class CaseFile
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(200)] public string Title { get; set; } = string.Empty;
        [MaxLength(64)] public string ReferenceCode { get; set; } = string.Empty;
        public ConfidentialityLevel Confidentiality { get; set; } = ConfidentialityLevel.Internal;
        public CaseStatus Status { get; set; } = CaseStatus.Incoming;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public string? AssignedTo { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
