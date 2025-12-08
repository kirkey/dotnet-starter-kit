using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for KYC documents.
/// Creates sample identity documents for members - demo database.
/// </summary>
internal static class KycDocumentSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 200;
        var existingCount = await context.KycDocuments.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Take(150).ToListAsync(cancellationToken).ConfigureAwait(false);
        if (!members.Any()) return;

        // Get staff IDs for verification (staff must be seeded before KYC documents)
        var staffIds = await context.Staff.Select(s => s.Id).ToListAsync(cancellationToken).ConfigureAwait(false);

        var random = new Random(42);
        int docCount = 0;

        // Document types common in Philippines
        var docTypes = new (string Type, string Authority, string Prefix, bool HasExpiry)[]
        {
            (KycDocument.TypeNationalId, "Philippine Statistics Authority", "PSA", true),
            (KycDocument.TypeDriversLicense, "Land Transportation Office", "LTO", true),
            (KycDocument.TypeVoterId, "Commission on Elections", "COMELEC", false),
            (KycDocument.TypePassport, "Department of Foreign Affairs", "DFA", true),
            (KycDocument.TypeTaxId, "Bureau of Internal Revenue", "BIR", false),
        };

        foreach (var member in members)
        {
            // Each member gets 2-4 KYC documents
            var numDocs = random.Next(2, 5);
            var selectedTypes = docTypes.OrderBy(_ => random.Next()).Take(numDocs).ToList();
            bool isPrimarySet = false;

            foreach (var docType in selectedTypes)
            {
                var docNumber = $"{docType.Prefix}-{random.Next(100000000, 999999999):D9}";
                var issueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-random.Next(1, 5)));
                var expiryDate = docType.HasExpiry ? issueDate.AddYears(5 + random.Next(0, 5)) : (DateOnly?)null;

                var kycDoc = KycDocument.Create(
                    memberId: member.Id,
                    documentType: docType.Type,
                    fileName: $"{member.MemberNumber}_{docType.Type.ToLower()}.pdf",
                    filePath: $"/documents/kyc/{member.MemberNumber}/{docType.Type.ToLower()}.pdf",
                    mimeType: "application/pdf",
                    fileSize: random.Next(100000, 2000000));

                kycDoc.WithDocumentDetails(
                    documentNumber: docNumber,
                    issueDate: issueDate,
                    expiryDate: expiryDate,
                    issuingAuthority: docType.Authority);

                // Set first document as primary
                if (!isPrimarySet)
                {
                    kycDoc.SetAsPrimary();
                    isPrimarySet = true;
                }

                // Most documents are verified for active members
                if (member.IsActive && random.NextDouble() > 0.2 && staffIds.Any())
                {
                    var verifierId = staffIds[random.Next(staffIds.Count)];
                    kycDoc.Verify(verifierId, "Verified during onboarding");
                }

                await context.KycDocuments.AddAsync(kycDoc, cancellationToken).ConfigureAwait(false);
                docCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} KYC documents for members", tenant, docCount);
    }
}
