using System;
using System.Collections.Generic;
using System.Text;

namespace DisputePortal.Application.Feature.Auth.Settings
{
    public class PasswordHashSettings
    {
        public const string SectionName = nameof(PasswordHashSettings);

        public string Salt { get; set; } = string.Empty;
    }
}
