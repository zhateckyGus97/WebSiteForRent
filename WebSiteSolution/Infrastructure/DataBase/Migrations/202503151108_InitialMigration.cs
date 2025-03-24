using Domain.Entities;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataBase.Migrations
{
    [Migration(202503151108)]
    public class InitialMigration : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("FullName").AsString(100).NotNullable()
                .WithColumn("Email").AsString(50).NotNullable()
                .WithColumn("PhoneNumber").AsString(20).NotNullable()
                .WithColumn("Role").AsString(20).NotNullable()
                .WithColumn("Passport").AsString(20).NotNullable()
                .WithColumn("DateOfBirth").AsDate().NotNullable();

            Create.Table("Apartment")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Title").AsString(100).NotNullable()
                .WithColumn("Description").AsString(500).NotNullable()
                .WithColumn("Address").AsString(150).NotNullable()
                .WithColumn("PricePerDay").AsDouble().NotNullable()
                .WithColumn("NumOfFloor").AsInt32()
                .WithColumn("Square").AsDouble()
                .WithColumn("Capacity").AsInt32().NotNullable();

            Create.Table("Deal")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("User", "Id")
                .WithColumn("ApartmentId").AsInt32().ForeignKey("Apartment", "Id")
                .WithColumn("CheckInDate").AsDateTime().NotNullable()
                .WithColumn("CheckOutDate").AsDateTime().NotNullable()
                .WithColumn("TotalPrice").AsDouble().NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();

            Create.Table("Review")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("User", "Id")
                .WithColumn("ApartmentId").AsInt32().ForeignKey("Apartment", "Id")
                .WithColumn("Rating").AsInt32().NotNullable()
                .WithColumn("Comment").AsString().WithDefaultValue("")
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();

            Insert.IntoTable("User")
                .Row(new
                {
                    FullName = "Ilya",
                    Email = "ggg@123.ru",
                    PhoneNumber = "22-22-22",
                    Role = "Admin",
                    Passport = "7865-123456",
                    DateOfBirth = DateTime.Now
                });

            Insert.IntoTable("Apartment")
                .Row(new
                {
                    Title = "Luxe Apartment",
                    Description = "No description",
                    Address = "Russia, Yaroslavl",
                    PricePerDay = 15000,
                    NumOfFloor = 2,
                    Square = 76,
                    Capacity = 6
                });

            Insert.IntoTable("Deal")
               .Row(new
               {
                   UserId = 1,
                   ApartmentId = 1,
                   CheckInDate = new DateTime(2025, 1, 15),
                   CheckOutDate = new DateTime(2025, 1, 25),
                   TotalPrice = 150000,
                   CreatedAt = DateTime.Now
               });

            Insert.IntoTable("Review")
                .Row(new 
                { 
                    UserId = 1, 
                    ApartmentId = 1, 
                    Rating = 5, 
                    Comment = "Perfect apartment", 
                    CreatedAt = DateTime.Now
                });
        }

        public override void Down()
        {
            Delete.Table("Review");
            Delete.Table("Deal");
            Delete.Table("Apartment");
            Delete.Table("User");
        }

    }
}