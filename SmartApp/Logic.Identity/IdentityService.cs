﻿using Data.Identity;
using Data.Shared.Identity.Entities;
using Logic.Identity.Extensions;
using Logic.Identity.Interfaces;
using Logic.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Shared.Configuration.Interfaces;
using Shared.Enums;
using Shared.Models.Identity;
using Shared.Models.Response;
using System;
using System.Security.Claims;
using System.Text;

namespace Logic.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IOptions<JwtData> _jwtData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityDbContext _identityDbContext;

        public IdentityService(IdentityDbContext context, IOptions<JwtData> jwtData, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtData = jwtData;
            _identityDbContext = context;
        }

        public async Task<ApiResponseBase<AuthTokenResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var response = new ApiResponseBase<AuthTokenResponse> { Success = false, Data = new AuthTokenResponse { RedirectUrl = "/" } };

            using (var unitOfWork = new IdentityUnitOfWork(_identityDbContext, _httpContextAccessor))
            {
                var user = await unitOfWork.UserRepository.GetSingle(usr => usr.Email == request.Email) ?? null;

                if (user == null)
                {
                    return response;
                }

                await LoadCredentials(unitOfWork, user.CredentialsId);
                await LoadUserRole(unitOfWork, user.RoleId);

                if (user.UserCredentials == null || user.UserRole == null) { return response; }

                if (user.UserCredentials.Password == GetEncodedPassword(request.Password, user.UserCredentials.Salt))
                {
                    var userModules = await LoadUserModules(unitOfWork, user.Id);

                    var jwtTokenGenerator = new JwtTokenGenerator(_jwtData.Value);

                    var (jwt, refreshToken) = jwtTokenGenerator.GenerateToken(LoadUserClaims(user, userModules), 30);

                    user.UserCredentials.RefreshToken = refreshToken;

                    await unitOfWork.UserCredentialsRepository.AddOrUpdate(user.UserCredentials, x => x.Id == user.UserCredentials.Id);

                    await unitOfWork.SaveChanges();

                    response.Success = true;
                    response.Data.Token = jwt;

                    return response;
                }

                return response;
            }
        }

        public async Task<ApiResponseBase<LogoutResponse>> LogoutAsync(int userId)
        {
            var response = new ApiResponseBase<LogoutResponse> { Success = false, Data = new LogoutResponse() };

            using (var unitOfWork = new IdentityUnitOfWork(_identityDbContext, _httpContextAccessor))
            {
                var user = await unitOfWork.UserRepository.GetFirstOrDefault(x => x.Id == userId);

                if (user == null) { return response; }

                await LoadCredentials(unitOfWork, user.CredentialsId);

                if (user.UserCredentials == null) { return response; }

                user.UserCredentials.RefreshToken = string.Empty;

                await unitOfWork.UserCredentialsRepository.AddOrUpdate(user.UserCredentials, x => x.Id == user.CredentialsId);

                await unitOfWork.SaveChanges();

                response.Success = true;
                response.Data.IsLoggedOut = true;

                return response;
            }
        }

        public async Task RequestAccount(AccountRequest request)
        {
            using (var unitOfWork = new IdentityUnitOfWork(_identityDbContext, _httpContextAccessor))
            {
                var existingAccount = await unitOfWork.UserRepository.GetFirstOrDefault(usr => usr.Email.ToLower() == request.Email.ToLower());
                var existingRequest = await unitOfWork.AccountRequestRepository.GetFirstOrDefault(req => req.Email.ToLower() == request.Email.ToLower());

                if (existingAccount == null && existingRequest == null)
                {
                    await unitOfWork.AccountRequestRepository.AddOrUpdate(new AccountRegistrationRequestEntity
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        IsGranded = false
                    }, x => x.Email.ToLower() == request.Email.ToLower());

                    await unitOfWork.SaveChanges();
                }
            }
        }

        // TODO set user modules
        public async Task<GrantAccountRequestResult> GrantAccountRequest(int requestId)
        {
            var result = new GrantAccountRequestResult { Success = false };

            using (var unitOfWork = new IdentityUnitOfWork(_identityDbContext, _httpContextAccessor))
            {
                var existingRequest = await unitOfWork.AccountRequestRepository.GetFirstOrDefault(req => req.Id == requestId && !req.IsGranded);

                if (existingRequest != null)
                {
                    var identity = existingRequest.ToIdentityBase();

                    var (credentials, randomPassword) = GetRandomUserCredentials();

                    identity.UserCredentials = credentials;

                    existingRequest.IsGranded = true;

                    var entity = await unitOfWork.AccountRequestRepository.AddOrUpdate(existingRequest, req => req.Id == requestId);

                    if(entity != null)
                    {
                        await unitOfWork.SaveChanges();

                        result.Success = true;
                        result.FirstName = entity.FirstName;
                        result.Email = entity.Email;
                        result.Password = randomPassword;

                        return result;
                    }
                }
                    

                return result;
            }
        }

        #region private members

        private async Task LoadCredentials(IdentityUnitOfWork unitOfWork, int credentialsId)
        {
            await unitOfWork.UserCredentialsRepository.GetFirstOrDefault(cred => cred.Id == credentialsId);
        }

        private async Task LoadUserRole(IdentityUnitOfWork unitOfWork, int roleId)
        {
            await unitOfWork.UserRoleRepository.GetFirstOrDefault(role => role.Id == roleId);
        }

        private async Task<List<UserModuleEntity>> LoadUserModules(IdentityUnitOfWork unitOfWork, int userId)
        {
            var modules = await unitOfWork.UserModuleRepository.GetAll(module => module.UserId == userId);

            return modules ?? new List<UserModuleEntity>();
        }

        private string GetEncodedPassword(string password, string salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password).ToList();
            passwordBytes.AddRange(Encoding.UTF8.GetBytes(salt));

            return Convert.ToBase64String(passwordBytes.ToArray());
        }

        private List<Claim> LoadUserClaims(UserIdentity user, List<UserModuleEntity> userModules)
        {
            var modules = (from module in userModules
                           let moduleId = module.ModuleId - 1
                           let moduleValue = (ModuleEnum)moduleId
                           select new ModuleAccessRight
                           {
                               ModuleType = (ModuleEnum)moduleId,
                               ModuleName = Enum.GetName(typeof(ModuleEnum), moduleValue),
                               Deny = module.Deny,
                               HasReadAccess = module.HasReadAccess,
                               HasWriteAccess = module.HasWriteAccess,
                           }).ToList();


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRoleEnum), user.UserRole.RoleType)),
                new Claim("isActive", user.IsActive.ToString()),
                new Claim("accessRights", JsonConvert.SerializeObject(modules))
            };

            return claims;
        }

        private static string GenerateRandomString(Random random, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private (UserCredentials credentials , string randomPassword) GetRandomUserCredentials()
        {
            var random = new Random();
            var salt = Guid.NewGuid().ToString();
            var randomPassword = GenerateRandomString(random, 8);

            return (new UserCredentials
            {
                Salt = salt,
                Password = GetEncodedPassword(randomPassword, salt),
                RefreshToken = string.Empty
            }, randomPassword);
        }
        #endregion
    }
}
