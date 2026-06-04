using Debales.Application.Common;

namespace Debales.Application.Sales.Commands.AcceptSalesQuote;

public sealed class AcceptSalesQuoteHandler
{
    private readonly ISalesQuoteRepository _quotes;
    private readonly IUnitOfWork _uow;

    public AcceptSalesQuoteHandler(ISalesQuoteRepository quotes, IUnitOfWork uow)
    {
        _quotes = quotes;
        _uow = uow;
    }

    public async Task Handle(AcceptSalesQuoteCommand command, CancellationToken cancellationToken = default)
    {
        var quote = await _quotes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Presupuesto '{command.Id}' no encontrado.");

        quote.Accept(command.UpdatedBy);
        _quotes.Update(quote);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
