using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Users.models
{
    public class Test
    {
        // In ConfigureServices method of Startup.cs
        public void ConfigureServices(IServiceCollection services)
        {
            // Add authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                // Configure the JWT bearer authentication options
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // Set your issuer signing key, audience, etc.
                    // Example:
                    // ValidIssuer = "https://yourdomain.com",
                    // ValidAudience = "your_client_id",
                    // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key")),
                };
            });

            // Other service configurations...
        }

    }
}