using Shared.Models.Administration;

namespace Logic.Administration.Interfaces
{
    public interface IUserAdministrationService
    {
        Task<List<UserAdministrationUserModel>> LoadUsers();
        Task<bool> UpdateUser(UserAdministrationUserModel model);
        Task ActivateUsers(Func<string, string, string, Task> sendMail);
    }
}
