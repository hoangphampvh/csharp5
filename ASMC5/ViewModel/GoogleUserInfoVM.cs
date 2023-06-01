using System.Security.Claims;
using System.IdentityModel;
using Newtonsoft.Json;
using ServiceStack;

namespace ASMC5.ViewModel
{
    public class GoogleUserInfoVM
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; } = "";

        [JsonProperty("locale")]
        public string Locale { get; set; } = "";

        [JsonProperty("gender")]
        public string Gender { get; set; } = "";

        [JsonProperty("hd")]
        public string HostedDomain { get; set; }

        [JsonProperty("sub")]
        public string Subject { get; set; } = "";

        [JsonProperty("iat")]
        public long IssuedAt { get; set; }

        [JsonProperty("exp")]
        public long ExpiresAt { get; set; }

        [JsonProperty("azp")]
        public string AuthorizedParty { get; set; } = "";

        [JsonProperty("aud")]
        public string Audience { get; set; } = "";
        [JsonProperty("Pass")]
        public string Password { get; set; }
        public IEnumerable<Claim> Claims
        {
            get
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(JwtClaimTypes.Email, Email));
                claims.Add(new Claim(JwtClaimTypes.Name, Name));
                claims.Add(new Claim(JwtClaimTypes.Picture, Picture));
                claims.Add(new Claim(JwtClaimTypes.GivenName, GivenName));
                claims.Add(new Claim(JwtClaimTypes.FamilyName, FamilyName));
                claims.Add(new Claim(JwtClaimTypes.Locale, Locale));
                claims.Add(new Claim(JwtClaimTypes.Gender, Gender));
                claims.Add(new Claim(JwtClaimTypes.Subject, Subject));
                claims.Add(new Claim(JwtClaimTypes.IssuedAt, IssuedAt.ToString()));
                claims.Add(new Claim(JwtClaimTypes.Expiration, ExpiresAt.ToString()));
                claims.Add(new Claim(JwtClaimTypes.AuthorizedParty, AuthorizedParty));
                claims.Add(new Claim(JwtClaimTypes.Audience, Audience));

                if (!string.IsNullOrEmpty(HostedDomain))
                {
                    claims.Add(new Claim(JwtClaimTypes.EmailVerified, "true"));
                    claims.Add(new Claim(JwtClaimTypes.AuthMethod, "google"));
                }
                return claims;
            }
        }
    }
}
