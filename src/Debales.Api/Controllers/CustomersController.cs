using Debales.Application.Catalog.Commands.DeleteCustomerItemCode;
using Debales.Application.Catalog.Commands.UpsertCustomerItemCode;
using Debales.Application.Catalog.Queries.GetCustomerItemCodes;
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
using Debales.Domain.CRM.Activities;
using Debales.Domain.CRM.Opportunities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CustomersController : ControllerBase
{
    private readonly CreateCustomerHandler _create;
    private readonly UpdateCustomerHandler _update;
    private readonly GetCustomerByIdHandler _getById;
    private readonly GetCustomersHandler _getAll;
    private readonly AddContactHandler _addContact;
    private readonly GetContactsByCustomerHandler _getContacts;
    private readonly LogActivityHandler _logActivity;
    private readonly CompleteActivityHandler _completeActivity;
    private readonly GetActivitiesByCustomerHandler _getActivities;
    private readonly AddNoteHandler _addNote;
    private readonly GetNotesByCustomerHandler _getNotes;
    private readonly CreateOpportunityHandler _createOpportunity;
    private readonly UpdateOpportunityStatusHandler _updateOpportunityStatus;
    private readonly GetOpportunitiesByCustomerHandler _getOpportunities;
    private readonly GetCustomerItemCodesHandler _getItemCodes;
    private readonly UpsertCustomerItemCodeHandler _upsertItemCode;
    private readonly DeleteCustomerItemCodeHandler _deleteItemCode;

    public CustomersController(
        CreateCustomerHandler create, UpdateCustomerHandler update,
        GetCustomerByIdHandler getById, GetCustomersHandler getAll,
        AddContactHandler addContact, GetContactsByCustomerHandler getContacts,
        LogActivityHandler logActivity, CompleteActivityHandler completeActivity, GetActivitiesByCustomerHandler getActivities,
        AddNoteHandler addNote, GetNotesByCustomerHandler getNotes,
        CreateOpportunityHandler createOpportunity, UpdateOpportunityStatusHandler updateOpportunityStatus,
        GetOpportunitiesByCustomerHandler getOpportunities,
        GetCustomerItemCodesHandler getItemCodes,
        UpsertCustomerItemCodeHandler upsertItemCode,
        DeleteCustomerItemCodeHandler deleteItemCode)
    {
        _create = create; _update = update; _getById = getById; _getAll = getAll;
        _addContact = addContact; _getContacts = getContacts;
        _logActivity = logActivity; _completeActivity = completeActivity; _getActivities = getActivities;
        _addNote = addNote; _getNotes = getNotes;
        _createOpportunity = createOpportunity; _updateOpportunityStatus = updateOpportunityStatus;
        _getOpportunities = getOpportunities;
        _getItemCodes = getItemCodes;
        _upsertItemCode = upsertItemCode;
        _deleteItemCode = deleteItemCode;
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
            var customer = await _create.Handle(new CreateCustomerCommand(req.Name, req.Sector, req.TaxId, req.Phone, req.Email, "api"), ct);
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
            var customer = await _update.Handle(new UpdateCustomerCommand(
                id, req.Name, req.Sector, req.TaxId, req.Phone, req.Email, req.Website,
                req.AddressStreet, req.AddressCity, req.AddressPostalCode, req.AddressCountry, "api"), ct);
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

    [HttpPatch("{id:guid}/activities/{activityId:guid}/complete")]
    public async Task<IActionResult> CompleteActivity(Guid id, Guid activityId, [FromBody] CompleteActivityRequest req, CancellationToken ct)
    {
        try
        {
            var activity = await _completeActivity.Handle(new CompleteActivityCommand(id, activityId, req.Notes, "api"), ct);
            return Ok(activity);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
    }

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

    [HttpGet("{id:guid}/notes")]
    public async Task<IActionResult> GetNotes(Guid id, CancellationToken ct)
        => Ok(await _getNotes.Handle(new GetNotesByCustomerQuery(id), ct));

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
            var opp = await _updateOpportunityStatus.Handle(new UpdateOpportunityStatusCommand(customerId, opportunityId, req.Status, "api"), ct);
            return Ok(opp);
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    // ── Item codes ────────────────────────────────────────────────────────────

    [HttpGet("{id:guid}/item-codes")]
    public async Task<IActionResult> GetItemCodes(Guid id, CancellationToken ct)
        => Ok(await _getItemCodes.Handle(new GetCustomerItemCodesQuery(id), ct));

    [HttpPost("{id:guid}/item-codes")]
    public async Task<IActionResult> UpsertItemCode(Guid id, [FromBody] UpsertCustomerItemCodeRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _upsertItemCode.Handle(
                new UpsertCustomerItemCodeCommand(id, req.ItemId, req.CustomerCode, req.Description, "api"), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpDelete("{id:guid}/item-codes/{itemId:guid}")]
    public async Task<IActionResult> DeleteItemCode(Guid id, Guid itemId, CancellationToken ct)
    {
        try
        {
            await _deleteItemCode.Handle(new DeleteCustomerItemCodeCommand(id, itemId), ct);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}

// ── Request DTOs ──────────────────────────────────────────────────────────────

public sealed record CompleteActivityRequest(string? Notes);
public sealed record CreateCustomerRequest(string Name, string? Sector, string? TaxId, string? Phone, string? Email);
public sealed record UpdateCustomerRequest(
    string Name, string? Sector, string? TaxId, string? Phone, string? Email, string? Website,
    string? AddressStreet, string? AddressCity, string? AddressPostalCode, string? AddressCountry);
public sealed record AddContactRequest(string FirstName, string LastName, string? JobTitle, string? Email, string? Phone);
public sealed record LogActivityRequest(ActivityType Type, string Subject, DateTime ScheduledAt, Guid? ContactId, string? Notes, string? AssignedTo);
public sealed record AddNoteRequest(string Content);
public sealed record CreateOpportunityRequest(string Title, decimal? EstimatedValue, DateTime? ExpectedCloseDate);
public sealed record UpdateOpportunityStatusRequest(OpportunityStatus Status);
public sealed record UpsertCustomerItemCodeRequest(Guid ItemId, string CustomerCode, string? Description);
