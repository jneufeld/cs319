namespace DREAM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestionType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "RequestID", "dbo.Requests");
            DropIndex("dbo.Questions", new[] { "RequestID" });
            AddColumn("dbo.Questions", "QuestionType_ID", c => c.Int());
            AddColumn("dbo.Questions", "Request_ID", c => c.Int());
            AddForeignKey("dbo.Questions", "QuestionType_ID", "dbo.QuestionTypes", "ID");
            AddForeignKey("dbo.Questions", "Request_ID", "dbo.Requests", "ID");
            CreateIndex("dbo.Questions", "QuestionType_ID");
            CreateIndex("dbo.Questions", "Request_ID");
            DropColumn("dbo.Questions", "RequestID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "RequestID", c => c.Int(nullable: false));
            DropIndex("dbo.Questions", new[] { "Request_ID" });
            DropIndex("dbo.Questions", new[] { "QuestionType_ID" });
            DropForeignKey("dbo.Questions", "Request_ID", "dbo.Requests");
            DropForeignKey("dbo.Questions", "QuestionType_ID", "dbo.QuestionTypes");
            DropColumn("dbo.Questions", "Request_ID");
            DropColumn("dbo.Questions", "QuestionType_ID");
            CreateIndex("dbo.Questions", "RequestID");
            AddForeignKey("dbo.Questions", "RequestID", "dbo.Requests", "ID", cascadeDelete: true);
        }
    }
}
