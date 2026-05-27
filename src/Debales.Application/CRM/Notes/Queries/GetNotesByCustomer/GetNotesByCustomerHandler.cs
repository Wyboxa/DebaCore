using Debales.Application.CRM.Notes.DTOs;

namespace Debales.Application.CRM.Notes.Queries.GetNotesByCustomer;

public sealed record GetNotesByCustomerQuery(Guid CustomerId);

public sealed class GetNotesByCustomerHandler
{
    private readonly INoteRepository _notes;

    public GetNotesByCustomerHandler(INoteRepository notes) => _notes = notes;

    public async Task<IReadOnlyList<NoteDto>> Handle(GetNotesByCustomerQuery query, CancellationToken cancellationToken = default)
    {
        var notes = await _notes.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

        return notes
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NoteDto(n.Id, n.CustomerId, n.Content, n.CreatedAt, n.CreatedBy))
            .ToList();
    }
}
