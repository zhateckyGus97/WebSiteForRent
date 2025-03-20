using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //.ForeignKey("TableName", "ColumnName")

            Insert.IntoTable("User")
                .Row(new { FullName = "Ilya", Email = "ggg@123.ru", PhoneNumber = "22-22-22", Role = "Admin", Passport = "7865-123456", DateOfBirth = DateTime.Now });

            Create.Table("Apartment")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Title").AsString(100).NotNullable()
                .WithColumn("Description").AsString(500).NotNullable()
                .WithColumn("Address").AsString(150).NotNullable()
                .WithColumn("PricePerDay").AsDouble().NotNullable()
                .WithColumn("NumOfFloor").AsInt32()
                .WithColumn("Square").AsDouble()
                .WithColumn("Capacity").AsInt32().NotNullable()
                .WithColumn("PhotoURLs").AsString();

        }

        public override void Down()
        {
            Delete.Table("User");
            Delete.Table("Apartment");
        }

    }
}
