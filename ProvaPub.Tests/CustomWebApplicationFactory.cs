using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProvaPub.Repository;
using Testcontainers.MsSql;

namespace ProvaPub.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer;

        public CustomWebApplicationFactory()
        {
            _msSqlContainer = new MsSqlBuilder().Build();
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();
            using IServiceScope scope = Services.CreateScope();
            TestDbContext dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<TestDbContext>));
                services.AddDbContext<TestDbContext>((_, option) => option.UseSqlServer(_msSqlContainer.GetConnectionString()));
            });
        }

        public async new Task DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync().AsTask();
        }
    }
}
