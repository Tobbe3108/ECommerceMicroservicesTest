using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService orderService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService orderService, IProductsService productsService, ICustomersService customersService)
        {
            this.orderService = orderService;
            this.productsService = productsService;
            this.customersService = customersService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
        {
            var orderResult = await orderService.GetOrdersAsync(customerId);
            var customersResult = await customersService.GetCostumerAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();

            if (orderResult.IsSuccess)
            {
                foreach (var order in orderResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ? productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name : "NaN";
                    }
                }
                var result = new
                {
                    Customer = customersResult.IsSuccess ? customersResult.Customer : new { Name = "NaN" },
                    Orders = orderResult.Orders
                };
                return (true, result);
            }
            return (false, new
            {
                Customer = customersResult.IsSuccess ? customersResult.Customer : new { Name = "NaN" },
                Orders = orderResult.IsSuccess ? orderResult.Orders : null
            });
        }
    }
}