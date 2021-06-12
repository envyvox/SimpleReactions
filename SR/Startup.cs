using System;
using System.Reflection;
using Autofac;
using Discord.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SR.Data;
using SR.Framework.Autofac;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DiscordClientOptions>(x => _config.GetSection("Discord").Bind(x));

            services
                .AddHealthChecks()
                .AddNpgSql(_config.GetConnectionString("main"))
                .AddDbContextCheck<AppDbContext>();

            services.AddDbContextPool<AppDbContext>(o =>
            {
                o.UseNpgsql(_config.GetConnectionString("main"),
                    x => { x.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name); });
            });

            services.AddSingleton<CommandService>();
            services.AddSingleton<IDiscordClientService, DiscordClientService>();

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MigrateDb(app.ApplicationServices);

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

        private static void MigrateDb(IServiceProvider app)
        {
            using var serviceScope = app.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
    }
}
