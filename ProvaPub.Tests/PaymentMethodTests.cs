using ProvaPub.Services.Payment;

namespace ProvaPub.Tests
{
    public class PaymentMethodTests
    {
        [Theory]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(1.0, 1.0)]
        [InlineData(100.0, 100.0)]
        [InlineData(345345345.0, 345345345.0)]
        [InlineData(10.55, 10.55)]
        public void PixTest(decimal value, decimal expected)
        {
            PaymentBase payment = new Pix(value);
            Assert.Equal(expected, payment.GetPaymentValue());
        }

        [Theory]
        [InlineData(-1, -1.15)]
        [InlineData(0, 0)]
        [InlineData(1, 1.15)]
        [InlineData(1.0, 1.15)]
        [InlineData(100.0, 115.0)]
        [InlineData(345345345.0, 397147146.75)]
        [InlineData(10.55, 12.1325)]
        public void CreditcardTest(decimal value, decimal expected)
        {
            PaymentBase payment = new Creditcard(value);
            Assert.Equal(expected, payment.GetPaymentValue());
        }

        [Theory]
        [InlineData(-1, -1.1)]
        [InlineData(0, 0)]
        [InlineData(1, 1.1)]
        [InlineData(1.0, 1.1)]
        [InlineData(100.0, 110.0)]
        [InlineData(345345345.0, 379879879.5)]
        [InlineData(10.55, 11.605)]
        public void PaypalTest(decimal value, decimal expected)
        {
            PaymentBase payment = new Paypal(value);
            Assert.Equal(expected, payment.GetPaymentValue());
        }

        [Theory]
        [InlineData(PaymentMethod.Pix, typeof(Pix))]
        [InlineData(PaymentMethod.Creditcard, typeof(Creditcard))]
        [InlineData(PaymentMethod.Paypal, typeof(Paypal))]
        public void PaymentClassTest(PaymentMethod method, Type expected)
        {
            Assert.IsType(expected, PaymentClass.Get(method, 1.0M));
        }
    }
}
