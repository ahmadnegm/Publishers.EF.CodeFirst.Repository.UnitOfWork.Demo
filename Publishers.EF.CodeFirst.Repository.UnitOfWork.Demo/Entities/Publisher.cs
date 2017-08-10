using System.Collections.Generic;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities
{
    public class Publisher : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }


        public virtual ICollection<Book> Books { get; set; }
    }
}
