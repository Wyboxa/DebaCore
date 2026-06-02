using Debales.Domain.Accounting;
using Debales.Domain.Catalog;
using Debales.Domain.Core.Audit;
using Debales.Domain.Core.Modules;
using Debales.Domain.Core.Roles;
using Debales.Domain.Core.Users;
using Debales.Domain.CRM.Activities;
using Debales.Domain.CRM.Contacts;
using Debales.Domain.CRM.Customers;
using Debales.Domain.CRM.Notes;
using Debales.Domain.CRM.Opportunities;
using Debales.Domain.Inventory;
using Debales.Domain.Purchasing;
using Debales.Domain.Sales;
using Debales.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Core
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<SystemModule> SystemModules => Set<SystemModule>();
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    // CRM
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();

    // Suppliers
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    // Catalog
    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemFamily> ItemFamilies => Set<ItemFamily>();
    public DbSet<UnitOfMeasure> UnitsOfMeasure => Set<UnitOfMeasure>();
    public DbSet<TaxType> TaxTypes => Set<TaxType>();

    // Sales
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderLine> SalesOrderLines => Set<SalesOrderLine>();
    public DbSet<SalesDeliveryNote> SalesDeliveryNotes => Set<SalesDeliveryNote>();
    public DbSet<SalesDeliveryNoteLine> SalesDeliveryNoteLines => Set<SalesDeliveryNoteLine>();

    // Purchasing
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<PurchaseDeliveryNote> PurchaseDeliveryNotes => Set<PurchaseDeliveryNote>();
    public DbSet<PurchaseDeliveryNoteLine> PurchaseDeliveryNoteLines => Set<PurchaseDeliveryNoteLine>();
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines => Set<PurchaseInvoiceLine>();
    public DbSet<PurchaseCreditNote> PurchaseCreditNotes => Set<PurchaseCreditNote>();
    public DbSet<PurchaseCreditNoteLine> PurchaseCreditNoteLines => Set<PurchaseCreditNoteLine>();
    public DbSet<Payable> Payables => Set<Payable>();
    public DbSet<SupplierPayment> SupplierPayments => Set<SupplierPayment>();

    // Sales — ERP-3
    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
    public DbSet<SalesInvoiceLine> SalesInvoiceLines => Set<SalesInvoiceLine>();
    public DbSet<SalesCreditNote> SalesCreditNotes => Set<SalesCreditNote>();
    public DbSet<SalesCreditNoteLine> SalesCreditNoteLines => Set<SalesCreditNoteLine>();
    public DbSet<Receivable> Receivables => Set<Receivable>();
    public DbSet<CustomerPayment> CustomerPayments => Set<CustomerPayment>();

    // Inventory — ERP-4
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<StockBalance> StockBalances => Set<StockBalance>();

    // Accounting — ERP-5
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<FiscalYear> FiscalYears => Set<FiscalYear>();
    public DbSet<FiscalPeriod> FiscalPeriods => Set<FiscalPeriod>();
    public DbSet<AccountingJournal> AccountingJournals => Set<AccountingJournal>();
    public DbSet<AccountingEntry> AccountingEntries => Set<AccountingEntry>();
    public DbSet<AccountingEntryLine> AccountingEntryLines => Set<AccountingEntryLine>();
    public DbSet<AccountingTemplate> AccountingTemplates => Set<AccountingTemplate>();
    public DbSet<AccountingTemplateLine> AccountingTemplateLines => Set<AccountingTemplateLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
