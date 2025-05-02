using Dapper;
using Infrastructure.DataBase.TypeMappings;

namespace Infrastructure
{
    public static class DapperConfig
    {
        public static void Configure()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new UserRolesTypeHandler());
        }
    }
}
