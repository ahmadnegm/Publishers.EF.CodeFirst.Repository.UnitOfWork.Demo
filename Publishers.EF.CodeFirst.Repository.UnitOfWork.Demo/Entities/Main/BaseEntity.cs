using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main.Interfaces;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main
{
    public class BaseEntity<TK> : Entity, IEntity<TK>
    {
        public virtual TK Id { get; set; }
    }
}
