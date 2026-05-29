using System;
using System.Collections.Generic;
using System.Text;

namespace DisputePortal.Application.Feature.Auth.Settings
{
    public class JwtBearerSettings
    {
        public const string SectionName = nameof(JwtBearerSettings);

        public string Issuer { get; set; } = string.Empty;
        
        public string Audience { get; set; } = string.Empty;
        
        public string SecretKey { get; set; } = string.Empty;
        
        public int AccessTokenExpiryMinutes { get; set; } = 15;

        public int RefreshTokenExpiryDays { get; set; } = 7;
    }
}
