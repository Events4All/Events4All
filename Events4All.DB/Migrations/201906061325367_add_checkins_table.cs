namespace Events4All.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_checkins_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CheckinTime = c.DateTime(),
                        RawBarcode = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy_Id = c.String(nullable: false, maxLength: 128),
                        Participants_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id, cascadeDelete: true)
                .ForeignKey("dbo.Participants", t => t.Participants_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Participants_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckIns", "Participants_Id", "dbo.Participants");
            DropForeignKey("dbo.CheckIns", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CheckIns", new[] { "Participants_Id" });
            DropIndex("dbo.CheckIns", new[] { "CreatedBy_Id" });
            DropTable("dbo.CheckIns");
        }
    }
}
