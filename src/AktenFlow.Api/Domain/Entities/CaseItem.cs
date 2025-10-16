using System.ComponentModel.DataAnnotations;
using AktenFlow.Api.Domain;

namespace AktenFlow.Api.Domain.Entities
{
    public class CaseItem
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CaseFileId { get; set; }
        public string Description { get; set; } = string.Empty;
        public CaseStatus TargetStatus { get; set; } = CaseStatus.Review;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAtUtc { get; set; }
    }
}
