namespace CSharp_laba3._1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Surname", c => c.String());
            AddColumn("dbo.Students", "Age", c => c.Int(nullable: false));
            //тут можно добавить номер группы
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Age");
            DropColumn("dbo.Students", "Surname");
        }
    }
}
