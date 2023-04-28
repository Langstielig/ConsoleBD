namespace CSharp_laba3._1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivityMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nameOfActivity = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentActivities",
                c => new
                    {
                        Student_Id = c.Int(nullable: false),
                        Activity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_Id, t.Activity_Id })
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.Activities", t => t.Activity_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Activity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentActivities", "Activity_Id", "dbo.Activities");
            DropForeignKey("dbo.StudentActivities", "Student_Id", "dbo.Students");
            DropIndex("dbo.StudentActivities", new[] { "Activity_Id" });
            DropIndex("dbo.StudentActivities", new[] { "Student_Id" });
            DropTable("dbo.StudentActivities");
            DropTable("dbo.Activities");
        }
    }
}
