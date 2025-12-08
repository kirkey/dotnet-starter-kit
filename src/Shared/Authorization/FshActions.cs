namespace Shared.Authorization;
public static class FshActions
{
    // Standard CRUD Actions
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Import = nameof(Import);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);

    // HR & Employee Actions
    public const string Regularize = nameof(Regularize);
    public const string Terminate = nameof(Terminate);
    public const string Assign = nameof(Assign);
    public const string Manage = nameof(Manage);
    
    // Workflow & Approval Actions
    public const string Approve = nameof(Approve);
    public const string Reject = nameof(Reject);
    public const string Submit = nameof(Submit);
    public const string Process = nameof(Process);
    public const string Complete = nameof(Complete);
    public const string Cancel = nameof(Cancel);
    public const string Void = nameof(Void);
    public const string Post = nameof(Post);
    public const string Send = nameof(Send);
    public const string Receive = nameof(Receive);
    public const string Acknowledge = nameof(Acknowledge);
    
    // Financial Actions
    public const string MarkAsPaid = nameof(MarkAsPaid);
    public const string Accrue = nameof(Accrue);
    public const string Disburse = nameof(Disburse);
    public const string Deposit = nameof(Deposit);
    public const string Withdraw = nameof(Withdraw);
    public const string Transfer = nameof(Transfer);
    public const string WriteOff = nameof(WriteOff);
    public const string Mature = nameof(Mature);
    
    // Status Actions
    public const string Activate = nameof(Activate);
    public const string Deactivate = nameof(Deactivate);
    public const string Suspend = nameof(Suspend);
    public const string Close = nameof(Close);
    public const string Freeze = nameof(Freeze);
    public const string Unfreeze = nameof(Unfreeze);
    public const string Renew = nameof(Renew);
    public const string Return = nameof(Return);
    
    // MicroFinance - Collections & Recovery Actions
    public const string Escalate = nameof(Escalate);
    public const string MarkBroken = nameof(MarkBroken);
    public const string RecordPayment = nameof(RecordPayment);
    
    // MicroFinance - AML & Compliance Actions
    public const string FileSar = nameof(FileSar);
    public const string Confirm = nameof(Confirm);
    public const string Clear = nameof(Clear);
    
    // MicroFinance - Insurance Actions
    public const string RecordPremium = nameof(RecordPremium);
    
    // MicroFinance - Investment Actions
    public const string Invest = nameof(Invest);
    public const string Redeem = nameof(Redeem);
    public const string SetupSip = nameof(SetupSip);
    
    // MicroFinance - Agent Banking Actions
    public const string RecordAudit = nameof(RecordAudit);
    public const string UpgradeTier = nameof(UpgradeTier);
    public const string CreditFloat = nameof(CreditFloat);
    public const string DebitFloat = nameof(DebitFloat);
    
    // MicroFinance - Loan Actions
    public const string ApplyPayment = nameof(ApplyPayment);
    public const string Restructure = nameof(Restructure);
}
