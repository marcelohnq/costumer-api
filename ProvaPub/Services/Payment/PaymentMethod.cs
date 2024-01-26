namespace ProvaPub.Services.Payment
{
    public enum PaymentMethod
    {
        Pix,
        Creditcard,
        Paypal
    }

    public static class PaymentClass
    {
        public static PaymentBase Get(PaymentMethod pm, decimal paymentValue) => pm switch
        {
            PaymentMethod.Pix => new Pix(paymentValue),
            PaymentMethod.Creditcard => new Creditcard(paymentValue),
            PaymentMethod.Paypal => new Paypal(paymentValue),
            _ => throw new ArgumentException("The payment method does not exist.")
        };
    }
}
