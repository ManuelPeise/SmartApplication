using Shared.Models.Administration.AccessRights;

namespace Logic.Administration.Interfaces
{
    public interface IAccessRightAdministrationService: IDisposable
    {
        Task<List<UserAccessRightModel>> GetUsersWithAccessRights();
        Task<UserAccessRightModel> GetUserrights(int userId);
    }
}
