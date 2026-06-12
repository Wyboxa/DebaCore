using Debales.Application.Core.Audit.Queries.GetAuditEntries;
using Debales.Application.AI.Briefing;
using Debales.Application.AI.Chat;
using Debales.Application.AI.Summary;
using Debales.Application.Catalog.Commands.CreateItem;
using Debales.Application.Catalog.Commands.UpdateItem;
using Debales.Application.Catalog.Queries.GetItems;
using Debales.Application.Catalog.Queries.GetItemById;
using Debales.Application.Catalog.Queries.GetCatalogLookups;
using Debales.Application.Catalog.Queries.GetPriceLists;
using Debales.Application.Catalog.Queries.GetPriceListById;
using Debales.Application.Catalog.Commands.CreatePriceList;
using Debales.Application.Catalog.Commands.UpdatePriceList;
using Debales.Application.Catalog.Commands.SetItemPrice;
using Debales.Application.Catalog.Commands.RemoveItemPrice;
using Debales.Application.Catalog.Queries.GetSupplierItemCodes;
using Debales.Application.Catalog.Commands.UpsertSupplierItemCode;
using Debales.Application.Catalog.Commands.DeleteSupplierItemCode;
using Debales.Application.Catalog.Queries.GetCustomerItemCodes;
using Debales.Application.Catalog.Commands.UpsertCustomerItemCode;
using Debales.Application.Catalog.Commands.DeleteCustomerItemCode;
using Debales.Application.Catalog.Queries.GetItemPrice;
using Debales.Application.CRM.Dashboard;
using Debales.Application.Suppliers.Commands.CreateSupplier;
using Debales.Application.Suppliers.Commands.UpdateSupplier;
using Debales.Application.Suppliers.Queries.GetSuppliers;
using Debales.Application.Suppliers.Queries.GetSupplierById;
using Debales.Application.Suppliers.Contacts.Commands.AddSupplierContact;
using Debales.Application.Suppliers.Contacts.Commands.UpdateSupplierContact;
using Debales.Application.Suppliers.Contacts.Commands.DeactivateSupplierContact;
using Debales.Application.Suppliers.Contacts.Queries.GetContactsBySupplier;
using Debales.Application.Core.Auth.Commands.Login;
using Debales.Application.Core.Users.Commands.CreateUser;
using Debales.Application.Core.Users.Queries.GetUserById;
using Debales.Application.CRM.Activities.Commands.CompleteActivity;
using Debales.Application.CRM.Activities.Commands.LogActivity;
using Debales.Application.CRM.Activities.Queries.GetActivitiesByCustomer;
using Debales.Application.CRM.Contacts.Commands.AddContact;
using Debales.Application.CRM.Contacts.Queries.GetContactsByCustomer;
using Debales.Application.CRM.Customers.Commands.CreateCustomer;
using Debales.Application.CRM.Customers.Commands.UpdateCustomer;
using Debales.Application.CRM.Customers.Queries.GetCustomerById;
using Debales.Application.CRM.Customers.Queries.GetCustomers;
using Debales.Application.CRM.Notes.Commands.AddNote;
using Debales.Application.CRM.Notes.Queries.GetNotesByCustomer;
using Debales.Application.CRM.Opportunities.Commands.CreateOpportunity;
using Debales.Application.CRM.Opportunities.Commands.UpdateOpportunityStatus;
using Debales.Application.CRM.Opportunities.Queries.GetOpportunitiesByCustomer;
using Debales.Application.Sales.Commands.CreateSalesOrder;
using Debales.Application.Sales.Commands.ConfirmSalesOrder;
using Debales.Application.Sales.Commands.CancelSalesOrder;
using Debales.Application.Sales.Commands.CreateSalesDeliveryNote;
using Debales.Application.Sales.Commands.PostSalesDeliveryNote;
using Debales.Application.Sales.Commands.GenerateDeliveryNoteFromOrder;
using Debales.Application.Sales.Commands.GenerateInvoiceFromDeliveryNote;
using Debales.Application.Sales.Commands.BatchGenerateDocuments;
using Debales.Application.Sales.Queries.GetAutomationPreview;
using Debales.Application.Sales.Queries.GetSalesOrders;
using Debales.Application.Sales.Queries.GetSalesOrderById;
using Debales.Application.Sales.Queries.GetSalesDeliveryNotes;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;
using Debales.Application.Purchasing.Commands.CreatePurchaseOrder;
using Debales.Application.Purchasing.Commands.ConfirmPurchaseOrder;
using Debales.Application.Purchasing.Commands.CancelPurchaseOrder;
using Debales.Application.Purchasing.Commands.CreatePurchaseDeliveryNote;
using Debales.Application.Purchasing.Commands.PostPurchaseDeliveryNote;
using Debales.Application.Purchasing.Commands.GenerateInvoiceFromPurchaseDeliveryNote;
using Debales.Application.Purchasing.Commands.CreatePurchaseInvoice;
using Debales.Application.Purchasing.Commands.PostPurchaseInvoice;
using Debales.Application.Purchasing.Commands.CancelPurchaseInvoice;
using Debales.Application.Purchasing.Commands.CreatePurchaseCreditNote;
using Debales.Application.Purchasing.Commands.PostPurchaseCreditNote;
using Debales.Application.Purchasing.Commands.CreateSupplierPayment;
using Debales.Application.Purchasing.Queries.GetPurchaseOrders;
using Debales.Application.Purchasing.Queries.GetPurchaseOrderById;
using Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNotes;
using Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNoteById;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoices;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;
using Debales.Application.Purchasing.Queries.GetPurchaseCreditNotes;
using Debales.Application.Purchasing.Queries.GetPurchaseCreditNoteById;
using Debales.Application.Purchasing.Queries.GetPayables;
using Debales.Application.Purchasing.Queries.GetPayablesAging;
using Debales.Application.Purchasing.Queries.GetSupplierPayments;
using Debales.Application.Purchasing.Queries.GetSupplierStatement;
using Debales.Application.Sales.Commands.CreatePaymentTerm;
using Debales.Application.Sales.Commands.UpdatePaymentTerm;
using Debales.Application.Sales.Commands.DeletePaymentTerm;
using Debales.Application.Sales.Queries.GetPaymentTerms;
using Debales.Application.Sales.Queries.GetPaymentTermById;
using Debales.Application.Sales.Commands.CreatePaymentMethod;
using Debales.Application.Sales.Commands.UpdatePaymentMethod;
using Debales.Application.Sales.Commands.DeletePaymentMethod;
using Debales.Application.Sales.Queries.GetPaymentMethods;
using Debales.Application.Sales.Queries.GetPaymentMethodById;
using Debales.Application.Sales.Commands.CreateSalesInvoice;
using Debales.Application.Sales.Commands.PostSalesInvoice;
using Debales.Application.Sales.Commands.CancelSalesInvoice;
using Debales.Application.Sales.Commands.CreateSalesCreditNote;
using Debales.Application.Sales.Commands.PostSalesCreditNote;
using Debales.Application.Sales.Commands.CreateCustomerPayment;
using Debales.Application.Sales.Queries.GetSalesInvoices;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;
using Debales.Application.Sales.Queries.GetSalesCreditNotes;
using Debales.Application.Sales.Queries.GetSalesCreditNoteById;
using Debales.Application.Sales.Queries.GetReceivables;
using Debales.Application.Sales.Queries.GetReceivablesAging;
using Debales.Application.Sales.Queries.GetCustomerStatement;
using Debales.Application.Sales.Queries.GetCustomerPayments;
using Debales.Application.AI.ERP;
using Debales.Application.Accounting.Commands.CloseFiscalPeriod;
using Debales.Application.Accounting.Commands.CloseFiscalYear;
using Debales.Application.Accounting.Commands.CreateAccount;
using Debales.Application.Accounting.Commands.CreateAccountingEntry;
using Debales.Application.Accounting.Commands.CreateAccountingJournal;
using Debales.Application.Accounting.Commands.CreateFiscalYear;
using Debales.Application.Accounting.Commands.PostAccountingEntry;
using Debales.Application.Accounting.Queries.GetAccountById;
using Debales.Application.Accounting.Queries.GetAccounts;
using Debales.Application.Accounting.Queries.GetAccountingEntries;
using Debales.Application.Accounting.Queries.GetAccountingEntryById;
using Debales.Application.Accounting.Queries.GetTrialBalance;
using Debales.Application.Accounting.Queries.GetJournalBook;
using Debales.Application.Accounting.Queries.GetBalanceSheet;
using Debales.Application.Accounting.Queries.GetAccountingJournals;
using Debales.Application.Accounting.Queries.GetFiscalYears;
using Debales.Application.Accounting.Commands.CreateBankAccount;
using Debales.Application.Accounting.Commands.UpdateBankAccount;
using Debales.Application.Accounting.Commands.DeleteBankAccount;
using Debales.Application.Accounting.Queries.GetBankAccounts;
using Debales.Application.Accounting.Queries.GetTreasuryPosition;
using Debales.Application.Accounting.Queries.GetBankAccountById;
using Debales.Application.Accounting.Commands.CreateCashAccount;
using Debales.Application.Accounting.Commands.UpdateCashAccount;
using Debales.Application.Accounting.Commands.DeleteCashAccount;
using Debales.Application.Accounting.Queries.GetCashAccounts;
using Debales.Application.Accounting.Queries.GetCashAccountById;
using Debales.Application.Accounting.Commands.CreateRemittance;
using Debales.Application.Accounting.Commands.UpdateRemittance;
using Debales.Application.Accounting.Commands.DeleteRemittance;
using Debales.Application.Accounting.Commands.AddRemittanceLine;
using Debales.Application.Accounting.Commands.RemoveRemittanceLine;
using Debales.Application.Accounting.Commands.SendRemittance;
using Debales.Application.Accounting.Commands.ConfirmRemittance;
using Debales.Application.Accounting.Commands.FailRemittance;
using Debales.Application.Accounting.Queries.GetRemittances;
using Debales.Application.Accounting.Queries.GetRemittanceById;
using Debales.Application.Accounting.Services;
using Debales.Application.Licensing;
using Debales.Application.Licensing.Commands.ActivateLicense;
using Debales.Application.Licensing.Queries.GetCurrentLicense;
using Debales.Application.Licensing.Queries.GetSubscriptionPlans;
using Debales.Application.Inventory.Commands.CreateWarehouse;
using Debales.Application.Inventory.Commands.AddWarehouseLocation;
using Debales.Application.Inventory.Commands.CreateStockMovement;
using Debales.Application.Inventory.Commands.AdjustStock;
using Debales.Application.Inventory.Commands.CreateInventoryCount;
using Debales.Application.Inventory.Commands.AddInventoryCountLine;
using Debales.Application.Inventory.Commands.SetCountedQuantity;
using Debales.Application.Inventory.Commands.CloseInventoryCount;
using Debales.Application.Inventory.Queries.GetInventoryCounts;
using Debales.Application.Inventory.Queries.GetInventoryCountById;
using Debales.Application.Inventory.Queries.GetWarehouses;
using Debales.Application.Inventory.Queries.GetWarehouseById;
using Debales.Application.Inventory.Queries.GetStockMovements;
using Debales.Application.Inventory.Queries.GetStockMovementById;
using Debales.Application.Inventory.Queries.GetStockBalance;
using Debales.Application.Core.Users.Queries.GetUsers;
using Debales.Application.Core.Users.Commands.DeactivateUser;
using Debales.Application.Core.Users.Commands.ReactivateUser;
using Debales.Application.Core.Users.Commands.ChangePassword;
using Debales.Application.Core.Users.Commands.AssignRole;
using Debales.Application.Core.Roles.Queries.GetRoles;
using Debales.Application.Core.NumberSeries.Queries.GetNumberSeries;
using Debales.Application.Core.NumberSeries.Commands.CreateNumberSeries;
using Debales.Application.Core.NumberSeries.Commands.UpdateNumberSeries;
using Debales.Application.Sales.Commands.CreateSalesQuote;
using Debales.Application.Sales.Commands.SendSalesQuote;
using Debales.Application.Sales.Commands.AcceptSalesQuote;
using Debales.Application.Sales.Commands.RejectSalesQuote;
using Debales.Application.Sales.Commands.ConvertQuoteToOrder;
using Debales.Application.Sales.Queries.GetSalesQuotes;
using Debales.Application.Sales.Queries.GetSalesQuoteById;
using Microsoft.Extensions.DependencyInjection;

namespace Debales.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Core — Auth
        services.AddScoped<LoginHandler>();

        // Core — Users
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<GetUserByIdHandler>();
        services.AddScoped<GetUsersHandler>();
        services.AddScoped<DeactivateUserHandler>();
        services.AddScoped<ReactivateUserHandler>();
        services.AddScoped<ChangePasswordHandler>();
        services.AddScoped<AssignRoleHandler>();

        // Core — Roles
        services.AddScoped<GetRolesHandler>();

        // Core — NumberSeries
        services.AddScoped<GetNumberSeriesHandler>();
        services.AddScoped<CreateNumberSeriesHandler>();
        services.AddScoped<UpdateNumberSeriesHandler>();

        // CRM — Customers
        services.AddScoped<CreateCustomerHandler>();
        services.AddScoped<UpdateCustomerHandler>();
        services.AddScoped<GetCustomerByIdHandler>();
        services.AddScoped<GetCustomersHandler>();

        // CRM — Contacts
        services.AddScoped<AddContactHandler>();
        services.AddScoped<GetContactsByCustomerHandler>();

        // CRM — Activities
        services.AddScoped<LogActivityHandler>();
        services.AddScoped<CompleteActivityHandler>();
        services.AddScoped<GetActivitiesByCustomerHandler>();

        // CRM — Notes
        services.AddScoped<AddNoteHandler>();
        services.AddScoped<GetNotesByCustomerHandler>();

        // CRM — Opportunities
        services.AddScoped<CreateOpportunityHandler>();
        services.AddScoped<UpdateOpportunityStatusHandler>();
        services.AddScoped<GetOpportunitiesByCustomerHandler>();

        // Catalog — Items
        services.AddScoped<CreateItemHandler>();
        services.AddScoped<UpdateItemHandler>();
        services.AddScoped<GetItemsHandler>();
        services.AddScoped<GetItemByIdHandler>();
        services.AddScoped<GetCatalogLookupsHandler>();

        // Catalog — Price Lists
        services.AddScoped<GetPriceListsHandler>();
        services.AddScoped<GetPriceListByIdHandler>();
        services.AddScoped<CreatePriceListHandler>();
        services.AddScoped<UpdatePriceListHandler>();
        services.AddScoped<SetItemPriceHandler>();
        services.AddScoped<RemoveItemPriceHandler>();

        // Catalog — Supplier / Customer item codes
        services.AddScoped<GetSupplierItemCodesHandler>();
        services.AddScoped<UpsertSupplierItemCodeHandler>();
        services.AddScoped<DeleteSupplierItemCodeHandler>();
        services.AddScoped<GetCustomerItemCodesHandler>();
        services.AddScoped<UpsertCustomerItemCodeHandler>();
        services.AddScoped<DeleteCustomerItemCodeHandler>();
        services.AddScoped<GetItemPriceHandler>();

        // Suppliers
        services.AddScoped<CreateSupplierHandler>();
        services.AddScoped<UpdateSupplierHandler>();
        services.AddScoped<GetSuppliersHandler>();
        services.AddScoped<GetSupplierByIdHandler>();

        // Suppliers — Contacts
        services.AddScoped<AddSupplierContactHandler>();
        services.AddScoped<UpdateSupplierContactHandler>();
        services.AddScoped<DeactivateSupplierContactHandler>();
        services.AddScoped<GetContactsBySupplierHandler>();

        // Sales — Quotes
        services.AddScoped<CreateSalesQuoteHandler>();
        services.AddScoped<SendSalesQuoteHandler>();
        services.AddScoped<AcceptSalesQuoteHandler>();
        services.AddScoped<RejectSalesQuoteHandler>();
        services.AddScoped<ConvertQuoteToOrderHandler>();
        services.AddScoped<GetSalesQuotesHandler>();
        services.AddScoped<GetSalesQuoteByIdHandler>();

        // Sales — Orders
        services.AddScoped<CreateSalesOrderHandler>();
        services.AddScoped<ConfirmSalesOrderHandler>();
        services.AddScoped<CancelSalesOrderHandler>();
        services.AddScoped<GetSalesOrdersHandler>();
        services.AddScoped<GetSalesOrderByIdHandler>();

        // Sales — Delivery Notes
        services.AddScoped<CreateSalesDeliveryNoteHandler>();
        services.AddScoped<PostSalesDeliveryNoteHandler>();
        services.AddScoped<GetSalesDeliveryNotesHandler>();
        services.AddScoped<GetSalesDeliveryNoteByIdHandler>();
        services.AddScoped<GenerateDeliveryNoteFromOrderHandler>();
        services.AddScoped<GenerateInvoiceFromDeliveryNoteHandler>();
        services.AddScoped<BatchGenerateDocumentsHandler>();
        services.AddScoped<GetAutomationPreviewHandler>();

        // Purchasing — Orders
        services.AddScoped<CreatePurchaseOrderHandler>();
        services.AddScoped<ConfirmPurchaseOrderHandler>();
        services.AddScoped<CancelPurchaseOrderHandler>();
        services.AddScoped<GetPurchaseOrdersHandler>();
        services.AddScoped<GetPurchaseOrderByIdHandler>();

        // Purchasing — Delivery Notes
        services.AddScoped<CreatePurchaseDeliveryNoteHandler>();
        services.AddScoped<PostPurchaseDeliveryNoteHandler>();
        services.AddScoped<GenerateInvoiceFromPurchaseDeliveryNoteHandler>();
        services.AddScoped<GetPurchaseDeliveryNotesHandler>();
        services.AddScoped<GetPurchaseDeliveryNoteByIdHandler>();

        // Payment Terms
        services.AddScoped<CreatePaymentTermHandler>();
        services.AddScoped<UpdatePaymentTermHandler>();
        services.AddScoped<DeletePaymentTermHandler>();
        services.AddScoped<GetPaymentTermsHandler>();
        services.AddScoped<GetPaymentTermByIdHandler>();

        // Payment Methods
        services.AddScoped<CreatePaymentMethodHandler>();
        services.AddScoped<UpdatePaymentMethodHandler>();
        services.AddScoped<DeletePaymentMethodHandler>();
        services.AddScoped<GetPaymentMethodsHandler>();
        services.AddScoped<GetPaymentMethodByIdHandler>();

        // Sales — Invoices
        services.AddScoped<CreateSalesInvoiceHandler>();
        services.AddScoped<PostSalesInvoiceHandler>();
        services.AddScoped<CancelSalesInvoiceHandler>();
        services.AddScoped<GetSalesInvoicesHandler>();
        services.AddScoped<GetSalesInvoiceByIdHandler>();

        // Sales — Credit Notes
        services.AddScoped<CreateSalesCreditNoteHandler>();
        services.AddScoped<PostSalesCreditNoteHandler>();
        services.AddScoped<GetSalesCreditNotesHandler>();
        services.AddScoped<GetSalesCreditNoteByIdHandler>();

        // Sales — Receivables & Payments
        services.AddScoped<GetReceivablesHandler>();
        services.AddScoped<CreateCustomerPaymentHandler>();
        services.AddScoped<GetCustomerPaymentsHandler>();

        // Purchasing — Invoices
        services.AddScoped<CreatePurchaseInvoiceHandler>();
        services.AddScoped<PostPurchaseInvoiceHandler>();
        services.AddScoped<CancelPurchaseInvoiceHandler>();
        services.AddScoped<GetPurchaseInvoicesHandler>();
        services.AddScoped<GetPurchaseInvoiceByIdHandler>();

        // Purchasing — Credit Notes
        services.AddScoped<CreatePurchaseCreditNoteHandler>();
        services.AddScoped<PostPurchaseCreditNoteHandler>();
        services.AddScoped<GetPurchaseCreditNotesHandler>();
        services.AddScoped<GetPurchaseCreditNoteByIdHandler>();

        // Purchasing — Payables & Payments
        services.AddScoped<GetPayablesHandler>();
        services.AddScoped<CreateSupplierPaymentHandler>();
        services.AddScoped<GetSupplierPaymentsHandler>();

        // Inventory — ERP-4
        services.AddScoped<CreateWarehouseHandler>();
        services.AddScoped<AddWarehouseLocationHandler>();
        services.AddScoped<GetWarehousesHandler>();
        services.AddScoped<GetWarehouseByIdHandler>();
        services.AddScoped<CreateStockMovementHandler>();
        services.AddScoped<AdjustStockHandler>();
        services.AddScoped<GetStockMovementsHandler>();
        services.AddScoped<GetStockMovementByIdHandler>();
        services.AddScoped<GetStockBalanceHandler>();
        services.AddScoped<CreateInventoryCountHandler>();
        services.AddScoped<AddInventoryCountLineHandler>();
        services.AddScoped<SetCountedQuantityHandler>();
        services.AddScoped<CloseInventoryCountHandler>();
        services.AddScoped<GetInventoryCountsHandler>();
        services.AddScoped<GetInventoryCountByIdHandler>();

        // AI — ERP-6
        services.AddScoped<ChatWithERPHandler>();
        services.AddScoped<GetERPAnomaliesHandler>();
        services.AddScoped<GetCustomerERPSummaryHandler>();
        services.AddScoped<GetSupplierERPSummaryHandler>();

        // Accounting — ERP-5
        services.AddScoped<CloseFiscalPeriodHandler>();
        services.AddScoped<CloseFiscalYearHandler>();
        services.AddScoped<IAccountingEntryService, AccountingEntryService>();
        services.AddScoped<CreateAccountHandler>();
        services.AddScoped<GetAccountsHandler>();
        services.AddScoped<GetAccountByIdHandler>();
        services.AddScoped<CreateFiscalYearHandler>();
        services.AddScoped<GetFiscalYearsHandler>();
        services.AddScoped<CreateAccountingJournalHandler>();
        services.AddScoped<GetAccountingJournalsHandler>();
        services.AddScoped<CreateAccountingEntryHandler>();
        services.AddScoped<PostAccountingEntryHandler>();
        services.AddScoped<GetAccountingEntriesHandler>();
        services.AddScoped<GetAccountingEntryByIdHandler>();

        // Accounting — Reports
        services.AddScoped<GetTrialBalanceHandler>();
        services.AddScoped<GetJournalBookHandler>();
        services.AddScoped<GetBalanceSheetHandler>();

        // Accounting — BankAccounts
        services.AddScoped<CreateBankAccountHandler>();
        services.AddScoped<UpdateBankAccountHandler>();
        services.AddScoped<DeleteBankAccountHandler>();
        services.AddScoped<GetBankAccountsHandler>();
        services.AddScoped<GetBankAccountByIdHandler>();

        // Accounting — CashAccounts
        services.AddScoped<CreateCashAccountHandler>();
        services.AddScoped<UpdateCashAccountHandler>();
        services.AddScoped<DeleteCashAccountHandler>();
        services.AddScoped<GetCashAccountsHandler>();
        services.AddScoped<GetCashAccountByIdHandler>();

        // Reports — Aging + Statement
        services.AddScoped<GetReceivablesAgingHandler>();
        services.AddScoped<GetPayablesAgingHandler>();
        services.AddScoped<GetTreasuryPositionHandler>();
        services.AddScoped<GetCustomerStatementHandler>();
        services.AddScoped<GetSupplierStatementHandler>();

        // Accounting — Remittances
        services.AddScoped<CreateRemittanceHandler>();
        services.AddScoped<UpdateRemittanceHandler>();
        services.AddScoped<DeleteRemittanceHandler>();
        services.AddScoped<AddRemittanceLineHandler>();
        services.AddScoped<RemoveRemittanceLineHandler>();
        services.AddScoped<SendRemittanceHandler>();
        services.AddScoped<ConfirmRemittanceHandler>();
        services.AddScoped<FailRemittanceHandler>();
        services.AddScoped<GetRemittancesHandler>();
        services.AddScoped<GetRemittanceByIdHandler>();

        // Licensing — Fase 6
        services.AddScoped<GetCurrentLicenseHandler>();
        services.AddScoped<GetSubscriptionPlansHandler>();
        services.AddScoped<ActivateLicenseHandler>();

        // Audit
        services.AddScoped<GetAuditEntriesHandler>();

        // Dashboard
        services.AddScoped<GetDashboardStatsHandler>();

        // AI
        services.AddScoped<ChatWithCustomerHandler>();
        services.AddScoped<GetCustomerSummaryHandler>();
        services.AddScoped<GetDashboardBriefingHandler>();

        return services;
    }
}
