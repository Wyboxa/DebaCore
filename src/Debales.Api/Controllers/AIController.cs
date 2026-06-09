using Debales.Application.AI;
using Debales.Application.AI.Chat;
using Debales.Application.AI.Summary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
[RequiresModule("AI")]
public sealed class AIController : ControllerBase
{
    private readonly ChatWithCustomerHandler _chat;
    private readonly GetCustomerSummaryHandler _summary;

    public AIController(ChatWithCustomerHandler chat, GetCustomerSummaryHandler summary)
    {
        _chat = chat;
        _summary = summary;
    }

    [HttpPost("customers/{id:guid}/chat")]
    public async Task<IActionResult> Chat(Guid id, [FromBody] ChatRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _chat.Handle(
                new ChatWithCustomerCommand(id, req.History, req.Message, "api"), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
    }

    [HttpGet("customers/{id:guid}/summary")]
    public async Task<IActionResult> GetSummary(Guid id, CancellationToken ct)
    {
        try
        {
            var summary = await _summary.Handle(new GetCustomerSummaryQuery(id), ct);
            return Ok(new { summary });
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}

public sealed record ChatRequest(IReadOnlyList<ChatMessage> History, string Message);
