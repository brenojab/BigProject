using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TheWorld
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddDbContext<WorldContext>();

      // é preciso adicionar o MVC nas configurações para
      // as páginas funcionarem
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      //#if DEBUG
      //app.UseDeveloperExceptionPage();
      //#endif

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }


      // A ordem faz diferença pois se trata da execução da Middleware.

      // É isso aqui que faz utilizar o wwwroot ou as Views do ASP.Net
      //app.UseDefaultFiles();

      app.UseStaticFiles();

      app.UseMvc(config =>
      {
        config.MapRoute(
          name: "Default",
          template: "{controller}/{action}/{id?}",
          defaults: new { controller = "App", action = "Index" }
          );
      });
      
      //app.Run(async (context) =>
      //{
      //    await context.Response.WriteAsync("Hello World!");
      //});
    }
  }
}
