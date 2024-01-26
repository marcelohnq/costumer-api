using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Payment;

namespace ProvaPub.Services
{
    public class OrderService
    {
        private readonly IGenericDbContext<Order> _ctxOrder;

        public OrderService(IGenericDbContext<Order> ctxOrder)
        {
            _ctxOrder = ctxOrder;
        }

        public async Task<Order> PayOrder(PaymentBase payment, int customerId)
        {
            Order order = payment.MakePayment(customerId);

            if (order != null && await _ctxOrder.Add(order))
            {
                return order;
            }

            return new();
        }
    }
}
