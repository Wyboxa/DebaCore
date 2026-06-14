using Debales.Application.AIGovernance.Commands.ApproveAIActionProposal;
using Debales.Application.AIGovernance.Commands.CreateAIActionProposal;
using Debales.Application.AIGovernance.Commands.CreateAIExecutionLog;
using Debales.Application.AIGovernance.Commands.CreateAIKnowledgeBase;
using Debales.Application.AIGovernance.Commands.CreateAIRule;
using Debales.Application.AIGovernance.Commands.RejectAIActionProposal;
using Debales.Application.AIGovernance.Commands.UpdateAIKnowledgeBase;
using Debales.Application.AIGovernance.Commands.UpdateAIRule;
using Debales.Application.AIGovernance.Queries.GetAIActionProposalById;
using Debales.Application.AIGovernance.Queries.GetAIActionProposals;
using Debales.Application.AIGovernance.Queries.GetAIExecutionLogs;
using Debales.Application.AIGovernance.Queries.GetAIKnowledgeBases;
using Debales.Application.AIGovernance.Queries.GetAIRules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public sealed class AIGovernanceController : ControllerBase
{
    private readonly CreateAIRuleHandler _createRule;
    private readonly UpdateAIRuleHandler _updateRule;
    private readonly GetAIRulesHandler _getRules;
    private readonly CreateAIKnowledgeBaseHandler _createKb;
    private readonly UpdateAIKnowledgeBaseHandler _updateKb;
    private readonly GetAIKnowledgeBasesHandler _getKbs;
    private readonly CreateAIActionProposalHandler _createProposal;
    private readonly ApproveAIActionProposalHandler _approve;
    private readonly RejectAIActionProposalHandler _reject;
    private readonly GetAIActionProposalsHandler _getProposals;
    private readonly GetAIActionProposalByIdHandler _getProposalById;
    private readonly CreateAIExecutionLogHandler _createLog;
    private readonly GetAIExecutionLogsHandler _getLogs;

    public AIGovernanceController(
        CreateAIRuleHandler createRule, UpdateAIRuleHandler updateRule, GetAIRulesHandler getRules,
        CreateAIKnowledgeBaseHandler createKb, UpdateAIKnowledgeBaseHandler updateKb, GetAIKnowledgeBasesHandler getKbs,
        CreateAIActionProposalHandler createProposal, ApproveAIActionProposalHandler approve, RejectAIActionProposalHandler reject,
        GetAIActionProposalsHandler getProposals, GetAIActionProposalByIdHandler getProposalById,
        CreateAIExecutionLogHandler createLog, GetAIExecutionLogsHandler getLogs)
    {
        _createRule = createRule; _updateRule = updateRule; _getRules = getRules;
        _createKb = createKb; _updateKb = updateKb; _getKbs = getKbs;
        _createProposal = createProposal; _approve = approve; _reject = reject;
        _getProposals = getProposals; _getProposalById = getProposalById;
        _createLog = createLog; _getLogs = getLogs;
    }

    // ── Rules ────────────────────────────────────────────────────────────────

    [HttpGet("rules")]
    public async Task<IActionResult> GetRules(CancellationToken ct) =>
        Ok(await _getRules.Handle(ct));

    [HttpPost("rules")]
    public async Task<IActionResult> CreateRule([FromBody] CreateAIRuleCommand cmd, CancellationToken ct) =>
        Ok(await _createRule.Handle(cmd, ct));

    [HttpPut("rules/{id:guid}")]
    public async Task<IActionResult> UpdateRule(Guid id, [FromBody] UpdateAIRuleCommand cmd, CancellationToken ct) =>
        Ok(await _updateRule.Handle(cmd with { Id = id }, ct));

    // ── Knowledge Base ───────────────────────────────────────────────────────

    [HttpGet("knowledge")]
    public async Task<IActionResult> GetKnowledge([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default) =>
        Ok(await _getKbs.Handle(new GetAIKnowledgeBasesQuery(search, page, pageSize), ct));

    [HttpPost("knowledge")]
    public async Task<IActionResult> CreateKnowledge([FromBody] CreateAIKnowledgeBaseCommand cmd, CancellationToken ct) =>
        Ok(await _createKb.Handle(cmd, ct));

    [HttpPut("knowledge/{id:guid}")]
    public async Task<IActionResult> UpdateKnowledge(Guid id, [FromBody] UpdateAIKnowledgeBaseCommand cmd, CancellationToken ct) =>
        Ok(await _updateKb.Handle(cmd with { Id = id }, ct));

    // ── Proposals ────────────────────────────────────────────────────────────

    [HttpGet("proposals")]
    public async Task<IActionResult> GetProposals([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default) =>
        Ok(await _getProposals.Handle(new GetAIActionProposalsQuery(status, page, pageSize), ct));

    [HttpGet("proposals/{id:guid}")]
    public async Task<IActionResult> GetProposalById(Guid id, CancellationToken ct)
    {
        var result = await _getProposalById.Handle(new GetAIActionProposalByIdQuery(id), ct);
        if (result is null) return NotFound();
        return Ok(new { result.Value.Proposal, result.Value.Approvals });
    }

    [HttpPost("proposals")]
    public async Task<IActionResult> CreateProposal([FromBody] CreateAIActionProposalCommand cmd, CancellationToken ct) =>
        Ok(await _createProposal.Handle(cmd, ct));

    [HttpPost("proposals/{id:guid}/approve")]
    public async Task<IActionResult> ApproveProposal(Guid id, [FromBody] ApproveAIActionProposalCommand cmd, CancellationToken ct) =>
        Ok(await _approve.Handle(cmd with { Id = id }, ct));

    [HttpPost("proposals/{id:guid}/reject")]
    public async Task<IActionResult> RejectProposal(Guid id, [FromBody] RejectAIActionProposalCommand cmd, CancellationToken ct) =>
        Ok(await _reject.Handle(cmd with { Id = id }, ct));

    // ── Execution Logs ───────────────────────────────────────────────────────

    [HttpGet("execution-logs")]
    public async Task<IActionResult> GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default) =>
        Ok(await _getLogs.Handle(new GetAIExecutionLogsQuery(page, pageSize), ct));

    [HttpPost("execution-logs")]
    public async Task<IActionResult> CreateLog([FromBody] CreateAIExecutionLogCommand cmd, CancellationToken ct) =>
        Ok(await _createLog.Handle(cmd, ct));
}
