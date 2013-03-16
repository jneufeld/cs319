namespace DREAM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "Gender", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "Gender");
        }
    }
}
