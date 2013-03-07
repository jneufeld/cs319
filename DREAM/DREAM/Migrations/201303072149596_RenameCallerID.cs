namespace DREAM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameCallerID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "CallerID_ID", "dbo.Callers");
            DropForeignKey("dbo.Requests", "PatientID_ID", "dbo.Patients");
            DropIndex("dbo.Requests", new[] { "CallerID_ID" });
            DropIndex("dbo.Requests", new[] { "PatientID_ID" });
            AddColumn("dbo.Requests", "Caller_ID", c => c.Int());
            AddColumn("dbo.Requests", "Patient_ID", c => c.Int());
            AddForeignKey("dbo.Requests", "Caller_ID", "dbo.Callers", "ID");
            AddForeignKey("dbo.Requests", "Patient_ID", "dbo.Patients", "ID");
            CreateIndex("dbo.Requests", "Caller_ID");
            CreateIndex("dbo.Requests", "Patient_ID");
            DropColumn("dbo.Requests", "CallerID_ID");
            DropColumn("dbo.Requests", "PatientID_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "PatientID_ID", c => c.Int());
            AddColumn("dbo.Requests", "CallerID_ID", c => c.Int());
            DropIndex("dbo.Requests", new[] { "Patient_ID" });
            DropIndex("dbo.Requests", new[] { "Caller_ID" });
            DropForeignKey("dbo.Requests", "Patient_ID", "dbo.Patients");
            DropForeignKey("dbo.Requests", "Caller_ID", "dbo.Callers");
            DropColumn("dbo.Requests", "Patient_ID");
            DropColumn("dbo.Requests", "Caller_ID");
            CreateIndex("dbo.Requests", "PatientID_ID");
            CreateIndex("dbo.Requests", "CallerID_ID");
            AddForeignKey("dbo.Requests", "PatientID_ID", "dbo.Patients", "ID");
            AddForeignKey("dbo.Requests", "CallerID_ID", "dbo.Callers", "ID");
        }
    }
}
