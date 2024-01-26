namespace ProvaPub.Services.Payment
{
    public class Creditcard : PaymentBase
    {
        private const decimal tax = 1.15M;

        public Creditcard(decimal paymentValue) : base(paymentValue) { }

        public override decimal GetPaymentValue() => paymentValue * tax;
    }
}
