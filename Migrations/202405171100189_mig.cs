namespace Busticket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false),
                        AdminName = c.String(),
                    })
                .PrimaryKey(t => t.AdminId);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Int(nullable: false),
                        PassengerId = c.Int(nullable: false),
                        SeatId = c.Int(nullable: false),
                        BusLogId = c.Int(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.BusLogs", t => t.BusLogId, cascadeDelete: true)
                .ForeignKey("dbo.Passengers", t => t.PassengerId, cascadeDelete: true)
                .ForeignKey("dbo.Seats", t => t.SeatId, cascadeDelete: true)
                .Index(t => t.PassengerId)
                .Index(t => t.SeatId)
                .Index(t => t.BusLogId);
            
            CreateTable(
                "dbo.BusLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JourneyDay = c.Int(nullable: false),
                        BusNo = c.String(maxLength: 128),
                        DepartureTime = c.DateTime(nullable: false),
                        ArrivalTime = c.DateTime(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Buses", t => t.BusNo)
                .Index(t => t.BusNo);
            
            CreateTable(
                "dbo.Buses",
                c => new
                    {
                        BusNo = c.String(nullable: false, maxLength: 128),
                        seats = c.Int(nullable: false),
                        vendorid = c.Int(nullable: false),
                        driver = c.String(nullable: false),
                        Origin = c.String(nullable: false),
                        Destination = c.String(nullable: false),
                        StartDay = c.DateTime(nullable: false),
                        EndDay = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BusNo)
                .ForeignKey("dbo.Vendors", t => t.vendorid, cascadeDelete: true)
                .Index(t => t.vendorid);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        SeatId = c.Int(nullable: false, identity: true),
                        SeatName = c.Int(nullable: false),
                        BusLogId = c.Int(nullable: false),
                        IsBooked = c.Boolean(nullable: false),
                        Bus_BusNo = c.String(maxLength: 128),
                        BusLog_Id = c.Int(),
                    })
                .PrimaryKey(t => t.SeatId)
                .ForeignKey("dbo.BusLogs", t => t.BusLogId)
                .ForeignKey("dbo.Buses", t => t.Bus_BusNo)
                .ForeignKey("dbo.BusLogs", t => t.BusLog_Id)
                .Index(t => t.BusLogId)
                .Index(t => t.Bus_BusNo)
                .Index(t => t.BusLog_Id);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        id = c.Int(nullable: false),
                        name = c.String(nullable: false),
                        Mail = c.String(),
                        PhoneNo = c.String(),
                        age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Passengers",
                c => new
                    {
                        PassengerId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.PassengerId);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        LoginId = c.Int(nullable: false),
                        password = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.LoginId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "SeatId", "dbo.Seats");
            DropForeignKey("dbo.Bookings", "PassengerId", "dbo.Passengers");
            DropForeignKey("dbo.Bookings", "BusLogId", "dbo.BusLogs");
            DropForeignKey("dbo.Seats", "BusLog_Id", "dbo.BusLogs");
            DropForeignKey("dbo.BusLogs", "BusNo", "dbo.Buses");
            DropForeignKey("dbo.Buses", "vendorid", "dbo.Vendors");
            DropForeignKey("dbo.Seats", "Bus_BusNo", "dbo.Buses");
            DropForeignKey("dbo.Seats", "BusLogId", "dbo.BusLogs");
            DropIndex("dbo.Seats", new[] { "BusLog_Id" });
            DropIndex("dbo.Seats", new[] { "Bus_BusNo" });
            DropIndex("dbo.Seats", new[] { "BusLogId" });
            DropIndex("dbo.Buses", new[] { "vendorid" });
            DropIndex("dbo.BusLogs", new[] { "BusNo" });
            DropIndex("dbo.Bookings", new[] { "BusLogId" });
            DropIndex("dbo.Bookings", new[] { "SeatId" });
            DropIndex("dbo.Bookings", new[] { "PassengerId" });
            DropTable("dbo.Logins");
            DropTable("dbo.Passengers");
            DropTable("dbo.Vendors");
            DropTable("dbo.Seats");
            DropTable("dbo.Buses");
            DropTable("dbo.BusLogs");
            DropTable("dbo.Bookings");
            DropTable("dbo.Admins");
        }
    }
}
