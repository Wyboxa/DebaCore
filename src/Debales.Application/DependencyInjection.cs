using Debales.Application.AI.Briefing;
using Debales.Application.AI.Chat;
using Debales.Application.AI.Summary;
using Debales.Application.Catalog.Commands.CreateItem;
using Debales.Application.Catalog.Commands.UpdateItem;
using Debales.Application.Catalog.Queries.GetItems;
using Debales.Application.Catalog.Queries.GetItemById;
using Debales.Application.Catalog.Queries.GetCatalogLookups;
using Debales.Application.CRM.Dashboard;
using Debales.Application.Suppliers.Commands.CreateSupplier;
using Debales.Application.Suppliers.Commands.UpdateSupplier;
using Debales.Application.Suppliers.Queries.GetSuppliers;
using Debales.Application.Suppliers.Queries.GetSupplierById;
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

        // Suppliers
        services.AddScoped<CreateSupplierHandler>();
        services.AddScoped<UpdateSupplierHandler>();
        services.AddScoped<GetSuppliersHandler>();
        services.AddScoped<GetSupplierByIdHandler>();

        // Dashboard
        services.AddScoped<GetDashboardStatsHandler>();

        // AI
        services.AddScoped<ChatWithCustomerHandler>();
        services.AddScoped<GetCustomerSummaryHandler>();
        services.AddScoped<GetDashboardBriefingHandler>();

        return services;
    }
}
