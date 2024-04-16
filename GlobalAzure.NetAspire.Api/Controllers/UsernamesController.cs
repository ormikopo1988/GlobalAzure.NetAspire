using GlobalAzure.NetAspire.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System;
using System.Threading.Tasks;
using GlobalAzure.NetAspire.Api.Contracts.Responses;
using GlobalAzure.NetAspire.Api.Contracts.Requests;

namespace GlobalAzure.NetAspire.Api.Controllers;

[ApiController]
public class UsernamesController : ControllerBase
{
    private readonly IGitHubService _gitHubService;
    private readonly ILogger<UsernamesController> _logger;

    public UsernamesController(IGitHubService gitHubService, 
        ILogger<UsernamesController> logger)
    {
        _gitHubService = gitHubService;
        _logger = logger;
    }

    [Consumes("application/json")]
    [ProducesResponseType(typeof(ValidateUsernameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost(ApiEndpoints.Customers.Validate)]
    public async Task<IActionResult> Validate(ValidateUsernameRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.GitHubUsername))
            {
                return CreateProblemDetailsObject(HttpStatusCode.BadRequest,
                    "Bad Request",
                    "GitHub username cannot be null or whitespace.");
            }

            var isValidGitHubUser = await _gitHubService.IsValidGitHubUserAsync(request.GitHubUsername);

            return Ok(new ValidateUsernameResponse
            {
                GitHubUsername = request.GitHubUsername,
                IsValid = isValidGitHubUser
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in {MethodName} -> {ClassName}", nameof(Validate), nameof(UsernamesController));

            return CreateProblemDetailsObject(HttpStatusCode.InternalServerError,
                "Internal Server Error",
                "An error occurred while processing the request.");
        }
    }

    private static ObjectResult CreateProblemDetailsObject(HttpStatusCode httpStatusCode, string title, string detail)
    {
        var problemDetails = new ProblemDetails
        {
            Status = (int)httpStatusCode,
            Title = title,
            Detail = detail
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = (int)httpStatusCode
        };
    }
}
