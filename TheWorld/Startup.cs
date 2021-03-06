﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TheWorld.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.Services;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace TheWorld
{
  public class Startup
  {
    private IHostingEnvironment _env;
    private IConfigurationRoot _config;
    public Startup(IHostingEnvironment env)
    {
      _env = env;
      var builder = new ConfigurationBuilder()
        .SetBasePath(_env.ContentRootPath)
        .AddJsonFile("config.json")
        .AddEnvironmentVariables();
      _config = builder.Build();
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton(_config);

      //if (_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
      //{
      //  services.AddScoped<IMailService, DebugMailService>();
      //}
      //else
      //{
      //  // Implement a real Mail Service
      //}


      services.AddDbContext<WorldContext>();

      

      services.AddIdentity<WorldUser, IdentityRole>(config =>
      {
        config.User.RequireUniqueEmail = true;
        config.Password.RequiredLength = 8;
        //config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login"; // TODO: deprecated

      }).AddEntityFrameworkStores<WorldContext>();


services.AddScoped<IWorldRepository, WorldRepository>();

      services.AddTransient<GeoCoordsService>();
      services.AddTransient<WorldContextSeedData>();


      services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Auth/Login"); //TODO: Alternativa para config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
      services.AddLogging();

      // é preciso adicionar o MVC nas configurações para
      // as páginas funcionarem
      services.AddMvc().
        AddJsonOptions(config =>
      config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
      );


    

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, WorldContextSeedData seeder, ILoggerFactory factory)
    {
      //#if DEBUG
      //app.UseDeveloperExceptionPage();
      //#endif

      //AutoMapper.Mapper.Configuration.CreateMapper<>

      Mapper.Initialize(config =>
      {
        config.CreateMap<TripViewModel, Trip>().ReverseMap();
        config.CreateMap<StopViewModel, Stop>().ReverseMap();
      });

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        factory.AddDebug(LogLevel.Information);
      }
      else
      {
        factory.AddDebug(LogLevel.Error);
      }


      // A ordem faz diferença pois se trata da execução da Middleware.

      // É isso aqui que faz utilizar o wwwroot ou as Views do ASP.Net
      //app.UseDefaultFiles();

      app.UseStaticFiles();

      // A ordem importa!
      //app.UseIdentity(); // TODO: deprecated

      AuthAppBuilderExtensions.UseAuthentication(app);

      app.UseMvc(config =>
      {
        config.MapRoute(
          name: "Default",
          template: "{controller}/{action}/{id?}",
          defaults: new { controller = "App", action = "Index" }
          );
      });

      seeder.EnsureSeedData().Wait();

      //app.Run(async (context) =>
      //{
      //    await context.Response.WriteAsync("Hello World!");
      //});
    }
  }
}
