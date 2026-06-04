using Debales.AI;
using Debales.Application;
using Microsoft.EntityFrameworkCore;
using Debales.Application.Common;
using Debales.Application.Core.Auth.Commands.Login;
using Debales.Application.Documents;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;
using Debales.Infrastructure;
using Debales.Infrastructure.Persistence;
using Debales.Infrastructure.Persistence.Seeders;
using Debales.Web.Components;
using Debales.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/auth/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, BlazorCookieAuthStateProvider>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAI(builder.Configuration);
builder.Services.AddScoped<ToastService>();

var app = builder.Build();

// Migraciones + seed al arrancar (idempotente — seguro en Docker y en local)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    await DbSeeder.SeedAsync(context, hasher);
    await CatalogSeeder.SeedAsync(context);
    await DemoDataSeeder.SeedAsync(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Endpoint de login — recibe form POST, valida credenciales y establece cookie
app.MapPost("/auth/login", async (
    HttpContext ctx,
    LoginHandler loginHandler,
    IPasswordHasher _) =>
{
    var form = await ctx.Request.ReadFormAsync();
    var usernameOrEmail = form["usernameOrEmail"].ToString().Trim();
    var password = form["password"].ToString();
    var returnUrl = form["returnUrl"].ToString();
    if (string.IsNullOrEmpty(returnUrl) || !returnUrl.StartsWith('/')) returnUrl = "/";

    try
    {
        var result = await loginHandler.Handle(new LoginCommand(usernameOrEmail, password));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new(ClaimTypes.Name, result.Username),
            new(ClaimTypes.Email, result.Email),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = false });

        return Results.Redirect(returnUrl);
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl)}");
    }
}).DisableAntiforgery();

// Endpoint de logout
app.MapPost("/auth/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
}).DisableAntiforgery();

// Endpoints de descarga PDF (sin antiforgery — descarga directa del navegador)
app.MapGet("/descargar/factura-venta/{id:guid}", async (
    Guid id,
    GetSalesInvoiceByIdHandler getInvoice,
    IInvoicePdfGenerator pdfGen,
    CancellationToken ct) =>
{
    var invoice = await getInvoice.Handle(new GetSalesInvoiceByIdQuery(id), ct);
    if (invoice is null) return Results.NotFound();
    var bytes = pdfGen.GenerateSalesInvoice(invoice);
    return Results.File(bytes, "application/pdf", $"Factura-{invoice.Number}.pdf");
}).RequireAuthorization();

app.MapGet("/descargar/factura-compra/{id:guid}", async (
    Guid id,
    GetPurchaseInvoiceByIdHandler getInvoice,
    IInvoicePdfGenerator pdfGen,
    CancellationToken ct) =>
{
    var invoice = await getInvoice.Handle(new GetPurchaseInvoiceByIdQuery(id), ct);
    if (invoice is null) return Results.NotFound();
    var bytes = pdfGen.GeneratePurchaseInvoice(invoice);
    return Results.File(bytes, "application/pdf", $"Factura-compra-{invoice.Number}.pdf");
}).RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
