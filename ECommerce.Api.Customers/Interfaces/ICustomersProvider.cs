using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Models.Customer> Products, string ErrorMessage)> GetCostumersAsync();

        Task<(bool IsSuccess, Models.Customer Product, string ErrorMessage)> GetCostumerAsync(int id);
    }
}