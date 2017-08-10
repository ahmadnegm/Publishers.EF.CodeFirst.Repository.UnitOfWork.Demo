namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
