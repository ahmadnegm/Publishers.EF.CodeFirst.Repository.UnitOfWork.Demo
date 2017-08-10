using System;
using System.Collections.Generic;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities
{
    public class Author : AuditableEntity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Mail { get; set; }


        public virtual ICollection<Book> Books { get; set; }
    }
}
