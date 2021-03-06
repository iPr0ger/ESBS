using System.Reflection;
using IdentityServerHost.Quickstart.UI;
using IdentityServer.Configs;
using IdentityServer.Models.DbConnection;
using IdentityServer.Models.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            var identityConnectionString = config.GetConnectionString("IdentityDbConnectionString");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            services.AddDbContext<IdentityDbConnection>(options =>
                options.UseNpgsql(config.GetConnectionString("IdentityDbConnectionString")));

            services.AddDbContext<UserDbConnection>(options =>
                options.UseNpgsql(config.GetConnectionString("UserDbConnectionString")));

            services.AddIdentity<RmsUser, IdentityRole>()
                .AddEntityFrameworkStores<UserDbConnection>()
                .AddDefaultTokenProviders();
            
            services.AddIdentityServer()
                // Dev settings
                .AddInMemoryClients(Clients.Get())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiScopes(Scopes.GetApiScopes())
                .AddTestUsers(TestUsers.Users)
                // Prod settings
                /*
                .AddAspNetIdentity<RmsUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseNpgsql(identityConnectionString, 
                        opt => opt.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseNpgsql(identityConnectionString, 
                        opt => opt.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                })
                */
                .AddDeveloperSigningCredential();
            
            return services;
        }
    }
}