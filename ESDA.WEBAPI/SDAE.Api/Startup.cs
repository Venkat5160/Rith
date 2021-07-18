using SDAE.Api.Configuration;
using SDAE.Api.Data;
using SDAE.Api.Models;
using SDAE.Api.Services;
using SDAE.Data;
using SDAE.Data.Model.OptionSettings;
using Autofac;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using TechRAQ.Common.LoggingService.Serilog.Common.Enums;
using TechRAQ.Common.LoggingService.Serilog.Interfaces.Initialization;
using TechRAQ.Common.LoggingService.Serilog.Interfaces.Service;
using SDAE.Api.Application.SDAEKeywords.Queries;
using SDAE.Api.Application.Account.Queries;

namespace SDAE.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			string connectionString = Configuration["connectionString"];
			services.AddDbContext<ApplicationDbContext>(cfg =>
			{
				cfg.UseSqlServer(connectionString);
			});

			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.User.RequireUniqueEmail = false;
			})
		  .AddEntityFrameworkStores<ApplicationDbContext>()
		  .AddDefaultTokenProviders();

			CorsServiceCollectionExtensions.AddCors(services);

			services.AddControllersWithViews().AddNewtonsoftJson();
			services.AddRazorPages();

			//var settings = Configuration.GetSection("TwilioSettings");
			//services.Configure<TwilioSettings>(settings);

			var appSettings = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettings);

			//var notifySettings = Configuration.GetSection("NotificationSettings");
			//services.Configure<NotificationSettings>(notifySettings);  

			services.AddMediatR(typeof(GetUsersQuery).Assembly);

			services.AddDbContextPool<SDAEDBContext>(options =>
			{
				options.UseSqlServer(Configuration["connectionString"]);
				options.EnableSensitiveDataLogging();
			});

			services.AddSingleton<Common.Notification.INotification, Common.Notification.Notifications>();

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "SDAE Web Api", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter JWT with Bearer into field",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement(){{new OpenApiSecurityScheme{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					},
					Scheme = "oauth2",
					Name = "Bearer",
					In = ParameterLocation.Header,
						},new List<string>()
					}
				});
			});

			services.AddIdentityServer(options =>
			{
				options.Events.RaiseErrorEvents = true;
				options.Events.RaiseFailureEvents = true;
				options.Events.RaiseInformationEvents = true;
				options.Events.RaiseSuccessEvents = true;
			}).AddDeveloperSigningCredential()
			  .AddInMemoryIdentityResources(InMemoryConfiguration.ApiIdentityResources())
			  .AddInMemoryClients(InMemoryConfiguration.ApiClient())
			  .AddInMemoryApiResources(InMemoryConfiguration.ApiResources());

			services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator<ApplicationUser>>();
			services.AddTransient<IProfileService, ProfileService>();		
			
			services.AddAuthentication().AddLocalApi(options =>
			{
				options.ExpectedScope = "SDAE.Api";
			});			

			var container = Bootstrapper.IntegrateContainer(services, Configuration);

			var loggerFactory = container.Resolve<ILoggingFactory>();
			loggerFactory.Initialize(LoggingType.Default);

			var _loggingService = container.Resolve<ILoggingService>();
			_loggingService.LogInfo("Services Lodded");
			return container.Resolve<IServiceProvider>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			#region Swagger

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();
			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "SDAE Api V1");
			});

			#endregion

			app.UseCors(x => x
			   .AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader().WithExposedHeaders("Authorization"));

			CorsMiddlewareExtensions.UseCors(app);

			if (env.IsDevelopment())
			{
				app.UseDatabaseErrorPage();
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory() + "//Images"),
				RequestPath = "/Images"
			});

			app.UseRouting();
			app.UseIdentityServer();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
			});
		}
	}
}
