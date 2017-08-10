namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        DateOfBirth = c.DateTime(),
                        Mail = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        Modified = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.FirstName)
                .Index(t => t.LastName);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Isbn = c.String(nullable: false, maxLength: 20),
                        ReleaseYear = c.Int(),
                        AuthorId = c.Int(nullable: false),
                        PublisherId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        Modified = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Publishers", t => t.PublisherId)
                .Index(t => t.Title)
                .Index(t => t.Isbn)
                .Index(t => t.AuthorId)
                .Index(t => t.PublisherId);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 500),
                        Phone = c.String(maxLength: 50),
                        Website = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        Modified = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "PublisherId", "dbo.Publishers");
            DropForeignKey("dbo.Books", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Publishers", new[] { "Name" });
            DropIndex("dbo.Books", new[] { "PublisherId" });
            DropIndex("dbo.Books", new[] { "AuthorId" });
            DropIndex("dbo.Books", new[] { "Isbn" });
            DropIndex("dbo.Books", new[] { "Title" });
            DropIndex("dbo.Authors", new[] { "LastName" });
            DropIndex("dbo.Authors", new[] { "FirstName" });
            DropTable("dbo.Publishers");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
