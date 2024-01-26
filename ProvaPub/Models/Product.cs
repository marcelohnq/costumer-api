using System.ComponentModel.DataAnnotations.Schema;

namespace ProvaPub.Models
{
    [Table("Products")]
    public class Product : Entity
    {
        public Product(string name) => Name = name;

        public string Name { get; set; }
    }
}
