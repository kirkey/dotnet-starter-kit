using FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure;

/// <summary>
/// Main module configuration for the MicroFinance system.
/// Registers all microfinance endpoints and services.
/// </summary>
public static class MicroFinanceModule
{
    /// <summary>
    /// Registers all microfinance endpoints with the application.
    /// All endpoints are auto-discovered by Carter via ICarterModule implementations.
    /// </summary>
    public static IEndpointRouteBuilder MapMicroFinanceEndpoints(this IEndpointRouteBuilder app)
    {
        return app;
    }

    /// <summary>
    /// Registers microfinance services with dependency injection container.
    /// </summary>
    public static WebApplicationBuilder RegisterMicroFinanceServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        // ============================================================================
        // CORE SERVICES
        // ============================================================================
        builder.Services.BindDbContext<MicroFinanceDbContext>();
        builder.Services.AddScoped<IDbInitializer, MicroFinanceDbInitializer>();
    
        // ============================================================================
        // NON-KEYED REPOSITORY REGISTRATIONS
        // ============================================================================
        
        // --- Member Management ---
        builder.Services.AddScoped<IRepository<Member>, MicroFinanceRepository<Member>>();
        builder.Services.AddScoped<IReadRepository<Member>, MicroFinanceRepository<Member>>();
        builder.Services.AddScoped<IRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>();
        builder.Services.AddScoped<IReadRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>();
        builder.Services.AddScoped<IRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>();
        builder.Services.AddScoped<IReadRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>();
        
        // --- Loan Management ---
        builder.Services.AddScoped<IRepository<Loan>, MicroFinanceRepository<Loan>>();
        builder.Services.AddScoped<IReadRepository<Loan>, MicroFinanceRepository<Loan>>();
        builder.Services.AddScoped<IRepository<LoanApplication>, MicroFinanceRepository<LoanApplication>>();
        builder.Services.AddScoped<IReadRepository<LoanApplication>, MicroFinanceRepository<LoanApplication>>();
        builder.Services.AddScoped<IRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>();
        builder.Services.AddScoped<IReadRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>();
        builder.Services.AddScoped<IRepository<LoanDisbursementTranche>, MicroFinanceRepository<LoanDisbursementTranche>>();
        builder.Services.AddScoped<IReadRepository<LoanDisbursementTranche>, MicroFinanceRepository<LoanDisbursementTranche>>();
        builder.Services.AddScoped<IRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>();
        builder.Services.AddScoped<IReadRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>();
        builder.Services.AddScoped<IRepository<LoanOfficerAssignment>, MicroFinanceRepository<LoanOfficerAssignment>>();
        builder.Services.AddScoped<IReadRepository<LoanOfficerAssignment>, MicroFinanceRepository<LoanOfficerAssignment>>();
        builder.Services.AddScoped<IRepository<LoanOfficerTarget>, MicroFinanceRepository<LoanOfficerTarget>>();
        builder.Services.AddScoped<IReadRepository<LoanOfficerTarget>, MicroFinanceRepository<LoanOfficerTarget>>();
        builder.Services.AddScoped<IRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>();
        builder.Services.AddScoped<IReadRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>();
        builder.Services.AddScoped<IRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>();
        builder.Services.AddScoped<IReadRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>();
        builder.Services.AddScoped<IRepository<LoanRestructure>, MicroFinanceRepository<LoanRestructure>>();
        builder.Services.AddScoped<IReadRepository<LoanRestructure>, MicroFinanceRepository<LoanRestructure>>();
        builder.Services.AddScoped<IRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>();
        builder.Services.AddScoped<IReadRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>();
        builder.Services.AddScoped<IRepository<LoanWriteOff>, MicroFinanceRepository<LoanWriteOff>>();
        builder.Services.AddScoped<IReadRepository<LoanWriteOff>, MicroFinanceRepository<LoanWriteOff>>();
        
        // --- Savings Management ---
        builder.Services.AddScoped<IRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>();
        builder.Services.AddScoped<IReadRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>();
        builder.Services.AddScoped<IRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>();
        builder.Services.AddScoped<IReadRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>();
        builder.Services.AddScoped<IRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>();
        builder.Services.AddScoped<IReadRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>();
        builder.Services.AddScoped<IRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>();
        builder.Services.AddScoped<IReadRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>();
        
        // --- Share Management ---
        builder.Services.AddScoped<IRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>();
        builder.Services.AddScoped<IReadRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>();
        builder.Services.AddScoped<IRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>();
        builder.Services.AddScoped<IReadRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>();
        builder.Services.AddScoped<IRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>();
        builder.Services.AddScoped<IReadRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>();
        
        // --- Fee Management ---
        builder.Services.AddScoped<IRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>();
        builder.Services.AddScoped<IReadRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>();
        builder.Services.AddScoped<IRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>();
        builder.Services.AddScoped<IReadRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>();
        
        // --- Collateral Management ---
        builder.Services.AddScoped<IRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>();
        builder.Services.AddScoped<IReadRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>();
        builder.Services.AddScoped<IRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>();
        builder.Services.AddScoped<IReadRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>();
        builder.Services.AddScoped<IRepository<CollateralType>, MicroFinanceRepository<CollateralType>>();
        builder.Services.AddScoped<IReadRepository<CollateralType>, MicroFinanceRepository<CollateralType>>();
        builder.Services.AddScoped<IRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>();
        builder.Services.AddScoped<IReadRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>();
        
        // --- Collections & Recovery ---
        builder.Services.AddScoped<IRepository<CollectionAction>, MicroFinanceRepository<CollectionAction>>();
        builder.Services.AddScoped<IReadRepository<CollectionAction>, MicroFinanceRepository<CollectionAction>>();
        builder.Services.AddScoped<IRepository<CollectionCase>, MicroFinanceRepository<CollectionCase>>();
        builder.Services.AddScoped<IReadRepository<CollectionCase>, MicroFinanceRepository<CollectionCase>>();
        builder.Services.AddScoped<IRepository<CollectionStrategy>, MicroFinanceRepository<CollectionStrategy>>();
        builder.Services.AddScoped<IReadRepository<CollectionStrategy>, MicroFinanceRepository<CollectionStrategy>>();
        builder.Services.AddScoped<IRepository<DebtSettlement>, MicroFinanceRepository<DebtSettlement>>();
        builder.Services.AddScoped<IReadRepository<DebtSettlement>, MicroFinanceRepository<DebtSettlement>>();
        builder.Services.AddScoped<IRepository<LegalAction>, MicroFinanceRepository<LegalAction>>();
        builder.Services.AddScoped<IReadRepository<LegalAction>, MicroFinanceRepository<LegalAction>>();
        builder.Services.AddScoped<IRepository<PromiseToPay>, MicroFinanceRepository<PromiseToPay>>();
        builder.Services.AddScoped<IReadRepository<PromiseToPay>, MicroFinanceRepository<PromiseToPay>>();
        
        // --- Risk Management & Credit ---
        builder.Services.AddScoped<IRepository<AmlAlert>, MicroFinanceRepository<AmlAlert>>();
        builder.Services.AddScoped<IReadRepository<AmlAlert>, MicroFinanceRepository<AmlAlert>>();
        builder.Services.AddScoped<IRepository<CreditBureauInquiry>, MicroFinanceRepository<CreditBureauInquiry>>();
        builder.Services.AddScoped<IReadRepository<CreditBureauInquiry>, MicroFinanceRepository<CreditBureauInquiry>>();
        builder.Services.AddScoped<IRepository<CreditBureauReport>, MicroFinanceRepository<CreditBureauReport>>();
        builder.Services.AddScoped<IReadRepository<CreditBureauReport>, MicroFinanceRepository<CreditBureauReport>>();
        builder.Services.AddScoped<IRepository<CreditScore>, MicroFinanceRepository<CreditScore>>();
        builder.Services.AddScoped<IReadRepository<CreditScore>, MicroFinanceRepository<CreditScore>>();
        builder.Services.AddScoped<IRepository<RiskAlert>, MicroFinanceRepository<RiskAlert>>();
        builder.Services.AddScoped<IReadRepository<RiskAlert>, MicroFinanceRepository<RiskAlert>>();
        builder.Services.AddScoped<IRepository<RiskCategory>, MicroFinanceRepository<RiskCategory>>();
        builder.Services.AddScoped<IReadRepository<RiskCategory>, MicroFinanceRepository<RiskCategory>>();
        builder.Services.AddScoped<IRepository<RiskIndicator>, MicroFinanceRepository<RiskIndicator>>();
        builder.Services.AddScoped<IReadRepository<RiskIndicator>, MicroFinanceRepository<RiskIndicator>>();
        
        // --- Insurance Management ---
        builder.Services.AddScoped<IRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>();
        builder.Services.AddScoped<IReadRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>();
        builder.Services.AddScoped<IRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>();
        builder.Services.AddScoped<IReadRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>();
        builder.Services.AddScoped<IRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>();
        builder.Services.AddScoped<IReadRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>();
        
        // --- Investment Management ---
        builder.Services.AddScoped<IRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>();
        builder.Services.AddScoped<IReadRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>();
        builder.Services.AddScoped<IRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>();
        builder.Services.AddScoped<IReadRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>();
        builder.Services.AddScoped<IRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>();
        builder.Services.AddScoped<IReadRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>();
        
        // --- Branch & Staff Management ---
        builder.Services.AddScoped<IRepository<Branch>, MicroFinanceRepository<Branch>>();
        builder.Services.AddScoped<IReadRepository<Branch>, MicroFinanceRepository<Branch>>();
        builder.Services.AddScoped<IRepository<BranchTarget>, MicroFinanceRepository<BranchTarget>>();
        builder.Services.AddScoped<IReadRepository<BranchTarget>, MicroFinanceRepository<BranchTarget>>();
        builder.Services.AddScoped<IRepository<CashVault>, MicroFinanceRepository<CashVault>>();
        builder.Services.AddScoped<IReadRepository<CashVault>, MicroFinanceRepository<CashVault>>();
        builder.Services.AddScoped<IRepository<Staff>, MicroFinanceRepository<Staff>>();
        builder.Services.AddScoped<IReadRepository<Staff>, MicroFinanceRepository<Staff>>();
        builder.Services.AddScoped<IRepository<StaffTraining>, MicroFinanceRepository<StaffTraining>>();
        builder.Services.AddScoped<IReadRepository<StaffTraining>, MicroFinanceRepository<StaffTraining>>();
        builder.Services.AddScoped<IRepository<TellerSession>, MicroFinanceRepository<TellerSession>>();
        builder.Services.AddScoped<IReadRepository<TellerSession>, MicroFinanceRepository<TellerSession>>();
        
        // --- Digital Banking & Payments ---
        builder.Services.AddScoped<IRepository<AgentBanking>, MicroFinanceRepository<AgentBanking>>();
        builder.Services.AddScoped<IReadRepository<AgentBanking>, MicroFinanceRepository<AgentBanking>>();
        builder.Services.AddScoped<IRepository<MobileTransaction>, MicroFinanceRepository<MobileTransaction>>();
        builder.Services.AddScoped<IReadRepository<MobileTransaction>, MicroFinanceRepository<MobileTransaction>>();
        builder.Services.AddScoped<IRepository<MobileWallet>, MicroFinanceRepository<MobileWallet>>();
        builder.Services.AddScoped<IReadRepository<MobileWallet>, MicroFinanceRepository<MobileWallet>>();
        builder.Services.AddScoped<IRepository<PaymentGateway>, MicroFinanceRepository<PaymentGateway>>();
        builder.Services.AddScoped<IReadRepository<PaymentGateway>, MicroFinanceRepository<PaymentGateway>>();
        builder.Services.AddScoped<IRepository<QrPayment>, MicroFinanceRepository<QrPayment>>();
        builder.Services.AddScoped<IReadRepository<QrPayment>, MicroFinanceRepository<QrPayment>>();
        builder.Services.AddScoped<IRepository<UssdSession>, MicroFinanceRepository<UssdSession>>();
        builder.Services.AddScoped<IReadRepository<UssdSession>, MicroFinanceRepository<UssdSession>>();
        
        // --- Workflow & Approvals ---
        builder.Services.AddScoped<IRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>();
        builder.Services.AddScoped<IReadRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>();
        builder.Services.AddScoped<IRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>();
        builder.Services.AddScoped<IReadRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>();
        
        // --- Communication & Documents ---
        builder.Services.AddScoped<IRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>();
        builder.Services.AddScoped<IReadRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>();
        builder.Services.AddScoped<IRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>();
        builder.Services.AddScoped<IReadRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>();
        builder.Services.AddScoped<IRepository<Document>, MicroFinanceRepository<Document>>();
        builder.Services.AddScoped<IReadRepository<Document>, MicroFinanceRepository<Document>>();
        builder.Services.AddScoped<IRepository<KycDocument>, MicroFinanceRepository<KycDocument>>();
        builder.Services.AddScoped<IReadRepository<KycDocument>, MicroFinanceRepository<KycDocument>>();
        
        // --- Customer Relationship Management ---
        builder.Services.AddScoped<IRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>();
        builder.Services.AddScoped<IReadRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>();
        builder.Services.AddScoped<IRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>();
        builder.Services.AddScoped<IReadRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>();
        builder.Services.AddScoped<IRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>();
        builder.Services.AddScoped<IReadRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>();
        builder.Services.AddScoped<IRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>();
        builder.Services.AddScoped<IReadRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>();
        
        // --- Reporting & Configuration ---
        builder.Services.AddScoped<IRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>();
        builder.Services.AddScoped<IReadRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>();
        builder.Services.AddScoped<IRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>();
        builder.Services.AddScoped<IReadRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>();
        builder.Services.AddScoped<IRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>();
        builder.Services.AddScoped<IReadRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>();
        
        // ============================================================================
        // KEYED REPOSITORY REGISTRATIONS - ENTITY-SPECIFIC KEYS
        // For handlers using [FromKeyedServices("microfinance:{entity}")]
        // ============================================================================
        
        // --- Member Management ---
        builder.Services.AddKeyedScoped<IRepository<Member>, MicroFinanceRepository<Member>>("microfinance:members");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, MicroFinanceRepository<Member>>("microfinance:members");
        builder.Services.AddKeyedScoped<IRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>("microfinance:membergroups");
        builder.Services.AddKeyedScoped<IReadRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>("microfinance:membergroups");
        builder.Services.AddKeyedScoped<IRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>("microfinance:groupmemberships");
        builder.Services.AddKeyedScoped<IReadRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>("microfinance:groupmemberships");
        
        // --- Loan Management ---
        builder.Services.AddKeyedScoped<IRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance:loans");
        builder.Services.AddKeyedScoped<IReadRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance:loans");
        builder.Services.AddKeyedScoped<IRepository<LoanApplication>, MicroFinanceRepository<LoanApplication>>("microfinance:loanapplications");
        builder.Services.AddKeyedScoped<IReadRepository<LoanApplication>, MicroFinanceRepository<LoanApplication>>("microfinance:loanapplications");
        builder.Services.AddKeyedScoped<IRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>("microfinance:loancollaterals");
        builder.Services.AddKeyedScoped<IReadRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>("microfinance:loancollaterals");
        builder.Services.AddKeyedScoped<IRepository<LoanDisbursementTranche>, MicroFinanceRepository<LoanDisbursementTranche>>("microfinance:loandisbursementtranches");
        builder.Services.AddKeyedScoped<IReadRepository<LoanDisbursementTranche>, MicroFinanceRepository<LoanDisbursementTranche>>("microfinance:loandisbursementtranches");
        builder.Services.AddKeyedScoped<IRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>("microfinance:loanguarantors");
        builder.Services.AddKeyedScoped<IReadRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>("microfinance:loanguarantors");
        builder.Services.AddKeyedScoped<IRepository<LoanOfficerAssignment>, MicroFinanceRepository<LoanOfficerAssignment>>("microfinance:loanofficerassignments");
        builder.Services.AddKeyedScoped<IReadRepository<LoanOfficerAssignment>, MicroFinanceRepository<LoanOfficerAssignment>>("microfinance:loanofficerassignments");
        builder.Services.AddKeyedScoped<IRepository<LoanOfficerTarget>, MicroFinanceRepository<LoanOfficerTarget>>("microfinance:loanofficertargets");
        builder.Services.AddKeyedScoped<IReadRepository<LoanOfficerTarget>, MicroFinanceRepository<LoanOfficerTarget>>("microfinance:loanofficertargets");
        builder.Services.AddKeyedScoped<IRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance:loanproducts");
        builder.Services.AddKeyedScoped<IReadRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance:loanproducts");
        builder.Services.AddKeyedScoped<IRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance:loanrepayments");
        builder.Services.AddKeyedScoped<IReadRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance:loanrepayments");
        builder.Services.AddKeyedScoped<IRepository<LoanRestructure>, MicroFinanceRepository<LoanRestructure>>("microfinance:loanrestructures");
        builder.Services.AddKeyedScoped<IReadRepository<LoanRestructure>, MicroFinanceRepository<LoanRestructure>>("microfinance:loanrestructures");
        builder.Services.AddKeyedScoped<IRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>("microfinance:loanschedules");
        builder.Services.AddKeyedScoped<IReadRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>("microfinance:loanschedules");
        builder.Services.AddKeyedScoped<IRepository<LoanWriteOff>, MicroFinanceRepository<LoanWriteOff>>("microfinance:loanwriteoffs");
        builder.Services.AddKeyedScoped<IReadRepository<LoanWriteOff>, MicroFinanceRepository<LoanWriteOff>>("microfinance:loanwriteoffs");
        builder.Services.AddKeyedScoped<IRepository<InterestRateChange>, MicroFinanceRepository<InterestRateChange>>("microfinance:interestratechanges");
        builder.Services.AddKeyedScoped<IReadRepository<InterestRateChange>, MicroFinanceRepository<InterestRateChange>>("microfinance:interestratechanges");
        
        // --- Savings Management ---
        builder.Services.AddKeyedScoped<IRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>("microfinance:fixeddeposits");
        builder.Services.AddKeyedScoped<IReadRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>("microfinance:fixeddeposits");
        builder.Services.AddKeyedScoped<IRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance:savingsaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance:savingsaccounts");
        builder.Services.AddKeyedScoped<IRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance:savingsproducts");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance:savingsproducts");
        builder.Services.AddKeyedScoped<IRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance:savingstransactions");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance:savingstransactions");
        
        // --- Share Management ---
        builder.Services.AddKeyedScoped<IRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>("microfinance:shareaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>("microfinance:shareaccounts");
        builder.Services.AddKeyedScoped<IRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>("microfinance:shareproducts");
        builder.Services.AddKeyedScoped<IReadRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>("microfinance:shareproducts");
        builder.Services.AddKeyedScoped<IRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>("microfinance:sharetransactions");
        builder.Services.AddKeyedScoped<IReadRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>("microfinance:sharetransactions");
        
        // --- Fee Management ---
        builder.Services.AddKeyedScoped<IRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>("microfinance:feecharges");
        builder.Services.AddKeyedScoped<IReadRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>("microfinance:feecharges");
        builder.Services.AddKeyedScoped<IRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>("microfinance:feedefinitions");
        builder.Services.AddKeyedScoped<IReadRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>("microfinance:feedefinitions");
        builder.Services.AddKeyedScoped<IRepository<FeePayment>, MicroFinanceRepository<FeePayment>>("microfinance:feepayments");
        builder.Services.AddKeyedScoped<IReadRepository<FeePayment>, MicroFinanceRepository<FeePayment>>("microfinance:feepayments");
        builder.Services.AddKeyedScoped<IRepository<FeeWaiver>, MicroFinanceRepository<FeeWaiver>>("microfinance:feewaivers");
        builder.Services.AddKeyedScoped<IReadRepository<FeeWaiver>, MicroFinanceRepository<FeeWaiver>>("microfinance:feewaivers");
        
        // --- Collateral Management ---
        builder.Services.AddKeyedScoped<IRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>("microfinance:collateralinsurances");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>("microfinance:collateralinsurances");
        builder.Services.AddKeyedScoped<IRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>("microfinance:collateralreleases");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>("microfinance:collateralreleases");
        builder.Services.AddKeyedScoped<IRepository<CollateralType>, MicroFinanceRepository<CollateralType>>("microfinance:collateraltypes");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralType>, MicroFinanceRepository<CollateralType>>("microfinance:collateraltypes");
        builder.Services.AddKeyedScoped<IRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>("microfinance:collateralvaluations");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>("microfinance:collateralvaluations");
        
        // --- Collections & Recovery ---
        builder.Services.AddKeyedScoped<IRepository<CollectionAction>, MicroFinanceRepository<CollectionAction>>("microfinance:collectionactions");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionAction>, MicroFinanceRepository<CollectionAction>>("microfinance:collectionactions");
        builder.Services.AddKeyedScoped<IRepository<CollectionCase>, MicroFinanceRepository<CollectionCase>>("microfinance:collectioncases");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionCase>, MicroFinanceRepository<CollectionCase>>("microfinance:collectioncases");
        builder.Services.AddKeyedScoped<IRepository<CollectionStrategy>, MicroFinanceRepository<CollectionStrategy>>("microfinance:collectionstrategies");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionStrategy>, MicroFinanceRepository<CollectionStrategy>>("microfinance:collectionstrategies");
        builder.Services.AddKeyedScoped<IRepository<DebtSettlement>, MicroFinanceRepository<DebtSettlement>>("microfinance:debtsettlements");
        builder.Services.AddKeyedScoped<IReadRepository<DebtSettlement>, MicroFinanceRepository<DebtSettlement>>("microfinance:debtsettlements");
        builder.Services.AddKeyedScoped<IRepository<LegalAction>, MicroFinanceRepository<LegalAction>>("microfinance:legalactions");
        builder.Services.AddKeyedScoped<IReadRepository<LegalAction>, MicroFinanceRepository<LegalAction>>("microfinance:legalactions");
        builder.Services.AddKeyedScoped<IRepository<PromiseToPay>, MicroFinanceRepository<PromiseToPay>>("microfinance:promisetopays");
        builder.Services.AddKeyedScoped<IReadRepository<PromiseToPay>, MicroFinanceRepository<PromiseToPay>>("microfinance:promisetopays");
        
        // --- Risk Management & Credit ---
        builder.Services.AddKeyedScoped<IRepository<AmlAlert>, MicroFinanceRepository<AmlAlert>>("microfinance:amlalerts");
        builder.Services.AddKeyedScoped<IReadRepository<AmlAlert>, MicroFinanceRepository<AmlAlert>>("microfinance:amlalerts");
        builder.Services.AddKeyedScoped<IRepository<CreditBureauInquiry>, MicroFinanceRepository<CreditBureauInquiry>>("microfinance:creditbureauinquiries");
        builder.Services.AddKeyedScoped<IReadRepository<CreditBureauInquiry>, MicroFinanceRepository<CreditBureauInquiry>>("microfinance:creditbureauinquiries");
        builder.Services.AddKeyedScoped<IRepository<CreditBureauReport>, MicroFinanceRepository<CreditBureauReport>>("microfinance:creditbureaureports");
        builder.Services.AddKeyedScoped<IReadRepository<CreditBureauReport>, MicroFinanceRepository<CreditBureauReport>>("microfinance:creditbureaureports");
        builder.Services.AddKeyedScoped<IRepository<CreditScore>, MicroFinanceRepository<CreditScore>>("microfinance:creditscores");
        builder.Services.AddKeyedScoped<IReadRepository<CreditScore>, MicroFinanceRepository<CreditScore>>("microfinance:creditscores");
        builder.Services.AddKeyedScoped<IRepository<RiskAlert>, MicroFinanceRepository<RiskAlert>>("microfinance:riskalerts");
        builder.Services.AddKeyedScoped<IReadRepository<RiskAlert>, MicroFinanceRepository<RiskAlert>>("microfinance:riskalerts");
        builder.Services.AddKeyedScoped<IRepository<RiskCategory>, MicroFinanceRepository<RiskCategory>>("microfinance:riskcategories");
        builder.Services.AddKeyedScoped<IReadRepository<RiskCategory>, MicroFinanceRepository<RiskCategory>>("microfinance:riskcategories");
        builder.Services.AddKeyedScoped<IRepository<RiskIndicator>, MicroFinanceRepository<RiskIndicator>>("microfinance:riskindicators");
        builder.Services.AddKeyedScoped<IReadRepository<RiskIndicator>, MicroFinanceRepository<RiskIndicator>>("microfinance:riskindicators");
        
        // --- Insurance Management ---
        builder.Services.AddKeyedScoped<IRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>("microfinance:insuranceclaims");
        builder.Services.AddKeyedScoped<IReadRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>("microfinance:insuranceclaims");
        builder.Services.AddKeyedScoped<IRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>("microfinance:insurancepolicies");
        builder.Services.AddKeyedScoped<IReadRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>("microfinance:insurancepolicies");
        builder.Services.AddKeyedScoped<IRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>("microfinance:insuranceproducts");
        builder.Services.AddKeyedScoped<IReadRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>("microfinance:insuranceproducts");
        
        // --- Investment Management ---
        builder.Services.AddKeyedScoped<IRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>("microfinance:investmentaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>("microfinance:investmentaccounts");
        builder.Services.AddKeyedScoped<IRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>("microfinance:investmentproducts");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>("microfinance:investmentproducts");
        builder.Services.AddKeyedScoped<IRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>("microfinance:investmenttransactions");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>("microfinance:investmenttransactions");
        
        // --- Branch & Staff Management ---
        builder.Services.AddKeyedScoped<IRepository<Branch>, MicroFinanceRepository<Branch>>("microfinance:branches");
        builder.Services.AddKeyedScoped<IReadRepository<Branch>, MicroFinanceRepository<Branch>>("microfinance:branches");
        builder.Services.AddKeyedScoped<IRepository<BranchTarget>, MicroFinanceRepository<BranchTarget>>("microfinance:branchtargets");
        builder.Services.AddKeyedScoped<IReadRepository<BranchTarget>, MicroFinanceRepository<BranchTarget>>("microfinance:branchtargets");
        builder.Services.AddKeyedScoped<IRepository<CashVault>, MicroFinanceRepository<CashVault>>("microfinance:cashvaults");
        builder.Services.AddKeyedScoped<IReadRepository<CashVault>, MicroFinanceRepository<CashVault>>("microfinance:cashvaults");
        builder.Services.AddKeyedScoped<IRepository<Staff>, MicroFinanceRepository<Staff>>("microfinance:staff");
        builder.Services.AddKeyedScoped<IReadRepository<Staff>, MicroFinanceRepository<Staff>>("microfinance:staff");
        builder.Services.AddKeyedScoped<IRepository<StaffTraining>, MicroFinanceRepository<StaffTraining>>("microfinance:stafftrainings");
        builder.Services.AddKeyedScoped<IReadRepository<StaffTraining>, MicroFinanceRepository<StaffTraining>>("microfinance:stafftrainings");
        builder.Services.AddKeyedScoped<IRepository<TellerSession>, MicroFinanceRepository<TellerSession>>("microfinance:tellersessions");
        builder.Services.AddKeyedScoped<IReadRepository<TellerSession>, MicroFinanceRepository<TellerSession>>("microfinance:tellersessions");
        
        // --- Digital Banking & Payments ---
        builder.Services.AddKeyedScoped<IRepository<AgentBanking>, MicroFinanceRepository<AgentBanking>>("microfinance:agentbankings");
        builder.Services.AddKeyedScoped<IReadRepository<AgentBanking>, MicroFinanceRepository<AgentBanking>>("microfinance:agentbankings");
        builder.Services.AddKeyedScoped<IRepository<MobileTransaction>, MicroFinanceRepository<MobileTransaction>>("microfinance:mobiletransactions");
        builder.Services.AddKeyedScoped<IReadRepository<MobileTransaction>, MicroFinanceRepository<MobileTransaction>>("microfinance:mobiletransactions");
        builder.Services.AddKeyedScoped<IRepository<MobileWallet>, MicroFinanceRepository<MobileWallet>>("microfinance:mobilewallets");
        builder.Services.AddKeyedScoped<IReadRepository<MobileWallet>, MicroFinanceRepository<MobileWallet>>("microfinance:mobilewallets");
        builder.Services.AddKeyedScoped<IRepository<PaymentGateway>, MicroFinanceRepository<PaymentGateway>>("microfinance:paymentgateways");
        builder.Services.AddKeyedScoped<IReadRepository<PaymentGateway>, MicroFinanceRepository<PaymentGateway>>("microfinance:paymentgateways");
        builder.Services.AddKeyedScoped<IRepository<QrPayment>, MicroFinanceRepository<QrPayment>>("microfinance:qrpayments");
        builder.Services.AddKeyedScoped<IReadRepository<QrPayment>, MicroFinanceRepository<QrPayment>>("microfinance:qrpayments");
        builder.Services.AddKeyedScoped<IRepository<UssdSession>, MicroFinanceRepository<UssdSession>>("microfinance:ussdsessions");
        builder.Services.AddKeyedScoped<IReadRepository<UssdSession>, MicroFinanceRepository<UssdSession>>("microfinance:ussdsessions");
        
        // --- Workflow & Approvals ---
        builder.Services.AddKeyedScoped<IRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>("microfinance:approvalrequests");
        builder.Services.AddKeyedScoped<IReadRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>("microfinance:approvalrequests");
        builder.Services.AddKeyedScoped<IRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>("microfinance:approvalworkflows");
        builder.Services.AddKeyedScoped<IReadRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>("microfinance:approvalworkflows");
        
        // --- Communication & Documents ---
        builder.Services.AddKeyedScoped<IRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>("microfinance:communicationlogs");
        builder.Services.AddKeyedScoped<IReadRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>("microfinance:communicationlogs");
        builder.Services.AddKeyedScoped<IRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>("microfinance:communicationtemplates");
        builder.Services.AddKeyedScoped<IReadRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>("microfinance:communicationtemplates");
        builder.Services.AddKeyedScoped<IRepository<Document>, MicroFinanceRepository<Document>>("microfinance:documents");
        builder.Services.AddKeyedScoped<IReadRepository<Document>, MicroFinanceRepository<Document>>("microfinance:documents");
        builder.Services.AddKeyedScoped<IRepository<KycDocument>, MicroFinanceRepository<KycDocument>>("microfinance:kycdocuments");
        builder.Services.AddKeyedScoped<IReadRepository<KycDocument>, MicroFinanceRepository<KycDocument>>("microfinance:kycdocuments");
        
        // --- Customer Relationship Management ---
        builder.Services.AddKeyedScoped<IRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>("microfinance:customercases");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>("microfinance:customercases");
        builder.Services.AddKeyedScoped<IRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>("microfinance:customersegments");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>("microfinance:customersegments");
        builder.Services.AddKeyedScoped<IRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>("microfinance:customersurveys");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>("microfinance:customersurveys");
        builder.Services.AddKeyedScoped<IRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>("microfinance:marketingcampaigns");
        builder.Services.AddKeyedScoped<IReadRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>("microfinance:marketingcampaigns");
        
        // --- Reporting & Configuration ---
        builder.Services.AddKeyedScoped<IRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>("microfinance:mficonfigurations");
        builder.Services.AddKeyedScoped<IReadRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>("microfinance:mficonfigurations");
        builder.Services.AddKeyedScoped<IRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>("microfinance:reportdefinitions");
        builder.Services.AddKeyedScoped<IReadRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>("microfinance:reportdefinitions");
        builder.Services.AddKeyedScoped<IRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>("microfinance:reportgenerations");
        builder.Services.AddKeyedScoped<IReadRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>("microfinance:reportgenerations");
        
        return builder;
    }

    /// <summary>
    /// Applies the microfinance module to the web application.
    /// </summary>
    public static WebApplication UseMicroFinanceModule(this WebApplication app)
    {
        return app;
    }
}
