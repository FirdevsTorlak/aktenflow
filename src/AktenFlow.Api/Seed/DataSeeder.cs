using AktenFlow.Api.Domain;
using AktenFlow.Api.Domain.Entities;
using AktenFlow.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AktenFlow.Api.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (await db.CaseFiles.AnyAsync()) return;

            var cf1 = new CaseFile { Title = "Road Maintenance Tenders 2025", ReferenceCode = "RM-2025-01", Confidentiality = ConfidentialityLevel.Internal };
            var cf2 = new CaseFile { Title = "Citizen Portal – Change Requests", ReferenceCode = "CP-CR-77", Confidentiality = ConfidentialityLevel.Internal };
            db.CaseFiles.AddRange(cf1, cf2);
            await db.SaveChangesAsync();
        }
    }
}
