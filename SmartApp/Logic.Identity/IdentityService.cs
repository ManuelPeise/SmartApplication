using Data.ContextAccessor.Interfaces;
using Data.Shared;
using Data.Shared.AccessRights;
using Data.Shared.Identity.Entities;
using Logic.Identity.Interfaces;
using Logic.Shared;
using Shared.Enums;
using Shared.Models.Identity;
using System.Security.Claims;


namespace Logic.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
       
        private readonly PasswordHandler _passwordHandler;

        public IdentityService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _passwordHandler = new PasswordHandler(applicationUnitOfWork.SecurityData);
        }

        public async Task<string> AuthenticateAsync(AuthenticationRequest request)
        {
            var response = string.Empty;

            var user = await _applicationUnitOfWork.IdentityRepository.UserIdentityRepository.GetFirstOrDefault(usr => usr.Email == request.Email) ?? null;

            if (user == null)
            {
                return response;
            }

            await LoadCredentials(_applicationUnitOfWork.IdentityRepository.UserCredentialsRepository, user.CredentialsId);
            await LoadUserRole(_applicationUnitOfWork.IdentityRepository.UserRoleRepository, user.RoleId);

            if (user.UserCredentials == null || user.UserRole == null) { return response; }

            var encodedPassword = _passwordHandler.Encrypt(request.Password);

            if (user.UserCredentials.Password == encodedPassword)
            {
                var jwtTokenGenerator = new JwtTokenGenerator(_applicationUnitOfWork.SecurityData.Value);

                var (jwt, refreshToken) = jwtTokenGenerator.GenerateToken(LoadUserClaims(user), 30);

                user.UserCredentials.RefreshToken = refreshToken;

                _applicationUnitOfWork.IdentityRepository.UserCredentialsRepository.Update(user.UserCredentials);

                await _applicationUnitOfWork.IdentityRepository.UserAccessRightRepository.SaveChangesAsync();

                response = jwt;
                return response;
            }

            return response;
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            var response = false;

          
                var user = await _applicationUnitOfWork.IdentityRepository.UserIdentityRepository.GetFirstOrDefault(x => x.Id == userId);

                if (user == null) { return response; }

                await LoadCredentials(_applicationUnitOfWork.IdentityRepository.UserCredentialsRepository, user.CredentialsId);

                if (user.UserCredentials == null) { return response; }

                user.UserCredentials.RefreshToken = string.Empty;

                await _applicationUnitOfWork.IdentityRepository.UserCredentialsRepository.AddIfNotExists(user.UserCredentials, x => x.Id == user.CredentialsId);

                await _applicationUnitOfWork.IdentityRepository.UserIdentityRepository.SaveChangesAsync();

                return response;
        }

        public async Task<bool> RequestAccount(AccountRequest request)
        {
            var existingAccount = await _applicationUnitOfWork.IdentityRepository.UserIdentityRepository.GetFirstOrDefault(usr => usr.Email.ToLower() == request.Email.ToLower());

            if (existingAccount == null)
            {
                var userRoleId = await LoadUserRoleId(_applicationUnitOfWork.IdentityRepository.UserRoleRepository, UserRoleEnum.User);

                if (userRoleId == null)
                {
                    return false;
                }

                var entity = new UserIdentity
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    IsActive = false,
                    IsNewUserRegistration = true,
                    UserCredentials = new UserCredentials
                    {
                        RefreshToken = "",
                        Password = string.Empty
                    },
                    RoleId = (int)userRoleId,
                };

                var result = await _applicationUnitOfWork.IdentityRepository.UserIdentityRepository.AddIfNotExists(entity, 
                    usr => usr.Email.ToLower() == request.Email.ToLower());

                await _applicationUnitOfWork.IdentityRepository.UserIdentityRepository.SaveChangesAsync();

                if (result)
                {
                    var accessRightEntities = await _applicationUnitOfWork.IdentityRepository.AccessRightRepository.GetAllAsync();

                    foreach (var right in AccessRights.AvailableAccessRights)
                    {
                        var accessRight = accessRightEntities.FirstOrDefault(r => r.Name == right.Key);

                        if (accessRight == null)
                        {
                            continue;
                        }

                        await _applicationUnitOfWork.IdentityRepository.UserAccessRightRepository.AddAsync(new UserAccessRightEntity
                        {
                            UserId = entity.Id,
                            AccessRightId = accessRight.Id,
                            View = false,
                            Edit = false,
                            Deny = true,
                        });
                    }
                }

                await _applicationUnitOfWork.IdentityRepository.UserIdentityRepository.SaveChangesAsync();
               

                return true;
            }


            return false;
        }

        #region private members

        private async Task LoadCredentials(IDbContextRepository<UserCredentials> credentialsRepository, int credentialsId)
        {
            await credentialsRepository.GetFirstOrDefault(cred => cred.Id == credentialsId);
        }

        private async Task LoadUserRole(IDbContextRepository<UserRole> userRoleRepository, int roleId)
        {
            await userRoleRepository.GetFirstOrDefault(role => role.Id == roleId);
        }

        private List<Claim> LoadUserClaims(UserIdentity user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("name", $"{user.FirstName} {user.LastName}"),
                new Claim("email", user.Email),
                new Claim("userRole", user.UserRole.RoleName),
                new Claim("isActive", user.IsActive.ToString()),
            };

            return claims;
        }

        private async Task<int?> LoadUserRoleId(IDbContextRepository<UserRole> userRoleRepository, UserRoleEnum userRole)
        {
            var role = await userRoleRepository.GetFirstOrDefault(r => r.RoleType == userRole);

            return role?.Id;
        }
        #endregion
    }
}
