namespace AbstractPizzeriaService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        WorkerId = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Workers", t => t.WorkerId)
                .Index(t => t.CustomerId)
                .Index(t => t.ArticleId)
                .Index(t => t.WorkerId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleIngridients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        IngridientId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Ingridients", t => t.IngridientId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.IngridientId);
            
            CreateTable(
                "dbo.Ingridients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IngridientName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResourceIngridients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourceId = c.Int(nullable: false),
                        IngridientId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingridients", t => t.IngridientId, cascadeDelete: true)
                .ForeignKey("dbo.Resources", t => t.ResourceId, cascadeDelete: true)
                .Index(t => t.ResourceId)
                .Index(t => t.IngridientId);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResourceName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "WorkerId", "dbo.Workers");
            DropForeignKey("dbo.Requests", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Requests", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ResourceIngridients", "ResourceId", "dbo.Resources");
            DropForeignKey("dbo.ResourceIngridients", "IngridientId", "dbo.Ingridients");
            DropForeignKey("dbo.ArticleIngridients", "IngridientId", "dbo.Ingridients");
            DropForeignKey("dbo.ArticleIngridients", "ArticleId", "dbo.Articles");
            DropIndex("dbo.ResourceIngridients", new[] { "IngridientId" });
            DropIndex("dbo.ResourceIngridients", new[] { "ResourceId" });
            DropIndex("dbo.ArticleIngridients", new[] { "IngridientId" });
            DropIndex("dbo.ArticleIngridients", new[] { "ArticleId" });
            DropIndex("dbo.Requests", new[] { "WorkerId" });
            DropIndex("dbo.Requests", new[] { "ArticleId" });
            DropIndex("dbo.Requests", new[] { "CustomerId" });
            DropTable("dbo.Workers");
            DropTable("dbo.Resources");
            DropTable("dbo.ResourceIngridients");
            DropTable("dbo.Ingridients");
            DropTable("dbo.ArticleIngridients");
            DropTable("dbo.Articles");
            DropTable("dbo.Requests");
            DropTable("dbo.Customers");
        }
    }
}
