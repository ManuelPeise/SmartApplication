using Data.AppContext;
using Data.ContextAccessor.Interfaces;
using Data.Identity;
using Data.Shared;
using Data.Shared.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Logic.Shared
{
    public abstract class ALogicBase
    {
        private readonly IdentityDbContext? _identityContext;
        private readonly ApplicationDbContext _applicationContext;
        private readonly ILogRepository? _logRepository;
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public IdentityDbContext? IdentityContext { get => _identityContext; }
        public ApplicationDbContext ApplicationContext { get => _applicationContext; }

        private Dictionary<string, string> _claimsDictionary = new Dictionary<string, string>();

        protected ALogicBase(ApplicationDbContext appDbContext, IdentityDbContext? identityDbContext = null, ILogRepository? logRepository = null, IHttpContextAccessor? httpContextAccessor = null)
        {
            _applicationContext = appDbContext;
            _identityContext = identityDbContext;
            _logRepository = logRepository;
            _httpContextAccessor = httpContextAccessor;

            if (httpContextAccessor != null) 
            { 
                LoadClaimsData();
            }
        }

        public async Task LogMessage(LogMessageEntity logMessage)
        {
            if (_logRepository != null)
            {
                await _logRepository.AddMessage(logMessage);
            }
        }

        public async Task SaveContextChangesAsync()
        {
            if (_identityContext != null)
            {
                await SaveIdentityContextChanges();
            }

            if (_applicationContext != null)
            {
                await SaveApplicationContextChanges();
            }
        }

        #region db context

        private async Task SaveIdentityContextChanges()
        {
            var currentUser = _httpContextAccessor?.HttpContext.User.Identity;

            if(_identityContext == null)
            {
                throw new InvalidOperationException("IdentityDbContext could not be null!");
            }

            var modifiedEntries = _identityContext.ChangeTracker.Entries()
              .Where(x => x.State == EntityState.Modified ||
              x.State == EntityState.Added);

            foreach (var entry in modifiedEntries)
            {
                if (entry != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((AEntityBase)entry.Entity).CreatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).CreatedAt = DateTime.Now;
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;

                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;
                    }
                }
            }

            await _identityContext.SaveChangesAsync();
        }

        private async Task SaveApplicationContextChanges()
        {
            var currentUser = _httpContextAccessor?.HttpContext.User.Identity;

            if(_applicationContext == null)
            {
                throw new InvalidOperationException("ApplicationDbContext could not be null!");
            }

            var modifiedEntries = _applicationContext.ChangeTracker.Entries()
              .Where(x => x.State == EntityState.Modified ||
              x.State == EntityState.Added);

            foreach (var entry in modifiedEntries)
            {
                if (entry != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((AEntityBase)entry.Entity).CreatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).CreatedAt = DateTime.Now;
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;

                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        ((AEntityBase)entry.Entity).UpdatedBy = currentUser?.Name ?? "System";
                        ((AEntityBase)entry.Entity).UpdatedAt = DateTime.Now;
                    }
                }
            }

            await _applicationContext.SaveChangesAsync();
        }

        #endregion

        #region claims

        public T? GetClaimsValue<T>(string key)
        {
            if (_claimsDictionary.ContainsKey(key))
            {
                var selectedClaimField = _claimsDictionary[key];
                var type = typeof(T);

                if (selectedClaimField == null)
                {
                    return default(T?);
                }

                if (type == typeof(int))
                {
                    return (T)Convert.ChangeType(int.Parse(selectedClaimField), type);
                }

                if (type == typeof(string))
                {
                    return (T)Convert.ChangeType(selectedClaimField, type);
                }

                if (type == typeof(Guid))
                {
                    return (T)Convert.ChangeType(new Guid(selectedClaimField), type);
                }

                if (type == typeof(DateTime))
                {
                    return (T)Convert.ChangeType(DateTime.Parse(selectedClaimField), type);
                }

            }

            return default;
        }

        private void LoadClaimsData()
        {
            var claims = ClaimsPrincipal.Current?.Claims.ToList() ?? new List<Claim>();

            foreach (var claim in claims)
            {
                if (!_claimsDictionary.ContainsKey(claim.Type))
                {
                    _claimsDictionary.Add(claim.Type, claim.Value);
                }
            }
        }

        #endregion
    }
}
