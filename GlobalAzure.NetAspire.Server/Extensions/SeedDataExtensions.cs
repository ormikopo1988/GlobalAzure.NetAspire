using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using GlobalAzure.NetAspire.Server.Data;
using System.Linq;
using GlobalAzure.NetAspire.Server.Data.Entities;

namespace GlobalAzure.NetAspire.Server.Extensions
{
    public static class SeedDataExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            if (!applicationDbContext.Customers.Any())
            {
                applicationDbContext.Customers.Add(new Customer
                {
                    Email = "orestis.meikopoulos@gmail.com",
                    FirstName = "Orestis",
                    LastName = "Meikopoulos",
                    GitHubUsername = "ormikopo1988"
                });

                applicationDbContext.SaveChanges();
            }
        }
    }
}
