namespace ProvaPub.Models
{
    public class GenericList<T>
    {
        public GenericList(List<T> itens) => Itens = itens;

        public List<T> Itens { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
    }
}
