using Data.ContextAccessor.Interfaces;
using Data.Shared.AccessRights;
using Data.Shared.Identity.Entities;
using Data.Shared.Logging;
using Logic.Administration.Interfaces;
using Shared.Enums;
using Shared.Models.Administration;

namespace Logic.Administration
{
    public class UserAdministrationService : IUserAdministrationService
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


        public async Task<bool> UpdateUser(UserAdministrationUserModel model)
        {
            try
            {
                var entity = await _administrationRepository.IdentityRepository.UserIdentityRepository.GetSingle(x => x.Id == model.UserId);

                if (entity == null) { return false; }

                entity.IsActive = model.IsActive;

                entity.RoleId = await LoadUserRoleId(_administrationRepository.IdentityRepository.UserRoleRepository,
                    model.IsAdmin ? UserRoleEnum.Admin : UserRoleEnum.User);

                var userAccessRights = await _administrationRepository.IdentityRepository.UserAccessRightRepository.GetAll(x => x.UserId == entity.Id)?? new List<UserAccessRightEntity>();

                foreach(var right in model.AccessRights)
                {
                    var rightEntity = userAccessRights.FirstOrDefault(x => x.UserId == entity.Id && x.AccessRightId == right.Id);

                    if (rightEntity == null) { continue; }

                    rightEntity.View = right.CanView;
                    rightEntity.Edit = right.CanEdit;
                    rightEntity.Deny = right.Deny;
                }

                await _administrationRepository.IdentityRepository.SaveChanges();

                await _administrationRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = $"User data and rights of user id [{model.UserId}] updated.",
                    ExceptionMessage = string.Empty,
                    MessageType = LogMessageTypeEnum.Info,
                    Module = nameof(AccessRightAdministrationService),
                    TimeStamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                });

                await _administrationRepository.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                await _administrationRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = $"Could not update user rights of user {model.UserId}.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(AccessRightAdministrationService),
                    TimeStamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                });

                await _administrationRepository.SaveChanges();

                return false;
            }
        }
        private async Task LoadUserRole(IRepositoryBase<UserRole> repository, int roleId)
        {
            await repository.GetSingle(x => x.Id == roleId);
        }

        private async Task<int> LoadUserRoleId(IRepositoryBase<UserRole> repository, UserRoleEnum roleType)
        {
            var role = await repository.GetSingle(x => x.RoleType == roleType);

            return role.Id;
        }
    }
}
