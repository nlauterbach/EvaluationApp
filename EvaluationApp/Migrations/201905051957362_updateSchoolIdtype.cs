namespace EvaluationApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSchoolIdtype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "SchoolId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "SchoolId", c => c.Int(nullable: false));
        }
    }
}
