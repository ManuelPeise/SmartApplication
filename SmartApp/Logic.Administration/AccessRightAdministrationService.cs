using Data.ContextAccessor.Interfaces;
using Data.Shared.AccessRights;
using Data.Shared.Logging;
using Logic.Administration.Interfaces;
using Shared.Enums;
using Shared.Models.Administration.AccessRights;

namespace Logic.Administration
{
    public class AccessRightAdministrationService : IAccessRightAdministrationService
    {
        private readonly IAdministrationRepository _administrationRepository;
        private bool disposedValue;

        public AccessRightAdministrationService(IAdministrationRepository administrationRepository)
        {
            _administrationRepository = administrationRepository;
        }

        public async Task<UserAccessRightModel> GetUserrights(int userId)
        {
            var userAccessRightModel = new UserAccessRightModel
            {
                UserId = userId,
                AccessRights = new List<AccessRight>()
            };

            try
            {
                var userAccessRights = await _administrationRepository.IdentityRepository.UserAccessRightRepository.GetAll(x => x.UserId == userId) ?? new List<UserAccessRightEntity>();

                if (!userAccessRights.Any())
                {
                    return userAccessRightModel;
                }

                foreach (var right in userAccessRights)
                {
                    var accessRight = await _administrationRepository.IdentityRepository.AccessRightRepository.GetSingle(x => x.Id == right.AccessRightId);

                    if (accessRight == null)
                    {
                        continue;
                    }

                    userAccessRightModel.AccessRights.Add(new AccessRight
                    {
                        Id = right.AccessRightId,
                        Name = accessRight.Name,
                        Group = accessRight.Group,
                        Deny = right.Deny,
                        CanView = right.View,
                        CanEdit = right.Edit
                    });
                }

                return userAccessRightModel;
            }
            catch (Exception exception)
            {
                await _administrationRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = $"Could not load access rights for user {userId}.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(AccessRightAdministrationService),
                    TimeStamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                });

                return userAccessRightModel;
            }
        }

        public async Task<List<UserAccessRightModel>> GetUsersWithAccessRights()
        {
            var accessRightModels = new List<UserAccessRightModel>();

            try
            {
                var userEntities = await _administrationRepository.IdentityRepository.UserIdentityRepository.GetAllAsync();


                if (!userEntities.Any())
                {
                    return new List<UserAccessRightModel>();
                }

                foreach (var userEntity in userEntities)
                {
                    var model = new UserAccessRightModel
                    {
                        UserId = userEntity.Id,
                        UserName = $"{userEntity.FirstName} {userEntity.LastName}",
                        AccessRights = new List<AccessRight>()
                    };

                    var userAccessRights = await _administrationRepository.IdentityRepository.UserAccessRightRepository.GetAll(x => x.UserId == userEntity.Id) ?? new List<UserAccessRightEntity>();

                    if (!userAccessRights.Any())
                    {
                        accessRightModels.Add(model);

                        continue;
                    }

                    foreach (var right in userAccessRights)
                    {
                        var accessRight = await _administrationRepository.IdentityRepository.AccessRightRepository.GetSingle(x => x.Id == right.AccessRightId);

                        if (accessRight == null)
                        {
                            continue;
                        }

                        model.AccessRights.Add(new AccessRight
                        {
                            Id = right.AccessRightId,
                            Group = accessRight.Group,
                            Name = accessRight.Name,
                            Deny = right.Deny,
                            CanView = right.View,
                            CanEdit = right.Edit
                        });
                    }

                    accessRightModels.Add(model);
                }

                return accessRightModels;
            }
            catch (Exception exception)
            {
                await _administrationRepository.LogRepository.AddMessage(new LogMessageEntity
                {
                    Message = "Could not load users with access rights.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(AccessRightAdministrationService),
                    TimeStamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                });

                return accessRightModels;
            }
        }

        #region dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _administrationRepository?.Dispose();
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
