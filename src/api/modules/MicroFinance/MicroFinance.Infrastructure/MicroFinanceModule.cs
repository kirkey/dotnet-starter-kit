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
    /// This method is kept for backward compatibility but can be empty.
    /// </summary>
    /// <param name="app">The endpoint route builder to configure.</param>
    /// <returns>The configured endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapMicroFinanceEndpoints(this IEndpointRouteBuilder app)
    {
        // All microfinance endpoints are now auto-discovered by Carter via ICarterModule implementations.
        // No manual endpoint mapping is required.
        // Individual endpoint classes implement ICarterModule and are automatically registered.
        return app;
    }

    /// <summary>
    /// Registers microfinance services with dependency injection container.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The configured web application builder.</returns>
    /// <remarks>
    /// This method registers all microfinance repositories and services in a highly organized structure:
    /// 
    /// 1. CORE SERVICES - DbContext, initializers, and business services
    /// 
    /// 2. NON-KEYED REPOSITORIES - Standard DI registrations (72 aggregate root entities)
    ///    Used by MediatR handlers that don't use [FromKeyedServices] attribute
    /// 
    /// 3. KEYED REPOSITORIES - Keyed DI registrations with "microfinance" key
    ///    For handlers that use [FromKeyedServices("microfinance")]
    ///    
    /// Entity Groups:
    /// - Member Management: Member, MemberGroup, GroupMembership
    /// - Loan Management: Loan, LoanProduct, LoanApplication, LoanRepayment, LoanSchedule, etc.
    /// - Savings Management: SavingsProduct, SavingsAccount, SavingsTransaction, FixedDeposit
    /// - Share Management: ShareProduct, ShareAccount, ShareTransaction
    /// - Fee Management: FeeDefinition, FeeCharge
    /// - Collateral Management: CollateralType, CollateralValuation, CollateralInsurance, CollateralRelease
    /// - Collections &amp; Recovery: CollectionCase, CollectionAction, CollectionStrategy, etc.
    /// - Risk Management: RiskCategory, RiskIndicator, RiskAlert, CreditScore, etc.
    /// - Insurance: InsuranceProduct, InsurancePolicy, InsuranceClaim
    /// - Investment: InvestmentProduct, InvestmentAccount, InvestmentTransaction
    /// - Branch &amp; Staff: Branch, BranchTarget, Staff, StaffTraining, TellerSession, CashVault
    /// - Digital Banking: MobileWallet, MobileTransaction, AgentBanking, PaymentGateway, QrPayment, UssdSession
    /// - Workflow &amp; Approvals: ApprovalWorkflow, ApprovalRequest
    /// - Communication &amp; Documents: Document, KycDocument, CommunicationTemplate, CommunicationLog
    /// - Customer Relationship: CustomerCase, CustomerSegment, CustomerSurvey, MarketingCampaign
    /// - Reporting &amp; Configuration: ReportDefinition, ReportGeneration, MfiConfiguration
    /// 
    /// Total: ~300+ repository registrations supporting 72 aggregate root entities
    /// </remarks>
    public static WebApplicationBuilder RegisterMicroFinanceServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        // ============================================================================
        // CORE SERVICES
        // ============================================================================
        builder.Services.BindDbContext<MicroFinanceDbContext>();
        builder.Services.AddScoped<IDbInitializer, MicroFinanceDbInitializer>();
    
        // ============================================================================
        // NON-KEYED REPOSITORY REGISTRATIONS (for MediatR handlers without keyed services)
        // Organized by functional area - 72 aggregate root entities total
        // ============================================================================
        
        // ----------------------------------------------------------------------------
        // MEMBER MANAGEMENT (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<Member>, MicroFinanceRepository<Member>>();
        builder.Services.AddScoped<IReadRepository<Member>, MicroFinanceRepository<Member>>();
        builder.Services.AddScoped<IRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>();
        builder.Services.AddScoped<IReadRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>();
        builder.Services.AddScoped<IRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>();
        builder.Services.AddScoped<IReadRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>();
        
        // ----------------------------------------------------------------------------
        // LOAN MANAGEMENT (12 entities)
        // ----------------------------------------------------------------------------
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
        
        // ----------------------------------------------------------------------------
        // SAVINGS MANAGEMENT (4 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>();
        builder.Services.AddScoped<IReadRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>();
        builder.Services.AddScoped<IRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>();
        builder.Services.AddScoped<IReadRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>();
        builder.Services.AddScoped<IRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>();
        builder.Services.AddScoped<IReadRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>();
        builder.Services.AddScoped<IRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>();
        builder.Services.AddScoped<IReadRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>();
        
        // ----------------------------------------------------------------------------
        // SHARE MANAGEMENT (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>();
        builder.Services.AddScoped<IReadRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>();
        builder.Services.AddScoped<IRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>();
        builder.Services.AddScoped<IReadRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>();
        builder.Services.AddScoped<IRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>();
        builder.Services.AddScoped<IReadRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>();
        
        // ----------------------------------------------------------------------------
        // FEE MANAGEMENT (2 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>();
        builder.Services.AddScoped<IReadRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>();
        builder.Services.AddScoped<IRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>();
        builder.Services.AddScoped<IReadRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>();
        
        // ----------------------------------------------------------------------------
        // COLLATERAL MANAGEMENT (4 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>();
        builder.Services.AddScoped<IReadRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>();
        builder.Services.AddScoped<IRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>();
        builder.Services.AddScoped<IReadRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>();
        builder.Services.AddScoped<IRepository<CollateralType>, MicroFinanceRepository<CollateralType>>();
        builder.Services.AddScoped<IReadRepository<CollateralType>, MicroFinanceRepository<CollateralType>>();
        builder.Services.AddScoped<IRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>();
        builder.Services.AddScoped<IReadRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>();
        
        // ----------------------------------------------------------------------------
        // COLLECTIONS & RECOVERY (6 entities)
        // ----------------------------------------------------------------------------
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
        
        // ----------------------------------------------------------------------------
        // RISK MANAGEMENT & CREDIT (7 entities)
        // ----------------------------------------------------------------------------
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
        
        // ----------------------------------------------------------------------------
        // INSURANCE MANAGEMENT (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>();
        builder.Services.AddScoped<IReadRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>();
        builder.Services.AddScoped<IRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>();
        builder.Services.AddScoped<IReadRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>();
        builder.Services.AddScoped<IRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>();
        builder.Services.AddScoped<IReadRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>();
        
        // ----------------------------------------------------------------------------
        // INVESTMENT MANAGEMENT (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>();
        builder.Services.AddScoped<IReadRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>();
        builder.Services.AddScoped<IRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>();
        builder.Services.AddScoped<IReadRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>();
        builder.Services.AddScoped<IRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>();
        builder.Services.AddScoped<IReadRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>();
        
        // ----------------------------------------------------------------------------
        // BRANCH & STAFF MANAGEMENT (6 entities)
        // ----------------------------------------------------------------------------
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
        
        // ----------------------------------------------------------------------------
        // DIGITAL BANKING & PAYMENTS (6 entities)
        // ----------------------------------------------------------------------------
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
        
        // ----------------------------------------------------------------------------
        // WORKFLOW & APPROVALS (2 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>();
        builder.Services.AddScoped<IReadRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>();
        builder.Services.AddScoped<IRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>();
        builder.Services.AddScoped<IReadRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>();
        
        // ----------------------------------------------------------------------------
        // COMMUNICATION & DOCUMENTS (4 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>();
        builder.Services.AddScoped<IReadRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>();
        builder.Services.AddScoped<IRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>();
        builder.Services.AddScoped<IReadRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>();
        builder.Services.AddScoped<IRepository<Document>, MicroFinanceRepository<Document>>();
        builder.Services.AddScoped<IReadRepository<Document>, MicroFinanceRepository<Document>>();
        builder.Services.AddScoped<IRepository<KycDocument>, MicroFinanceRepository<KycDocument>>();
        builder.Services.AddScoped<IReadRepository<KycDocument>, MicroFinanceRepository<KycDocument>>();
        
        // ----------------------------------------------------------------------------
        // CUSTOMER RELATIONSHIP MANAGEMENT (4 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>();
        builder.Services.AddScoped<IReadRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>();
        builder.Services.AddScoped<IRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>();
        builder.Services.AddScoped<IReadRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>();
        builder.Services.AddScoped<IRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>();
        builder.Services.AddScoped<IReadRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>();
        builder.Services.AddScoped<IRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>();
        builder.Services.AddScoped<IReadRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>();
        
        // ----------------------------------------------------------------------------
        // REPORTING & CONFIGURATION (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddScoped<IRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>();
        builder.Services.AddScoped<IReadRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>();
        builder.Services.AddScoped<IRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>();
        builder.Services.AddScoped<IReadRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>();
        builder.Services.AddScoped<IRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>();
        builder.Services.AddScoped<IReadRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>();
        
        // ============================================================================
        // KEYED REPOSITORY REGISTRATIONS - GENERIC "microfinance" KEY
        // For handlers that use [FromKeyedServices("microfinance")]
        // Organized by functional area - 72 aggregate root entities total
        // ============================================================================
        
        // ----------------------------------------------------------------------------
        // MEMBER MANAGEMENT (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<Member>, MicroFinanceRepository<Member>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, MicroFinanceRepository<Member>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // LOAN MANAGEMENT (12 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanApplication>, MicroFinanceRepository<LoanApplication>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanApplication>, MicroFinanceRepository<LoanApplication>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanDisbursementTranche>, MicroFinanceRepository<LoanDisbursementTranche>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanDisbursementTranche>, MicroFinanceRepository<LoanDisbursementTranche>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanOfficerAssignment>, MicroFinanceRepository<LoanOfficerAssignment>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanOfficerAssignment>, MicroFinanceRepository<LoanOfficerAssignment>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanOfficerTarget>, MicroFinanceRepository<LoanOfficerTarget>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanOfficerTarget>, MicroFinanceRepository<LoanOfficerTarget>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanRestructure>, MicroFinanceRepository<LoanRestructure>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanRestructure>, MicroFinanceRepository<LoanRestructure>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LoanWriteOff>, MicroFinanceRepository<LoanWriteOff>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LoanWriteOff>, MicroFinanceRepository<LoanWriteOff>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // SAVINGS MANAGEMENT (4 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // SHARE MANAGEMENT (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // FEE MANAGEMENT (2 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // COLLATERAL MANAGEMENT (4 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralInsurance>, MicroFinanceRepository<CollateralInsurance>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralRelease>, MicroFinanceRepository<CollateralRelease>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CollateralType>, MicroFinanceRepository<CollateralType>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralType>, MicroFinanceRepository<CollateralType>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CollateralValuation>, MicroFinanceRepository<CollateralValuation>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // COLLECTIONS & RECOVERY (6 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<CollectionAction>, MicroFinanceRepository<CollectionAction>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionAction>, MicroFinanceRepository<CollectionAction>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CollectionCase>, MicroFinanceRepository<CollectionCase>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionCase>, MicroFinanceRepository<CollectionCase>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CollectionStrategy>, MicroFinanceRepository<CollectionStrategy>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CollectionStrategy>, MicroFinanceRepository<CollectionStrategy>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<DebtSettlement>, MicroFinanceRepository<DebtSettlement>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<DebtSettlement>, MicroFinanceRepository<DebtSettlement>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<LegalAction>, MicroFinanceRepository<LegalAction>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<LegalAction>, MicroFinanceRepository<LegalAction>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<PromiseToPay>, MicroFinanceRepository<PromiseToPay>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<PromiseToPay>, MicroFinanceRepository<PromiseToPay>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // RISK MANAGEMENT & CREDIT (7 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<AmlAlert>, MicroFinanceRepository<AmlAlert>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<AmlAlert>, MicroFinanceRepository<AmlAlert>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CreditBureauInquiry>, MicroFinanceRepository<CreditBureauInquiry>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CreditBureauInquiry>, MicroFinanceRepository<CreditBureauInquiry>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CreditBureauReport>, MicroFinanceRepository<CreditBureauReport>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CreditBureauReport>, MicroFinanceRepository<CreditBureauReport>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CreditScore>, MicroFinanceRepository<CreditScore>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CreditScore>, MicroFinanceRepository<CreditScore>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<RiskAlert>, MicroFinanceRepository<RiskAlert>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<RiskAlert>, MicroFinanceRepository<RiskAlert>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<RiskCategory>, MicroFinanceRepository<RiskCategory>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<RiskCategory>, MicroFinanceRepository<RiskCategory>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<RiskIndicator>, MicroFinanceRepository<RiskIndicator>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<RiskIndicator>, MicroFinanceRepository<RiskIndicator>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // INSURANCE MANAGEMENT (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<InsuranceClaim>, MicroFinanceRepository<InsuranceClaim>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<InsurancePolicy>, MicroFinanceRepository<InsurancePolicy>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<InsuranceProduct>, MicroFinanceRepository<InsuranceProduct>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // INVESTMENT MANAGEMENT (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentAccount>, MicroFinanceRepository<InvestmentAccount>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentProduct>, MicroFinanceRepository<InvestmentProduct>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<InvestmentTransaction>, MicroFinanceRepository<InvestmentTransaction>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // BRANCH & STAFF MANAGEMENT (6 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<Branch>, MicroFinanceRepository<Branch>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<Branch>, MicroFinanceRepository<Branch>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<BranchTarget>, MicroFinanceRepository<BranchTarget>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<BranchTarget>, MicroFinanceRepository<BranchTarget>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CashVault>, MicroFinanceRepository<CashVault>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CashVault>, MicroFinanceRepository<CashVault>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<Staff>, MicroFinanceRepository<Staff>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<Staff>, MicroFinanceRepository<Staff>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<StaffTraining>, MicroFinanceRepository<StaffTraining>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<StaffTraining>, MicroFinanceRepository<StaffTraining>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<TellerSession>, MicroFinanceRepository<TellerSession>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<TellerSession>, MicroFinanceRepository<TellerSession>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // DIGITAL BANKING & PAYMENTS (6 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<AgentBanking>, MicroFinanceRepository<AgentBanking>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<AgentBanking>, MicroFinanceRepository<AgentBanking>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<MobileTransaction>, MicroFinanceRepository<MobileTransaction>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<MobileTransaction>, MicroFinanceRepository<MobileTransaction>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<MobileWallet>, MicroFinanceRepository<MobileWallet>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<MobileWallet>, MicroFinanceRepository<MobileWallet>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<PaymentGateway>, MicroFinanceRepository<PaymentGateway>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<PaymentGateway>, MicroFinanceRepository<PaymentGateway>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<QrPayment>, MicroFinanceRepository<QrPayment>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<QrPayment>, MicroFinanceRepository<QrPayment>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<UssdSession>, MicroFinanceRepository<UssdSession>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<UssdSession>, MicroFinanceRepository<UssdSession>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // WORKFLOW & APPROVALS (2 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<ApprovalRequest>, MicroFinanceRepository<ApprovalRequest>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<ApprovalWorkflow>, MicroFinanceRepository<ApprovalWorkflow>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // COMMUNICATION & DOCUMENTS (4 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CommunicationLog>, MicroFinanceRepository<CommunicationLog>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CommunicationTemplate>, MicroFinanceRepository<CommunicationTemplate>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<Document>, MicroFinanceRepository<Document>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<Document>, MicroFinanceRepository<Document>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<KycDocument>, MicroFinanceRepository<KycDocument>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<KycDocument>, MicroFinanceRepository<KycDocument>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // CUSTOMER RELATIONSHIP MANAGEMENT (4 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerCase>, MicroFinanceRepository<CustomerCase>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerSegment>, MicroFinanceRepository<CustomerSegment>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<CustomerSurvey>, MicroFinanceRepository<CustomerSurvey>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<MarketingCampaign>, MicroFinanceRepository<MarketingCampaign>>("microfinance");
        
        // ----------------------------------------------------------------------------
        // REPORTING & CONFIGURATION (3 entities)
        // ----------------------------------------------------------------------------
        builder.Services.AddKeyedScoped<IRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<MfiConfiguration>, MicroFinanceRepository<MfiConfiguration>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<ReportDefinition>, MicroFinanceRepository<ReportDefinition>>("microfinance");
        builder.Services.AddKeyedScoped<IRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>("microfinance");
        builder.Services.AddKeyedScoped<IReadRepository<ReportGeneration>, MicroFinanceRepository<ReportGeneration>>("microfinance");
        
        return builder;
    }

    /// <summary>
    /// Applies the microfinance module to the web application.
    /// Currently a no-op placeholder for future middleware or configuration.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication UseMicroFinanceModule(this WebApplication app)
    {
        return app;
    }
}
