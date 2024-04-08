using GlobalAzure.NetAspire.Server.Contracts.Requests;
using GlobalAzure.NetAspire.Server.Contracts.Responses;
using GlobalAzure.NetAspire.Server.Data.Entities;
using GlobalAzure.NetAspire.Server.Dtos;
using GlobalAzure.NetAspire.Server.Options;
using System.Collections.Generic;
using System.Linq;

namespace GlobalAzure.NetAspire.Server.Extensions
{
    public static class CustomerExtensions
    {
        public static CustomerDto ToCustomerDto(this Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                GitHubUsername = customer.GitHubUsername,
                LastName = customer.LastName
            };
        }

        public static List<CustomerDto> ToCustomerDtos(this List<Customer> customers)
        {
            return customers.Select(c => c.ToCustomerDto()).ToList();
        }

        public static CustomerResponse ToCustomerResponse(this CustomerDto customer)
        {
            return new CustomerResponse
            {
                Id = customer.Id,
                FullName = $"{customer.FirstName} {customer.LastName}",
                GitHubUsername = customer.GitHubUsername
            };
        }

        public static List<CustomerResponse> ToCustomersResponse(this List<CustomerDto> customers)
        {
            return customers.Select(c => c.ToCustomerResponse()).ToList();
        }

        public static CreateCustomerOptions ToCustomerOptions(this CreateCustomerRequest customer)
        {
            return new CreateCustomerOptions
            {
                FirstName = customer.FirstName,
                GitHubUsername = customer.GitHubUsername,
                LastName = customer.LastName,
            };
        }

        public static Customer ToCustomer(this CreateCustomerOptions customer)
        {
            return new Customer
            {
                FirstName = customer.FirstName,
                GitHubUsername = customer.GitHubUsername,
                LastName = customer.LastName
            };
        }
    }
}
