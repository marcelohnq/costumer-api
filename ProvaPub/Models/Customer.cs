using System.ComponentModel.DataAnnotations.Schema;

namespace ProvaPub.Models
{
    [Table("Customers")]
    public class Customer : Entity
    {
        public Customer(string name) => Name = name;

        public string Name { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
