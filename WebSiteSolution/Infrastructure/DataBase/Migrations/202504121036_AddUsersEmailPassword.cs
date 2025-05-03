using FluentMigrator;
using FluentMigrator.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataBase.Migrations
{
    [Migration(202504121036)]
    public class _202504121036_AddUsersEmailPassword : Migration
    {       
        public override void Up()
        {
            Execute.Sql("CREATE TYPE user_role AS ENUM ('User', 'Admin')");

            Delete.Column("role")
                .FromTable("users");

            Alter.Table("users")
                .AddColumn("password_hash").AsString(255).Nullable()
                .AddColumn("role").AsCustom("user_role").WithDefaultValue("User");
        }

        public override void Down()
        {
            Delete.Column("password_hash")
                .FromTable("users");

            Execute.Sql("DROP TYPE user_role");
        }
    }
}
