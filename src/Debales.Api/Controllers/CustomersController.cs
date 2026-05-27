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
using Debales.Domain.CRM.Activities;
using Debales.Domain.CRM.Opportunities;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController : ControllerBase
{
    private readonly CreateCustomerHandler _create;
    private readonly UpdateCustomerHandler _update;
    private readonly GetCustomerByIdHandler _getById;
    private readonly GetCustomersHandler _getAll;
    private readonly AddContactHandler _addContact;
    private readonly GetContactsByCustomerHandler _getContacts;
    private readonly LogActivityHandler _logActivity;
    private readonly GetActivitiesByCustomerHandler _getActivities;
    private readonly AddNoteHandler _addNote;
    private readonly CreateOpportunityHandler _createOpportunity;
    private readonly UpdateOpportunityStatusHandler _updateOpportunityStatus;
    private readonly GetOpportunitiesByCustomerHandler _getOpportunities;

    public CustomersController(
        CreateCustomerHandler create, UpdateCustomerHandler update,
        GetCustomerByIdHandler getById, GetCustomersHandler getAll,
        AddContactHandler addContact, GetContactsByCustomerHandler getContacts,
        LogActivityHandler logActivity, GetActivitiesByCustomerHandler getActivities,
        AddNoteHandler addNote,
        CreateOpportunityHandler createOpportunity, UpdateOpportunityStatusHandler updateOpportunityStatus,
        GetOpportunitiesByCustomerHandler getOpportunities)
    {
        _create = create; _update = update; _getById = getById; _getAll = getAll;
        _addContact = addContact; _getContacts = getContacts;
        _logActivity = logActivity; _getActivities = getActivities;
        _addNote = addNote;
        _createOpportunity = createOpportunity; _updateOpportunityStatus = updateOpportunityStatus;
        _getOpportunities = getOpportunities;
    }

    // ── Customers ─────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await _getAll.Handle(new GetCustomersQuery(search, page, pageSize), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var customer = await _getById.Handle(new GetCustomerByIdQuery(id), ct);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest req, CancellationToken ct)
    {
        try
        {
            var customer = await _create.Handle(new CreateCustomerCommand(req.Name, req.Sector, req.TaxId, req.Phone, "api"), ct);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest req, CancellationToken ct)
    {
        try
        {
            var customer = await _update.Handle(new UpdateCustomerCommand(id, req.Name, req.Sector, req.TaxId, req.Phone, req.Website, "api"), ct);
            return Ok(customer);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Contacts ──────────────────────────────────────────────────────────────

    [HttpGet("{id:guid}/contacts")]
    public async Task<IActionResult> GetContacts(Guid id, CancellationToken ct)
        => Ok(await _getContacts.Handle(new GetContactsByCustomerQuery(id), ct));

    [HttpPost("{id:guid}/contacts")]
    public async Task<IActionResult> AddContact(Guid id, [FromBody] AddContactRequest req, CancellationToken ct)
    {
        try
        {
            var contact = await _addContact.Handle(new AddContactCommand(id, req.FirstName, req.LastName, req.JobTitle, req.Email, req.Phone, "api"), ct);
            return Created(string.Empty, contact);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Activities ────────────────────────────────────────────────────────────

    [HttpGet("{id:guid}/activities")]
    public async Task<IActionResult> GetActivities(Guid id, CancellationToken ct)
        => Ok(await _getActivities.Handle(new GetActivitiesByCustomerQuery(id), ct));

    [HttpPost("{id:guid}/activities")]
    public async Task<IActionResult> LogActivity(Guid id, [FromBody] LogActivityRequest req, CancellationToken ct)
    {
        try
        {
            var activity = await _logActivity.Handle(
                new LogActivityCommand(id, req.Type, req.Subject, req.ScheduledAt, "api", req.ContactId, req.Notes, req.AssignedTo), ct);
            return Created(string.Empty, activity);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Notes ─────────────────────────────────────────────────────────────────

    [HttpPost("{id:guid}/notes")]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] AddNoteRequest req, CancellationToken ct)
    {
        try
        {
            var note = await _addNote.Handle(new AddNoteCommand(id, req.Content, "api"), ct);
            return Created(string.Empty, note);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    // ── Opportunities ─────────────────────────────────────────────────────────

    [HttpGet("{id:guid}/opportunities")]
    public async Task<IActionResult> GetOpportunities(Guid id, CancellationToken ct)
        => Ok(await _getOpportunities.Handle(new GetOpportunitiesByCustomerQuery(id), ct));

    [HttpPost("{id:guid}/opportunities")]
    public async Task<IActionResult> CreateOpportunity(Guid id, [FromBody] CreateOpportunityRequest req, CancellationToken ct)
    {
        try
        {
            var opp = await _createOpportunity.Handle(
                new CreateOpportunityCommand(id, req.Title, "api", req.EstimatedValue, req.ExpectedCloseDate), ct);
            return Created(string.Empty, opp);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpPatch("{customerId:guid}/opportunities/{opportunityId:guid}/status")]
    public async Task<IActionResult> UpdateOpportunityStatus(Guid customerId, Guid opportunityId, [FromBody] UpdateOpportunityStatusRequest req, CancellationToken ct)
    {
        try
        {
            var opp = await _updateOpportunityStatus.Handle(new UpdateOpportunityStatusCommand(opportunityId, req.Status, "api"), ct);
            return Ok(opp);
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}

// ── Request DTOs ──────────────────────────────────────────────────────────────

public sealed record CreateCustomerRequest(string Name, string? Sector, string? TaxId, string? Phone);
public sealed record UpdateCustomerRequest(string Name, string? Sector, string? TaxId, string? Phone, string? Website);
public sealed record AddContactRequest(string FirstName, string LastName, string? JobTitle, string? Email, string? Phone);
public sealed record LogActivityRequest(ActivityType Type, string Subject, DateTime ScheduledAt, Guid? ContactId, string? Notes, string? AssignedTo);
public sealed record AddNoteRequest(string Content);
public sealed record CreateOpportunityRequest(string Title, decimal? EstimatedValue, DateTime? ExpectedCloseDate);
public sealed record UpdateOpportunityStatusRequest(OpportunityStatus Status);
