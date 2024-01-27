using Moq;
using Org.BouncyCastle.Asn1.X509;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using System.Linq.Expressions;

namespace ProvaPub.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;
        private readonly Mock<IGenericDbContext<Customer>> _ctxCustomerMock;
        private readonly Mock<IGenericDbContext<Order>> _ctxOrderMock;

        public CustomerServiceTests()
        {
            _ctxCustomerMock = new Mock<IGenericDbContext<Customer>>(MockBehavior.Strict);
            _ctxOrderMock = new Mock<IGenericDbContext<Order>>(MockBehavior.Strict);
            _customerService = new(_ctxCustomerMock.Object, _ctxOrderMock.Object);
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(-1, 133.55)]
        public async Task CanPurchase_ArgumentOutOfRangeException(int customerId, decimal purchaseValue)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _ = _customerService.CanPurchase(customerId, purchaseValue));
        }

        [Fact]
        public async Task CanPurchase_InvalidOperationException()
        {
            // Customer não existe
            _ = _ctxCustomerMock.Setup(c => c.Get(1)).ReturnsAsync((Customer?)null);
            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _ = _customerService.CanPurchase(1, 1000));
            Assert.Equal("Customer Id 1 does not exists", exception.Message);
        }

        [Fact]
        public async Task CanPurchase_OnlySingleTimeMonth()
        {
            Customer customer = new("Teste") { Id = 1 };
            Order order = new() { CustomerId = customer.Id, OrderDate = DateTime.UtcNow.AddMonths(-1).Date };

            // Customer existe
            _ = _ctxCustomerMock.Setup(c => c.Get(1)).ReturnsAsync(customer);
            // Já comprou no mês
            _ = _ctxOrderMock.Setup(o => o.Count(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync(
                (Expression<Func<Order, bool>> param) => MockCount(param, order));
            Assert.False(await _customerService.CanPurchase(1, 10));
        }

        [Theory]
        [InlineData(1000, false)]
        [InlineData(101, false)]
        [InlineData(100, true)]
        [InlineData(99, true)]
        public async Task CanPurchase_FirstPurchaseMax(decimal payment, bool expected)
        {
            Customer customer = new("Teste") { Id = 1 };
            Order order = new();

            // Customer existe
            _ = _ctxCustomerMock.Setup(c => c.Get(1)).ReturnsAsync(customer);
            // Não comprou no mês
            _ = _ctxOrderMock.Setup(o => o.Count(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync(
                (Expression<Func<Order, bool>> param) => MockCount(param, order));
            // Nunca comprou
            _ = _ctxCustomerMock.Setup(c => c.Count(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(
                (Expression<Func<Customer, bool>> param) => MockCount(param, customer));
            Assert.Equal(expected, await _customerService.CanPurchase(1, payment));
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(101)]
        [InlineData(100)]
        [InlineData(99)]
        public async Task CanPurchase(decimal payment)
        {
            Customer customer = new("Teste")
            {
                Id = 1,
                Orders = new List<Order>() { new() { CustomerId = 1, OrderDate = DateTime.UtcNow.AddMonths(-2).Date } }
            };

            // Customer existe
            _ = _ctxCustomerMock.Setup(c => c.Get(1)).ReturnsAsync(customer);
            // Não comprou no mês
            _ = _ctxOrderMock.Setup(o => o.Count(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync(
                (Expression<Func<Order, bool>> param) => MockCount(param, customer.Orders.First()));
            // Já fez a primeira compra
            _ = _ctxCustomerMock.Setup(c => c.Count(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(
                (Expression<Func<Customer, bool>> param) => MockCount(param, customer));
            Assert.True(await _customerService.CanPurchase(1, payment));
        }

        private static int MockCount<T>(Expression<Func<T, bool>> param, T entity)
        {
            if (param.Compile().Invoke(entity))
            {
                return 1;
            }

            return 0;
        }
    }
}