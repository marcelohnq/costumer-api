using ProvaPub.Models;

namespace ProvaPub.Services.Payment
{
    public abstract class PaymentBase
    {
        protected decimal paymentValue;

        public PaymentBase(decimal paymentValue)
        {
            this.paymentValue = paymentValue;
        }

        public abstract decimal GetPaymentValue();

        public Order MakePayment(int customerId)
        {
            return new()
            {
                Value = GetPaymentValue(),
                CustomerId = customerId,
                OrderDate = DateTime.Now,
            };
        }
    }
}
