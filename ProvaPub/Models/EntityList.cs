namespace ProvaPub.Models
{
    public class EntityList<Entity>
    {
        public EntityList(List<Entity> itens) => Itens = itens;

        public List<Entity> Itens { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
    }
}
