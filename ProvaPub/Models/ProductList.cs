namespace ProvaPub.Models
{
	public class ProductList
	{
        public ProductList(List<Product> products) => Products = products;

        public List<Product> Products { get; set; }
		public int TotalCount { get; set; }
		public bool HasNext { get; set; }
	}
}
