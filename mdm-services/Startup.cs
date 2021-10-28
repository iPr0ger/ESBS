using System.Net;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using mdm_services.Extensions;
using mdm_services.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace mdm_services
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setting for the release build for server
            /*services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("51.210.99.16"));
            });*/
            
            services.AddApplicationServices(Configuration);
            
            services.AddControllers();
            
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "The RMS API - Metadata management documentation", Version = "v1" });
                c.EnableAnnotations();
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Setting for the release build for server
            /*app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });*/
            
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "The RMS API | ECRIN";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "The RMS API - Metadata management documentation (v.1)");
                c.InjectStylesheet("/documentation/swagger-custom/swagger-custom-styles.css");
                c.InjectJavascript("/documentation/swagger-custom/swagger-custom-script.js");
                c.RoutePrefix = "rest/documentation";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Open");
            
            app.UseEndpoints(x =>
            {
                x.MapGraphQL("/graphql/v1");
            });
            
            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/graphql/v1",
                Path = "/graphql/ui"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}