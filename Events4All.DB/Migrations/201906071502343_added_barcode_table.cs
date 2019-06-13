namespace Events4All.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_barcode_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CheckIns", "Participants_Id", "dbo.Participants");
            DropIndex("dbo.CheckIns", new[] { "Participants_Id" });
            CreateTable(
                "dbo.Barcodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Barcode = c.Guid(nullable: false),
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
            
            AddColumn("dbo.CheckIns", "Barcodes_Id", c => c.Int());
            CreateIndex("dbo.CheckIns", "Barcodes_Id");
            AddForeignKey("dbo.CheckIns", "Barcodes_Id", "dbo.Barcodes", "Id");
            DropColumn("dbo.CheckIns", "RawBarcode");
            DropColumn("dbo.CheckIns", "Participants_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CheckIns", "Participants_Id", c => c.Int());
            AddColumn("dbo.CheckIns", "RawBarcode", c => c.String(nullable: false));
            DropForeignKey("dbo.Barcodes", "Participants_Id", "dbo.Participants");
            DropForeignKey("dbo.Barcodes", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CheckIns", "Barcodes_Id", "dbo.Barcodes");
            DropIndex("dbo.CheckIns", new[] { "Barcodes_Id" });
            DropIndex("dbo.Barcodes", new[] { "Participants_Id" });
            DropIndex("dbo.Barcodes", new[] { "CreatedBy_Id" });
            DropColumn("dbo.CheckIns", "Barcodes_Id");
            DropTable("dbo.Barcodes");
            CreateIndex("dbo.CheckIns", "Participants_Id");
            AddForeignKey("dbo.CheckIns", "Participants_Id", "dbo.Participants", "Id");
        }
    }
}
