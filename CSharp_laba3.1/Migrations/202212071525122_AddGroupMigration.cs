namespace CSharp_laba3._1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameOfTeacher = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Students", "GroupId", c => c.Int());
            CreateIndex("dbo.Students", "GroupId");
            AddForeignKey("dbo.Students", "GroupId", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropColumn("dbo.Students", "GroupId");
            DropTable("dbo.Groups");
        }
    }
}
