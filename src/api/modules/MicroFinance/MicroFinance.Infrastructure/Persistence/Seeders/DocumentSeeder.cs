using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for documents.
/// Creates sample documents for members and loans.
/// </summary>
internal static class DocumentSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.Documents.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var members = await context.Members.Take(30).ToListAsync(cancellationToken).ConfigureAwait(false);
        var loans = await context.Loans.Take(20).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (!members.Any()) return;

        // Get staff IDs for verification (staff must be seeded before documents)
        var staffIds = await context.Staff.Select(s => s.Id).ToListAsync(cancellationToken).ConfigureAwait(false);

        var random = new Random(42);
        int docCount = 0;

        // Member documents
        foreach (var member in members)
        {
            // Profile photo
            var photo = Document.Create(
                name: $"{member.FirstName} {member.LastName} - Photo",
                documentType: Document.TypePhoto,
                entityType: Document.EntityMember,
                entityId: member.Id,
                filePath: $"/documents/members/{member.MemberNumber}/photo.jpg",
                fileSizeBytes: random.Next(50000, 500000),
                mimeType: "image/jpeg",
                category: "Profile",
                originalFileName: "profile_photo.jpg");

            if (staffIds.Any())
                photo.Verify(staffIds[random.Next(staffIds.Count)]);
            await context.Documents.AddAsync(photo, cancellationToken).ConfigureAwait(false);
            docCount++;

            // ID document
            var idDoc = Document.Create(
                name: $"{member.FirstName} {member.LastName} - Valid ID",
                documentType: Document.TypeIdentification,
                entityType: Document.EntityMember,
                entityId: member.Id,
                filePath: $"/documents/members/{member.MemberNumber}/valid_id.pdf",
                fileSizeBytes: random.Next(100000, 2000000),
                mimeType: "application/pdf",
                category: "KYC",
                originalFileName: "valid_id_scan.pdf");

            idDoc.SetDocumentDetails(
                issueDate: DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-random.Next(1, 5))),
                expiryDate: DateOnly.FromDateTime(DateTime.UtcNow.AddYears(random.Next(1, 5))),
                issuingAuthority: "Philippine Statistics Authority");

            if (random.NextDouble() > 0.2 && staffIds.Any())
                idDoc.Verify(staffIds[random.Next(staffIds.Count)]);

            await context.Documents.AddAsync(idDoc, cancellationToken).ConfigureAwait(false);
            docCount++;

            // Proof of address (50% of members)
            if (random.NextDouble() > 0.5)
            {
                var addressDoc = Document.Create(
                    name: $"{member.FirstName} {member.LastName} - Proof of Address",
                    documentType: Document.TypeProofOfAddress,
                    entityType: Document.EntityMember,
                    entityId: member.Id,
                    filePath: $"/documents/members/{member.MemberNumber}/utility_bill.pdf",
                    fileSizeBytes: random.Next(50000, 500000),
                    mimeType: "application/pdf",
                    category: "KYC",
                    originalFileName: "meralco_bill.pdf");

                await context.Documents.AddAsync(addressDoc, cancellationToken).ConfigureAwait(false);
                docCount++;
            }
        }

        // Loan documents
        foreach (var loan in loans)
        {
            // Loan contract
            var contract = Document.Create(
                name: $"Loan Contract - {loan.LoanNumber}",
                documentType: Document.TypeContract,
                entityType: Document.EntityLoan,
                entityId: loan.Id,
                filePath: $"/documents/loans/{loan.LoanNumber}/contract.pdf",
                fileSizeBytes: random.Next(200000, 1000000),
                mimeType: "application/pdf",
                category: "Legal",
                originalFileName: "loan_contract.pdf");

            if (staffIds.Any())
                contract.Verify(staffIds[random.Next(staffIds.Count)]);
            await context.Documents.AddAsync(contract, cancellationToken).ConfigureAwait(false);
            docCount++;

            // Income proof
            var incomeProof = Document.Create(
                name: $"Income Proof - {loan.LoanNumber}",
                documentType: Document.TypeIncomeProof,
                entityType: Document.EntityLoan,
                entityId: loan.Id,
                filePath: $"/documents/loans/{loan.LoanNumber}/income_proof.pdf",
                fileSizeBytes: random.Next(100000, 500000),
                mimeType: "application/pdf",
                category: "Financial",
                originalFileName: "payslip_or_itr.pdf");

            await context.Documents.AddAsync(incomeProof, cancellationToken).ConfigureAwait(false);
            docCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} documents for members and loans", tenant, docCount);
    }
}
