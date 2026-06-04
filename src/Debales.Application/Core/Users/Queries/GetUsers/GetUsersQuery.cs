namespace Debales.Application.Core.Users.Queries.GetUsers;

public sealed record GetUsersQuery(string? Search = null, bool? IsActive = null);
