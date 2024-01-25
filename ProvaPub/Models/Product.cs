using System.ComponentModel.DataAnnotations.Schema;

namespace ProvaPub.Models
{
    [Table("Products")]
    public class Product
    {
        public Product(string name) => Name = name;

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
