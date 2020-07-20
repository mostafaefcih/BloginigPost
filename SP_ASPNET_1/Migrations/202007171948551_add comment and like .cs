namespace SP_ASPNET_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcommentandlike : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostComment",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(storeType: "ntext"),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Author", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.BlogPost", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.PostLike",
                c => new
                    {
                        LikeId = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LikeId)
                .ForeignKey("dbo.Author", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.BlogPost", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.AuthorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostLike", "PostId", "dbo.BlogPost");
            DropForeignKey("dbo.PostLike", "AuthorId", "dbo.Author");
            DropForeignKey("dbo.PostComment", "PostId", "dbo.BlogPost");
            DropForeignKey("dbo.PostComment", "AuthorId", "dbo.Author");
            DropIndex("dbo.PostLike", new[] { "AuthorId" });
            DropIndex("dbo.PostLike", new[] { "PostId" });
            DropIndex("dbo.PostComment", new[] { "AuthorId" });
            DropIndex("dbo.PostComment", new[] { "PostId" });
            DropTable("dbo.PostLike");
            DropTable("dbo.PostComment");
        }
    }
}
