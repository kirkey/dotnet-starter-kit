using FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

/// <summary>
/// Database initializer for the MicroFinance module.
/// Handles migrations and orchestrates seeding of initial data.
/// Individual entity seeding is delegated to specialized seeder classes.
/// </summary>
internal sealed class MicroFinanceDbInitializer(
    ILogger<MicroFinanceDbInitializer> logger,
    MicroFinanceDbContext context) : IDbInitializer
{
    /// <inheritdoc/>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for microfinance module", context.TenantInfo!.Identifier);
        }
    }

    /// <inheritdoc/>
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var tenant = context.TenantInfo?.Identifier ?? "default";

        // ============================================================
        // PHASE 1: Infrastructure & Configuration
        // ============================================================
        
        // 1.1) Core infrastructure (branches, staff, collateral types, templates)
        await BranchSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await StaffSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await CollateralTypeSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await CommunicationTemplateSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 1.2) Cash management infrastructure
        await CashVaultSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        
        // 1.3) Payment and channel infrastructure
        await PaymentGatewaySeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await AgentBankingSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 1.4) Configuration and settings
        await MfiConfigurationSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // ============================================================
        // PHASE 2: Products & Risk Framework
        // ============================================================
        
        // 2.1) Product definitions (loan, savings, share, insurance, fee, investment)
        await LoanProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await SavingsProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await ShareProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await InsuranceProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await FeeDefinitionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await InvestmentProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 2.2) Risk framework
        await RiskCategorySeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await RiskIndicatorSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await CollectionStrategySeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        
        // 2.3) Customer segmentation
        await CustomerSegmentSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 2.4) Workflow and approval definitions
        await ApprovalWorkflowSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 2.5) Report definitions
        await ReportDefinitionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // ============================================================
        // PHASE 3: Members & Groups
        // ============================================================
        
        // 3.1) Members (required for accounts and loans)
        await MemberSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 3.2) Member groups and memberships
        await MemberGroupSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await GroupMembershipSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 3.3) Member-related data
        await CreditScoreSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await MobileWalletSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await KycDocumentSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await DocumentSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // ============================================================
        // PHASE 4: Accounts & Loans
        // ============================================================
        
        // 4.1) Savings and share accounts (depend on members and products)
        await SavingsAccountSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await ShareAccountSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await FixedDepositSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await InvestmentAccountSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 4.2) Loan applications (precedes loans)
        await LoanApplicationSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 4.3) Loans (depend on members and loan products)
        await LoanSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 4.4) Loan supporting data (schedules, guarantors, collateral)
        await LoanScheduleSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await LoanGuarantorSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await LoanCollateralSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 4.5) Collateral management
        await CollateralValuationSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await CollateralInsuranceSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await CollateralReleaseSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 4.6) Loan officer assignments
        await LoanOfficerAssignmentSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // ============================================================
        // PHASE 5: Transactions & Operations
        // ============================================================
        
        // 5.1) Account transactions
        await SavingsTransactionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await ShareTransactionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await InvestmentTransactionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await MobileTransactionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 5.2) Loan transactions
        await LoanRepaymentSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await FeeChargeSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 5.3) Insurance policies and claims
        await InsurancePolicySeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await InsuranceClaimSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 5.4) Teller operations
        await TellerSessionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // ============================================================
        // PHASE 6: Collections & Delinquency
        // ============================================================
        
        // 6.1) Collection cases and actions
        await CollectionCaseSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await CollectionActionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await PromiseToPaySeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 6.2) Loan restructuring and write-offs
        await LoanRestructureSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await LoanWriteOffSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // ============================================================
        // PHASE 7: Risk & Compliance
        // ============================================================
        
        // 7.1) Risk alerts
        await RiskAlertSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 7.2) AML monitoring
        await AmlAlertSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 7.3) Communication logs
        await CommunicationLogSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // ============================================================
        // PHASE 8: Performance & Staff
        // ============================================================
        
        // 8.1) Targets
        await BranchTargetSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await LoanOfficerTargetSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 8.2) Staff training
        await StaffTrainingSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // ============================================================
        // PHASE 9: Customer Engagement
        // ============================================================
        
        // 9.1) Customer cases
        await CustomerCaseSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 9.2) Customer surveys
        await CustomerSurveySeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 9.3) Marketing campaigns
        await MarketingCampaignSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] completed seeding microfinance module with comprehensive sample data ({Count} seeders executed)", tenant, 54);
    }
}

