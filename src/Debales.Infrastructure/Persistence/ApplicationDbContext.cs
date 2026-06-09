using System.Text.Json;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Debales.Domain.Catalog;
using Debales.Domain.Common;
using Debales.Domain.Core.Audit;
using Debales.Domain.Core.Modules;
using Debales.Domain.Core.NumberSeries;
using Debales.Domain.Core.Roles;
using Debales.Domain.Core.Users;
using Debales.Domain.CRM.Activities;
using Debales.Domain.CRM.Contacts;
using Debales.Domain.CRM.Customers;
using Debales.Domain.CRM.Notes;
using Debales.Domain.CRM.Opportunities;
using Debales.Domain.Inventory;
using Debales.Domain.Licensing;
using Debales.Domain.Purchasing;
using Debales.Domain.Sales;
using Debales.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService? _currentUser;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService? currentUser = null) : base(options)
    {
        _currentUser = currentUser;
    }

    // Core
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<SystemModule> SystemModules => Set<SystemModule>();
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();
    public DbSet<NumberSeries> NumberSeries => Set<NumberSeries>();

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
    public DbSet<PriceList> PriceLists => Set<PriceList>();
    public DbSet<ItemPrice> ItemPrices => Set<ItemPrice>();
    public DbSet<SupplierItemCode> SupplierItemCodes => Set<SupplierItemCode>();
    public DbSet<CustomerItemCode> CustomerItemCodes => Set<CustomerItemCode>();

    // Sales
    public DbSet<SalesQuote> SalesQuotes => Set<SalesQuote>();
    public DbSet<SalesQuoteLine> SalesQuoteLines => Set<SalesQuoteLine>();
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

    // Licensing — Fase 6
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<License> Licenses => Set<License>();
    public DbSet<LicenseModule> LicenseModules => Set<LicenseModule>();

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddAuditEntries();
        return await base.SaveChangesAsync(cancellationToken);
    }

    // ── Audit ────────────────────────────────────────────────────────────

    private static readonly HashSet<string> _auditExclusions = new(StringComparer.Ordinal)
    {
        nameof(AuditEntry),
        nameof(UserRole),
        nameof(RolePermission),
        nameof(StockBalance),        // updated frequently by stock movements
        nameof(FiscalPeriod),        // child of FiscalYear, created in batch
        nameof(AccountingTemplateLine),
        nameof(AccountingEntryLine),
        nameof(SalesOrderLine),
        nameof(SalesQuoteLine),
        nameof(SalesDeliveryNoteLine),
        nameof(SalesInvoiceLine),
        nameof(SalesCreditNoteLine),
        nameof(PurchaseOrderLine),
        nameof(PurchaseDeliveryNoteLine),
        nameof(PurchaseInvoiceLine),
        nameof(PurchaseCreditNoteLine),
    };

    private static bool IsSensitiveProp(string name) =>
        name.Contains("Password", StringComparison.OrdinalIgnoreCase) ||
        name.Contains("Hash", StringComparison.OrdinalIgnoreCase) ||
        name.Contains("Token", StringComparison.OrdinalIgnoreCase) ||
        name.Contains("Salt", StringComparison.OrdinalIgnoreCase);

    private void AddAuditEntries()
    {
        var user = _currentUser?.Username;

        var changed = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Where(e => e.Entity is Entity)
            .Where(e => !_auditExclusions.Contains(e.Entity.GetType().Name))
            .ToList();

        foreach (var entry in changed)
        {
            var entityName = entry.Entity.GetType().Name;
            var entityId = (entry.Property("Id").CurrentValue ?? entry.Property("Id").OriginalValue)?.ToString()
                           ?? string.Empty;

            string action = entry.State switch
            {
                EntityState.Added => "Created",
                EntityState.Modified => "Updated",
                _ => "Deleted"
            };

            string? oldValues = null;
            string? newValues = null;

            try
            {
                if (entry.State == EntityState.Modified)
                {
                    var changedProps = entry.Properties
                        .Where(p => p.IsModified && !IsSensitiveProp(p.Metadata.Name))
                        .ToList();

                    if (changedProps.Count > 0)
                    {
                        oldValues = JsonSerializer.Serialize(
                            changedProps.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue?.ToString()));
                        newValues = JsonSerializer.Serialize(
                            changedProps.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue?.ToString()));
                    }
                }
                else if (entry.State == EntityState.Added)
                {
                    newValues = JsonSerializer.Serialize(
                        entry.Properties
                            .Where(p => !IsSensitiveProp(p.Metadata.Name))
                            .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue?.ToString()));
                }
                else
                {
                    oldValues = JsonSerializer.Serialize(
                        entry.Properties
                            .Where(p => !IsSensitiveProp(p.Metadata.Name))
                            .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue?.ToString()));
                }
            }
            catch
            {
                // Serialization failures must not block the save
            }

            AuditEntries.Add(AuditEntry.Record(entityName, entityId, action, user, oldValues, newValues));
        }
    }
}
