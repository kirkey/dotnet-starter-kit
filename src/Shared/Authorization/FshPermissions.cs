﻿using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class FshPermissions
{
    private static readonly FshPermission[] AllPermissions =
    [     
        //tenants
        new("View Tenants", FshActions.View, FshResources.Tenants, IsRoot: true),
        new("Create Tenants", FshActions.Create, FshResources.Tenants, IsRoot: true),
        new("Update Tenants", FshActions.Update, FshResources.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", FshActions.UpgradeSubscription, FshResources.Tenants, IsRoot: true),

        //identity
        new("View Users", FshActions.View, FshResources.Users),
        new("Search Users", FshActions.Search, FshResources.Users),
        new("Create Users", FshActions.Create, FshResources.Users),
        new("Update Users", FshActions.Update, FshResources.Users),
        new("Delete Users", FshActions.Delete, FshResources.Users),
        new("Export Users", FshActions.Export, FshResources.Users),
        new("View UserRoles", FshActions.View, FshResources.UserRoles),
        new("Update UserRoles", FshActions.Update, FshResources.UserRoles),
        new("View Roles", FshActions.View, FshResources.Roles),
        new("Create Roles", FshActions.Create, FshResources.Roles),
        new("Update Roles", FshActions.Update, FshResources.Roles),
        new("Delete Roles", FshActions.Delete, FshResources.Roles),
        new("View RoleClaims", FshActions.View, FshResources.RoleClaims),
        new("Update RoleClaims", FshActions.Update, FshResources.RoleClaims),
        
        //products
        new("View Products", FshActions.View, FshResources.Products, IsBasic: true),
        new("Search Products", FshActions.Search, FshResources.Products, IsBasic: true),
        new("Create Products", FshActions.Create, FshResources.Products),
        new("Update Products", FshActions.Update, FshResources.Products),
        new("Delete Products", FshActions.Delete, FshResources.Products),
        new("Import Products", FshActions.Import, FshResources.Products),
        new("Export Products", FshActions.Export, FshResources.Products),

        //brands
        new("View Brands", FshActions.View, FshResources.Brands, IsBasic: true),
        new("Search Brands", FshActions.Search, FshResources.Brands, IsBasic: true),
        new("Create Brands", FshActions.Create, FshResources.Brands),
        new("Update Brands", FshActions.Update, FshResources.Brands),
        new("Delete Brands", FshActions.Delete, FshResources.Brands),
        new("Import Brands", FshActions.Import, FshResources.Brands),
        new("Export Brands", FshActions.Export, FshResources.Brands),

        //todos
        new("View Todos", FshActions.View, FshResources.Todos, IsBasic: true),
        new("Search Todos", FshActions.Search, FshResources.Todos, IsBasic: true),
        new("Create Todos", FshActions.Create, FshResources.Todos),
        new("Update Todos", FshActions.Update, FshResources.Todos),
        new("Delete Todos", FshActions.Delete, FshResources.Todos),
        new("Export Todos", FshActions.Export, FshResources.Todos),

         new("View Hangfire", FshActions.View, FshResources.Hangfire),
         new("View Dashboard", FshActions.View, FshResources.Dashboard),
         new("View Analytics", FshActions.View, FshResources.Analytics),

        //audit
        new("View Audit Trails", FshActions.View, FshResources.AuditTrails),
        
        //Accounting
        new("View Accounting", FshActions.View, FshResources.Accounting),
        new("Search Accounting", FshActions.Search, FshResources.Accounting),
        new("Create Accounting", FshActions.Create, FshResources.Accounting),
        new("Update Accounting", FshActions.Update, FshResources.Accounting),
        new("Delete Accounting", FshActions.Delete, FshResources.Accounting),
        new("Import Accounting", FshActions.Import, FshResources.Accounting),
        new("Export Accounting", FshActions.Export, FshResources.Accounting),
        new("Approve Accounting", FshActions.Approve, FshResources.Accounting),
        new("Reject Accounting", FshActions.Reject, FshResources.Accounting),
        new("Post Accounting", FshActions.Post, FshResources.Accounting),
        new("Void Accounting", FshActions.Void, FshResources.Accounting),
        new("Cancel Accounting", FshActions.Cancel, FshResources.Accounting),
        new("Send Accounting", FshActions.Send, FshResources.Accounting),
        new("Process Accounting", FshActions.Process, FshResources.Accounting),
        new("Complete Accounting", FshActions.Complete, FshResources.Accounting),

        //Store
        new("View Store", FshActions.View, FshResources.Store),
        new("Search Store", FshActions.Search, FshResources.Store),
        new("Create Store", FshActions.Create, FshResources.Store),
        new("Update Store", FshActions.Update, FshResources.Store),
        new("Delete Store", FshActions.Delete, FshResources.Store),
        new("Import Store", FshActions.Import, FshResources.Store),
        new("Export Store", FshActions.Export, FshResources.Store),
        
        //Warehouse
        new("View Warehouse", FshActions.View, FshResources.Warehouse),
        new("Search Warehouse", FshActions.Search, FshResources.Warehouse),
        new("Create Warehouse", FshActions.Create, FshResources.Warehouse),
        new("Update Warehouse", FshActions.Update, FshResources.Warehouse),
        new("Delete Warehouse", FshActions.Delete, FshResources.Warehouse),
        new("Import Warehouse", FshActions.Import, FshResources.Warehouse),
        new("Export Warehouse", FshActions.Export, FshResources.Warehouse),

        //Messaging
        new("View Messaging", FshActions.View, FshResources.Messaging, IsBasic: true),
        new("Search Messaging", FshActions.Search, FshResources.Messaging, IsBasic: true),
        new("Create Messaging", FshActions.Create, FshResources.Messaging, IsBasic: true),
        new("Update Messaging", FshActions.Update, FshResources.Messaging),
        new("Delete Messaging", FshActions.Delete, FshResources.Messaging),

        //Human Resources - Organization & Setup
        new("View Organization", FshActions.View, FshResources.Organization),
        new("Search Organization", FshActions.Search, FshResources.Organization),
        new("Create Organization", FshActions.Create, FshResources.Organization),
        new("Update Organization", FshActions.Update, FshResources.Organization),
        new("Delete Organization", FshActions.Delete, FshResources.Organization),
        new("Import Organization", FshActions.Import, FshResources.Organization),
        new("Export Organization", FshActions.Export, FshResources.Organization),

        //Human Resources - Employees
        new("View Employees", FshActions.View, FshResources.Employees),
        new("Search Employees", FshActions.Search, FshResources.Employees),
        new("Create Employees", FshActions.Create, FshResources.Employees),
        new("Update Employees", FshActions.Update, FshResources.Employees),
        new("Delete Employees", FshActions.Delete, FshResources.Employees),
        new("Import Employees", FshActions.Import, FshResources.Employees),
        new("Export Employees", FshActions.Export, FshResources.Employees),
        new("Manage Employees", FshActions.Manage, FshResources.Employees),
        new("Assign Employees", FshActions.Assign, FshResources.Employees),
        new("Submit Employees", FshActions.Submit, FshResources.Employees),
        new("Complete Employees", FshActions.Complete, FshResources.Employees),

        //Human Resources - Attendance
        new("View Attendance", FshActions.View, FshResources.Attendance),
        new("Search Attendance", FshActions.Search, FshResources.Attendance),
        new("Create Attendance", FshActions.Create, FshResources.Attendance),
        new("Update Attendance", FshActions.Update, FshResources.Attendance),
        new("Delete Attendance", FshActions.Delete, FshResources.Attendance),
        new("Import Attendance", FshActions.Import, FshResources.Attendance),
        new("Export Attendance", FshActions.Export, FshResources.Attendance),

        //Human Resources - Timesheets
        new("View Timesheets", FshActions.View, FshResources.Timesheets),
        new("Search Timesheets", FshActions.Search, FshResources.Timesheets),
        new("Create Timesheets", FshActions.Create, FshResources.Timesheets),
        new("Update Timesheets", FshActions.Update, FshResources.Timesheets),
        new("Delete Timesheets", FshActions.Delete, FshResources.Timesheets),
        new("Import Timesheets", FshActions.Import, FshResources.Timesheets),
        new("Export Timesheets", FshActions.Export, FshResources.Timesheets),

        //Human Resources - Leaves
        new("View Leaves", FshActions.View, FshResources.Leaves),
        new("Search Leaves", FshActions.Search, FshResources.Leaves),
        new("Create Leaves", FshActions.Create, FshResources.Leaves),
        new("Update Leaves", FshActions.Update, FshResources.Leaves),
        new("Delete Leaves", FshActions.Delete, FshResources.Leaves),
        new("Import Leaves", FshActions.Import, FshResources.Leaves),
        new("Export Leaves", FshActions.Export, FshResources.Leaves),
        new("Approve Leaves", FshActions.Approve, FshResources.Leaves),
        new("Reject Leaves", FshActions.Reject, FshResources.Leaves),
        new("Submit Leaves", FshActions.Submit, FshResources.Leaves),
        new("Cancel Leaves", FshActions.Cancel, FshResources.Leaves),

        //Human Resources - Payroll
        new("View Payroll", FshActions.View, FshResources.Payroll),
        new("Search Payroll", FshActions.Search, FshResources.Payroll),
        new("Create Payroll", FshActions.Create, FshResources.Payroll),
        new("Update Payroll", FshActions.Update, FshResources.Payroll),
        new("Delete Payroll", FshActions.Delete, FshResources.Payroll),
        new("Import Payroll", FshActions.Import, FshResources.Payroll),
        new("Export Payroll", FshActions.Export, FshResources.Payroll),
        new("Process Payroll", FshActions.Process, FshResources.Payroll),

        //Human Resources - Benefits
        new("View Benefits", FshActions.View, FshResources.Benefits),
        new("Search Benefits", FshActions.Search, FshResources.Benefits),
        new("Create Benefits", FshActions.Create, FshResources.Benefits),
        new("Update Benefits", FshActions.Update, FshResources.Benefits),
        new("Delete Benefits", FshActions.Delete, FshResources.Benefits),
        new("Import Benefits", FshActions.Import, FshResources.Benefits),
        new("Export Benefits", FshActions.Export, FshResources.Benefits),
        new("Approve Benefits", FshActions.Approve, FshResources.Benefits),
        new("Reject Benefits", FshActions.Reject, FshResources.Benefits),

        // Employees - special operations
        new("Regularize Employees", FshActions.Regularize, FshResources.Employees),
        new("Terminate Employees", FshActions.Terminate, FshResources.Employees),
        
        // Benefit Enrollments - special operation
        new("Terminate BenefitEnrollments", FshActions.Terminate, FshResources.Benefits),

        //Human Resources - Taxes
        new("View Taxes", FshActions.View, FshResources.Taxes),
        new("Search Taxes", FshActions.Search, FshResources.Taxes),
        new("Create Taxes", FshActions.Create, FshResources.Taxes),
        new("Update Taxes", FshActions.Update, FshResources.Taxes),
        new("Delete Taxes", FshActions.Delete, FshResources.Taxes),
        new("Import Taxes", FshActions.Import, FshResources.Taxes),
        new("Export Taxes", FshActions.Export, FshResources.Taxes),

        //MicroFinance - Generic Module Permission
        new("View MicroFinance", FshActions.View, FshResources.MicroFinance),
        
        //MicroFinance - Organization & Setup
        new("View Branches", FshActions.View, FshResources.Branches),
        new("Search Branches", FshActions.Search, FshResources.Branches),
        new("Create Branches", FshActions.Create, FshResources.Branches),
        new("Update Branches", FshActions.Update, FshResources.Branches),
        new("Delete Branches", FshActions.Delete, FshResources.Branches),
        new("Export Branches", FshActions.Export, FshResources.Branches),
        new("Activate Branches", FshActions.Activate, FshResources.Branches),
        new("Deactivate Branches", FshActions.Deactivate, FshResources.Branches),
        new("Close Branches", FshActions.Close, FshResources.Branches),
        
        new("View BranchTargets", FshActions.View, FshResources.BranchTargets),
        new("Create BranchTargets", FshActions.Create, FshResources.BranchTargets),
        
        new("View MfiConfigurations", FshActions.View, FshResources.MfiConfigurations),
        new("Update MfiConfigurations", FshActions.Update, FshResources.MfiConfigurations),
        
        new("View Staff", FshActions.View, FshResources.Staff),
        new("Search Staff", FshActions.Search, FshResources.Staff),
        new("Create Staff", FshActions.Create, FshResources.Staff),
        new("Update Staff", FshActions.Update, FshResources.Staff),
        new("Delete Staff", FshActions.Delete, FshResources.Staff),
        
        new("View StaffTrainings", FshActions.View, FshResources.StaffTrainings),
        new("Create StaffTrainings", FshActions.Create, FshResources.StaffTrainings),
        new("Complete StaffTrainings", FshActions.Complete, FshResources.StaffTrainings),
        
        //MicroFinance - Member Management
        new("View Members", FshActions.View, FshResources.Members),
        new("Search Members", FshActions.Search, FshResources.Members),
        new("Create Members", FshActions.Create, FshResources.Members),
        new("Update Members", FshActions.Update, FshResources.Members),
        new("Delete Members", FshActions.Delete, FshResources.Members),
        new("Export Members", FshActions.Export, FshResources.Members),
        new("Approve Members", FshActions.Approve, FshResources.Members),
        new("Activate Members", FshActions.Activate, FshResources.Members),
        new("Deactivate Members", FshActions.Deactivate, FshResources.Members),
        
        new("View MemberGroups", FshActions.View, FshResources.MemberGroups),
        new("Search MemberGroups", FshActions.Search, FshResources.MemberGroups),
        new("Create MemberGroups", FshActions.Create, FshResources.MemberGroups),
        new("Update MemberGroups", FshActions.Update, FshResources.MemberGroups),
        new("Delete MemberGroups", FshActions.Delete, FshResources.MemberGroups),
        new("Activate MemberGroups", FshActions.Activate, FshResources.MemberGroups),
        new("Deactivate MemberGroups", FshActions.Deactivate, FshResources.MemberGroups),
        
        new("View GroupMemberships", FshActions.View, FshResources.GroupMemberships),
        new("Create GroupMemberships", FshActions.Create, FshResources.GroupMemberships),
        new("Activate GroupMemberships", FshActions.Activate, FshResources.GroupMemberships),
        new("Terminate GroupMemberships", FshActions.Terminate, FshResources.GroupMemberships),
        
        new("View KycDocuments", FshActions.View, FshResources.KycDocuments),
        new("Create KycDocuments", FshActions.Create, FshResources.KycDocuments),
        
        new("View CustomerSegments", FshActions.View, FshResources.CustomerSegments),
        new("Create CustomerSegments", FshActions.Create, FshResources.CustomerSegments),
        new("Activate CustomerSegments", FshActions.Activate, FshResources.CustomerSegments),
        new("Deactivate CustomerSegments", FshActions.Deactivate, FshResources.CustomerSegments),
        
        new("View CustomerSurveys", FshActions.View, FshResources.CustomerSurveys),
        new("Create CustomerSurveys", FshActions.Create, FshResources.CustomerSurveys),
        new("Activate CustomerSurveys", FshActions.Activate, FshResources.CustomerSurveys),
        new("Complete CustomerSurveys", FshActions.Complete, FshResources.CustomerSurveys),
        
        //MicroFinance - Product Catalog
        new("View LoanProducts", FshActions.View, FshResources.LoanProducts),
        new("Search LoanProducts", FshActions.Search, FshResources.LoanProducts),
        new("Create LoanProducts", FshActions.Create, FshResources.LoanProducts),
        new("Update LoanProducts", FshActions.Update, FshResources.LoanProducts),
        new("Delete LoanProducts", FshActions.Delete, FshResources.LoanProducts),
        new("Activate LoanProducts", FshActions.Activate, FshResources.LoanProducts),
        new("Deactivate LoanProducts", FshActions.Deactivate, FshResources.LoanProducts),
        
        new("View SavingsProducts", FshActions.View, FshResources.SavingsProducts),
        new("Search SavingsProducts", FshActions.Search, FshResources.SavingsProducts),
        new("Create SavingsProducts", FshActions.Create, FshResources.SavingsProducts),
        new("Update SavingsProducts", FshActions.Update, FshResources.SavingsProducts),
        new("Delete SavingsProducts", FshActions.Delete, FshResources.SavingsProducts),
        new("Activate SavingsProducts", FshActions.Activate, FshResources.SavingsProducts),
        new("Deactivate SavingsProducts", FshActions.Deactivate, FshResources.SavingsProducts),
        
        new("View ShareProducts", FshActions.View, FshResources.ShareProducts),
        new("Search ShareProducts", FshActions.Search, FshResources.ShareProducts),
        new("Create ShareProducts", FshActions.Create, FshResources.ShareProducts),
        new("Update ShareProducts", FshActions.Update, FshResources.ShareProducts),
        new("Delete ShareProducts", FshActions.Delete, FshResources.ShareProducts),
        new("Activate ShareProducts", FshActions.Activate, FshResources.ShareProducts),
        new("Deactivate ShareProducts", FshActions.Deactivate, FshResources.ShareProducts),
        
        new("View FeeDefinitions", FshActions.View, FshResources.FeeDefinitions),
        new("Search FeeDefinitions", FshActions.Search, FshResources.FeeDefinitions),
        new("Create FeeDefinitions", FshActions.Create, FshResources.FeeDefinitions),
        new("Update FeeDefinitions", FshActions.Update, FshResources.FeeDefinitions),
        
        new("View InsuranceProducts", FshActions.View, FshResources.InsuranceProducts),
        new("Create InsuranceProducts", FshActions.Create, FshResources.InsuranceProducts),
        new("Update InsuranceProducts", FshActions.Update, FshResources.InsuranceProducts),
        new("Activate InsuranceProducts", FshActions.Activate, FshResources.InsuranceProducts),
        new("Deactivate InsuranceProducts", FshActions.Deactivate, FshResources.InsuranceProducts),
        
        new("View InvestmentProducts", FshActions.View, FshResources.InvestmentProducts),
        new("Create InvestmentProducts", FshActions.Create, FshResources.InvestmentProducts),
        new("Update InvestmentProducts", FshActions.Update, FshResources.InvestmentProducts),
        new("Activate InvestmentProducts", FshActions.Activate, FshResources.InvestmentProducts),
        new("Deactivate InvestmentProducts", FshActions.Deactivate, FshResources.InvestmentProducts),
        
        //MicroFinance - Accounts
        new("View SavingsAccounts", FshActions.View, FshResources.SavingsAccounts),
        new("Search SavingsAccounts", FshActions.Search, FshResources.SavingsAccounts),
        new("Create SavingsAccounts", FshActions.Create, FshResources.SavingsAccounts),
        new("Update SavingsAccounts", FshActions.Update, FshResources.SavingsAccounts),
        new("Approve SavingsAccounts", FshActions.Approve, FshResources.SavingsAccounts),
        new("Activate SavingsAccounts", FshActions.Activate, FshResources.SavingsAccounts),
        new("Deactivate SavingsAccounts", FshActions.Deactivate, FshResources.SavingsAccounts),
        new("Close SavingsAccounts", FshActions.Close, FshResources.SavingsAccounts),
        new("Freeze SavingsAccounts", FshActions.Freeze, FshResources.SavingsAccounts),
        new("Unfreeze SavingsAccounts", FshActions.Unfreeze, FshResources.SavingsAccounts),
        new("Deposit SavingsAccounts", FshActions.Deposit, FshResources.SavingsAccounts),
        new("Withdraw SavingsAccounts", FshActions.Withdraw, FshResources.SavingsAccounts),
        
        new("View ShareAccounts", FshActions.View, FshResources.ShareAccounts),
        new("Search ShareAccounts", FshActions.Search, FshResources.ShareAccounts),
        new("Create ShareAccounts", FshActions.Create, FshResources.ShareAccounts),
        new("Approve ShareAccounts", FshActions.Approve, FshResources.ShareAccounts),
        new("Activate ShareAccounts", FshActions.Activate, FshResources.ShareAccounts),
        new("Close ShareAccounts", FshActions.Close, FshResources.ShareAccounts),
        
        new("View FixedDeposits", FshActions.View, FshResources.FixedDeposits),
        new("Search FixedDeposits", FshActions.Search, FshResources.FixedDeposits),
        new("Create FixedDeposits", FshActions.Create, FshResources.FixedDeposits),
        new("Mature FixedDeposits", FshActions.Mature, FshResources.FixedDeposits),
        new("Renew FixedDeposits", FshActions.Renew, FshResources.FixedDeposits),
        new("Close FixedDeposits", FshActions.Close, FshResources.FixedDeposits),
        
        new("View InvestmentAccounts", FshActions.View, FshResources.InvestmentAccounts),
        new("Create InvestmentAccounts", FshActions.Create, FshResources.InvestmentAccounts),
        new("Approve InvestmentAccounts", FshActions.Approve, FshResources.InvestmentAccounts),
        new("Activate InvestmentAccounts", FshActions.Activate, FshResources.InvestmentAccounts),
        new("Close InvestmentAccounts", FshActions.Close, FshResources.InvestmentAccounts),
        
        new("View InsurancePolicies", FshActions.View, FshResources.InsurancePolicies),
        new("Create InsurancePolicies", FshActions.Create, FshResources.InsurancePolicies),
        new("Activate InsurancePolicies", FshActions.Activate, FshResources.InsurancePolicies),
        new("Suspend InsurancePolicies", FshActions.Suspend, FshResources.InsurancePolicies),
        new("Mature InsurancePolicies", FshActions.Mature, FshResources.InsurancePolicies),
        new("Renew InsurancePolicies", FshActions.Renew, FshResources.InsurancePolicies),
        
        //MicroFinance - Loan Operations
        new("View Loans", FshActions.View, FshResources.Loans),
        new("Search Loans", FshActions.Search, FshResources.Loans),
        new("Create Loans", FshActions.Create, FshResources.Loans),
        new("Update Loans", FshActions.Update, FshResources.Loans),
        new("Approve Loans", FshActions.Approve, FshResources.Loans),
        new("Disburse Loans", FshActions.Disburse, FshResources.Loans),
        new("Close Loans", FshActions.Close, FshResources.Loans),
        new("WriteOff Loans", FshActions.WriteOff, FshResources.Loans),
        new("Restructure Loans", FshActions.Restructure, FshResources.Loans),
        new("ApplyPayment Loans", FshActions.ApplyPayment, FshResources.Loans),
        
        new("View LoanApplications", FshActions.View, FshResources.LoanApplications),
        new("Search LoanApplications", FshActions.Search, FshResources.LoanApplications),
        new("Create LoanApplications", FshActions.Create, FshResources.LoanApplications),
        new("Update LoanApplications", FshActions.Update, FshResources.LoanApplications),
        new("Submit LoanApplications", FshActions.Submit, FshResources.LoanApplications),
        new("Approve LoanApplications", FshActions.Approve, FshResources.LoanApplications),
        new("Reject LoanApplications", FshActions.Reject, FshResources.LoanApplications),
        
        new("View LoanSchedules", FshActions.View, FshResources.LoanSchedules),
        
        new("View LoanRepayments", FshActions.View, FshResources.LoanRepayments),
        new("Search LoanRepayments", FshActions.Search, FshResources.LoanRepayments),
        new("Create LoanRepayments", FshActions.Create, FshResources.LoanRepayments),
        
        new("View LoanCollaterals", FshActions.View, FshResources.LoanCollaterals),
        new("Create LoanCollaterals", FshActions.Create, FshResources.LoanCollaterals),
        new("Update LoanCollaterals", FshActions.Update, FshResources.LoanCollaterals),
        
        new("View LoanGuarantors", FshActions.View, FshResources.LoanGuarantors),
        new("Create LoanGuarantors", FshActions.Create, FshResources.LoanGuarantors),
        new("Update LoanGuarantors", FshActions.Update, FshResources.LoanGuarantors),
        new("Delete LoanGuarantors", FshActions.Delete, FshResources.LoanGuarantors),
        
        new("View LoanDisbursementTranches", FshActions.View, FshResources.LoanDisbursementTranches),
        new("Create LoanDisbursementTranches", FshActions.Create, FshResources.LoanDisbursementTranches),
        new("Disburse LoanDisbursementTranches", FshActions.Disburse, FshResources.LoanDisbursementTranches),
        
        new("View LoanOfficerAssignments", FshActions.View, FshResources.LoanOfficerAssignments),
        new("Create LoanOfficerAssignments", FshActions.Create, FshResources.LoanOfficerAssignments),
        
        new("View LoanOfficerTargets", FshActions.View, FshResources.LoanOfficerTargets),
        new("Create LoanOfficerTargets", FshActions.Create, FshResources.LoanOfficerTargets),
        
        new("View LoanRestructures", FshActions.View, FshResources.LoanRestructures),
        new("Create LoanRestructures", FshActions.Create, FshResources.LoanRestructures),
        new("Approve LoanRestructures", FshActions.Approve, FshResources.LoanRestructures),
        
        new("View LoanWriteOffs", FshActions.View, FshResources.LoanWriteOffs),
        new("Create LoanWriteOffs", FshActions.Create, FshResources.LoanWriteOffs),
        new("Approve LoanWriteOffs", FshActions.Approve, FshResources.LoanWriteOffs),
        
        //MicroFinance - Collateral Management
        new("View CollateralTypes", FshActions.View, FshResources.CollateralTypes),
        new("Create CollateralTypes", FshActions.Create, FshResources.CollateralTypes),
        
        new("View CollateralValuations", FshActions.View, FshResources.CollateralValuations),
        new("Create CollateralValuations", FshActions.Create, FshResources.CollateralValuations),
        new("Submit CollateralValuations", FshActions.Submit, FshResources.CollateralValuations),
        new("Approve CollateralValuations", FshActions.Approve, FshResources.CollateralValuations),
        new("Reject CollateralValuations", FshActions.Reject, FshResources.CollateralValuations),
        
        new("View CollateralInsurances", FshActions.View, FshResources.CollateralInsurances),
        new("Create CollateralInsurances", FshActions.Create, FshResources.CollateralInsurances),
        new("Renew CollateralInsurances", FshActions.Renew, FshResources.CollateralInsurances),
        new("RecordPremium CollateralInsurances", FshActions.RecordPremium, FshResources.CollateralInsurances),
        
        new("View CollateralReleases", FshActions.View, FshResources.CollateralReleases),
        new("Create CollateralReleases", FshActions.Create, FshResources.CollateralReleases),
        new("Approve CollateralReleases", FshActions.Approve, FshResources.CollateralReleases),
        
        //MicroFinance - Collections & Recovery
        new("View CollectionCases", FshActions.View, FshResources.CollectionCases),
        new("Search CollectionCases", FshActions.Search, FshResources.CollectionCases),
        new("Create CollectionCases", FshActions.Create, FshResources.CollectionCases),
        new("Assign CollectionCases", FshActions.Assign, FshResources.CollectionCases),
        new("Close CollectionCases", FshActions.Close, FshResources.CollectionCases),
        
        new("View CollectionActions", FshActions.View, FshResources.CollectionActions),
        new("Create CollectionActions", FshActions.Create, FshResources.CollectionActions),
        
        new("View CollectionStrategies", FshActions.View, FshResources.CollectionStrategies),
        new("Create CollectionStrategies", FshActions.Create, FshResources.CollectionStrategies),
        new("Activate CollectionStrategies", FshActions.Activate, FshResources.CollectionStrategies),
        new("Deactivate CollectionStrategies", FshActions.Deactivate, FshResources.CollectionStrategies),
        
        new("View PromiseToPays", FshActions.View, FshResources.PromiseToPays),
        new("Create PromiseToPays", FshActions.Create, FshResources.PromiseToPays),
        new("MarkBroken PromiseToPays", FshActions.MarkBroken, FshResources.PromiseToPays),
        
        new("View DebtSettlements", FshActions.View, FshResources.DebtSettlements),
        new("Create DebtSettlements", FshActions.Create, FshResources.DebtSettlements),
        new("Approve DebtSettlements", FshActions.Approve, FshResources.DebtSettlements),
        new("RecordPayment DebtSettlements", FshActions.RecordPayment, FshResources.DebtSettlements),
        
        new("View LegalActions", FshActions.View, FshResources.LegalActions),
        new("Create LegalActions", FshActions.Create, FshResources.LegalActions),
        new("Update LegalActions", FshActions.Update, FshResources.LegalActions),
        
        //MicroFinance - Transactions
        new("View SavingsTransactions", FshActions.View, FshResources.SavingsTransactions),
        new("Search SavingsTransactions", FshActions.Search, FshResources.SavingsTransactions),
        new("Create SavingsTransactions", FshActions.Create, FshResources.SavingsTransactions),
        
        new("View ShareTransactions", FshActions.View, FshResources.ShareTransactions),
        new("Search ShareTransactions", FshActions.Search, FshResources.ShareTransactions),
        new("Create ShareTransactions", FshActions.Create, FshResources.ShareTransactions),
        
        new("View FeeCharges", FshActions.View, FshResources.FeeCharges),
        new("Search FeeCharges", FshActions.Search, FshResources.FeeCharges),
        new("Create FeeCharges", FshActions.Create, FshResources.FeeCharges),
        new("RecordPayment FeeCharges", FshActions.RecordPayment, FshResources.FeeCharges),
        
        new("View MobileTransactions", FshActions.View, FshResources.MobileTransactions),
        new("Search MobileTransactions", FshActions.Search, FshResources.MobileTransactions),
        new("Create MobileTransactions", FshActions.Create, FshResources.MobileTransactions),
        
        new("View InvestmentTransactions", FshActions.View, FshResources.InvestmentTransactions),
        new("Create InvestmentTransactions", FshActions.Create, FshResources.InvestmentTransactions),
        
        //MicroFinance - Insurance
        new("View InsuranceClaims", FshActions.View, FshResources.InsuranceClaims),
        new("Create InsuranceClaims", FshActions.Create, FshResources.InsuranceClaims),
        new("Submit InsuranceClaims", FshActions.Submit, FshResources.InsuranceClaims),
        new("Approve InsuranceClaims", FshActions.Approve, FshResources.InsuranceClaims),
        new("Reject InsuranceClaims", FshActions.Reject, FshResources.InsuranceClaims),
        
        //MicroFinance - Risk & Compliance
        new("View AmlAlerts", FshActions.View, FshResources.AmlAlerts),
        new("Search AmlAlerts", FshActions.Search, FshResources.AmlAlerts),
        new("Create AmlAlerts", FshActions.Create, FshResources.AmlAlerts),
        new("Assign AmlAlerts", FshActions.Assign, FshResources.AmlAlerts),
        new("Confirm AmlAlerts", FshActions.Confirm, FshResources.AmlAlerts),
        new("Clear AmlAlerts", FshActions.Clear, FshResources.AmlAlerts),
        new("Escalate AmlAlerts", FshActions.Escalate, FshResources.AmlAlerts),
        new("FileSar AmlAlerts", FshActions.FileSar, FshResources.AmlAlerts),
        
        new("View CreditScores", FshActions.View, FshResources.CreditScores),
        new("Create CreditScores", FshActions.Create, FshResources.CreditScores),
        
        new("View CreditBureauInquiries", FshActions.View, FshResources.CreditBureauInquiries),
        new("Create CreditBureauInquiries", FshActions.Create, FshResources.CreditBureauInquiries),
        new("Complete CreditBureauInquiries", FshActions.Complete, FshResources.CreditBureauInquiries),
        
        new("View CreditBureauReports", FshActions.View, FshResources.CreditBureauReports),
        new("Create CreditBureauReports", FshActions.Create, FshResources.CreditBureauReports),
        
        new("View RiskAlerts", FshActions.View, FshResources.RiskAlerts),
        new("Create RiskAlerts", FshActions.Create, FshResources.RiskAlerts),
        new("Acknowledge RiskAlerts", FshActions.Acknowledge, FshResources.RiskAlerts),
        
        new("View RiskCategories", FshActions.View, FshResources.RiskCategories),
        new("Create RiskCategories", FshActions.Create, FshResources.RiskCategories),
        
        new("View RiskIndicators", FshActions.View, FshResources.RiskIndicators),
        new("Create RiskIndicators", FshActions.Create, FshResources.RiskIndicators),
        
        new("View Documents", FshActions.View, FshResources.Documents),
        new("Create Documents", FshActions.Create, FshResources.Documents),
        
        //MicroFinance - Workflows & Approvals
        new("View ApprovalWorkflows", FshActions.View, FshResources.ApprovalWorkflows),
        new("Create ApprovalWorkflows", FshActions.Create, FshResources.ApprovalWorkflows),
        new("Activate ApprovalWorkflows", FshActions.Activate, FshResources.ApprovalWorkflows),
        new("Deactivate ApprovalWorkflows", FshActions.Deactivate, FshResources.ApprovalWorkflows),
        
        new("View ApprovalRequests", FshActions.View, FshResources.ApprovalRequests),
        new("Create ApprovalRequests", FshActions.Create, FshResources.ApprovalRequests),
        new("Approve ApprovalRequests", FshActions.Approve, FshResources.ApprovalRequests),
        new("Reject ApprovalRequests", FshActions.Reject, FshResources.ApprovalRequests),
        new("Cancel ApprovalRequests", FshActions.Cancel, FshResources.ApprovalRequests),
        
        new("View CustomerCases", FshActions.View, FshResources.CustomerCases),
        new("Create CustomerCases", FshActions.Create, FshResources.CustomerCases),
        new("Assign CustomerCases", FshActions.Assign, FshResources.CustomerCases),
        new("Close CustomerCases", FshActions.Close, FshResources.CustomerCases),
        
        //MicroFinance - Communications
        new("View CommunicationTemplates", FshActions.View, FshResources.CommunicationTemplates),
        new("Create CommunicationTemplates", FshActions.Create, FshResources.CommunicationTemplates),
        new("Activate CommunicationTemplates", FshActions.Activate, FshResources.CommunicationTemplates),
        
        new("View CommunicationLogs", FshActions.View, FshResources.CommunicationLogs),
        new("Create CommunicationLogs", FshActions.Create, FshResources.CommunicationLogs),
        
        new("View MarketingCampaigns", FshActions.View, FshResources.MarketingCampaigns),
        new("Create MarketingCampaigns", FshActions.Create, FshResources.MarketingCampaigns),
        new("Activate MarketingCampaigns", FshActions.Activate, FshResources.MarketingCampaigns),
        
        //MicroFinance - Digital Channels
        new("View AgentBankings", FshActions.View, FshResources.AgentBankings),
        new("Search AgentBankings", FshActions.Search, FshResources.AgentBankings),
        new("Create AgentBankings", FshActions.Create, FshResources.AgentBankings),
        new("Update AgentBankings", FshActions.Update, FshResources.AgentBankings),
        new("Approve AgentBankings", FshActions.Approve, FshResources.AgentBankings),
        new("Suspend AgentBankings", FshActions.Suspend, FshResources.AgentBankings),
        new("CreditFloat AgentBankings", FshActions.CreditFloat, FshResources.AgentBankings),
        new("DebitFloat AgentBankings", FshActions.DebitFloat, FshResources.AgentBankings),
        new("RecordAudit AgentBankings", FshActions.RecordAudit, FshResources.AgentBankings),
        new("UpgradeTier AgentBankings", FshActions.UpgradeTier, FshResources.AgentBankings),
        
        new("View MobileWallets", FshActions.View, FshResources.MobileWallets),
        new("Create MobileWallets", FshActions.Create, FshResources.MobileWallets),
        new("Activate MobileWallets", FshActions.Activate, FshResources.MobileWallets),
        new("Deactivate MobileWallets", FshActions.Deactivate, FshResources.MobileWallets),
        
        new("View PaymentGateways", FshActions.View, FshResources.PaymentGateways),
        new("Create PaymentGateways", FshActions.Create, FshResources.PaymentGateways),
        new("Activate PaymentGateways", FshActions.Activate, FshResources.PaymentGateways),
        new("Deactivate PaymentGateways", FshActions.Deactivate, FshResources.PaymentGateways),
        
        new("View QrPayments", FshActions.View, FshResources.QrPayments),
        new("Create QrPayments", FshActions.Create, FshResources.QrPayments),
        
        new("View UssdSessions", FshActions.View, FshResources.UssdSessions),
        new("Create UssdSessions", FshActions.Create, FshResources.UssdSessions),
        
        //MicroFinance - Cash Management
        new("View CashVaults", FshActions.View, FshResources.CashVaults),
        new("Create CashVaults", FshActions.Create, FshResources.CashVaults),
        new("Deposit CashVaults", FshActions.Deposit, FshResources.CashVaults),
        new("Withdraw CashVaults", FshActions.Withdraw, FshResources.CashVaults),
        
        new("View TellerSessions", FshActions.View, FshResources.TellerSessions),
        new("Create TellerSessions", FshActions.Create, FshResources.TellerSessions),
        new("Close TellerSessions", FshActions.Close, FshResources.TellerSessions),
        
        //MicroFinance - Reporting
        new("View ReportDefinitions", FshActions.View, FshResources.ReportDefinitions),
        new("Create ReportDefinitions", FshActions.Create, FshResources.ReportDefinitions),
        
        new("View ReportGenerations", FshActions.View, FshResources.ReportGenerations),
        new("Generate ReportGenerations", FshActions.Generate, FshResources.ReportGenerations),
    ];

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>([.. AllPermissions.Where(p => p.IsRoot)]);
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>([.. AllPermissions.Where(p => !p.IsRoot)]);
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>([.. AllPermissions.Where(p => p.IsBasic)]);
}

public record FshPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}
