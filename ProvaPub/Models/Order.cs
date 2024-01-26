using System.ComponentModel.DataAnnotations.Schema;

namespace ProvaPub.Models
{
    [Table("Orders")]
    public class Order : Entity
    {
        public decimal Value { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer? Customer { get; set; }
    }
}
