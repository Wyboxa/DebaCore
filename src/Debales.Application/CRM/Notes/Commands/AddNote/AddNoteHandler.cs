using Debales.Application.Common;
using Debales.Application.CRM.Customers;
using Debales.Application.CRM.Notes.DTOs;
using Debales.Domain.CRM.Notes;

namespace Debales.Application.CRM.Notes.Commands.AddNote;

public sealed record AddNoteCommand(Guid CustomerId, string Content, string CreatedBy);

public sealed class AddNoteHandler
{
    private readonly INoteRepository _notes;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _unitOfWork;

    public AddNoteHandler(INoteRepository notes, ICustomerRepository customers, IUnitOfWork unitOfWork)
    {
        _notes = notes;
        _customers = customers;
        _unitOfWork = unitOfWork;
    }

    public async Task<NoteDto> Handle(AddNoteCommand command, CancellationToken cancellationToken = default)
    {
        var customerExists = await _customers.GetByIdAsync(command.CustomerId, cancellationToken) is not null;
        if (!customerExists)
            throw new KeyNotFoundException($"Cliente '{command.CustomerId}' no encontrado.");

        var note = Note.Create(command.CustomerId, command.Content, command.CreatedBy);

        await _notes.AddAsync(note, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new NoteDto(note.Id, note.CustomerId, note.Content, note.CreatedAt, note.CreatedBy);
    }
}
