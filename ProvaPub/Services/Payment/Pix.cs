namespace ProvaPub.Services.Payment
{
    public class Pix : PaymentBase
    {
        public Pix(decimal paymentValue) : base(paymentValue) {}

        public override decimal GetPaymentValue() => paymentValue;
    }
}
