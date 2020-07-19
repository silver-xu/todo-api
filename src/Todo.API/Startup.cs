using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sciensoft.Hateoas.Extensions;
using Todo.API.Middlewares;
using Todo.Controllers;
using Todo.DataModels;
using Todo.DataModels.Interfaces;
using Todo.DomainModels;
using Todo.Repositories;
using Todo.Repositories.Interfaces;
using Todo.Services;
using Todo.Services.Interfaces;

namespace Todo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                })
                .AddLink(policy =>
                {
                    policy
                      .AddPolicy<GetScheduleModel>(model =>
                      {
                          model
                          .AddSelf(m => m.ScheduleId, ScheduleController.GetSchedule)
                          .AddRoute(m => m.ScheduleId, ScheduleController.UpdateSchedule)
                          .AddRoute(m => m.ScheduleId, ScheduleController.DeleteSchedule);
                      });
                });

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddSingleton<ITodoContext, InMemoryTodoContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<AuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
