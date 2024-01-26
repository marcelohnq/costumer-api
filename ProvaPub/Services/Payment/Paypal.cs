namespace ProvaPub.Services.Payment
{
    public class Paypal : PaymentBase
    {
        private const decimal tax = 1.1M;

        public Paypal(decimal paymentValue) : base(paymentValue) { }

        public override decimal GetPaymentValue() => paymentValue * tax;
    }
}
