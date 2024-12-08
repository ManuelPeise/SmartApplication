using Shared.Enums;

namespace Logic.Identity
{
    internal class ModuleDefinition
    {
        internal UserRoleEnum RequiredRole { get; set; }
        internal ModuleEnum ModuleType { get; set; }
        internal bool Deny { get; set; }
        internal bool CanView { get; set; }
        internal bool CanEdit { get; set; }
    }

    internal class AppModule
    {
        internal List<ModuleDefinition> DefautModuleDefinitions = new List<ModuleDefinition>
        {
            new ModuleDefinition
            {
                ModuleType = ModuleEnum.Administration,
                RequiredRole = UserRoleEnum.Admin
            }
        };

        //internal List<UserModuleEntity> GetDefaultUserModules(int userId, UserRoleEnum userRole)
        //{
        //    var modules = DefautModuleDefinitions.Where(x => x.RequiredRole == userRole).ToList();

        //    var entities = (from module in modules
        //                    select new UserModuleEntity
        //                    {
        //                        ModuleId = (int)module.ModuleType + 1,
        //                        UserId = userId,
        //                        c
        //                    });
        //}
    }
}
