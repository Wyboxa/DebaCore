using Debales.Application.Core.Users.Commands.CreateUser;
using Debales.Application.Core.Users.DTOs;
using Debales.Application.Core.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly CreateUserHandler _createUser;
    private readonly GetUserByIdHandler _getUserById;

    public UsersController(CreateUserHandler createUser, GetUserByIdHandler getUserById)
    {
        _createUser = createUser;
        _getUserById = getUserById;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _getUserById.Handle(new GetUserByIdQuery(id), cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateUserCommand(request.Username, request.Email, request.Password, "api");
            var user = await _createUser.Handle(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public sealed record CreateUserRequest(string Username, string Email, string Password);
