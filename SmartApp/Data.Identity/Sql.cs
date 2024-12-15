using Data.Shared.Identity.Entities;
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

        internal static void SeedModules(Func<string, bool, OperationBuilder<SqlOperation>> sql)
        {
            var modules = Enum.GetValues(typeof(ModuleEnum));
            var timeStamp = DateTime.Now.ToString("yyyy-MM-dd");

            foreach(var module in modules)
            {
                var moduleName = Enum.GetName(typeof(ModuleEnum), module);

                var query = $"INSERT INTO Modules (ModuleName, ModuleType, CreatedBy, CreatedAt, UpdatedBy, UpdatedAt) VALUES ('{moduleName}', {(int)module}, 'System', '{timeStamp}', 'System', '{timeStamp}');";

                sql(query, true);
            }
        }
    }
}
