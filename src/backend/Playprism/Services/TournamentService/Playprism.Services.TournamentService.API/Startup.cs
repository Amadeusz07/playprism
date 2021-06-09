using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Playprism.Services.TournamentService.BLL.Extensions;
using Playprism.Services.TournamentService.DAL.Extensions;
using Playprism.Services.TournamentService.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace Playprism.Services.TournamentService.API
{
    public class Startup
    {
        private const string AuthenticationProviderKey = "AuthKey";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddControllers();

            services.AddTournamentDbContext(Configuration);
            services.AddTournamentService();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tournament API", Version = "v1" });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthenticationProviderKey;
                options.DefaultChallengeScheme = AuthenticationProviderKey;
            }).AddJwtBearer(AuthenticationProviderKey, options =>
            {
                options.Authority = "https://dev-e821827o.eu.auth0.com/";
                options.Audience = "https://playprism/api/v1";
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TournamentOwnerPolicy", policy => policy.Requirements.Add(new TournamentOwnerRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, TournamentOwnerHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tournament API v1");
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
