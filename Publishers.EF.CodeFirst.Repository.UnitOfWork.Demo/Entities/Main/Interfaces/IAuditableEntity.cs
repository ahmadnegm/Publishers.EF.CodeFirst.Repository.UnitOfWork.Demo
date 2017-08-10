using System;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime Created { get; set; }
        string CreatedBy { get; set; }
        DateTime Modified { get; set; }
        string ModifiedBy { get; set; }
    }
}
