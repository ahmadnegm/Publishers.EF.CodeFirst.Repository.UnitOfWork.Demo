using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Mapping
{
    public class PublisherMap : EntityTypeConfiguration<Publisher>
    {
        public PublisherMap()
        {
            #region Table Definition

            ToTable("Publishers");
            HasKey(k => k.Id);

            #endregion

            #region Length, Type and Required

            Property(p => p.Name).HasMaxLength(100).IsRequired();
            Property(p => p.Address).HasMaxLength(500).IsRequired();

            #endregion

            #region Length, Type and Optional

            Property(p => p.Phone).HasMaxLength(50).IsOptional();
            Property(p => p.Website).HasMaxLength(100).IsOptional();

            #endregion

            #region Indexes

            Property(i => i.Name).HasColumnAnnotation(IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute()));

            #endregion

            #region Relationships

            // this is to handle the one-to-many relationship between Publishers and Books
            // as Publisher has many books, and each book has a required property to accept PublisherId

            HasMany(m => m.Books)
                .WithRequired()
                .HasForeignKey(o => o.PublisherId)
                .WillCascadeOnDelete(false);

            #endregion
        }
    }
}