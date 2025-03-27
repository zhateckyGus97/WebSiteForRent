using FluentMigrator;

namespace Infrastructure.DataBase.Migrations
{
    [Migration(202503261235)]
    public class InitialMigration : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("full_name").AsString(100).NotNullable()
                .WithColumn("email").AsString(50).NotNullable()
                .WithColumn("phone_number").AsString(20).NotNullable()
                .WithColumn("role").AsString(20).NotNullable()
                .WithColumn("passport").AsString(20).NotNullable()
                .WithColumn("date_of_birth").AsDate().NotNullable();

            Create.Table("apartments")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("title").AsString(100).NotNullable()
                .WithColumn("description").AsString(500).NotNullable()
                .WithColumn("address").AsString(150).NotNullable()
                .WithColumn("price_per_day").AsDouble().NotNullable()
                .WithColumn("num_of_floor").AsInt32()
                .WithColumn("square").AsDouble()
                .WithColumn("capacity").AsInt32().NotNullable();

            Create.Table("deals")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id")
                .WithColumn("apartment_id").AsInt32().ForeignKey("apartments", "id")
                .WithColumn("check_in_date").AsDateTime().NotNullable()
                .WithColumn("check_out_date").AsDateTime().NotNullable()
                .WithColumn("total_price").AsDouble().NotNullable()
                .WithColumn("created_at").AsDateTime().NotNullable()
                .WithColumn("updated_at").AsDateTime().Nullable();

            Create.Table("reviews")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id")
                .WithColumn("apartment_id").AsInt32().ForeignKey("apartments", "id")
                .WithColumn("rating").AsInt32().NotNullable()
                .WithColumn("comment").AsString().WithDefaultValue("")
                .WithColumn("created_at").AsDateTime().NotNullable()
                .WithColumn("updated_at").AsDateTime().Nullable();

            Insert.IntoTable("users")
                .Row(new
                {
                    full_name = "Ilya",
                    email = "ggg@123.ru",
                    phone_number = "22-22-22",
                    role = "Admin",
                    passport = "7865-123456",
                    date_of_birth = DateTime.Now
                });

            Insert.IntoTable("apartments")
                .Row(new
                {
                    title = "Luxe Apartment",
                    description = "No description",
                    address = "Russia, Yaroslavl",
                    price_per_day = 15000,
                    num_of_floor = 2,
                    square = 76,
                    capacity = 6
                });

            Insert.IntoTable("deals")
               .Row(new
               {
                   user_id = 1,
                   apartment_id = 1,
                   check_in_date = new DateTime(2025, 1, 15),
                   check_out_date = new DateTime(2025, 1, 25),
                   total_price = 150000,
                   created_at = DateTime.Now
               });

            Insert.IntoTable("reviews")
                .Row(new
                {
                    user_id = 1,
                    apartment_id = 1,
                    rating = 5,
                    comment = "Perfect apartment",
                    created_at = DateTime.Now
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