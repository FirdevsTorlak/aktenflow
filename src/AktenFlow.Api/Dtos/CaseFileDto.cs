using AktenFlow.Api.Domain;

namespace AktenFlow.Api.Dtos
{
    public record CaseFileDto(Guid Id, string Title, string ReferenceCode, ConfidentialityLevel Confidentiality, string? AssignedTo, string Status, DateTime CreatedAtUtc);
}
