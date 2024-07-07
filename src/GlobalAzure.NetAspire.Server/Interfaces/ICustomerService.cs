using GlobalAzure.NetAspire.Server.Dtos;
using GlobalAzure.NetAspire.Server.Models;
using GlobalAzure.NetAspire.Server.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GlobalAzure.NetAspire.Server.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<CustomerDto>> GetCustomerAsync(Guid customerId, CancellationToken ct = default);

        Task<Result<List<CustomerDto>>> GetCustomersAsync(CancellationToken ct = default);

        Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerOptions createCustomerOptions, CancellationToken ct = default);
    }
}
