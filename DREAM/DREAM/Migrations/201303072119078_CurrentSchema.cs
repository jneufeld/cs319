namespace DREAM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrentSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false),
                        CompletionTime = c.DateTime(),
                        CreatedBy = c.Guid(nullable: false),
                        ClosedBy = c.Guid(nullable: false),
                        Type_ID = c.Int(),
                        CallerID_ID = c.Int(),
                        PatientID_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RequestTypes", t => t.Type_ID)
                .ForeignKey("dbo.Callers", t => t.CallerID_ID)
                .ForeignKey("dbo.Patients", t => t.PatientID_ID)
                .Index(t => t.Type_ID)
                .Index(t => t.CallerID_ID)
                .Index(t => t.PatientID_ID);
            
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Callers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        RequestID = c.Int(nullable: false),
                        Region_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Regions", t => t.Region_ID)
                .Index(t => t.Region_ID);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AgencyID = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        TimeTaken = c.Int(nullable: false),
                        Response = c.String(),
                        Probability = c.Int(nullable: false),
                        Severity = c.Int(nullable: false),
                        SpecialNotes = c.String(),
                        RequestID = c.Int(nullable: false),
                        TumourGroup_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TumourGroups", t => t.TumourGroup_ID)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.TumourGroup_ID)
                .Index(t => t.RequestID);
            
            CreateTable(
                "dbo.TumourGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        KeywordText = c.String(),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.References",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        QuestionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.QuestionID);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Guid(nullable: false),
                        RequestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID);
            
            CreateTable(
                "dbo.Locks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExpireTime = c.DateTime(nullable: false),
                        UserID = c.Guid(nullable: false),
                        RequestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PreviousPasswords",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PasswordSalt = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PasswordResetRequests",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserID = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.KeywordQuestions",
                c => new
                    {
                        Keyword_ID = c.Int(nullable: false),
                        Question_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Keyword_ID, t.Question_ID })
                .ForeignKey("dbo.Keywords", t => t.Keyword_ID, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_ID, cascadeDelete: true)
                .Index(t => t.Keyword_ID)
                .Index(t => t.Question_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.KeywordQuestions", new[] { "Question_ID" });
            DropIndex("dbo.KeywordQuestions", new[] { "Keyword_ID" });
            DropIndex("dbo.Logs", new[] { "RequestID" });
            DropIndex("dbo.References", new[] { "QuestionID" });
            DropIndex("dbo.Questions", new[] { "RequestID" });
            DropIndex("dbo.Questions", new[] { "TumourGroup_ID" });
            DropIndex("dbo.Callers", new[] { "Region_ID" });
            DropIndex("dbo.Requests", new[] { "PatientID_ID" });
            DropIndex("dbo.Requests", new[] { "CallerID_ID" });
            DropIndex("dbo.Requests", new[] { "Type_ID" });
            DropForeignKey("dbo.KeywordQuestions", "Question_ID", "dbo.Questions");
            DropForeignKey("dbo.KeywordQuestions", "Keyword_ID", "dbo.Keywords");
            DropForeignKey("dbo.Logs", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.References", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.Questions", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.Questions", "TumourGroup_ID", "dbo.TumourGroups");
            DropForeignKey("dbo.Callers", "Region_ID", "dbo.Regions");
            DropForeignKey("dbo.Requests", "PatientID_ID", "dbo.Patients");
            DropForeignKey("dbo.Requests", "CallerID_ID", "dbo.Callers");
            DropForeignKey("dbo.Requests", "Type_ID", "dbo.RequestTypes");
            DropTable("dbo.KeywordQuestions");
            DropTable("dbo.PasswordResetRequests");
            DropTable("dbo.PreviousPasswords");
            DropTable("dbo.Locks");
            DropTable("dbo.Logs");
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.References");
            DropTable("dbo.Keywords");
            DropTable("dbo.TumourGroups");
            DropTable("dbo.Questions");
            DropTable("dbo.Patients");
            DropTable("dbo.Regions");
            DropTable("dbo.Callers");
            DropTable("dbo.RequestTypes");
            DropTable("dbo.Requests");
        }
    }
}
