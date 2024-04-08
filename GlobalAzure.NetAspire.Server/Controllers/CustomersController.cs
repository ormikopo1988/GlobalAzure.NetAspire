using GlobalAzure.NetAspire.Server.Extensions;
using GlobalAzure.NetAspire.Server.Contracts.Requests;
using GlobalAzure.NetAspire.Server.Contracts.Responses;
using GlobalAzure.NetAspire.Server.Interfaces;
using GlobalAzure.NetAspire.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace GlobalAzure.NetAspire.Server.Controllers
{
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [Consumes("application/json")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost(ApiEndpoints.Customers.Create)]
        public async Task<IActionResult> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var customerResult = await _customerService.CreateCustomerAsync(request.ToCustomerOptions(), cancellationToken);

                if (customerResult.Data is not null)
                {
                    return CreatedAtAction(nameof(GetById), new { id = customerResult.Data.Id }, customerResult.Data.ToCustomerResponse());
                }

                return ProblemDetailsResultBasedOnError(customerResult.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in {MethodName} -> {ClassName}", nameof(Create), nameof(CustomersController));

                return CreateProblemDetailsObject(HttpStatusCode.InternalServerError,
                    "Internal Server Error",
                    "An error occurred while processing the request.");
            }
        }

        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet(ApiEndpoints.Customers.GetAll)]

        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var customersResult = await _customerService.GetCustomersAsync(cancellationToken);

                if (customersResult.Data is not null)
                {
                    return Ok(customersResult.Data.ToCustomersResponse());
                }

                return ProblemDetailsResultBasedOnError(customersResult.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in {MethodName} -> {ClassName}", nameof(GetAll), nameof(CustomersController));

                return CreateProblemDetailsObject(HttpStatusCode.InternalServerError,
                    "Internal Server Error",
                    "An error occurred while processing the request.");
            }
        }

        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet(ApiEndpoints.Customers.GetById)]

        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var customerResult = await _customerService.GetCustomerAsync(id, cancellationToken);

                if (customerResult.Data is not null)
                {
                    return Ok(customerResult.Data.ToCustomerResponse());
                }

                return ProblemDetailsResultBasedOnError(customerResult.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in {MethodName} -> {ClassName}", nameof(GetById), nameof(CustomersController));

                return CreateProblemDetailsObject(HttpStatusCode.InternalServerError,
                    "Internal Server Error",
                    "An error occurred while processing the request.");
            }
        }

        private static ObjectResult ProblemDetailsResultBasedOnError(Error error)
        {
            return error.ErrorCode switch
            {
                ErrorCode.CustomerNotFound =>
                    CreateProblemDetailsObject(HttpStatusCode.NotFound,
                        "Not Found",
                        error.Message),
                ErrorCode.CustomerWithSameGitHubUsernameExists or ErrorCode.InvalidGitHubUsername =>
                    CreateProblemDetailsObject(HttpStatusCode.BadRequest,
                        "Bad Request",
                        error.Message),
                _ => CreateProblemDetailsObject(HttpStatusCode.InternalServerError,
                        "Internal Server Error",
                        error.Message),
            };
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
}
