using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreatePaymentTerm;

public sealed class CreatePaymentTermHandler
{
    private readonly IPaymentTermRepository _paymentTerms;
    private readonly IUnitOfWork _uow;

    public CreatePaymentTermHandler(IPaymentTermRepository paymentTerms, IUnitOfWork uow)
    {
        _paymentTerms = paymentTerms;
        _uow = uow;
    }

    public async Task<PaymentTermDto> Handle(CreatePaymentTermCommand command, CancellationToken cancellationToken = default)
    {
        var lines = command.Lines.Select(l => (l.DueDays, l.Percentage));
        var pt = PaymentTerm.Create(command.Name, command.Description, lines, command.CreatedBy);

        await _paymentTerms.AddAsync(pt, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _paymentTerms.GetByIdAsync(pt.Id, cancellationToken);
        return ToDto(saved!);
    }

    internal static PaymentTermDto ToDto(PaymentTerm pt) => new(
        pt.Id, pt.Name, pt.Description, pt.IsActive,
        pt.Lines.Select(l => new PaymentTermLineDto(l.DueDays, l.Percentage, l.SortOrder)).ToList());
}
