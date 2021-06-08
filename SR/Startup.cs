using System.Reflection;
using Autofac;
using Dapper;
using Discord.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SR.Data;
using SR.Framework.Autofac;
using SR.Framework.Database;
using SR.Framework.EF;
using SR.Services.DiscordServices.DiscordClientService;
using SR.Services.DiscordServices.DiscordClientService.Impl;
using SR.Services.DiscordServices.DiscordClientService.Options;

namespace SR
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConnectionOptions>(x => x.ConnectionString = _config.GetConnectionString("main"));
            services.Configure<DiscordClientOptions>(x => _config.GetSection("Discord").Bind(x));
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            services
                .AddHealthChecks()
                .AddNpgSql(_config.GetConnectionString("main"))
                .AddDbContextCheck<AppDbContext>();

            services.AddDbContextPool<DbContext, AppDbContext>(o =>
            {
                o.UseNpgsql(_config.GetConnectionString("main"),
                    s => { s.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name); });
            });

            services.AddSingleton<CommandService>();
            services.AddSingleton<IDiscordClientService, DiscordClientService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.MigrateDb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.StartDiscord();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assembly = typeof(IDiscordClientService).Assembly;
            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsDefined(typeof(InjectableServiceAttribute), false) &&
                            x.GetCustomAttribute<InjectableServiceAttribute>().IsSingletone)
                .As(x => x.GetInterfaces()[0])
                .SingleInstance();

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsDefined(typeof(InjectableServiceAttribute), false) &&
                            !x.GetCustomAttribute<InjectableServiceAttribute>().IsSingletone)
                .As(x => x.GetInterfaces()[0])
                .InstancePerLifetimeScope();
        }
    }
}
