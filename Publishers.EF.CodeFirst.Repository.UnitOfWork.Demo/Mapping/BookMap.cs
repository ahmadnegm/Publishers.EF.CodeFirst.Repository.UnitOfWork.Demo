using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Mapping
{
    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            #region Table Definition

            ToTable("Books");
            HasKey(k => k.Id);

            #endregion

            #region Length, Type and Required

            Property(p => p.Title).HasMaxLength(100).IsRequired();
            Property(p => p.Isbn).HasMaxLength(20).IsRequired();
            Property(p => p.AuthorId).IsRequired();
            Property(p => p.PublisherId).IsRequired();

            #endregion

            #region Length, Type and Optional

            Property(p => p.ReleaseYear).IsOptional();

            #endregion

            #region Indexes

            Property(i => i.Title).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(i => i.Isbn).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(i => i.AuthorId).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            #endregion

            #region Relationships

            

            #endregion
        }
    }
}
