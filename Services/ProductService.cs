using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService
	{
		private readonly TestDbContext _ctx;

		public ProductService(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		public ProductList ListProducts(int page)
		{
			return new(_ctx.Products.ToList()) { HasNext=false, TotalCount=10 };
		}
	}
}
