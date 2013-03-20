namespace DREAM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateGroupTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Code = c.String(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.UserGroups",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    UserID = c.Guid(nullable: false),
                    GroupID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.UserProfiles",
                c => new
                {
                    UserId = c.Guid(nullable: false),
                    FirstName = c.String(),
                    LastName = c.String(),
                })
                .PrimaryKey(t => t.UserId);
        }
        
        public override void Down()
        {
            DropTable("dbo.UserProfiles");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Groups");
        }
    }
}
