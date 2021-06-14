namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRentalTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rentals", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Rentals", new[] { "Customer_Id" });
            AddColumn("dbo.Rentals", "DateRented", c => c.DateTime(nullable: false));
            AddColumn("dbo.Rentals", "DateDue", c => c.DateTime(nullable: false));
            AddColumn("dbo.Rentals", "DateReturned", c => c.DateTime());
            AddColumn("dbo.Rentals", "Movie_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Rentals", "Customer_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Rentals", "Customer_Id");
            CreateIndex("dbo.Rentals", "Movie_Id");
            AddForeignKey("dbo.Rentals", "Movie_Id", "dbo.Movies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Rentals", "Customer_Id", "dbo.Customers", "Id", cascadeDelete: true);
            DropColumn("dbo.Rentals", "DueDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rentals", "DueDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Rentals", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Rentals", "Movie_Id", "dbo.Movies");
            DropIndex("dbo.Rentals", new[] { "Movie_Id" });
            DropIndex("dbo.Rentals", new[] { "Customer_Id" });
            AlterColumn("dbo.Rentals", "Customer_Id", c => c.Int());
            DropColumn("dbo.Rentals", "Movie_Id");
            DropColumn("dbo.Rentals", "DateReturned");
            DropColumn("dbo.Rentals", "DateDue");
            DropColumn("dbo.Rentals", "DateRented");
            CreateIndex("dbo.Rentals", "Customer_Id");
            AddForeignKey("dbo.Rentals", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
