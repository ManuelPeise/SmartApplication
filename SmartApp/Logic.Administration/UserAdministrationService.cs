using Data.ContextAccessor.Interfaces;
using Data.Resources.Properties;
using Data.Shared;
using Data.Shared.AccessRights;
using Data.Shared.Identity.Entities;
using Data.Shared.Logging;
using Logic.Administration.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Enums;
using Shared.Models.Administration;
using Shared.Models.Identity;
using System.Text;



namespace Logic.Administration
{
    public class UserAdministrationService : IUserAdministrationService
    {
        private readonly IAccessRightAdministrationService _accessRightAdministrationService;
        private readonly IAdministrationRepository _administrationRepository;
        private readonly IOptions<SecurityData> _securityData;
       
        public UserAdministrationService(IOptions<SecurityData> securityData, IAccessRightAdministrationService accessRightAdministrationService, IAdministrationRepository administrationRepository)
        {
            _accessRightAdministrationService = accessRightAdministrationService;
            _administrationRepository = administrationRepository;
            _securityData = securityData;
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

                var userAccessRights = await _administrationRepository.IdentityRepository.UserAccessRightRepository.GetAll(x => x.UserId == entity.Id) ?? new List<UserAccessRightEntity>();

                foreach (var right in model.AccessRights)
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
                    Module = nameof(UserAdministrationService),
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
                    Module = nameof(UserAdministrationService),
                    TimeStamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                });

                await _administrationRepository.SaveChanges();

                return false;
            }
        }

        public async Task ActivateUsers(Func<string, string, string, Task> sendMail)
        {
            try
            {
                var userEntities = await _administrationRepository.IdentityRepository.UserIdentityRepository.GetAll(x => x.IsNewUserRegistration) ?? new List<UserIdentity>();
                
                var defaultActivatedUserRights = AccessRights.DefaultActivatedUserAccessRights;

                if (userEntities.Any())
                {
                    var passwordHandler = new PasswordHandler(_securityData);

                    foreach (var entity in userEntities)
                    {
                        await LoadCredentials(_administrationRepository.IdentityRepository.UserCredentialsRepository, entity.CredentialsId);

                        if (entity.UserCredentials != null)
                        {
                            var generatedPassword = GenerateRandomPassword(12);

                            entity.UserCredentials.Password = passwordHandler.Encrypt(generatedPassword);
                            entity.IsNewUserRegistration = false;

                            var userRightEntities = await _administrationRepository.IdentityRepository.UserAccessRightRepository.GetAll(x => x.UserId == entity.Id);

                            if (userRightEntities != null && userRightEntities.Any())
                            {
                                foreach (var right in userRightEntities)
                                {
                                    var accessRight = await _administrationRepository.IdentityRepository.AccessRightRepository.GetSingle(x => x.Id == right.AccessRightId);

                                    if (accessRight != null && defaultActivatedUserRights.ContainsKey(accessRight.Name))
                                    {
                                        right.Deny = defaultActivatedUserRights[accessRight.Name].Deny;
                                        right.View = defaultActivatedUserRights[accessRight.Name].CanView;
                                        right.Edit = defaultActivatedUserRights[accessRight.Name].CanEdit;
                                    }
                                }
                            }

                            await _administrationRepository.IdentityRepository.SaveChanges();

                            await sendMail(entity.Email, "Your account is activated now!", GetAccountActivationBody(entity.FirstName, entity.Email, generatedPassword));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                await _administrationRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = $"Could not activate users.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(UserAdministrationService),
                    TimeStamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                });

                await _administrationRepository.SaveChanges();
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

        private async Task LoadCredentials(IRepositoryBase<UserCredentials> credentialsRepository, int credentialsId)
        {
            await credentialsRepository.GetFirstOrDefault(cred => cred.Id == credentialsId);
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }

            return password.ToString();
        }

        private string GetAccountActivationBody(string name, string email, string password)
        {
            return Resources.AccountActivationEmailBody.Replace("{USER}", name).Replace("{EMAIL}", email).Replace("{PASSWORD}", password);
        }
    }
}
