using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SR.Data;
using SR.Services.Discord.Client;
using SR.Services.Discord.Client.Extensions;
using SR.Services.Discord.Client.Impl;

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

            services.AddMediatR(typeof(IDiscordClientService).Assembly);
            services.AddAutoMapper(typeof(IDiscordClientService).Assembly);

            services.AddSingleton<IDiscordClientService, DiscordClientService>();
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

        private static void MigrateDb(IServiceProvider app)
        {
            using var serviceScope = app.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
    }
}
