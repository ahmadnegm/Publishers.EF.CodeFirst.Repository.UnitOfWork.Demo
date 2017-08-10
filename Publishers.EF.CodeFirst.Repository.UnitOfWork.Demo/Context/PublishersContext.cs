using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Context
{
    public class PublishersContext:DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }


        static PublishersContext()
        {
            Database.SetInitializer<PublishersContext>(null);
        }

        public PublishersContext() : base("name=PublishersConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public static PublishersContext Create()
        {
            return new PublishersContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // apply *Map classes configurations to db
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic) configurationInstance);
            }
        }
    }
}
