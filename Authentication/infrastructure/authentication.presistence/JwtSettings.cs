using Microsoft.Extensions.Configuration;

namespace authentication.presistence
{
    /// <summary>
    /// JWT settings shared by token creation (login) and token validation (Program.cs).
    /// The signing key is read from "appSetting:Token" and must come from user-secrets or an
    /// environment variable (appSetting__Token) — never from a committed appsettings file.
    /// </summary>
    public sealed class JwtSettings
    {
        public const int MinimumKeyBytes = 32; // HMAC-SHA256 needs at least 256 bits

        public string Key { get; init; } = string.Empty;
        public string Issuer { get; init; } = "industrial-office-auth";
        public string Audience { get; init; } = "industrial-office";
        public int LifetimeHours { get; init; } = 8;

        public static JwtSettings From(IConfiguration configuration)
        {
            var key = configuration["appSetting:Token"];
            if (string.IsNullOrWhiteSpace(key) || System.Text.Encoding.UTF8.GetByteCount(key) < MinimumKeyBytes)
            {
                throw new InvalidOperationException(
                    "JWT signing key 'appSetting:Token' is missing or shorter than 32 bytes. " +
                    "Set it with `dotnet user-secrets set \"appSetting:Token\" \"<random 64+ chars>\"` " +
                    "or the environment variable appSetting__Token. Use the SAME value for the auth service and the webapi.");
            }

            return new JwtSettings
            {
                Key = key,
                Issuer = configuration["appSetting:Issuer"] ?? "industrial-office-auth",
                Audience = configuration["appSetting:Audience"] ?? "industrial-office",
                LifetimeHours = int.TryParse(configuration["appSetting:TokenLifetimeHours"], out var h) && h > 0 ? h : 8,
            };
        }
    }
}
