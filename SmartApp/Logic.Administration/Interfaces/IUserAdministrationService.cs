using Shared.Models.Administration;

namespace Logic.Administration.Interfaces
{
    public interface IUserAdministrationService
    {
        Task<List<UserAdministrationUserModel>> LoadUsers();
    }
}
