namespace EvaluationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchoolId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SchoolId", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SchoolId");
        }
    }
}
