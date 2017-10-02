namespace Group_Ledger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Verified = c.Boolean(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        From_Id = c.String(maxLength: 128),
                        To_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.From_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.To_Id)
                .Index(t => t.From_Id)
                .Index(t => t.To_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "To_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "From_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Transactions", new[] { "To_Id" });
            DropIndex("dbo.Transactions", new[] { "From_Id" });
            DropTable("dbo.Transactions");
        }
    }
}
