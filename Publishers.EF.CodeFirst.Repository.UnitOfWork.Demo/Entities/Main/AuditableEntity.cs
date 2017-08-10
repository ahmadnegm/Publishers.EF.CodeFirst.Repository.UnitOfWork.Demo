using System;
using System.ComponentModel.DataAnnotations;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main.Interfaces;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main
{
    public abstract class AuditableEntity<T> : BaseEntity<T>, IAuditableEntity
    {
        [ScaffoldColumn(false)]
        public DateTime Created { get; set; }


        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime Modified { get; set; }

        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
    }
}
