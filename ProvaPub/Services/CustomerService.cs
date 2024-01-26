using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class CustomerService
    {
        private readonly IGenericDbContext<Customer> _ctxCustomer;
        private readonly IGenericDbContext<Order> _ctxOrder;

        public CustomerService(IGenericDbContext<Customer> ctxCustomer, IGenericDbContext<Order> ctxOrder)
        {
            _ctxCustomer = ctxCustomer;
            _ctxOrder = ctxOrder;
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));
            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            //Business Rule: Non registered Customers cannot purchase
            Customer customer = await _ctxCustomer.Get(customerId) ?? throw new InvalidOperationException($"Customer Id {customerId} does not exists");

            //Business Rule: A customer can purchase only a single time per month
            DateTime baseDate = DateTime.UtcNow.AddMonths(-1);
            int ordersInThisMonth = await _ctxOrder.Count(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            if (ordersInThisMonth > 0)
            {
                return false;
            }

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            int haveBoughtBefore = await _ctxCustomer.Count(s => s.Id == customerId && s.Orders != null && s.Orders.Any());
            
            return haveBoughtBefore != 0 || purchaseValue <= 100;
        }

    }
}
