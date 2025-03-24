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
            Create.Table("users")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("fullname").AsString(100).NotNullable()
                .WithColumn("email").AsString(50).NotNullable()
                .WithColumn("phonenumber").AsString(20).NotNullable()
                .WithColumn("role").AsString(20).NotNullable()
                .WithColumn("passport").AsString(20).NotNullable()
                .WithColumn("dateofbirth").AsDate().NotNullable();

            Create.Table("apartments")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("title").AsString(100).NotNullable()
                .WithColumn("description").AsString(500).NotNullable()
                .WithColumn("address").AsString(150).NotNullable()
                .WithColumn("priceperday").AsDouble().NotNullable()
                .WithColumn("numoffloor").AsInt32()
                .WithColumn("square").AsDouble()
                .WithColumn("capacity").AsInt32().NotNullable();

            Create.Table("deals")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("userid").AsInt32().ForeignKey("users", "id")
                .WithColumn("apartmentid").AsInt32().ForeignKey("apartments", "id")
                .WithColumn("checkindate").AsDateTime().NotNullable()
                .WithColumn("checkoutdate").AsDateTime().NotNullable()
                .WithColumn("totalprice").AsDouble().NotNullable()
                .WithColumn("createdat").AsDateTime().NotNullable()
                .WithColumn("updatedat").AsDateTime().Nullable();

            Create.Table("reviews")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("userid").AsInt32().ForeignKey("users", "id")
                .WithColumn("apartmentid").AsInt32().ForeignKey("apartments", "id")
                .WithColumn("rating").AsInt32().NotNullable()
                .WithColumn("comment").AsString().WithDefaultValue("")
                .WithColumn("createdat").AsDateTime().NotNullable()
                .WithColumn("updatedat").AsDateTime().Nullable();

            Insert.IntoTable("users")
                .Row(new
                {
                    fullname = "Ilya",
                    email = "ggg@123.ru",
                    phonenumber = "22-22-22",
                    role = "Admin",
                    passport = "7865-123456",
                    dateofbirth = DateTime.Now
                });

            Insert.IntoTable("apartments")
                .Row(new
                {
                    title = "Luxe Apartment",
                    description = "No description",
                    address = "Russia, Yaroslavl",
                    priceperday = 15000,
                    numoffloor = 2,
                    square = 76,
                    capacity = 6
                });

            Insert.IntoTable("deals")
               .Row(new
               {
                   userid = 1,
                   apartmentid = 1,
                   checkindate = new DateTime(2025, 1, 15),
                   checkoutdate = new DateTime(2025, 1, 25),
                   totalprice = 150000,
                   createdat = DateTime.Now
               });

            Insert.IntoTable("reviews")
                .Row(new 
                {
                    userid = 1,
                    apartmentid = 1, 
                    rating = 5, 
                    comment = "Perfect apartment",
                    createdat = DateTime.Now
                });
        }

        public override void Down()
        {
            Delete.Table("reviews");
            Delete.Table("deals");
            Delete.Table("apartments");
            Delete.Table("users");
        }

    }
}