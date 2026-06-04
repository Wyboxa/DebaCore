using Debales.Application.Common;

namespace Debales.Application.Sales.Commands.RejectSalesQuote;

public sealed class RejectSalesQuoteHandler
{
    private readonly ISalesQuoteRepository _quotes;
    private readonly IUnitOfWork _uow;

    public RejectSalesQuoteHandler(ISalesQuoteRepository quotes, IUnitOfWork uow)
    {
        _quotes = quotes;
        _uow = uow;
    }

    public async Task Handle(RejectSalesQuoteCommand command, CancellationToken cancellationToken = default)
    {
        var quote = await _quotes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Presupuesto '{command.Id}' no encontrado.");

        quote.Reject(command.UpdatedBy);
        _quotes.Update(quote);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
