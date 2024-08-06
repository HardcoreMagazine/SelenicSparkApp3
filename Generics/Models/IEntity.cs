namespace Generics.Models
{
    /// <summary>
    /// Generalized database entity. Implies that entity has ID: int
    /// </summary>
    public interface IEntity
    {
        int ID { get; set; }
    }
}
