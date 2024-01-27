using Moq;
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
            _ctxCustomerMock = new Mock<IGenericDbContext<Customer>>();
            _ctxOrderMock = new Mock<IGenericDbContext<Order>>();
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
            _ = _ctxCustomerMock.SetupSequence(c => c.Get(1)).ReturnsAsync((Customer?)null);
            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _ = _customerService.CanPurchase(1, 1000));
            Assert.Equal("Customer Id 1 does not exists", exception.Message);
        }

        [Fact]
        public async Task CanPurchase_OnlySingleTimeMonth()
        {
            Customer customer = new("Teste") { Id = 1 };

            // Customer existe
            _ = _ctxCustomerMock.SetupSequence(c => c.Get(1)).ReturnsAsync(customer);
            // Já comprou no mês
            _ = _ctxOrderMock.SetupSequence(o => o.Count(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync(1);
            Assert.False(await _customerService.CanPurchase(1, 1000));
        }

        [Theory]
        [InlineData(1000, false)]
        [InlineData(101, false)]
        [InlineData(100, true)]
        [InlineData(99, true)]
        public async Task CanPurchase_FirstPurchaseMax(decimal payment, bool expected)
        {
            Customer customer = new("Teste") { Id = 1 };

            // Customer existe
            _ = _ctxCustomerMock.SetupSequence(c => c.Get(1)).ReturnsAsync(customer);
            // Não comprou no mês
            _ = _ctxOrderMock.SetupSequence(o => o.Count(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync(0);
            // Nunca comprou
            _ = _ctxCustomerMock.SetupSequence(c => c.Count(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(0);
            Assert.Equal(expected, await _customerService.CanPurchase(1, payment));
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(101)]
        [InlineData(100)]
        [InlineData(99)]
        public async Task CanPurchase(decimal payment)
        {
            Customer customer = new("Teste") { Id = 1 };

            // Customer existe
            _ = _ctxCustomerMock.SetupSequence(c => c.Get(1)).ReturnsAsync(customer);
            // Não comprou no mês
            _ = _ctxOrderMock.SetupSequence(o => o.Count(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync(0);
            // Já fez a primeira compra
            _ = _ctxCustomerMock.SetupSequence(c => c.Count(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(1);
            Assert.True(await _customerService.CanPurchase(1, payment));
        }
    }
}