using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Shared.Enums;

namespace Data.Identity
{
    internal static class Sql
    {
        internal static void SeedUserRoles(Func<string, bool, OperationBuilder<SqlOperation>> sql) 
        {
            var userRoles = Enum.GetValues(typeof(UserRoleEnum));
            var timeStamp = DateTime.Now.ToString("yyyy-MM-dd");
            foreach (var userRole in userRoles)
            {
                
                var roleName = Enum.GetName(typeof(UserRoleEnum), userRole);

                var query = $"INSERT INTO USERROLES (ResourceKey, RoleName, RoleType, CreatedBy, CreatedAt, UpdatedBy, UpdatedAt) VALUES('label{roleName}','{roleName}',{(int)userRole}, 'System', '{timeStamp}', 'System', '{timeStamp}');";

                sql(query, true);
            }
        }
    }
}
