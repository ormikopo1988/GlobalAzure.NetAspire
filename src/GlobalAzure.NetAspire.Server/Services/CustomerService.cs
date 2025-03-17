using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using GlobalAzure.NetAspire.Server.Dtos;
using GlobalAzure.NetAspire.Server.Extensions;
using GlobalAzure.NetAspire.Server.Interfaces;
using GlobalAzure.NetAspire.Server.Models;
using GlobalAzure.NetAspire.Server.Options;
using GlobalAzure.NetAspire.Server.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalAzure.NetAspire.Server.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUserValidatorClient _userValidatorClient;
        private readonly IDistributedCache _distributedCache;

        public CustomerService(ApplicationDbContext applicationDbContext,
            IUserValidatorClient userValidatorClient,
            IDistributedCache distributedCache)
        {
            _applicationDbContext = applicationDbContext;
            _userValidatorClient = userValidatorClient;
            _distributedCache = distributedCache;
        }

        public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerOptions createCustomerOptions, CancellationToken ct = default)
        {
            var existingUser = await _applicationDbContext
                .Customers
                .SingleOrDefaultAsync(c => c.GitHubUsername == createCustomerOptions.GitHubUsername, ct);
            
            if (existingUser is not null)
            {
                return new Result<CustomerDto>
                {
                    Error = new Error
                    {
                        ErrorCode = ErrorCode.CustomerWithSameGitHubUsernameExists,
                        Message = $"A user with GitHub username {createCustomerOptions.GitHubUsername} already exists."
                    }
                };
            }

            var isValidGitHubUser = await _userValidatorClient.IsValidUsernameAsync(createCustomerOptions.GitHubUsername);
            
            if (!isValidGitHubUser)
            {
                return new Result<CustomerDto>
                {
                    Error = new Error
                    {
                        ErrorCode = ErrorCode.InvalidGitHubUsername,
                        Message = $"There is no GitHub user with username {createCustomerOptions.GitHubUsername}"
                    }
                };
            }

            var newCustomer = _applicationDbContext
                .Customers
                .Add(createCustomerOptions.ToCustomer());

            var result = await _applicationDbContext.SaveChangesAsync(ct);

            if (result != 1)
            {
                return new Result<CustomerDto>
                {
                    Error = new Error
                    {
                        Message = "Unable to save customer in db.",
                        ErrorCode = ErrorCode.UnableToSaveCustomer
                    }
                };
            }

            return new Result<CustomerDto>
            {
                Data = newCustomer.Entity.ToCustomerDto()
            };
        }

        public async Task<Result<CustomerDto>> GetCustomerAsync(Guid customerId, CancellationToken ct = default)
        {
            var cachedCustomer = await _distributedCache.GetAsync($"customer:{customerId}");

            if (cachedCustomer is not null)
            {
                return new Result<CustomerDto>
                {
                    Data = JsonSerializer.Deserialize<CustomerDto>(cachedCustomer)!
                };
            }

            var customer = await _applicationDbContext
                .Customers
                .SingleOrDefaultAsync(c => c.Id == customerId, ct);

            if (customer is null)
            {
                return new Result<CustomerDto>
                {
                    Error = new Error
                    {
                        ErrorCode = ErrorCode.CustomerNotFound,
                        Message = $"Customer with id {customerId} not found."
                    }
                };
            }

            var customerDto = customer.ToCustomerDto();

            await _distributedCache.SetAsync(
                $"customer:{customerId}", 
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(customerDto)), 
                new()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(60)
                }, 
                ct);

            return new Result<CustomerDto>
            {
                Data = customerDto
            };
        }

        public async Task<Result<List<CustomerDto>>> GetCustomersAsync(CancellationToken ct = default)
        {
            var customers = await _applicationDbContext
                .Customers
                .ToListAsync(ct);

            var customerDtos = customers.ToCustomerDtos();

            return new Result<List<CustomerDto>>
            {
                Data = customerDtos
            };
        }
    }
}
