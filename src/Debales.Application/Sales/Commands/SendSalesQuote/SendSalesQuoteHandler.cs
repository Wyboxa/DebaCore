using Debales.Application.Common;

namespace Debales.Application.Sales.Commands.SendSalesQuote;

public sealed class SendSalesQuoteHandler
{
    private readonly ISalesQuoteRepository _quotes;
    private readonly IUnitOfWork _uow;

    public SendSalesQuoteHandler(ISalesQuoteRepository quotes, IUnitOfWork uow)
    {
        _quotes = quotes;
        _uow = uow;
    }

    public async Task Handle(SendSalesQuoteCommand command, CancellationToken cancellationToken = default)
    {
        var quote = await _quotes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Presupuesto '{command.Id}' no encontrado.");

        quote.Send(command.UpdatedBy);
        _quotes.Update(quote);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
