using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Tenant;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

/// <summary>
/// Database context for the MicroFinance module.
/// Manages all microfinance-related entities including members, loans, savings, shares, and fees.
/// </summary>
public sealed class MicroFinanceDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<MicroFinanceDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor, options, publisher, settings)
{
    // ============================================
    // Member Management
    // ============================================

    /// <summary>Gets or sets the Members DbSet.</summary>
    public DbSet<Member> Members { get; set; } = null!;

    /// <summary>Gets or sets the MemberGroups DbSet.</summary>
    public DbSet<MemberGroup> MemberGroups { get; set; } = null!;

    /// <summary>Gets or sets the GroupMemberships DbSet.</summary>
    public DbSet<GroupMembership> GroupMemberships { get; set; } = null!;

    // ============================================
    // Loan Management
    // ============================================

    /// <summary>Gets or sets the LoanProducts DbSet.</summary>
    public DbSet<LoanProduct> LoanProducts { get; set; } = null!;

    /// <summary>Gets or sets the Loans DbSet.</summary>
    public DbSet<Loan> Loans { get; set; } = null!;

    /// <summary>Gets or sets the LoanRepayments DbSet.</summary>
    public DbSet<LoanRepayment> LoanRepayments { get; set; } = null!;

    /// <summary>Gets or sets the LoanSchedules DbSet.</summary>
    public DbSet<LoanSchedule> LoanSchedules { get; set; } = null!;

    /// <summary>Gets or sets the LoanGuarantors DbSet.</summary>
    public DbSet<LoanGuarantor> LoanGuarantors { get; set; } = null!;

    /// <summary>Gets or sets the LoanCollaterals DbSet.</summary>
    public DbSet<LoanCollateral> LoanCollaterals { get; set; } = null!;

    /// <summary>Gets or sets the LoanApplications DbSet.</summary>
    public DbSet<LoanApplication> LoanApplications { get; set; } = null!;

    /// <summary>Gets or sets the LoanDisbursementTranches DbSet.</summary>
    public DbSet<LoanDisbursementTranche> LoanDisbursementTranches { get; set; } = null!;

    /// <summary>Gets or sets the LoanOfficerAssignments DbSet.</summary>
    public DbSet<LoanOfficerAssignment> LoanOfficerAssignments { get; set; } = null!;

    /// <summary>Gets or sets the LoanOfficerTargets DbSet.</summary>
    public DbSet<LoanOfficerTarget> LoanOfficerTargets { get; set; } = null!;

    /// <summary>Gets or sets the LoanRestructures DbSet.</summary>
    public DbSet<LoanRestructure> LoanRestructures { get; set; } = null!;

    /// <summary>Gets or sets the LoanWriteOffs DbSet.</summary>
    public DbSet<LoanWriteOff> LoanWriteOffs { get; set; } = null!;

    /// <summary>Gets or sets the InterestRateChanges DbSet.</summary>
    public DbSet<InterestRateChange> InterestRateChanges { get; set; } = null!;

    // ============================================
    // Savings Management
    // ============================================

    /// <summary>Gets or sets the SavingsProducts DbSet.</summary>
    public DbSet<SavingsProduct> SavingsProducts { get; set; } = null!;

    /// <summary>Gets or sets the SavingsAccounts DbSet.</summary>
    public DbSet<SavingsAccount> SavingsAccounts { get; set; } = null!;

    /// <summary>Gets or sets the SavingsTransactions DbSet.</summary>
    public DbSet<SavingsTransaction> SavingsTransactions { get; set; } = null!;

    /// <summary>Gets or sets the FixedDeposits DbSet.</summary>
    public DbSet<FixedDeposit> FixedDeposits { get; set; } = null!;

    // ============================================
    // Share Management
    // ============================================

    /// <summary>Gets or sets the ShareProducts DbSet.</summary>
    public DbSet<ShareProduct> ShareProducts { get; set; } = null!;

    /// <summary>Gets or sets the ShareAccounts DbSet.</summary>
    public DbSet<ShareAccount> ShareAccounts { get; set; } = null!;

    /// <summary>Gets or sets the ShareTransactions DbSet.</summary>
    public DbSet<ShareTransaction> ShareTransactions { get; set; } = null!;

    // ============================================
    // Fee Management
    // ============================================

    /// <summary>Gets or sets the FeeDefinitions DbSet.</summary>
    public DbSet<FeeDefinition> FeeDefinitions { get; set; } = null!;

    /// <summary>Gets or sets the FeeCharges DbSet.</summary>
    public DbSet<FeeCharge> FeeCharges { get; set; } = null!;

    /// <summary>Gets or sets the FeePayments DbSet.</summary>
    public DbSet<FeePayment> FeePayments { get; set; } = null!;

    /// <summary>Gets or sets the FeeWaivers DbSet.</summary>
    public DbSet<FeeWaiver> FeeWaivers { get; set; } = null!;

    // ============================================
    // Collateral Management
    // ============================================

    /// <summary>Gets or sets the CollateralTypes DbSet.</summary>
    public DbSet<CollateralType> CollateralTypes { get; set; } = null!;

    /// <summary>Gets or sets the CollateralValuations DbSet.</summary>
    public DbSet<CollateralValuation> CollateralValuations { get; set; } = null!;

    /// <summary>Gets or sets the CollateralInsurances DbSet.</summary>
    public DbSet<CollateralInsurance> CollateralInsurances { get; set; } = null!;

    /// <summary>Gets or sets the CollateralReleases DbSet.</summary>
    public DbSet<CollateralRelease> CollateralReleases { get; set; } = null!;

    // ============================================
    // Collections & Recovery
    // ============================================

    /// <summary>Gets or sets the CollectionCases DbSet.</summary>
    public DbSet<CollectionCase> CollectionCases { get; set; } = null!;

    /// <summary>Gets or sets the CollectionActions DbSet.</summary>
    public DbSet<CollectionAction> CollectionActions { get; set; } = null!;

    /// <summary>Gets or sets the CollectionStrategies DbSet.</summary>
    public DbSet<CollectionStrategy> CollectionStrategies { get; set; } = null!;

    /// <summary>Gets or sets the PromiseToPays DbSet.</summary>
    public DbSet<PromiseToPay> PromiseToPays { get; set; } = null!;

    /// <summary>Gets or sets the DebtSettlements DbSet.</summary>
    public DbSet<DebtSettlement> DebtSettlements { get; set; } = null!;

    /// <summary>Gets or sets the LegalActions DbSet.</summary>
    public DbSet<LegalAction> LegalActions { get; set; } = null!;

    // ============================================
    // Risk Management
    // ============================================

    /// <summary>Gets or sets the RiskCategories DbSet.</summary>
    public DbSet<RiskCategory> RiskCategories { get; set; } = null!;

    /// <summary>Gets or sets the RiskIndicators DbSet.</summary>
    public DbSet<RiskIndicator> RiskIndicators { get; set; } = null!;

    /// <summary>Gets or sets the RiskAlerts DbSet.</summary>
    public DbSet<RiskAlert> RiskAlerts { get; set; } = null!;

    /// <summary>Gets or sets the CreditScores DbSet.</summary>
    public DbSet<CreditScore> CreditScores { get; set; } = null!;

    /// <summary>Gets or sets the CreditBureauInquiries DbSet.</summary>
    public DbSet<CreditBureauInquiry> CreditBureauInquiries { get; set; } = null!;

    /// <summary>Gets or sets the CreditBureauReports DbSet.</summary>
    public DbSet<CreditBureauReport> CreditBureauReports { get; set; } = null!;

    /// <summary>Gets or sets the AmlAlerts DbSet.</summary>
    public DbSet<AmlAlert> AmlAlerts { get; set; } = null!;

    // ============================================
    // Insurance Management
    // ============================================

    /// <summary>Gets or sets the InsuranceProducts DbSet.</summary>
    public DbSet<InsuranceProduct> InsuranceProducts { get; set; } = null!;

    /// <summary>Gets or sets the InsurancePolicies DbSet.</summary>
    public DbSet<InsurancePolicy> InsurancePolicies { get; set; } = null!;

    /// <summary>Gets or sets the InsuranceClaims DbSet.</summary>
    public DbSet<InsuranceClaim> InsuranceClaims { get; set; } = null!;

    // ============================================
    // Investment Management
    // ============================================

    /// <summary>Gets or sets the InvestmentProducts DbSet.</summary>
    public DbSet<InvestmentProduct> InvestmentProducts { get; set; } = null!;

    /// <summary>Gets or sets the InvestmentAccounts DbSet.</summary>
    public DbSet<InvestmentAccount> InvestmentAccounts { get; set; } = null!;

    /// <summary>Gets or sets the InvestmentTransactions DbSet.</summary>
    public DbSet<InvestmentTransaction> InvestmentTransactions { get; set; } = null!;

    // ============================================
    // Branch & Staff Management
    // ============================================

    /// <summary>Gets or sets the Branches DbSet.</summary>
    public DbSet<Branch> Branches { get; set; } = null!;

    /// <summary>Gets or sets the BranchTargets DbSet.</summary>
    public DbSet<BranchTarget> BranchTargets { get; set; } = null!;

    /// <summary>Gets or sets the Staff DbSet.</summary>
    public DbSet<Staff> Staff { get; set; } = null!;

    /// <summary>Gets or sets the StaffTrainings DbSet.</summary>
    public DbSet<StaffTraining> StaffTrainings { get; set; } = null!;

    /// <summary>Gets or sets the TellerSessions DbSet.</summary>
    public DbSet<TellerSession> TellerSessions { get; set; } = null!;

    /// <summary>Gets or sets the CashVaults DbSet.</summary>
    public DbSet<CashVault> CashVaults { get; set; } = null!;

    // ============================================
    // Digital Banking & Payments
    // ============================================

    /// <summary>Gets or sets the MobileWallets DbSet.</summary>
    public DbSet<MobileWallet> MobileWallets { get; set; } = null!;

    /// <summary>Gets or sets the MobileTransactions DbSet.</summary>
    public DbSet<MobileTransaction> MobileTransactions { get; set; } = null!;

    /// <summary>Gets or sets the AgentBankings DbSet.</summary>
    public DbSet<AgentBanking> AgentBankings { get; set; } = null!;

    /// <summary>Gets or sets the PaymentGateways DbSet.</summary>
    public DbSet<PaymentGateway> PaymentGateways { get; set; } = null!;

    /// <summary>Gets or sets the QrPayments DbSet.</summary>
    public DbSet<QrPayment> QrPayments { get; set; } = null!;

    /// <summary>Gets or sets the UssdSessions DbSet.</summary>
    public DbSet<UssdSession> UssdSessions { get; set; } = null!;

    // ============================================
    // Workflow & Approvals
    // ============================================

    /// <summary>Gets or sets the ApprovalWorkflows DbSet.</summary>
    public DbSet<ApprovalWorkflow> ApprovalWorkflows { get; set; } = null!;

    /// <summary>Gets or sets the ApprovalRequests DbSet.</summary>
    public DbSet<ApprovalRequest> ApprovalRequests { get; set; } = null!;

    // ============================================
    // Communication & Documents
    // ============================================

    /// <summary>Gets or sets the Documents DbSet.</summary>
    public DbSet<Document> Documents { get; set; } = null!;

    /// <summary>Gets or sets the KycDocuments DbSet.</summary>
    public DbSet<KycDocument> KycDocuments { get; set; } = null!;

    /// <summary>Gets or sets the CommunicationTemplates DbSet.</summary>
    public DbSet<CommunicationTemplate> CommunicationTemplates { get; set; } = null!;

    /// <summary>Gets or sets the CommunicationLogs DbSet.</summary>
    public DbSet<CommunicationLog> CommunicationLogs { get; set; } = null!;

    // ============================================
    // Customer Relationship Management
    // ============================================

    /// <summary>Gets or sets the CustomerCases DbSet.</summary>
    public DbSet<CustomerCase> CustomerCases { get; set; } = null!;

    /// <summary>Gets or sets the CustomerSegments DbSet.</summary>
    public DbSet<CustomerSegment> CustomerSegments { get; set; } = null!;

    /// <summary>Gets or sets the CustomerSurveys DbSet.</summary>
    public DbSet<CustomerSurvey> CustomerSurveys { get; set; } = null!;

    /// <summary>Gets or sets the MarketingCampaigns DbSet.</summary>
    public DbSet<MarketingCampaign> MarketingCampaigns { get; set; } = null!;

    // ============================================
    // Reporting & Configuration
    // ============================================

    /// <summary>Gets or sets the ReportDefinitions DbSet.</summary>
    public DbSet<ReportDefinition> ReportDefinitions { get; set; } = null!;

    /// <summary>Gets or sets the ReportGenerations DbSet.</summary>
    public DbSet<ReportGeneration> ReportGenerations { get; set; } = null!;

    /// <summary>Gets or sets the MfiConfigurations DbSet.</summary>
    public DbSet<MfiConfiguration> MfiConfigurations { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MicroFinanceDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.MicroFinance);
    }

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }
}

