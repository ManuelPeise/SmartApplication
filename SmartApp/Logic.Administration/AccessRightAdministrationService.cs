﻿using Data.ContextAccessor.Interfaces;
using Data.Shared.AccessRights;
using Data.Shared.Logging;
using Logic.Administration.Interfaces;
using Shared.Enums;
using Shared.Models.Administration.AccessRights;

namespace Logic.Administration
{
    public class AccessRightAdministrationService : IAccessRightAdministrationService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private bool disposedValue;

        public AccessRightAdministrationService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
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
                var userAccessRights = await _applicationUnitOfWork.IdentityRepository.UserAccessRightRepository.GetAllAsync() ?? new List<UserAccessRightEntity>();

                var rights = userAccessRights.Where(x => x.UserId == userId);

                if (!rights.Any())
                {
                    return userAccessRightModel;
                }

                foreach (var right in userAccessRights)
                {
                    var accessRight = await _applicationUnitOfWork.IdentityRepository.AccessRightRepository.GetFirstOrDefault(x => x.Id == right.AccessRightId);

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
                await _applicationUnitOfWork.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = $"Could not load access rights for user {userId}.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(AccessRightAdministrationService),
                    TimeStamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                });

                await _applicationUnitOfWork.LogMessageRepository.SaveChangesAsync();

                return userAccessRightModel;
            }
        }

        public async Task<List<UserAccessRightModel>> GetUsersWithAccessRights()
        {
            var accessRightModels = new List<UserAccessRightModel>();

            try
            {
                var userEntities = await _applicationUnitOfWork.IdentityRepository.UserIdentityRepository.GetAllAsync();


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

                    var userAccessRights = await _applicationUnitOfWork.IdentityRepository.UserAccessRightRepository.GetAllAsync() ?? new List<UserAccessRightEntity>();

                    var rights = userAccessRights.Where(x => x.UserId == userEntity.Id);

                    if (!rights.Any())
                    {
                        accessRightModels.Add(model);

                        continue;
                    }

                    foreach (var right in userAccessRights)
                    {
                        var accessRight = await _applicationUnitOfWork.IdentityRepository.AccessRightRepository.GetFirstOrDefault(x => x.Id == right.AccessRightId);

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
                await _applicationUnitOfWork.LogMessageRepository.AddAsync(new LogMessageEntity
                {
                    Message = "Could not load users with access rights.",
                    ExceptionMessage = exception.Message,
                    MessageType = LogMessageTypeEnum.Error,
                    Module = nameof(AccessRightAdministrationService),
                    TimeStamp = DateTime.UtcNow,
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                });

                await _applicationUnitOfWork.LogMessageRepository.SaveChangesAsync();
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
                    _applicationUnitOfWork?.Dispose();
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
