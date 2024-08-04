namespace Generics.Models
{
    /// <summary>
    /// Generalized database entity. Implies that entity has ID: int
    /// </summary>
    public interface IEntity
    {
        public int ID { get; set; }
    }
}
