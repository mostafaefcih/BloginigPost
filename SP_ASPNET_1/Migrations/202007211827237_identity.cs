namespace SP_ASPNET_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class identity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlogPost", "Author_AuthorID", "dbo.Author");
            DropForeignKey("dbo.PostLike", "AuthorId", "dbo.Author");
            DropIndex("dbo.BlogPost", new[] { "Author_AuthorID" });
            DropIndex("dbo.PostComment", new[] { "AuthorId" });
            DropIndex("dbo.PostLike", new[] { "AuthorId" });
            RenameColumn(table: "dbo.BlogPost", name: "Author_AuthorID", newName: "AuthorID");
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 300),
                        Surname = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            AlterColumn("dbo.BlogPost", "Title", c => c.String(maxLength: 200));
            AlterColumn("dbo.BlogPost", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.BlogPost", "AuthorID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PostComment", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PostComment", "Content", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.PostLike", "AuthorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.BlogPost", "AuthorID");
            CreateIndex("dbo.PostComment", "AuthorId");
            CreateIndex("dbo.PostLike", "AuthorId");
            AddForeignKey("dbo.BlogPost", "AuthorID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PostLike", "AuthorId", "dbo.AspNetUsers", "Id");
            DropTable("dbo.Author");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        AuthorID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                    })
                .PrimaryKey(t => t.AuthorID);
            
            DropForeignKey("dbo.PostLike", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BlogPost", "AuthorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PostLike", new[] { "AuthorId" });
            DropIndex("dbo.PostComment", new[] { "AuthorId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.BlogPost", new[] { "AuthorID" });
            AlterColumn("dbo.PostLike", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("dbo.PostComment", "Content", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.PostComment", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("dbo.BlogPost", "AuthorID", c => c.Int());
            AlterColumn("dbo.BlogPost", "Content", c => c.String());
            AlterColumn("dbo.BlogPost", "Title", c => c.String());
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            RenameColumn(table: "dbo.BlogPost", name: "AuthorID", newName: "Author_AuthorID");
            CreateIndex("dbo.PostLike", "AuthorId");
            CreateIndex("dbo.PostComment", "AuthorId");
            CreateIndex("dbo.BlogPost", "Author_AuthorID");
            AddForeignKey("dbo.PostLike", "AuthorId", "dbo.Author", "AuthorID", cascadeDelete: true);
            AddForeignKey("dbo.BlogPost", "Author_AuthorID", "dbo.Author", "AuthorID");
        }
    }
}
