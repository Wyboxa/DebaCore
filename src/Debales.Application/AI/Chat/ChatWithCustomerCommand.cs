namespace Debales.Application.AI.Chat;

public sealed record ChatWithCustomerCommand(
    Guid CustomerId,
    IReadOnlyList<ChatMessage> History,
    string NewMessage,
    string User);

public sealed record ChatResponseDto(string Message);
