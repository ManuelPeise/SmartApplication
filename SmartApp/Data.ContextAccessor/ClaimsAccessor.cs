using Data.ContextAccessor.Interfaces;
using System.Security.Claims;

namespace Data.ContextAccessor
{
    public class ClaimsAccessor: IClaimsAccessor
    {
        private readonly Dictionary<string, string> _claimsDictionary;

        public ClaimsAccessor()
        {
            _claimsDictionary = LoadClaimsData();
        }

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

        private Dictionary<string, string> LoadClaimsData()
        {
            var dictionary = new Dictionary<string, string>();

            var claims = ClaimsPrincipal.Current?.Claims.ToList() ?? new List<Claim>();

            foreach (var claim in claims)
            {
                if (!dictionary.ContainsKey(claim.Type))
                {
                    dictionary.Add(claim.Type, claim.Value);
                }
            }

            return dictionary;
        }
    }
}
