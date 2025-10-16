using AktenFlow.Api.Domain.Entities;
using AktenFlow.Api.Dtos;

namespace AktenFlow.Api.Mappings
{
    public static class MappingExtensions
    {
        public static CaseFileDto ToDto(this CaseFile cf)
            => new(cf.Id, cf.Title, cf.ReferenceCode, cf.Confidentiality, cf.AssignedTo, cf.Status.ToString(), cf.CreatedAtUtc);

        public static DocumentDto ToDto(this Document d)
            => new(d.Id, d.CaseFileId, d.Title, d.FileName, d.Version, d.IsLatest, d.MimeType, d.CreatedAtUtc);
    }
}
