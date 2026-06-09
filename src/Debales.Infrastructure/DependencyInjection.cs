using Debales.Application.Accounting;
using Debales.Application.Catalog;
using Debales.Application.Licensing;
using Debales.Application.Common;
using Debales.Application.Core.Audit;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Core.Auth;
using Debales.Application.Core.Roles;
using Debales.Application.Core.Users;
using Debales.Application.Documents;
using Debales.Infrastructure.Documents;
using Debales.Application.CRM.Activities;
using Debales.Application.CRM.Contacts;
using Debales.Application.CRM.Customers;
using Debales.Application.CRM.Notes;
using Debales.Application.CRM.Opportunities;
using Debales.Application.Inventory;
using Debales.Application.Purchasing;
using Debales.Application.Sales;
using Debales.Application.Suppliers;
using Debales.Infrastructure.Persistence;
using Debales.Infrastructure.Persistence.Repositories;
using Debales.Infrastructure.Persistence.Repositories.Accounting;
using Debales.Infrastructure.Persistence.Repositories.Licensing;
using Debales.Infrastructure.Services;
using Debales.Infrastructure.Persistence.Repositories.Catalog;
using Debales.Infrastructure.Persistence.Repositories.CRM;
using Debales.Infrastructure.Persistence.Repositories.Inventory;
using Debales.Infrastructure.Persistence.Repositories.Purchasing;
using Debales.Infrastructure.Persistence.Repositories.Sales;
using Debales.Infrastructure.Persistence.Repositories.Suppliers;
using Debales.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Debales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAuditEntryRepository, AuditEntryRepository>();
        services.AddScoped<INumberSeriesRepository, NumberSeriesRepository>();

        // Documents
        services.AddSingleton<IInvoicePdfGenerator, InvoicePdfGenerator>();

        // Core
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.Configure<JwtSettings>(opts => configuration.GetSection("Jwt").Bind(opts));
        services.AddScoped<ITokenService, JwtTokenService>();

        // Catalog
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemFamilyRepository, ItemFamilyRepository>();
        services.AddScoped<IUnitOfMeasureRepository, UnitOfMeasureRepository>();
        services.AddScoped<ITaxTypeRepository, TaxTypeRepository>();
        services.AddScoped<IPriceListRepository, PriceListRepository>();
        services.AddScoped<ISupplierItemCodeRepository, SupplierItemCodeRepository>();
        services.AddScoped<ICustomerItemCodeRepository, CustomerItemCodeRepository>();

        // CRM
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IOpportunityRepository, OpportunityRepository>();

        // Payment Terms
        services.AddScoped<IPaymentTermRepository, PaymentTermRepository>();

        // Payment Methods
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

        // Sales
        services.AddScoped<ISalesQuoteRepository, SalesQuoteRepository>();
        services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
        services.AddScoped<ISalesDeliveryNoteRepository, SalesDeliveryNoteRepository>();

        // Purchasing
        services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
        services.AddScoped<IPurchaseDeliveryNoteRepository, PurchaseDeliveryNoteRepository>();
        services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
        services.AddScoped<IPurchaseCreditNoteRepository, PurchaseCreditNoteRepository>();
        services.AddScoped<IPayableRepository, PayableRepository>();
        services.AddScoped<ISupplierPaymentRepository, SupplierPaymentRepository>();

        // Sales — ERP-3
        services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();
        services.AddScoped<ISalesCreditNoteRepository, SalesCreditNoteRepository>();
        services.AddScoped<IReceivableRepository, ReceivableRepository>();
        services.AddScoped<ICustomerPaymentRepository, CustomerPaymentRepository>();

        // Accounting — ERP-5
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IFiscalYearRepository, FiscalYearRepository>();
        services.AddScoped<IAccountingJournalRepository, AccountingJournalRepository>();
        services.AddScoped<IAccountingEntryRepository, AccountingEntryRepository>();
        services.AddScoped<IAccountingTemplateRepository, AccountingTemplateRepository>();

        // Licensing — Fase 6
        services.AddScoped<ILicenseRepository, LicenseRepository>();
        services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
        services.AddScoped<ILicenseService, LicenseService>();

        // Inventory — ERP-4
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IWarehouseLocationRepository, WarehouseLocationRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<IStockBalanceRepository, StockBalanceRepository>();

        return services;
    }
}
