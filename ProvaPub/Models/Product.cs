namespace ProvaPub.Models
{
    public class Product
    {
        public Product(string name) => Name = name;

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
