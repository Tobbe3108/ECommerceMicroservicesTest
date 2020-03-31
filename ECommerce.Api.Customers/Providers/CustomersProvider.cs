using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly IMapper mapper;
        private readonly ILogger logger;
        private readonly CustomersDbContext dbContext;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.dbContext = dbContext;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Add(new Db.Customer() { Id = 1, Name = "Tobias", Address = "Test Vej 12" });
                dbContext.Add(new Db.Customer() { Id = 2, Name = "Janni", Address = "Test Vej 12" });
                dbContext.Add(new Db.Customer() { Id = 3, Name = "Ole", Address = "Hansens Vej 1444" });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Products, string ErrorMessage)> GetCostumersAsync()
        {
            try
            {
                logger?.LogInformation("Query Customers");
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    logger?.LogInformation($"{customers.Count} customer(s) found");
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Product, string ErrorMessage)> GetCostumerAsync(int id)
        {
            try
            {
                logger?.LogInformation("Query Customers");
                var product = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (product != null)
                {
                    logger?.LogInformation("Customer(s) found");
                    var result = mapper.Map<Db.Customer, Models.Customer>(product);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}