using FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure;

/// <summary>
/// MicroFinance module configuration and service registration.
/// Handles dependency injection setup for microfinance features.
/// Endpoints are discovered automatically by Carter from individual ICarterModule implementations.
/// </summary>
public static class MicroFinanceModule
{
    /// <summary>
    /// Registers all microfinance services in the dependency injection container.
    /// Configures DbContext, repositories, and database initializers.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for chaining.</returns>
    public static WebApplicationBuilder RegisterMicroFinanceServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.BindDbContext<MicroFinanceDbContext>();
        builder.Services.AddScoped<IDbInitializer, MicroFinanceDbInitializer>();

        // ============================================
        // Member Management Repositories
        // ============================================

        // Members repository
        builder.Services.AddKeyedScoped<IRepository<Member>, MicroFinanceRepository<Member>>("microfinance:members");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, MicroFinanceRepository<Member>>("microfinance:members");

        // MemberGroups repository
        builder.Services.AddKeyedScoped<IRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>("microfinance:membergroups");
        builder.Services.AddKeyedScoped<IReadRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>("microfinance:membergroups");

        // GroupMemberships repository
        builder.Services.AddKeyedScoped<IRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>("microfinance:groupmemberships");
        builder.Services.AddKeyedScoped<IReadRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>("microfinance:groupmemberships");

        // ============================================
        // Loan Management Repositories
        // ============================================

        // LoanProducts repository
        builder.Services.AddKeyedScoped<IRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance:loanproducts");
        builder.Services.AddKeyedScoped<IReadRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance:loanproducts");

        // Loans repository
        builder.Services.AddKeyedScoped<IRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance:loans");
        builder.Services.AddKeyedScoped<IReadRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance:loans");

        // LoanRepayments repository
        builder.Services.AddKeyedScoped<IRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance:loanrepayments");
        builder.Services.AddKeyedScoped<IReadRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance:loanrepayments");

        // LoanSchedules repository
        builder.Services.AddKeyedScoped<IRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>("microfinance:loanschedules");
        builder.Services.AddKeyedScoped<IReadRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>("microfinance:loanschedules");

        // LoanGuarantors repository
        builder.Services.AddKeyedScoped<IRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>("microfinance:loanguarantors");
        builder.Services.AddKeyedScoped<IReadRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>("microfinance:loanguarantors");

        // LoanCollaterals repository
        builder.Services.AddKeyedScoped<IRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>("microfinance:loancollaterals");
        builder.Services.AddKeyedScoped<IReadRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>("microfinance:loancollaterals");

        // ============================================
        // Savings Management Repositories
        // ============================================

        // SavingsProducts repository
        builder.Services.AddKeyedScoped<IRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance:savingsproducts");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance:savingsproducts");

        // SavingsAccounts repository
        builder.Services.AddKeyedScoped<IRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance:savingsaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance:savingsaccounts");

        // SavingsTransactions repository
        builder.Services.AddKeyedScoped<IRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance:savingstransactions");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance:savingstransactions");

        // FixedDeposits repository
        builder.Services.AddKeyedScoped<IRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>("microfinance:fixeddeposits");
        builder.Services.AddKeyedScoped<IReadRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>("microfinance:fixeddeposits");

        // ============================================
        // Share Management Repositories
        // ============================================

        // ShareProducts repository
        builder.Services.AddKeyedScoped<IRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>("microfinance:shareproducts");
        builder.Services.AddKeyedScoped<IReadRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>("microfinance:shareproducts");

        // ShareAccounts repository
        builder.Services.AddKeyedScoped<IRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>("microfinance:shareaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>("microfinance:shareaccounts");

        // ShareTransactions repository
        builder.Services.AddKeyedScoped<IRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>("microfinance:sharetransactions");
        builder.Services.AddKeyedScoped<IReadRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>("microfinance:sharetransactions");

        // ============================================
        // Fee Management Repositories
        // ============================================

        // FeeDefinitions repository
        builder.Services.AddKeyedScoped<IRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>("microfinance:feedefinitions");
        builder.Services.AddKeyedScoped<IReadRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>("microfinance:feedefinitions");

        // FeeCharges repository
        builder.Services.AddKeyedScoped<IRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>("microfinance:feecharges");
        builder.Services.AddKeyedScoped<IReadRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>("microfinance:feecharges");

        // ============================================
        // Branch & Operations Repositories
        // ============================================

        // Branches repository
        builder.Services.AddKeyedScoped<IRepository<Branch>, MicroFinanceRepository<Branch>>("microfinance:branches");
        builder.Services.AddKeyedScoped<IReadRepository<Branch>, MicroFinanceRepository<Branch>>("microfinance:branches");

        // CashVaults repository
        builder.Services.AddKeyedScoped<IRepository<CashVault>, MicroFinanceRepository<CashVault>>("microfinance:cashvaults");
        builder.Services.AddKeyedScoped<IReadRepository<CashVault>, MicroFinanceRepository<CashVault>>("microfinance:cashvaults");

        // TellerSessions repository
        builder.Services.AddKeyedScoped<IRepository<TellerSession>, MicroFinanceRepository<TellerSession>>("microfinance:tellersessions");
        builder.Services.AddKeyedScoped<IReadRepository<TellerSession>, MicroFinanceRepository<TellerSession>>("microfinance:tellersessions");

        // ============================================
        // Insurance Repositories
        // ============================================

        // InsuranceProducts repository
        builder.Services.AddKeyedScoped<IRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>("microfinance:insuranceproducts");
        builder.Services.AddKeyedScoped<IReadRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>("microfinance:insuranceproducts");

        // InsurancePolicies repository
        builder.Services.AddKeyedScoped<IRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>("microfinance:insurancepolicies");
        builder.Services.AddKeyedScoped<IReadRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>("microfinance:insurancepolicies");

        // InsuranceClaims repository
        builder.Services.AddKeyedScoped<IRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>("microfinance:insuranceclaims");
        builder.Services.AddKeyedScoped<IReadRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>("microfinance:insuranceclaims");

        // ============================================
        // Loan Applications Repositories
        // ============================================

        // LoanApplications repository
        builder.Services.AddKeyedScoped<IRepository<LoanApplication>, MicroFinanceRepository<LoanApplication>>("microfinance:loanapplications");
        builder.Services.AddKeyedScoped<IReadRepository<LoanApplication>, MicroFinanceRepository<LoanApplication>>("microfinance:loanapplications");

        // ============================================
        // Collections Repositories
        // ============================================

        // CollectionCases repository
        builder.Services.AddKeyedScoped<IRepository<CollectionCase>, MicroFinanceRepository<CollectionCase>>("microfinance:collectioncases");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionCase>, MicroFinanceRepository<CollectionCase>>("microfinance:collectioncases");

        // ============================================
        // Investment Management Repositories
        // ============================================

        // InvestmentProducts repository
        builder.Services.AddKeyedScoped<IRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>("microfinance:investmentproducts");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>("microfinance:investmentproducts");

        // InvestmentAccounts repository
        builder.Services.AddKeyedScoped<IRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>("microfinance:investmentaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>("microfinance:investmentaccounts");

        // InvestmentTransactions repository
        builder.Services.AddKeyedScoped<IRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>("microfinance:investmenttransactions");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>("microfinance:investmenttransactions");

        // ============================================
        // Staff & HR Repositories
        // ============================================

        // Staff repository
        builder.Services.AddKeyedScoped<IRepository<Staff>, MicroFinanceRepository<Staff>>("microfinance:staff");
        builder.Services.AddKeyedScoped<IReadRepository<Staff>, MicroFinanceRepository<Staff>>("microfinance:staff");

        // ============================================
        // Mobile Banking Repositories
        // ============================================

        // MobileWallets repository
        builder.Services.AddKeyedScoped<IRepository<MobileWallet>, MicroFinanceRepository<MobileWallet>>("microfinance:mobilewallets");
        builder.Services.AddKeyedScoped<IReadRepository<MobileWallet>, MicroFinanceRepository<MobileWallet>>("microfinance:mobilewallets");

        // ============================================
        // AML/Compliance Repositories
        // ============================================

        // AmlAlerts repository
        builder.Services.AddKeyedScoped<IRepository<AmlAlert>, MicroFinanceRepository<AmlAlert>>("microfinance:amlalerts");
        builder.Services.AddKeyedScoped<IReadRepository<AmlAlert>, MicroFinanceRepository<AmlAlert>>("microfinance:amlalerts");

        // ============================================
        // Risk Management Repositories
        // ============================================

        // CreditScores repository
        builder.Services.AddKeyedScoped<IRepository<CreditScore>, MicroFinanceRepository<CreditScore>>("microfinance:creditscores");
        builder.Services.AddKeyedScoped<IReadRepository<CreditScore>, MicroFinanceRepository<CreditScore>>("microfinance:creditscores");

        // RiskAlerts repository
        builder.Services.AddKeyedScoped<IRepository<RiskAlert>, MicroFinanceRepository<RiskAlert>>("microfinance:riskalerts");
        builder.Services.AddKeyedScoped<IReadRepository<RiskAlert>, MicroFinanceRepository<RiskAlert>>("microfinance:riskalerts");

        // ============================================
        // Customer Service Repositories
        // ============================================

        // CustomerCases repository
        builder.Services.AddKeyedScoped<IRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>("microfinance:customercases");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>("microfinance:customercases");

        // ============================================
        // Document Repositories
        // ============================================

        // Documents repository
        builder.Services.AddKeyedScoped<IRepository<Document>, MicroFinanceRepository<Document>>("microfinance:documents");
        builder.Services.AddKeyedScoped<IReadRepository<Document>, MicroFinanceRepository<Document>>("microfinance:documents");

        // ============================================
        // Communication Repositories
        // ============================================

        // CommunicationLogs repository
        builder.Services.AddKeyedScoped<IRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>("microfinance:communicationlogs");
        builder.Services.AddKeyedScoped<IReadRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>("microfinance:communicationlogs");

        // ============================================
        // Collateral Management Repositories
        // ============================================

        // CollateralTypes repository
        builder.Services.AddKeyedScoped<IRepository<CollateralType>, MicroFinanceRepository<CollateralType>>("microfinance:collateraltypes");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralType>, MicroFinanceRepository<CollateralType>>("microfinance:collateraltypes");

        // ============================================
        // Collections Management Repositories
        // ============================================

        // CollectionActions repository
        builder.Services.AddKeyedScoped<IRepository<CollectionAction>, MicroFinanceRepository<CollectionAction>>("microfinance:collectionactions");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionAction>, MicroFinanceRepository<CollectionAction>>("microfinance:collectionactions");

        // PromiseToPays repository
        builder.Services.AddKeyedScoped<IRepository<PromiseToPay>, MicroFinanceRepository<PromiseToPay>>("microfinance:promisetopays");
        builder.Services.AddKeyedScoped<IReadRepository<PromiseToPay>, MicroFinanceRepository<PromiseToPay>>("microfinance:promisetopays");

        // ============================================
        // Collateral Valuation & Insurance Repositories
        // ============================================

        // CollateralValuations repository
        builder.Services.AddKeyedScoped<IRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>("microfinance:collateralvaluations");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>("microfinance:collateralvaluations");

        // CollateralInsurances repository
        builder.Services.AddKeyedScoped<IRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>("microfinance:collateralinsurances");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>("microfinance:collateralinsurances");

        // CollateralReleases repository
        builder.Services.AddKeyedScoped<IRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>("microfinance:collateralreleases");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>("microfinance:collateralreleases");

        // ============================================
        // Collection Strategies Repository
        // ============================================

        // CollectionStrategies repository
        builder.Services.AddKeyedScoped<IRepository<CollectionStrategy>, MicroFinanceRepository<CollectionStrategy>>("microfinance:collectionstrategies");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionStrategy>, MicroFinanceRepository<CollectionStrategy>>("microfinance:collectionstrategies");

        // ============================================
        // Communication Templates Repository
        // ============================================

        // CommunicationTemplates repository
        builder.Services.AddKeyedScoped<IRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>("microfinance:communicationtemplates");
        builder.Services.AddKeyedScoped<IReadRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>("microfinance:communicationtemplates");

        // ============================================
        // Credit Bureau Repositories
        // ============================================

        // CreditBureauInquiries repository
        builder.Services.AddKeyedScoped<IRepository<CreditBureauInquiry>, MicroFinanceRepository<CreditBureauInquiry>>("microfinance:creditbureauinquiries");
        builder.Services.AddKeyedScoped<IReadRepository<CreditBureauInquiry>, MicroFinanceRepository<CreditBureauInquiry>>("microfinance:creditbureauinquiries");

        // ============================================
        // Debt Settlement Repositories
        // ============================================

        // DebtSettlements repository
        builder.Services.AddKeyedScoped<IRepository<DebtSettlement>, MicroFinanceRepository<DebtSettlement>>("microfinance:debtsettlements");
        builder.Services.AddKeyedScoped<IReadRepository<DebtSettlement>, MicroFinanceRepository<DebtSettlement>>("microfinance:debtsettlements");

        // ============================================
        // Loan Recovery Repositories
        // ============================================

        // LegalActions repository
        builder.Services.AddKeyedScoped<IRepository<LegalAction>, MicroFinanceRepository<LegalAction>>("microfinance:legalactions");
        builder.Services.AddKeyedScoped<IReadRepository<LegalAction>, MicroFinanceRepository<LegalAction>>("microfinance:legalactions");

        // LoanRestructures repository
        builder.Services.AddKeyedScoped<IRepository<LoanRestructure>, MicroFinanceRepository<LoanRestructure>>("microfinance:loanrestructures");
        builder.Services.AddKeyedScoped<IReadRepository<LoanRestructure>, MicroFinanceRepository<LoanRestructure>>("microfinance:loanrestructures");

        // LoanWriteOffs repository
        builder.Services.AddKeyedScoped<IRepository<LoanWriteOff>, MicroFinanceRepository<LoanWriteOff>>("microfinance:loanwriteoffs");
        builder.Services.AddKeyedScoped<IReadRepository<LoanWriteOff>, MicroFinanceRepository<LoanWriteOff>>("microfinance:loanwriteoffs");

        // ============================================
        // Loan Operations Repositories
        // ============================================

        // LoanDisbursementTranches repository
        builder.Services.AddKeyedScoped<IRepository<LoanDisbursementTranche>, MicroFinanceRepository<LoanDisbursementTranche>>("microfinance:loandisbursementtranches");
        builder.Services.AddKeyedScoped<IReadRepository<LoanDisbursementTranche>, MicroFinanceRepository<LoanDisbursementTranche>>("microfinance:loandisbursementtranches");

        // LoanOfficerAssignments repository
        builder.Services.AddKeyedScoped<IRepository<LoanOfficerAssignment>, MicroFinanceRepository<LoanOfficerAssignment>>("microfinance:loanofficerassignments");
        builder.Services.AddKeyedScoped<IReadRepository<LoanOfficerAssignment>, MicroFinanceRepository<LoanOfficerAssignment>>("microfinance:loanofficerassignments");

        // ============================================
        // Document Repositories
        // ============================================

        // KycDocuments repository
        builder.Services.AddKeyedScoped<IRepository<KycDocument>, MicroFinanceRepository<KycDocument>>("microfinance:kycdocuments");
        builder.Services.AddKeyedScoped<IReadRepository<KycDocument>, MicroFinanceRepository<KycDocument>>("microfinance:kycdocuments");

        // ============================================
        // Customer Analytics Repositories
        // ============================================

        // CustomerSegments repository
        builder.Services.AddKeyedScoped<IRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>("microfinance:customersegments");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>("microfinance:customersegments");

        // ============================================
        // Mobile Banking Transactions Repositories
        // ============================================

        // MobileTransactions repository
        builder.Services.AddKeyedScoped<IRepository<MobileTransaction>, MicroFinanceRepository<MobileTransaction>>("microfinance:mobiletransactions");
        builder.Services.AddKeyedScoped<IReadRepository<MobileTransaction>, MicroFinanceRepository<MobileTransaction>>("microfinance:mobiletransactions");

        // ============================================
        // Risk Management Extended Repositories
        // ============================================

        // RiskCategories repository
        builder.Services.AddKeyedScoped<IRepository<RiskCategory>, MicroFinanceRepository<RiskCategory>>("microfinance:riskcategories");
        builder.Services.AddKeyedScoped<IReadRepository<RiskCategory>, MicroFinanceRepository<RiskCategory>>("microfinance:riskcategories");

        // RiskIndicators repository
        builder.Services.AddKeyedScoped<IRepository<RiskIndicator>, MicroFinanceRepository<RiskIndicator>>("microfinance:riskindicators");
        builder.Services.AddKeyedScoped<IReadRepository<RiskIndicator>, MicroFinanceRepository<RiskIndicator>>("microfinance:riskindicators");

        // ============================================
        // Staff Training Repositories
        // ============================================

        // StaffTrainings repository
        builder.Services.AddKeyedScoped<IRepository<StaffTraining>, MicroFinanceRepository<StaffTraining>>("microfinance:stafftrainings");
        builder.Services.AddKeyedScoped<IReadRepository<StaffTraining>, MicroFinanceRepository<StaffTraining>>("microfinance:stafftrainings");

        // ============================================
        // Approval Workflow Repositories
        // ============================================

        // ApprovalWorkflows repository
        builder.Services.AddKeyedScoped<IRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>("microfinance:approvalworkflows");
        builder.Services.AddKeyedScoped<IReadRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>("microfinance:approvalworkflows");

        // ApprovalRequests repository
        builder.Services.AddKeyedScoped<IRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>("microfinance:approvalrequests");
        builder.Services.AddKeyedScoped<IReadRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>("microfinance:approvalrequests");

        // ============================================
        // Performance Targets Repositories
        // ============================================

        // BranchTargets repository
        builder.Services.AddKeyedScoped<IRepository<BranchTarget>, MicroFinanceRepository<BranchTarget>>("microfinance:branchtargets");
        builder.Services.AddKeyedScoped<IReadRepository<BranchTarget>, MicroFinanceRepository<BranchTarget>>("microfinance:branchtargets");

        // LoanOfficerTargets repository
        builder.Services.AddKeyedScoped<IRepository<LoanOfficerTarget>, MicroFinanceRepository<LoanOfficerTarget>>("microfinance:loanofficertargets");
        builder.Services.AddKeyedScoped<IReadRepository<LoanOfficerTarget>, MicroFinanceRepository<LoanOfficerTarget>>("microfinance:loanofficertargets");

        // ============================================
        // Agent Banking & Digital Channels Repositories
        // ============================================

        // AgentBankings repository
        builder.Services.AddKeyedScoped<IRepository<AgentBanking>, MicroFinanceRepository<AgentBanking>>("microfinance:agentbankings");
        builder.Services.AddKeyedScoped<IReadRepository<AgentBanking>, MicroFinanceRepository<AgentBanking>>("microfinance:agentbankings");

        // QrPayments repository
        builder.Services.AddKeyedScoped<IRepository<QrPayment>, MicroFinanceRepository<QrPayment>>("microfinance:qrpayments");
        builder.Services.AddKeyedScoped<IReadRepository<QrPayment>, MicroFinanceRepository<QrPayment>>("microfinance:qrpayments");

        // UssdSessions repository
        builder.Services.AddKeyedScoped<IRepository<UssdSession>, MicroFinanceRepository<UssdSession>>("microfinance:ussdsessions");
        builder.Services.AddKeyedScoped<IReadRepository<UssdSession>, MicroFinanceRepository<UssdSession>>("microfinance:ussdsessions");

        // PaymentGateways repository
        builder.Services.AddKeyedScoped<IRepository<PaymentGateway>, MicroFinanceRepository<PaymentGateway>>("microfinance:paymentgateways");
        builder.Services.AddKeyedScoped<IReadRepository<PaymentGateway>, MicroFinanceRepository<PaymentGateway>>("microfinance:paymentgateways");

        // ============================================
        // Configuration & Reporting Repositories
        // ============================================

        // MfiConfigurations repository
        builder.Services.AddKeyedScoped<IRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>("microfinance:mficonfigurations");
        builder.Services.AddKeyedScoped<IReadRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>("microfinance:mficonfigurations");

        // ReportDefinitions repository
        builder.Services.AddKeyedScoped<IRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>("microfinance:reportdefinitions");
        builder.Services.AddKeyedScoped<IReadRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>("microfinance:reportdefinitions");

        // ReportGenerations repository
        builder.Services.AddKeyedScoped<IRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>("microfinance:reportgenerations");
        builder.Services.AddKeyedScoped<IReadRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>("microfinance:reportgenerations");

        // ============================================
        // Credit Bureau Repositories
        // ============================================

        // CreditBureauReports repository
        builder.Services.AddKeyedScoped<IRepository<CreditBureauReport>, MicroFinanceRepository<CreditBureauReport>>("microfinance:creditbureaureports");
        builder.Services.AddKeyedScoped<IReadRepository<CreditBureauReport>, MicroFinanceRepository<CreditBureauReport>>("microfinance:creditbureaureports");

        // ============================================
        // Marketing Repositories
        // ============================================

        // MarketingCampaigns repository
        builder.Services.AddKeyedScoped<IRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>("microfinance:marketingcampaigns");
        builder.Services.AddKeyedScoped<IReadRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>("microfinance:marketingcampaigns");

        // CustomerSurveys repository
        builder.Services.AddKeyedScoped<IRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>("microfinance:customersurveys");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>("microfinance:customersurveys");

        return builder;
    }

    /// <summary>
    /// Applies the microfinance module to the web application.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication UseMicroFinanceModule(this WebApplication app)
    {
        return app;
    }
}

