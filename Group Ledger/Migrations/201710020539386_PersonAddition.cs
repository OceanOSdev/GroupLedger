namespace Group_Ledger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonAddition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.AspNetUsers", t => t.PersonId)
                .Index(t => t.PersonId);
            
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            DropForeignKey("dbo.People", "PersonId", "dbo.AspNetUsers");
            DropIndex("dbo.People", new[] { "PersonId" });
            DropTable("dbo.People");
        }
    }
}
