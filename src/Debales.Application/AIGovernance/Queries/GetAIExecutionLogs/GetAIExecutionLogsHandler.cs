using Debales.Application.AIGovernance.Commands.CreateAIExecutionLog;
using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;

namespace Debales.Application.AIGovernance.Queries.GetAIExecutionLogs;

public sealed class GetAIExecutionLogsHandler
{
    private readonly IAIExecutionLogRepository _repo;

    public GetAIExecutionLogsHandler(IAIExecutionLogRepository repo) => _repo = repo;

    public async Task<PagedResult<AIExecutionLogDto>> Handle(GetAIExecutionLogsQuery query, CancellationToken ct = default)
    {
        var paged = await _repo.SearchAsync(query.Page, query.PageSize, ct);
        var dtos = paged.Items.Select(CreateAIExecutionLogHandler.ToDto).ToList();
        return new PagedResult<AIExecutionLogDto>(dtos, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
