using System.ComponentModel.DataAnnotations.Schema;

namespace ProvaPub.Models
{
    [Table("Customers")]
    public class Customer
    {
        public Customer(string name) => Name = name;

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
