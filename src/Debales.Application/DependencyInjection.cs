using Debales.Application.Core.Users.Commands.CreateUser;
using Debales.Application.Core.Users.Queries.GetUserById;
using Debales.Application.CRM.Activities.Commands.LogActivity;
using Debales.Application.CRM.Activities.Queries.GetActivitiesByCustomer;
using Debales.Application.CRM.Contacts.Commands.AddContact;
using Debales.Application.CRM.Contacts.Queries.GetContactsByCustomer;
using Debales.Application.CRM.Customers.Commands.CreateCustomer;
using Debales.Application.CRM.Customers.Commands.UpdateCustomer;
using Debales.Application.CRM.Customers.Queries.GetCustomerById;
using Debales.Application.CRM.Customers.Queries.GetCustomers;
using Debales.Application.CRM.Notes.Commands.AddNote;
using Debales.Application.CRM.Opportunities.Commands.CreateOpportunity;
using Debales.Application.CRM.Opportunities.Commands.UpdateOpportunityStatus;
using Debales.Application.CRM.Opportunities.Queries.GetOpportunitiesByCustomer;
using Microsoft.Extensions.DependencyInjection;

namespace Debales.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Core
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
        services.AddScoped<GetActivitiesByCustomerHandler>();

        // CRM — Notes
        services.AddScoped<AddNoteHandler>();

        // CRM — Opportunities
        services.AddScoped<CreateOpportunityHandler>();
        services.AddScoped<UpdateOpportunityStatusHandler>();
        services.AddScoped<GetOpportunitiesByCustomerHandler>();

        return services;
    }
}
