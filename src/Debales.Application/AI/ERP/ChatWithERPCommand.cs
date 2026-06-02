using Debales.Application.AI.Chat;

namespace Debales.Application.AI.ERP;

public sealed record ChatWithERPCommand(
    IReadOnlyList<ChatMessage> History,
    string NewMessage,
    string User);
