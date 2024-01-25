namespace ProvaPub.Models
{
    public class CustomerList
    {
        public CustomerList(List<Customer> customers) => Customers = customers;

        public List<Customer> Customers { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
    }
}
