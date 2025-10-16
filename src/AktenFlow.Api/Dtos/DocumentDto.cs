namespace AktenFlow.Api.Dtos
{
    public record DocumentDto(Guid Id, Guid CaseFileId, string Title, string FileName, int Version, bool IsLatest, string MimeType, DateTime CreatedAtUtc);
}
