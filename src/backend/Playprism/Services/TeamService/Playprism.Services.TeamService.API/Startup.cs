using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Playprism.Services.TeamService.API.Filters;
using Playprism.Services.TeamService.API.Interface.Repositories;
using Playprism.Services.TeamService.API.Interfaces.Repositories;
using Playprism.Services.TeamService.API.Interfaces.Services;
using Playprism.Services.TeamService.API.Models;
using Playprism.Services.TeamService.API.Repositories;
using Playprism.Services.TeamService.API.Services;
using RestSharp;

namespace Playprism.Services.TeamService.API
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
            services.AddControllers(config =>
            {
                config.Filters.Add<UserAuth0Filter>();
            });
            services.AddDbContext<TeamDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("TeamDbConnection"),
                    options => options.MigrationsAssembly("Playprism.Services.TeamService.API")
                    );
            });

            var auth0Section = Configuration.GetSection("Auth0");
            services.Configure<AuthConfig>(auth0Section);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IPlayerRepository), typeof(PlayerRepository));
            services.AddScoped(typeof(ITeamRepository), typeof(TeamRepository));
            services.AddScoped(typeof(ITeamPlayerAssignmentRepository), typeof(TeamPlayerAssignmentRepository));
            services.AddScoped<IAuth0Repository, Auth0Repository>();
            services.AddScoped<ITeamManageService, TeamManageService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddTransient<IRestClient, RestClient>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Team API", Version = "v1" });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthenticationProviderKey;
                options.DefaultChallengeScheme = AuthenticationProviderKey;
            }).AddJwtBearer(AuthenticationProviderKey, options =>
            {
                options.Authority = auth0Section.GetValue<string>("Authority");
                options.Audience = auth0Section.GetValue<string>("Audience");
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TeamOwnerPolicy", policy => policy.Requirements.Add(new TeamOwnerRequirement()));
            }); 
            services.AddSingleton<IAuthorizationHandler, TeamOwnerHandler>();
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Team API v1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
