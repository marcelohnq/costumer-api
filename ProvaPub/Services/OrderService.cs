using ProvaPub.Models;
using ProvaPub.Services.Payment;

namespace ProvaPub.Services
{
    public class OrderService
    {
        public async Task<Order> PayOrder(PaymentBase payment, int customerId)
        {
            return await Task.FromResult
            (
                payment.MakePayment(customerId)
            );
        }
    }
}
