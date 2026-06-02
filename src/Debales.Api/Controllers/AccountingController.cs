using Debales.Application.Accounting.Commands.CreateAccount;
using Debales.Application.Accounting.Commands.CreateAccountingEntry;
using Debales.Application.Accounting.Commands.CreateAccountingJournal;
using Debales.Application.Accounting.Commands.CreateFiscalYear;
using Debales.Application.Accounting.Commands.PostAccountingEntry;
using Debales.Application.Accounting.Queries.GetAccountById;
using Debales.Application.Accounting.Queries.GetAccounts;
using Debales.Application.Accounting.Queries.GetAccountingEntries;
using Debales.Application.Accounting.Queries.GetAccountingEntryById;
using Debales.Application.Accounting.Queries.GetAccountingJournals;
using Debales.Application.Accounting.Queries.GetFiscalYears;
using Debales.Domain.Accounting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/accounting")]
[Authorize]
public sealed class AccountingController : ControllerBase
{
    private readonly CreateAccountHandler _createAccount;
    private readonly GetAccountsHandler _getAccounts;
    private readonly GetAccountByIdHandler _getAccountById;
    private readonly CreateFiscalYearHandler _createFiscalYear;
    private readonly GetFiscalYearsHandler _getFiscalYears;
    private readonly CreateAccountingJournalHandler _createJournal;
    private readonly GetAccountingJournalsHandler _getJournals;
    private readonly CreateAccountingEntryHandler _createEntry;
    private readonly PostAccountingEntryHandler _postEntry;
    private readonly GetAccountingEntriesHandler _getEntries;
    private readonly GetAccountingEntryByIdHandler _getEntryById;

    public AccountingController(
        CreateAccountHandler createAccount,
        GetAccountsHandler getAccounts,
        GetAccountByIdHandler getAccountById,
        CreateFiscalYearHandler createFiscalYear,
        GetFiscalYearsHandler getFiscalYears,
        CreateAccountingJournalHandler createJournal,
        GetAccountingJournalsHandler getJournals,
        CreateAccountingEntryHandler createEntry,
        PostAccountingEntryHandler postEntry,
        GetAccountingEntriesHandler getEntries,
        GetAccountingEntryByIdHandler getEntryById)
    {
        _createAccount = createAccount;
        _getAccounts = getAccounts;
        _getAccountById = getAccountById;
        _createFiscalYear = createFiscalYear;
        _getFiscalYears = getFiscalYears;
        _createJournal = createJournal;
        _getJournals = getJournals;
        _createEntry = createEntry;
        _postEntry = postEntry;
        _getEntries = getEntries;
        _getEntryById = getEntryById;
    }

    // Accounts
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        var result = await _getAccounts.Handle(new GetAccountsQuery(search, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("accounts/{id:guid}")]
    public async Task<IActionResult> GetAccountById(Guid id, CancellationToken ct = default)
    {
        var account = await _getAccountById.Handle(new GetAccountByIdQuery(id), ct);
        return account is null ? NotFound() : Ok(account);
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request, CancellationToken ct = default)
    {
        var account = await _createAccount.Handle(
            new CreateAccountCommand(request.Code, request.Name, request.Type, request.IsPostable, request.ParentCode, "api"), ct);
        return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
    }

    // Fiscal Years
    [HttpGet("fiscal-years")]
    public async Task<IActionResult> GetFiscalYears(CancellationToken ct = default)
    {
        var result = await _getFiscalYears.Handle(new GetFiscalYearsQuery(), ct);
        return Ok(result);
    }

    [HttpPost("fiscal-years")]
    public async Task<IActionResult> CreateFiscalYear([FromBody] CreateFiscalYearRequest request, CancellationToken ct = default)
    {
        var year = await _createFiscalYear.Handle(
            new CreateFiscalYearCommand(request.Name, request.StartDate, request.EndDate, "api"), ct);
        return Ok(year);
    }

    // Journals
    [HttpGet("journals")]
    public async Task<IActionResult> GetJournals(CancellationToken ct = default)
    {
        var result = await _getJournals.Handle(new GetAccountingJournalsQuery(), ct);
        return Ok(result);
    }

    [HttpPost("journals")]
    public async Task<IActionResult> CreateJournal([FromBody] CreateJournalRequest request, CancellationToken ct = default)
    {
        var journal = await _createJournal.Handle(
            new CreateAccountingJournalCommand(request.Code, request.Name, "api"), ct);
        return Ok(journal);
    }

    // Entries
    [HttpGet("entries")]
    public async Task<IActionResult> GetEntries(
        [FromQuery] string? search,
        [FromQuery] Guid? journalId,
        [FromQuery] Guid? fiscalPeriodId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _getEntries.Handle(
            new GetAccountingEntriesQuery(search, journalId, fiscalPeriodId, page, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("entries/{id:guid}")]
    public async Task<IActionResult> GetEntryById(Guid id, CancellationToken ct = default)
    {
        var entry = await _getEntryById.Handle(new GetAccountingEntryByIdQuery(id), ct);
        return entry is null ? NotFound() : Ok(entry);
    }

    [HttpPost("entries")]
    public async Task<IActionResult> CreateEntry([FromBody] CreateEntryRequest request, CancellationToken ct = default)
    {
        var entry = await _createEntry.Handle(
            new CreateAccountingEntryCommand(
                request.Date, request.Description, request.JournalId, request.FiscalPeriodId,
                request.Lines, "api"), ct);
        return Ok(entry);
    }

    [HttpPost("entries/{id:guid}/post")]
    public async Task<IActionResult> PostEntry(Guid id, CancellationToken ct = default)
    {
        var entry = await _postEntry.Handle(new PostAccountingEntryCommand(id, "api"), ct);
        return Ok(entry);
    }

    public sealed record CreateAccountRequest(string Code, string Name, AccountType Type, bool IsPostable, string? ParentCode);
    public sealed record CreateFiscalYearRequest(string Name, DateOnly StartDate, DateOnly EndDate);
    public sealed record CreateJournalRequest(string Code, string Name);
    public sealed record CreateEntryRequest(
        DateOnly Date, string Description, Guid JournalId, Guid FiscalPeriodId,
        IReadOnlyList<CreateEntryLineDto> Lines);
}
