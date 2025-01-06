using Data.ContextAccessor.Interfaces;
using Data.Shared.Identity.Entities;
using Data.Shared.Logging;
using Logic.Administration.Interfaces;
using Shared.Enums;
using Shared.Models.Administration;

namespace Logic.Administration
{
    public class UserAdministrationService: IUserAdministrationService
    {
        private readonly IAccessRightAdministrationService _accessRightAdministrationService;
        private readonly IAdministrationRepository _administrationRepository;

        public UserAdministrationService(IAccessRightAdministrationService accessRightAdministrationService, IAdministrationRepository administrationRepository)
        {
            _accessRightAdministrationService = accessRightAdministrationService;
            _administrationRepository = administrationRepository;
        }

        public async Task<List<UserAdministrationUserModel>> LoadUsers()
        {
            var userModels = new List<UserAdministrationUserModel>();

            try
            {
                var userEntities = await _administrationRepository.IdentityRepository.UserIdentityRepository.GetAllAsync() ?? new List<UserIdentity>();

                foreach (var userEntity in userEntities)
                {
                    await LoadUserRole(_administrationRepository.IdentityRepository.UserRoleRepository, userEntity.RoleId);

                    var userRights = await _accessRightAdministrationService.GetUserrights(userEntity.Id);

                    userModels.Add(new UserAdministrationUserModel
                    {
                        UserId = userEntity.Id,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        Email = userEntity.Email,
                        IsActive = userEntity.IsActive,
                        IsAdmin = userEntity.UserRole?.RoleType == UserRoleEnum.Admin,
                        AccessRights = userRights.AccessRights
                    });
                }

                return userModels;
            }
            catch (Exception exception)
            {
                await _administrationRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not load users.",
                    ExceptionMessage = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(UserAdministrationService),
                });

                await _administrationRepository.SaveChanges();
            }

            return userModels;
        }

        private async Task LoadUserRole(IRepositoryBase<UserRole> repository, int roleId)
        {
            await repository.GetSingle(x => x.Id == roleId);
        }
    }
}
