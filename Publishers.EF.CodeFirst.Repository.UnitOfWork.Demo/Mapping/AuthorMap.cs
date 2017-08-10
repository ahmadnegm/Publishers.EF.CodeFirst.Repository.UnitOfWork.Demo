using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Mapping
{
    public class AuthorMap : EntityTypeConfiguration<Author>
    {
        public AuthorMap()
        {
            #region Table Definition

            ToTable("Authors");
            HasKey(k => k.Id);

            #endregion

            #region Length, Type and Required

            Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            Property(p => p.LastName).HasMaxLength(50).IsRequired();

            #endregion

            #region Length, Type and Optional

            Property(p => p.DateOfBirth).IsOptional();
            Property(p => p.Mail).HasMaxLength(100).IsOptional();

            #endregion

            #region Indexes

            Property(i => i.FirstName).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));
            Property(i => i.LastName).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            #endregion

            #region Relationships

            // this is to handle the one-to-many relationship between Authors and Books
            // as Author has many books, and each book has a required property to accept AuthorId

            HasMany(m => m.Books)
                .WithRequired()
                .HasForeignKey(o => o.AuthorId)
                .WillCascadeOnDelete(false);

            #endregion
        }
    }
}
