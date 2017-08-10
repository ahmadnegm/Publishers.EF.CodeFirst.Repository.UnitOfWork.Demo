using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities
{
    public class Book : AuditableEntity<int>
    {
        public string Title { get; set; }
        public string Isbn { get; set; }
        public int ReleaseYear { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }


        public virtual Author Author { get; set; }
        public virtual Publisher Publisher { get; set; }
    }
}
