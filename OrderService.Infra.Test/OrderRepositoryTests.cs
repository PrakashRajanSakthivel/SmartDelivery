using Xunit;
using Microsoft.EntityFrameworkCore;
using OrderService.Infra.Repository;
using OrderService.Infra.Data;
using System;
using System.Threading.Tasks;
using OrderService.Domain.Entites;

namespace OrderService.Infra.Test
{
    public class OrderRepositoryTests
    {
        private OrderDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new OrderDbContext(options);
        }

        [Fact]
        public async Task AddOrder_SavesOrderToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new OrderRepository(context);
            var order = new Order { OrderId = Guid.NewGuid(), Status = OrderStatus.PendingPayment };

            // Act
            await repository.AddAsync(order);
            await context.SaveChangesAsync();

            // Assert
            var savedOrder = await context.Orders.FindAsync(order.OrderId);
            Assert.NotNull(savedOrder);
            Assert.Equal(OrderStatus.PendingPayment, savedOrder.Status);
        }
    }
}